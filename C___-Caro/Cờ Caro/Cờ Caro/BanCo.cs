using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Cờ_Caro
{
    class BanCo
    {
        private int _Sodong;
        private int _Socot;

        public int Sodong
        {
            get { return _Sodong; }
        }
        public int Socot
        {
            get { return _Socot; }
        }
        public BanCo()
        {
            _Sodong = 0;
            _Socot = 0;
        }
        public BanCo(int Sodong, int Socot)
        {
            _Sodong = Sodong;
            _Socot = Socot;
        }
        public void Vebanco(Graphics g)
        {
            for (int i = 0; i <= _Socot; i++)
            {
                g.DrawLine(Carochess.pen, i * Ô_cờ._chieuRong, 0, i * Ô_cờ._chieuRong, _Sodong * Ô_cờ._chieuCao);
            }
            for (int j = 0; j <= _Sodong; j++)
            {
                g.DrawLine(Carochess.pen, 0, j * Ô_cờ._chieuCao, _Socot * Ô_cờ._chieuRong, j * Ô_cờ._chieuCao);
            }
        }
        public void VeQuanCo(Graphics g,Point point,SolidBrush sb)
        {
            g.FillEllipse(sb, point.X + 2, point.Y + 2, Ô_cờ._chieuRong - 4, Ô_cờ._chieuCao - 4);
        }
        public void XoaQuanCo(Graphics g, Point point, SolidBrush sb)
        {
            g.FillRectangle(sb, point.X + 1, point.Y + 1, Ô_cờ._chieuRong - 2, Ô_cờ._chieuCao - 2);
        }
    }
}

