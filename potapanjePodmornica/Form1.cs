using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace potapanjePodmornica
{
    public partial class frmPotop : Form
    {
        static Dictionary<(int duzina, string ime), (int x, int y, bool horizontalno)> brodovi = new Dictionary<(int, string), (int, int, bool)>();
        static int sirinaPolja;
        public frmPotop()
        {
            InitializeComponent();
        }
        static bool JelMozeDaSePostavi((int duzina, string ime) kljuc, (int x, int y, bool horizontalno) vrednost)
        {
            int ax = vrednost.x;
            int ay = vrednost.y;
            int bx, by;
            if (vrednost.horizontalno)
            {
                bx = vrednost.x + kljuc.duzina;
                by = vrednost.y;
            }
            else
            {
                bx = vrednost.x;
                by = vrednost.y + kljuc.duzina;
            }
            foreach (var i in brodovi.Keys)
            {
                if (i != kljuc && brodovi[i].x != -1)
                {
                    int cx = brodovi[i].x;
                    int cy = brodovi[i].y;
                    int dx, dy;
                    if (vrednost.horizontalno)
                    {
                        dx = brodovi[i].x + i.duzina;
                        dy = brodovi[i].y;
                    }
                    else
                    {
                        dx = brodovi[i].x;
                        dy = brodovi[i].y + i.duzina;
                    }
                    double duzina = Math.Abs((cx - ax) * (by - dy) - (cy - ay) * (bx - dx))
                        * 1.0 / Math.Sqrt((cx - ax) * (cx - ax) + (by - dy) * (by - dy));
                    if (duzina < 1) return false;
                }
            }
            return true;
        }
        //pictureBox1.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
        //pictureBox1.Refresh();
        //imena brodova: 1a,1b,1c,1d,2a,2b,2c,3a,3b,4a
        private void UnosPozicija(bool postoji)
        {
            pbx1a.Enabled = postoji;
            pbx1a.Visible = postoji;
            pbx1b.Enabled = postoji;
            pbx1b.Visible = postoji;
            pbx1c.Enabled = postoji;
            pbx1c.Visible = postoji;
            pbx1d.Enabled = postoji;
            pbx1d.Visible = postoji;
            pbx2a.Enabled = postoji;
            pbx2a.Visible = postoji;
            pbx2b.Enabled = postoji;
            pbx2b.Visible = postoji;
            pbx2c.Enabled = postoji;
            pbx2c.Visible = postoji;
            pbx3a.Enabled = postoji;
            pbx3a.Visible = postoji;
            pbx3b.Enabled = postoji;
            pbx3b.Visible = postoji;
            pbx4a.Enabled = postoji;
            pbx4a.Visible = postoji;
            pbxJa.Enabled = postoji;
            pbxJa.Visible = postoji;
            btnHelp.Enabled = postoji;
            btnHelp.Visible = postoji;
            btnSpreman.Enabled = postoji;
            btnSpreman.Visible = postoji;
            btnRestartPozicije.Enabled = postoji;
            btnRestartPozicije.Visible = postoji;
            lblIgrac1.Visible = postoji;
            pbxAvion.Enabled = false;
            pbxAvion.Visible = false;
            pbxProtivnik.Enabled = false;
            pbxProtivnik.Visible = false;
            lblIgrac2.Enabled = false;
            lblIgrac2.Visible = false;
            lblIspisPobednik.Enabled = false;
            lblIspisPobednik.Visible = false;
            btnIgrajOpet.Enabled = false;
            btnIgrajOpet.Visible = false;
            btnIzlaz.Enabled = false;
            btnIzlaz.Visible = false; 
        }
        private void btnStartProg_Click(object sender, EventArgs e)
        {
            UnosPozicija(true);
            btnStartProg.Enabled = false;
            btnStartProg.Visible = false;
        }

        private void btnSpreman_Click(object sender, EventArgs e)
        {

        }

        private void frmPotop_Load(object sender, EventArgs e)
        {
            UnosPozicija(false);
        }
    }
}
