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
        static int sirinaPolja;
        bool prviNaPotezu = true;
        int[,] tablaPrvog = new int[10, 10];
        int[,] tablaDrugog = new int[10, 10];
        (int, int,string)[] pozicijeBrodovaZaPostavljanje = new (int, int,string)[10];
        public frmPotop()
        {
            InitializeComponent();
        }
        //0 - prazno, 1 - pogodjeno prazno, 2 - brod, 3 - pogodjen brod
        //pictureBox1.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
        //pictureBox1.Refresh();
        //imena brodova: 1a,1b,1c,1d,2a,2b,2c,3a,3b,4a
        static bool MozeDaSePostaviBrod(int x, int y, int duzina, bool horizontalno, int[,]tabela)
        {
            if (horizontalno)
            {
                int krajX = x + duzina - 1;
                if (krajX > 9) return false;
                for (int i = x; i <= krajX; i++)
                {
                    if (tabela[y, i] != 0) return false;
                }
                if (y != 0)
                {
                    for (int i = x; i <= krajX; i++)
                    {
                        if (tabela[y - 1, i] != 0) return false;
                    }
                }
                if (y != 9)
                {
                    for (int i = x; i <= krajX; i++)
                    {
                        if (tabela[y + 1, i] != 0) return false;
                    }
                }
                if (x != 0 && tabela[y, x - 1] != 0) return false;
                if (krajX != 9 && tabela[y, krajX + 1] != 0) return false;
                if (x != 0 && y != 0 && tabela[y - 1, x - 1] != 0) return false;
                if (krajX != 9 && y != 0 && tabela[y - 1, krajX + 1] != 0) return false;
                if (x != 0 && y != 9 && tabela[y + 1, x - 1] != 0) return false;
                if (krajX != 9 && y != 9 && tabela[y + 1, krajX + 1] != 0) return false;
                return true;

            }
            else
            {
                int krajY = y + duzina - 1;
                if (krajY > 9) return false;
                for (int i = y; i <= krajY; i++)
                {
                    if (tabela[i, x] != 0) return false;
                }
                if (x != 0)
                {
                    for (int i = y; i <= krajY; i++)
                    {
                        if (tabela[i, x - 1] != 0) return false;
                    }
                }
                if (x != 9)
                {
                    for (int i = y; i <= krajY; i++)
                    {
                        if (tabela[i, x + 1] != 0) return false;
                    }
                }
                if (y != 0 && tabela[y - 1, x] != 0) return false;
                if (krajY != 9 && tabela[krajY + 1, x] != 0) return false;
                if (y != 0 && x != 0 && tabela[y - 1, x - 1] != 0) return false;
                if (krajY != 9 && x != 0 && tabela[krajY + 1, x - 1] != 0) return false;
                if (y != 0 && x != 9 && tabela[y - 1, x + 1] != 0) return false;
                if (krajY != 9 && x != 9 && tabela[krajY + 1, x + 1] != 0) return false;
                return true;
            }
        }
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
            this.BackgroundImage = null;
            int poslednjeSlovo = 100, brVrste = 1;
            for (int i = 0,k = 97; i < pozicijeBrodovaZaPostavljanje.Length && k<=poslednjeSlovo; i++,k++)
            {
                string imeBroda = brVrste + Convert.ToString(Convert.ToChar(k));
                PictureBox a = (PictureBox)this.Controls.Find("pbx" + imeBroda, true)[0];
                pozicijeBrodovaZaPostavljanje[i] = (a.Top, a.Left,"pbx"+imeBroda);
                if(k == poslednjeSlovo) { k = 96; poslednjeSlovo--; brVrste++; }  
            }
        }
        private void IscrtajTablu(PictureBox tabla, PaintEventArgs e)
        { 
            sirinaPolja = tabla.Width / 11;
            //e.Graphics.DrawString(tekst, new Font("Arial", 12),Brushes.Green, new Point(x, y));
            for (int i = 1; i < 11; i++)
            {
                e.Graphics.DrawString(Convert.ToString(i), new Font("Georgia", 12), Brushes.Blue, new Point(sirinaPolja*i+5, 5));
                e.Graphics.DrawString(Convert.ToString((char)(64+i)), new Font("Georgia", 12), Brushes.Blue, new Point(5, sirinaPolja * i + 5));
            }
            Pen olovka = new Pen(Color.Blue, (float)(tabla.Width * 0.01 + 1));
            for (int i = 0; i < 11; i++)
            {
                e.Graphics.DrawLine(olovka,(i+1)*sirinaPolja, sirinaPolja, (i + 1) * sirinaPolja,sirinaPolja*11);
                e.Graphics.DrawLine( olovka, sirinaPolja, (i + 1) * sirinaPolja, sirinaPolja * 11, (i + 1) * sirinaPolja);
            }

        }
        private void btnSpreman_Click(object sender, EventArgs e)
        {
            if(prviNaPotezu)
            {
                prviNaPotezu = false;
                lblIgrac1.Enabled = false;
                lblIgrac1.Visible = false;
                lblIgrac2.Enabled = true;
                lblIgrac2.Visible = true;
                pbxJa.Refresh();
                for (int i = 0; i < pozicijeBrodovaZaPostavljanje.Length; i++)
                {
                    PictureBox a = (PictureBox)this.Controls.Find(pozicijeBrodovaZaPostavljanje[i].Item3,true)[0];
                    a.Top = pozicijeBrodovaZaPostavljanje[i].Item1;
                    a.Left = pozicijeBrodovaZaPostavljanje[i].Item2;
                }
                //zapamti pozicije prvog
            }
            /*
             else ispisi obe tabele
             */
        }

        private void frmPotop_Load(object sender, EventArgs e)
        {
            UnosPozicija(false);
        }

        private void pbxJa_Paint(object sender, PaintEventArgs e)
        {
            IscrtajTablu(pbxJa, e);
        }
    }
}
