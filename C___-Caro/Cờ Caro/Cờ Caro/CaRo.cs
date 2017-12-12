using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cờ_Caro
{
    public partial class frmCoCaro : Form
    {
        private Carochess carochess;
        private Graphics grs;
        public frmCoCaro()
        {
            InitializeComponent();
            carochess = new Carochess();
            carochess.KhoiTaoMangOCo();
            grs = pnlBanCo.CreateGraphics();
            playerVsAIToolStripMenuItem.Click += new EventHandler(PvC_Click);
            button2.Click += new EventHandler(PvC_Click);

        }

       

        private void reToolStripMenuItem_Click(object sender, EventArgs e)
        {
            carochess.Redo(grs);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            carochess.Vebanco(grs);
            carochess.VeLaiQuanCo(grs);
        }

        private void frmCoCaro_Load(object sender, EventArgs e)
        {
            lbchu.Text = "Ai đi được 5 ô liên tiếp thẳng , ngang, \n" +
                                 "chéo hàng thì thắng " +  "(lưu ý : khi bị \n" +
                                 "chặn hai đầu cho dù đi được 5 ô liên\n" +
                                 "tiếp cũng sẽ không tính là chiến\n" +
                                 " thắng) ";
            tmchuchay.Enabled = true;
            carochess.Vebanco(grs);
        }

        private void pnlBanCo_MouseClick(object sender, MouseEventArgs e)
        {
            if (!carochess.Ready)
                return;
            carochess.DanhCo(e.X, e.Y, grs);
            if (carochess.KiemtraWin())
                carochess.KetthucGame();
            else
            {
                if (carochess.CheDoChoi == 2)
                {
                    carochess.KhoiDongCOM(grs);
                    if (carochess.KiemtraWin())
                    {
                        carochess.KetthucGame();
                    }

                }
            }
        }

        private void PvP(object sender, EventArgs e)
        {
            grs.Clear(pnlBanCo.BackColor);
            carochess.startPvP(grs);
        }


        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            carochess.Undo(grs);
        }

        private void PvP_Click(object sender, EventArgs e)
        {
            grs.Clear(pnlBanCo.BackColor);
            carochess.startPvP(grs);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult h = MessageBox.Show("Bạn có chắc muốn thoát không?","Cờ Caro", MessageBoxButtons.YesNo);
            if (h == DialogResult.Yes)
                Application.Exit();
        }

        private void PvC_Click(object sender, EventArgs e)
        {
            grs.Clear(pnlBanCo.BackColor);
            carochess.startPvC(grs);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            grs.Clear(pnlBanCo.BackColor);
            if(carochess.CheDoChoi == 1)
            {
                carochess.startPvP(grs);
            }
            else
                carochess.startPvC(grs);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult h = MessageBox.Show("Bạn có chắc muốn thoát không?", "Error", MessageBoxButtons.YesNo);
            if (h == DialogResult.Yes)
                Application.Exit();
        }
    }
}
