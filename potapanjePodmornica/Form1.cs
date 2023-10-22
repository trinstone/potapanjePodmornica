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
        private void btnStartProg_Click(object sender, EventArgs e)
        {

        }

        private void btnSpreman_Click(object sender, EventArgs e)
        {

        }
    }
}
