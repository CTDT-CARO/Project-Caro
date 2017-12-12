using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cờ_Caro
{
    public enum END
    {
        HoaCo,
        P1,
        P2,
        AI
    }
    class Carochess
    {
        public static Pen pen;
        public static SolidBrush sbwhite;
        public static SolidBrush sbblack;
        public static SolidBrush sbcontroldark;
        private BanCo _BanCo;
        private Ô_cờ[,] _MangOCo;
        private Stack<Ô_cờ> stkcacnuocdadi;
        private Stack<Ô_cờ> stkcacnuocUndo;
        private END _end;
        private int luotdi;
        private bool _Ready;
        private int _chedochoi;

        public int CheDoChoi
        {
            get { return _chedochoi; }
        }
        public bool Ready
        {
            get { return _Ready; }
        }
        public Carochess()
        {
            _BanCo = new BanCo(20, 20);
            _MangOCo = new Ô_cờ[_BanCo.Sodong, _BanCo.Sodong];
            pen = new Pen(Color.Red);  //224, 224, 224
            sbwhite = new SolidBrush(Color.White);
            sbblack = new SolidBrush(Color.Black);
            sbcontroldark = new SolidBrush(Color.FromArgb(192, 192, 255));
            stkcacnuocdadi = new Stack<Ô_cờ>();
            stkcacnuocUndo = new Stack<Ô_cờ>();
            luotdi = 1;
        }
        public void Vebanco(Graphics g)
        {
            _BanCo.Vebanco(g);
        }
        public void KhoiTaoMangOCo()
        {
            for (int i = 0; i < _BanCo.Sodong; i++)
                for (int j = 0; j < _BanCo.Sodong; j++)
                {
                    _MangOCo[i, j] = new Ô_cờ(i, j, new Point(j * Ô_cờ._chieuRong, i * Ô_cờ._chieuCao), 0);
                }
        }
        public bool DanhCo(int MouseX, int MouseY, Graphics g)
        {
            int Cot = MouseX / Ô_cờ._chieuRong;
            int Dong = MouseY / Ô_cờ._chieuCao;
            if (_MangOCo[Dong, Cot].SoHuu != 0)
            {
                return false;
            }
            switch (luotdi)
            {
                case 1:
                    _MangOCo[Dong, Cot].SoHuu = 1;
                    _BanCo.VeQuanCo(g, _MangOCo[Dong, Cot].Vitri, sbblack);
                    luotdi = 2;
                    break;
                case 2:
                    _MangOCo[Dong, Cot].SoHuu = 2;
                    _BanCo.VeQuanCo(g, _MangOCo[Dong, Cot].Vitri, sbwhite);
                    luotdi = 1;
                    break;
                default:
                    MessageBox.Show("Lỗi");
                    break;
            }
            stkcacnuocUndo = new Stack<Ô_cờ>();
            Ô_cờ oco = new Ô_cờ(_MangOCo[Dong, Cot].Dong, _MangOCo[Dong, Cot].Cot, _MangOCo[Dong, Cot].Vitri, _MangOCo[Dong, Cot].SoHuu);
            stkcacnuocdadi.Push(oco);
            return true;
        }
        public void VeLaiQuanCo(Graphics g)
        {
            foreach (Ô_cờ oco in stkcacnuocdadi)
            {
                if (oco.SoHuu == 1)
                {
                    _BanCo.VeQuanCo(g, oco.Vitri, sbblack);
                }
                else if (oco.SoHuu == 2)
                    _BanCo.VeQuanCo(g, oco.Vitri, sbwhite);
            }
        }
        public void startPvP(Graphics g)
        {
            _Ready = true;
            stkcacnuocdadi = new Stack<Ô_cờ>();
            stkcacnuocUndo = new Stack<Ô_cờ>();
            luotdi = 1;
            _chedochoi = 1;
            KhoiTaoMangOCo();
            Vebanco(g);
        }
        public void startPvC(Graphics g)
        {
            _Ready = true;
            stkcacnuocdadi = new Stack<Ô_cờ>();
            stkcacnuocUndo = new Stack<Ô_cờ>();
            luotdi = 1;
            _chedochoi = 2;
            KhoiTaoMangOCo();
            Vebanco(g);
            KhoiDongCOM(g);
        }
        #region UndoRedo    
        public void Undo(Graphics g)
        {
            if (stkcacnuocdadi.Count != 0)
            {
                Ô_cờ oco = stkcacnuocdadi.Pop();
                stkcacnuocUndo.Push(new Ô_cờ(oco.Dong, oco.Cot, oco.Vitri, oco.SoHuu));
                _MangOCo[oco.Dong, oco.Cot].SoHuu = 0;
                _BanCo.XoaQuanCo(g, oco.Vitri, sbcontroldark);
                if (luotdi == 1)
                    luotdi = 2;
                else
                    luotdi = 1;
            }

        }
        public void Redo(Graphics g)
        {
            if (stkcacnuocUndo.Count != 0)
            {
                Ô_cờ oco = stkcacnuocUndo.Pop();
                stkcacnuocdadi.Push(new Ô_cờ(oco.Dong, oco.Cot, oco.Vitri, oco.SoHuu));
                _MangOCo[oco.Dong, oco.Cot].SoHuu = oco.SoHuu;
                _BanCo.VeQuanCo(g, oco.Vitri, oco.SoHuu == 1 ? sbblack : sbwhite);
                if (luotdi == 1)
                    luotdi = 2;
                else
                    luotdi = 1;
            }

        }
        #endregion
        #region DuyetWin
        public void KetthucGame()
        {
            switch (_end)
            {
                case END.HoaCo:
                    MessageBox.Show("HoaCo!");
                    break;
                case END.P1:
                    MessageBox.Show("P1 win!");
                    break;
                case END.P2:
                    MessageBox.Show("P2 win!");
                    break;
                case END.AI:
                    MessageBox.Show("COM win!");
                    break;
            }
            _Ready = false;
        }
        public bool KiemtraWin()
        {
            if (stkcacnuocdadi.Count() == _BanCo.Socot * _BanCo.Sodong)
            {
                _end = END.HoaCo;
                return true;
            }
            foreach (Ô_cờ oco in stkcacnuocdadi)
            {
                if (DuyetDoc(oco.Dong, oco.Cot, oco.SoHuu) || DuyetNgang(oco.Dong, oco.Cot, oco.SoHuu) || CheoXuoi(oco.Dong, oco.Cot, oco.SoHuu) || CheoNguoc(oco.Dong, oco.Cot, oco.SoHuu))
                {
                    _end = oco.SoHuu == 1 ? END.P1 : END.P2;
                    return true;
                }
            }
            return false;
        }
        private bool DuyetDoc(int currDong, int currCot, int currSoHuu)
        {
            if (currDong > _BanCo.Sodong - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot].SoHuu != currSoHuu)
                    return false;
            }
            if (currDong == 0 || currDong + Dem == _BanCo.Sodong)
            {
                return true;
            }
            if (_MangOCo[currDong - 1, currCot].SoHuu == 0 || _MangOCo[currDong + Dem, currCot].SoHuu == 0)
            {
                return true;
            }
            return false;
        }
        private bool DuyetNgang(int currDong, int currCot, int currSoHuu)
        {
            if (currCot > _BanCo.Socot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currDong, currCot + Dem].SoHuu != currSoHuu)
                    return false;
            }
            if (currCot == 0 || currCot + Dem == _BanCo.Socot)
            {
                return true;
            }
            if (_MangOCo[currDong, currCot - 1].SoHuu == 0 || _MangOCo[currDong, currCot + Dem].SoHuu == 0)
            {
                return true;
            }
            return false;
        }
        private bool CheoXuoi(int currDong, int currCot, int currSoHuu)
        {
            if (currDong > _BanCo.Sodong - 5 || currCot > _BanCo.Socot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu != currSoHuu)
                    return false;
            }
            if (currCot == 0 || currCot + Dem == _BanCo.Socot || currDong == 0 || currDong + Dem == _BanCo.Sodong)
            {
                return true;
            }
            if (_MangOCo[currDong - 1, currCot - 1].SoHuu == 0 || _MangOCo[currDong + Dem, currCot + Dem].SoHuu == 0)
            {
                return true;
            }
            return false;
        }
        private bool CheoNguoc(int currDong, int currCot, int currSoHuu)
        {
            if (currDong < 4 || currCot > _BanCo.Socot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu != currSoHuu)
                    return false;
            }
            if (currCot == 4 || currDong == _BanCo.Socot - 1 || currCot == 0 || currCot + Dem == _BanCo.Socot)
            {
                return true;
            }
            if (_MangOCo[currDong + 1, currCot - 1].SoHuu == 0 || _MangOCo[currDong - Dem, currCot + Dem].SoHuu == 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region AI
        private long[] MangDiemTanCong = new long[7] { 0, 3, 24, 192, 1536, 12288, 98304 };
        private long[] MangDiemPhongThu = new long[7] { 0, 1, 9, 81, 729, 6561, 59049 };

        public void KhoiDongCOM(Graphics g)
        {
            if (stkcacnuocdadi.Count == 0)
            {
                DanhCo(_BanCo.Sodong / 2 * Ô_cờ._chieuRong + 1, _BanCo.Sodong / 2 * Ô_cờ._chieuCao + 1, g);
            }
            else
            {
                Ô_cờ oco = TimKiemNuocDi();
                DanhCo(oco.Vitri.X + 1, oco.Vitri.Y + 1, g);
            }

        }
        private Ô_cờ TimKiemNuocDi()
        {
            Ô_cờ Ocoresult = new Ô_cờ();
            long DiemMax = 0;
            for (int i = 0; i < _BanCo.Sodong; i++)
            {
                for (int j = 0; j < _BanCo.Socot; j++)
                {
                    if (_MangOCo[i, j].SoHuu == 0)
                    {
                        long DiemTanCong = DiemTanCong_DuyetDoc(i, j) + DiemTanCong_DuyetNgang(i, j) + DiemTanCong_DuyetCheoXuoi(i, j) + DiemTanCong_DuyetCheoNguoc(i, j);
                        long DiemPhongThu = DiemPhongThu_DuyetDoc(i, j) + DiemPhongThu_DuyetNgang(i, j) + DiemPhongThu_DuyetCheoXuoi(i, j) + DiemPhongThu_DuyetCheoNguoc(i, j);
                        long DiemTam = DiemTanCong > DiemPhongThu ? DiemTanCong : DiemPhongThu;
                        if(DiemMax < DiemTam)
                        {
                            DiemMax = DiemTam;
                            Ocoresult = new Ô_cờ(_MangOCo[i, j].Dong, _MangOCo[i, j].Cot, _MangOCo[i, j].Vitri, _MangOCo[i, j].SoHuu);
                        }
                    }
                }
            }
            return Ocoresult;
        }
        #region Tan Cong
        private long DiemTanCong_DuyetDoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currDong + Dem < _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot].SoHuu == 1)
                    SoQuanTa++;
                else if (_MangOCo[currDong + Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currDong - Dem >= _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot].SoHuu == 1)
                    SoQuanTa++;
                else if (_MangOCo[currDong - Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanDich == 2)
                return 0;
            DiemTong -= MangDiemPhongThu[SoQuanDich + 1];
            DiemTong += MangDiemTanCong[SoQuanTa];
            return DiemTong;
        }
        private long DiemTanCong_DuyetNgang(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.Socot; Dem++)
            {
                if (_MangOCo[currDong, currCot + Dem].SoHuu == 1)
                    SoQuanTa++;
                else if (_MangOCo[currDong, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currDong - Dem >= _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong, currCot - Dem].SoHuu == 1)
                    SoQuanTa++;
                else if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanDich == 2)
                return 0;
            DiemTong -= MangDiemPhongThu[SoQuanDich + 1];
            DiemTong += MangDiemTanCong[SoQuanTa];
            return DiemTong;
        }
        private long DiemTanCong_DuyetCheoNguoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.Socot && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu == 1)
                    SoQuanTa++;
                else if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong + Dem < _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot - Dem].SoHuu == 1)
                    SoQuanTa++;
                else if (_MangOCo[currDong + Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanDich == 2)
                return 0;
            DiemTong -= MangDiemPhongThu[SoQuanDich + 1];
            DiemTong += MangDiemTanCong[SoQuanTa];
            return DiemTong;
        }
        private long DiemTanCong_DuyetCheoXuoi(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.Socot && _BanCo.Sodong + Dem <= _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu == 1)
                    SoQuanTa++;
                else if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 1)
                    SoQuanTa++;
                else if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanDich == 2)
                return 0;
            DiemTong -= MangDiemPhongThu[SoQuanDich + 1];
            DiemTong += MangDiemTanCong[SoQuanTa];
            return DiemTong;
        }
        #endregion
        #region PhongNgu
        private long DiemPhongThu_DuyetDoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currDong + Dem < _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot].SoHuu == 1)
                { 
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong + Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currDong - Dem >= _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong - Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            if (SoQuanTa == 2)
                return 0;
            DiemTong += MangDiemPhongThu[SoQuanDich];
            return DiemTong;
        }
        private long DiemPhongThu_DuyetNgang(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.Socot; Dem++)
            {
                if (_MangOCo[currDong, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currDong - Dem >= _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            if (SoQuanTa == 2)
                return 0;
            DiemTong += MangDiemPhongThu[SoQuanDich];
            return DiemTong;
        }
        private long DiemPhongThu_DuyetCheoNguoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.Socot && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong + Dem < _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong + Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            if (SoQuanTa == 2)
                return 0;
            DiemTong += MangDiemPhongThu[SoQuanDich];
            return DiemTong;
        }
        private long DiemPhongThu_DuyetCheoXuoi(int currDong, int currCot)
        {
            long DiemTong = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.Socot && _BanCo.Sodong + Dem <= _BanCo.Sodong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            if (SoQuanTa == 2)
                return 0;
            DiemTong += MangDiemPhongThu[SoQuanDich];
            return DiemTong;
        }
        #endregion
        #endregion
    }
}
//cccc
