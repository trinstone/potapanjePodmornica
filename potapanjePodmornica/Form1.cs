using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace potapanjePodmornica
{
    public partial class frmPotop : Form
    {
        int sirinaPolja;
        bool prviNaPotezu = true;
        (int, string)[,] tablaPrvog = new (int, string)[10, 10];
        (int, string)[,] tablaDrugog = new (int, string)[10, 10];
        string[] naziviBrodova = { "1a", "1b", "1c", "1d", "2a", "2b", "2c", "3a", "3b", "4a" };
        (int, int)[] pozicijeBrodovaZaPostavljanje = new (int, int)[10];
        (int, int, bool)[] pozicijeBrodovaPrvog = new (int, int, bool)[10];
        (int, int, bool)[] pozicijeBrodovaDrugog = new (int, int, bool)[10];
        bool pomeranjeBroda = false;
        bool namestanjeBrodova = false;
        bool sledeci;
        int pozX;
        int pozY;
        public frmPotop()
        {
            InitializeComponent();/*
            Rectangle intersection = Rectangle.Intersect(pbxProtivnik.Bounds, pbxAvion.Bounds);
            Region region = new Region(intersection);
            pbxAvion.Region = region;*/
        }
        //0 - prazno, 1 - pogodjeno prazno, 2 - brod, 3 - pogodjen brod, 5 - pogodjen ceo brod?
        //imena brodova: 1a,1b,1c,1d,2a,2b,2c,3a,3b,4a
        private bool MozeDaSePostaviBrod(int x, int y, int duzina, bool horizontalno, (int, string)[,] tabela, string ime)
        {
            if (horizontalno)
            {
                int krajX = x + duzina - 1;
                if (krajX > 9) return false;
                for (int i = x; i <= krajX; i++)
                {
                    if (tabela[y, i].Item2 != ime && tabela[y, i].Item1 != 0) return false;
                }
                if (y != 0)
                {
                    for (int i = x; i <= krajX; i++)
                    {
                        if (tabela[y - 1, i].Item2 != ime && tabela[y - 1, i].Item1 != 0) return false;
                    }
                }
                if (y != 9)
                {
                    for (int i = x; i <= krajX; i++)
                    {
                        if (tabela[y + 1, i].Item2 != ime && tabela[y + 1, i].Item1 != 0) return false;
                    }
                }
                if (x != 0 && tabela[y, x - 1].Item2 != ime && tabela[y, x - 1].Item1 != 0) return false;
                if (krajX != 9 && tabela[y, krajX + 1].Item2 != ime && tabela[y, krajX + 1].Item1 != 0) return false;
                if (x != 0 && y != 0 && tabela[y - 1, x - 1].Item2 != ime && tabela[y - 1, x - 1].Item1 != 0) return false;
                if (krajX != 9 && y != 0 && tabela[y - 1, krajX + 1].Item2 != ime && tabela[y - 1, krajX + 1].Item1 != 0) return false;
                if (x != 0 && y != 9 && tabela[y + 1, x - 1].Item2 != ime && tabela[y + 1, x - 1].Item1 != 0) return false;
                if (krajX != 9 && y != 9 && tabela[y + 1, krajX + 1].Item2 != ime && tabela[y + 1, krajX + 1].Item1 != 0) return false;
                return true;

            }
            else
            {
                int krajY = y + duzina - 1;
                if (krajY > 9) return false;
                for (int i = y; i <= krajY; i++)
                {
                    if (tabela[i, x].Item2 != ime && tabela[i, x].Item1 != 0) return false;
                }
                if (x != 0)
                {
                    for (int i = y; i <= krajY; i++)
                    {
                        if (tabela[i, x - 1].Item2 != ime && tabela[i, x - 1].Item1 != 0) return false;
                    }
                }
                if (x != 9)
                {
                    for (int i = y; i <= krajY; i++)
                    {
                        if (tabela[i, x + 1].Item2 != ime && tabela[i, x + 1].Item1 != 0) return false;
                    }
                }
                if (y != 0 && tabela[y - 1, x].Item2 != ime && tabela[y - 1, x].Item1 != 0) return false;
                if (krajY != 9 && tabela[krajY + 1, x].Item2 != ime && tabela[krajY + 1, x].Item1 != 0) return false;
                if (y != 0 && x != 0 && tabela[y - 1, x - 1].Item2 != ime && tabela[y - 1, x - 1].Item1 != 0) return false;
                if (krajY != 9 && x != 0 && tabela[krajY + 1, x - 1].Item2 != ime && tabela[krajY + 1, x - 1].Item1 != 0) return false;
                if (y != 0 && x != 9 && tabela[y - 1, x + 1].Item2 != ime && tabela[y - 1, x + 1].Item1 != 0) return false;
                if (krajY != 9 && x != 9 && tabela[krajY + 1, x + 1].Item2 != ime && tabela[krajY + 1, x + 1].Item1 != 0) return false;
                return true;
            }
        }
        private void PogodjenCeoBrod((int, string)[,] tabla, string ime, bool horizontalno)
        {
            int x = 0, y = 0;
            bool nadjeno = false;
            for (int i = 0; i < 10 && !nadjeno; i++)
            {
                for (int j = 0; j < 10 && !nadjeno; j++)
                {
                    if (tabla[i, j].Item2 == ime)
                    {
                        x = j;
                        y = i;
                        nadjeno = true;
                    }
                }
            }
            int duzina = int.Parse(ime[0].ToString());
            if (horizontalno)
            {
                for (int k = 0; k < duzina; k++)
                {
                    if (tabla[y, x + k].Item1 != 3) nadjeno = false;
                }
                if (nadjeno)
                {
                    for (int k = 0; k < duzina; k++)
                    {
                        tabla[y, x + k].Item1 = 5;
                    }
                }
            }
            else
            {
                for (int k = 0; k < duzina; k++)
                {
                    if (tabla[y + k, x].Item1 != 3) nadjeno = false;
                }
                if (nadjeno)
                {
                    for (int k = 0; k < duzina; k++)
                    {
                        tabla[y + k, x].Item1 = 5;
                    }
                }
            }
        }
        private bool KrajIgre((int, string)[,] tabla)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (tabla[i, j].Item2 != null && tabla[i, j].Item1 != 5) return false;
                }
            }
            return true;
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
            lblIgrac1.Enabled = postoji;
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
            lblSledeci.Enabled = false;
            lblSledeci.Visible = false;
        }
        private void btnStartProg_Click(object sender, EventArgs e)
        {
            UnosPozicija(true);
            btnStartProg.Enabled = false;
            btnStartProg.Visible = false;
            this.BackgroundImage = null;
            namestanjeBrodova = true;
            UpisiPocetnePozicije();
        }
        private void IscrtajTablu(PictureBox tabla, PaintEventArgs e, (int, string)[,] igrac)
        {
            sirinaPolja = tabla.Width / 11;
            //e.Graphics.DrawString(tekst, new Font("Arial", 12),Brushes.Green, new Point(x, y));
            for (int i = 1; i < 11; i++)
            {
                e.Graphics.DrawString(Convert.ToString(i), new Font("Georgia", 12), Brushes.Blue, new Point(sirinaPolja * i + 5, 5));
                e.Graphics.DrawString(Convert.ToString((char)(64 + i)), new Font("Georgia", 12), Brushes.Blue, new Point(5, sirinaPolja * i + 5));
            }
            Pen olovka = new Pen(Color.Blue, (float)(tabla.Width * 0.01 + 1));
            for (int i = 0; i < 11; i++)
            {
                e.Graphics.DrawLine(olovka, (i + 1) * sirinaPolja, sirinaPolja, (i + 1) * sirinaPolja, sirinaPolja * 11);
                e.Graphics.DrawLine(olovka, sirinaPolja, (i + 1) * sirinaPolja, sirinaPolja * 11, (i + 1) * sirinaPolja);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    if (igrac[i - 1, j - 1].Item1 == 1) PogodakPrazno(tabla, e, j, i);
                    else if (igrac[i - 1, j - 1].Item1 == 3) PogodakBrod(tabla, e, j, i);
                    else if (igrac[i - 1, j - 1].Item1 == 5) PogodakCeoBrod(tabla, e, j, i);
                }
            }
        }
        private void PogodakPrazno(PictureBox tabla, PaintEventArgs e, int x, int y)
        {
            Pen olovka = new Pen(Color.Blue, (float)(tabla.Width * 0.005 + 1));
            for (int i = 1; i <= 5; i++)
            {
                e.Graphics.DrawLine(olovka, sirinaPolja * x + (int)Math.Round(sirinaPolja / 5.0 * i, 0), sirinaPolja * y, x * sirinaPolja, y * sirinaPolja + (int)Math.Round(sirinaPolja / 5.0 * i, 0));
                e.Graphics.DrawLine(olovka, sirinaPolja * x + (int)Math.Round(sirinaPolja / 5.0 * i, 0), sirinaPolja * (y + 1), (x + 1) * sirinaPolja, y * sirinaPolja + (int)Math.Round(sirinaPolja / 5.0 * i, 0));
            }
        }
        private void PogodakBrod(PictureBox tabla, PaintEventArgs e, int x, int y)
        {
            Pen olovka = new Pen(Color.Red, (float)(tabla.Width * 0.005 + 1));
            e.Graphics.DrawLine(olovka, sirinaPolja * x + 2, sirinaPolja * y + 2, (x + 1) * sirinaPolja - 2, (y + 1) * sirinaPolja - 2);
            e.Graphics.DrawLine(olovka, sirinaPolja * x + 2, sirinaPolja * (y + 1) - 2, (x + 1) * sirinaPolja - 2, y * sirinaPolja + 2);
        }
        private void PogodakCeoBrod(PictureBox tabla, PaintEventArgs e, int x, int y)
        {
            Brush cetkica = new SolidBrush(Color.Red);
            e.Graphics.FillRectangle(cetkica, sirinaPolja * x + 2, sirinaPolja * y + 2, sirinaPolja - 4, sirinaPolja - 4);
        }
        private void btnSpreman_Click(object sender, EventArgs e)
        {
            //if (SviPostavljeni())
            //{
                if (prviNaPotezu)
                {
                    lblIgrac1.Enabled = false;
                    lblIgrac1.Visible = false;
                    lblIgrac2.Enabled = true;
                    lblIgrac2.Visible = true;
                    pbxJa.Refresh();
                    for (int i = 0; i < 10; i++)
                    {
                        PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                        pozicijeBrodovaPrvog[i].Item1 = a.Left;
                        pozicijeBrodovaPrvog[i].Item2 = a.Top;
                    }
                    PostaviBrodoveNaPozicije(true);
                    prviNaPotezu = false;
                }

                else
                {
                    prviNaPotezu = true;
                    lblIgrac1.Enabled = true;
                    lblIgrac1.Visible = true;
                    lblIgrac2.Enabled = false;
                    lblIgrac2.Visible = false;
                    btnSpreman.Visible = false;
                    btnSpreman.Enabled = false;
                    btnRestartPozicije.Visible = false;
                    btnRestartPozicije.Enabled = false;
                    pbxProtivnik.Visible = true;
                    pbxProtivnik.Enabled = true;
                    pbxJa.Refresh();
                    pbxProtivnik.Refresh();
                    for (int i = 0; i < 10; i++)
                    {
                        PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                        pozicijeBrodovaDrugog[i].Item1 = a.Left;
                        pozicijeBrodovaDrugog[i].Item2 = a.Top;
                    }
                    PostaviBrodoveNaPozicije(false);
                    namestanjeBrodova = false;
                }
        //}
        //    else
        //    {
        //        MessageBox.Show("Neophodno je postaviti sve brodove na tablu");
        //    }
}
        private bool SviPostavljeni()
        {
            for (int i = 0; i < 10; i++)
            {
                PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                if (a.Left == pozicijeBrodovaZaPostavljanje[i].Item1) return false;
            }
            return true;
        }
        private void UpisiPocetnePozicije()
        {
            for (int i = 0; i < 10; i++)
            {
                PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                pozicijeBrodovaZaPostavljanje[i] = (a.Left, a.Top);
            }
        }
        private void PostaviBrodoveNaPozicije(bool pocetak)
        {
            if (pocetak)
            {
                for (int i = 0; i < 10; i++)
                {
                    PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                    a.Left = pozicijeBrodovaZaPostavljanje[i].Item1;
                    a.Top = pozicijeBrodovaZaPostavljanje[i].Item2;
                    if ((prviNaPotezu && !pozicijeBrodovaPrvog[i].Item3) || (!prviNaPotezu && !pozicijeBrodovaDrugog[i].Item3)) 
                    {
                        a.BackgroundImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
                        a.Refresh();
                        int b = a.Width;
                        a.Width = a.Height;
                        a.Height = b;
                    }
                }
            }
            else
            {
                if (prviNaPotezu)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                        a.Left = pozicijeBrodovaPrvog[i].Item1;
                        a.Top = pozicijeBrodovaPrvog[i].Item2;
                        if (pozicijeBrodovaDrugog[i].Item3 && !pozicijeBrodovaPrvog[i].Item3)
                        {
                            a.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                            a.Refresh();
                            int b = a.Width;
                            a.Width = a.Height;
                            a.Height = b;
                        }
                        else if(!pozicijeBrodovaDrugog[i].Item3 && pozicijeBrodovaPrvog[i].Item3)
                        {
                            a.BackgroundImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
                            a.Refresh();
                            int b = a.Width;
                            a.Width = a.Height;
                            a.Height = b;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                        a.Left = pozicijeBrodovaDrugog[i].Item1;
                        a.Top = pozicijeBrodovaDrugog[i].Item2;
                        if (pozicijeBrodovaDrugog[i].Item3 && !pozicijeBrodovaPrvog[i].Item3)
                        {
                            a.BackgroundImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
                            a.Refresh();
                            int b = a.Width;
                            a.Width = a.Height;
                            a.Height = b;
                        }
                        else if (!pozicijeBrodovaDrugog[i].Item3 && pozicijeBrodovaPrvog[i].Item3)
                        {
                            a.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                            a.Refresh();
                            int b = a.Width;
                            a.Width = a.Height;
                            a.Height = b;
                        }
                    }
                }
            }
        }
        private void KretanjeAviona(int x, int y)
        {
            pbxAvion.Top = sirinaPolja * (1 + y) + pbxProtivnik.Top + 2;
            pbxAvion.Left = pbxProtivnik.Left - pbxAvion.Width;
            pbxAvion.Visible = true;
            pbxAvion.Enabled = true;
            tajmerAviona.Start();
        }

        private void frmPotop_Load(object sender, EventArgs e)
        {
            VelicinaLokacijaSvega();
            UnosPozicija(false);
            for (int i = 0; i < 10; i++)
            {
                pozicijeBrodovaPrvog[i].Item3 = true;
                pozicijeBrodovaDrugog[i].Item3 = true;
            }
        }

        private void pbxJa_Paint(object sender, PaintEventArgs e)
        {
            IscrtajTablu(pbxJa, e, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void btnRestartPozicije_Click(object sender, EventArgs e)
        {
            PostaviBrodoveNaPozicije(true);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (prviNaPotezu) tablaPrvog[i, j] = (0, null);
                    else tablaDrugog[i, j] = (0, null);
                }
                if (prviNaPotezu) pozicijeBrodovaPrvog[i].Item3 = true;
                else pozicijeBrodovaDrugog[i].Item3 = true;
            }
        }

        private void btnIzlaz_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.Exit();
            Close();
        }

        private void frmPotop_SizeChanged(object sender, EventArgs e)
        {
            VelicinaLokacijaSvega();
            this.Refresh();
            UpisiPocetnePozicije();
        }

        private void tajmer_Tick(object sender, EventArgs e)
        {
            if (pbxAvion.Left < pbxProtivnik.Right)
            {
                pbxAvion.Left += sirinaPolja;
            }
            else
            {
                pbxAvion.Visible = false;
                pbxAvion.Enabled = false;
                tajmerAviona.Stop();
                pbxProtivnik.Refresh();
                if (sledeci) SledeciIgrac();
            }
        }

        private void pbxProtivnik_Paint(object sender, PaintEventArgs e)
        {
            IscrtajTablu(pbxProtivnik, e, prviNaPotezu ? tablaDrugog : tablaPrvog);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }
        private void VelicinaLokacijaSvega()
        {
            //jaTabla
            pbxJa.Left = (int)(0.01 * this.Width);
            pbxJa.Top = (int)(0.1 * this.Height);
            pbxJa.Width = (int)(0.73 * this.Height);
            pbxJa.Height = (int)(0.73 * this.Height);
            //protivnikTabla
            pbxProtivnik.Left = (int)(0.51 * this.Width);
            pbxProtivnik.Top = (int)(0.1 * this.Height);
            pbxProtivnik.Width = (int)(0.73 * this.Height);
            pbxProtivnik.Height = (int)(0.73 * this.Height);
            sirinaPolja = pbxJa.Width / 11;
            //3a
            pbx3a.Left = (int)(0.67 * this.Width);
            pbx3a.Top = (int)(0.14 * this.Height);
            pbx3a.Width = (int)(3 * sirinaPolja - 4);
            pbx3a.Height = (int)(sirinaPolja - 4);
            //3b
            pbx3b.Left = (int)(0.82 * this.Width);
            pbx3b.Top = (int)(0.14 * this.Height);
            pbx3b.Width = (int)(3 * sirinaPolja - 4);
            pbx3b.Height = (int)(sirinaPolja - 4);
            //1c
            pbx1c.Left = (int)(0.67 * this.Width);
            pbx1c.Top = (int)(0.31 * this.Height);
            pbx1c.Width = (int)(sirinaPolja - 4);
            pbx1c.Height = (int)(sirinaPolja - 4);
            //1d
            pbx1d.Left = (int)(0.74 * this.Width);
            pbx1d.Top = (int)(0.31 * this.Height);
            pbx1d.Width = (int)(sirinaPolja - 4);
            pbx1d.Height = (int)(sirinaPolja - 4);
            //2a
            pbx2a.Left = (int)(0.83 * this.Width);
            pbx2a.Top = (int)(0.31 * this.Height);
            pbx2a.Width = (int)(2 * sirinaPolja - 4);
            pbx2a.Height = (int)(sirinaPolja - 4);
            //4a
            pbx4a.Left = (int)(0.64 * this.Width);
            pbx4a.Top = (int)(0.47 * this.Height);
            pbx4a.Width = (int)(4 * sirinaPolja - 4);
            pbx4a.Height = (int)(sirinaPolja - 4);
            //2b
            pbx2b.Left = (int)(0.83 * this.Width);
            pbx2b.Top = (int)(0.47 * this.Height);
            pbx2b.Width = (int)(2 * sirinaPolja - 4);
            pbx2b.Height = (int)(sirinaPolja - 4);
            //1a
            pbx1a.Left = (int)(0.64 * this.Width);
            pbx1a.Top = (int)(0.65 * this.Height);
            pbx1a.Width = (int)(sirinaPolja - 4);
            pbx1a.Height = (int)(sirinaPolja - 4);
            //1b
            pbx1b.Left = (int)(0.74 * this.Width);
            pbx1b.Top = (int)(0.65 * this.Height);
            pbx1b.Width = (int)(sirinaPolja - 4);
            pbx1b.Height = (int)(sirinaPolja - 4);
            //2c
            pbx2c.Left = (int)(0.83 * this.Width);
            pbx2c.Top = (int)(0.65 * this.Height);
            pbx2c.Width = (int)(2 * sirinaPolja - 4);
            pbx2c.Height = (int)(sirinaPolja - 4);
            //btnPomoc
            btnHelp.Left = (int)(0.01 * this.Width);
            btnHelp.Top = (int)(0.85 * this.Height);
            btnHelp.Width = (int)(0.12 * this.Width);
            btnHelp.Height = (int)(0.063 * this.Height);
            //btnSpreman
            btnSpreman.Left = (int)(0.29 * this.Width);
            btnSpreman.Top = (int)(0.85 * this.Height);
            btnSpreman.Width = (int)(0.12 * this.Width);
            btnSpreman.Height = (int)(0.063 * this.Height);
            //btnObrisi
            btnRestartPozicije.Left = (int)(0.51 * this.Width);
            btnRestartPozicije.Top = (int)(0.85 * this.Height);
            btnRestartPozicije.Width = (int)(0.12 * this.Width);
            btnRestartPozicije.Height = (int)(0.063 * this.Height);
            //lblIgrac1
            lblIgrac1.Left = (int)(0.44 * this.Width);
            lblIgrac1.Top = (int)(0.02 * this.Height);
            //lblIgrac2
            lblIgrac2.Left = (int)(0.44 * this.Width);
            lblIgrac2.Top = (int)(0.02 * this.Height);
            //lblSledeci
            lblSledeci.Left = (int)(this.Width / 2 - lblSledeci.Width / 2);
            lblSledeci.Top = (int)(this.Height / 2 - lblSledeci.Height / 2);
            //btnStart
            btnStartProg.Left = (int)(0.41 * this.Width);
            btnStartProg.Top = (int)(0.39 * this.Height);
            btnStartProg.Width = (int)(0.13 * this.Width);
            btnStartProg.Height = (int)(0.1 * this.Height);
            //btnOpet
            btnIgrajOpet.Left = (int)(0.37 * this.Width);
            btnIgrajOpet.Top = (int)(0.39 * this.Height);
            btnIgrajOpet.Width = (int)(0.11 * this.Width);
            btnIgrajOpet.Height = (int)(0.05 * this.Height);
            //btnIzlaz
            btnIzlaz.Left = (int)(0.51 * this.Width);
            btnIzlaz.Top = (int)(0.39 * this.Height);
            btnIzlaz.Width = (int)(0.11 * this.Width);
            btnIzlaz.Height = (int)(0.05 * this.Height);
            //avion
            pbxAvion.Top = sirinaPolja * 5 + pbxProtivnik.Top;
            pbxAvion.Left = pbxProtivnik.Left - pbxAvion.Width;
            pbxAvion.Width = (int)(1.2 * sirinaPolja);
            pbxAvion.Height = (int)(sirinaPolja - 4);
        }
        private void PostaviBrod(PictureBox brod, int broj)
        {
            pomeranjeBroda = false;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (prviNaPotezu && tablaPrvog[i, j].Item2 == naziviBrodova[broj]) tablaPrvog[i, j] = (0, null);
                    else if (!prviNaPotezu && tablaDrugog[i, j].Item2 == naziviBrodova[broj]) tablaDrugog[i, j] = (0, null);
                }
            }
            if (brod.Right <= pbxJa.Left + sirinaPolja || brod.Left >= pbxJa.Right || brod.Top >= pbxJa.Bottom || brod.Bottom <= pbxJa.Top + sirinaPolja)
            {
                brod.Left = pozicijeBrodovaZaPostavljanje[broj].Item1;
                brod.Top = pozicijeBrodovaZaPostavljanje[broj].Item2;
                if ((prviNaPotezu && !pozicijeBrodovaPrvog[broj].Item3) || (!prviNaPotezu && !pozicijeBrodovaDrugog[broj].Item3)) 
                {
                    brod.BackgroundImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
                    brod.Refresh();
                    int a = brod.Width;
                    brod.Width = brod.Height;
                    brod.Height = a;
                    if (prviNaPotezu) pozicijeBrodovaPrvog[broj].Item3 = true;
                    else pozicijeBrodovaDrugog[broj].Item3 = true;
                }
                return;
            }
            int x = 9, y = 9;
            for (int i = 1; i <= 9; i++)
            {
                if (brod.Top <= pbxJa.Top + sirinaPolja * i + sirinaPolja / 2)
                {
                    y = i - 1;
                    break;
                }
            }
            for (int i = 1; i <= 9; i++)
            {
                if (brod.Left <= pbxJa.Left + sirinaPolja * i + sirinaPolja / 2)
                {
                    x = i - 1;
                    break;
                }
            }
            int duzina = int.Parse(naziviBrodova[broj][0].ToString());
            if (MozeDaSePostaviBrod(x, y, duzina, prviNaPotezu ? pozicijeBrodovaPrvog[broj].Item3 : pozicijeBrodovaDrugog[broj].Item3, prviNaPotezu ? tablaPrvog : tablaDrugog, naziviBrodova[broj])) 
            {
                brod.Left = pbxJa.Left + (x + 1) * sirinaPolja + 2;
                brod.Top = pbxJa.Top + (y + 1) * sirinaPolja + 2;
                if (prviNaPotezu)
                {
                    if (pozicijeBrodovaPrvog[broj].Item3)
                    {
                        for (int i = 0; i < duzina; i++) tablaPrvog[y, x + i] = (2, naziviBrodova[broj]);
                    }
                    else for (int i = 0; i < duzina; i++) tablaPrvog[y + i, x] = (2, naziviBrodova[broj]);
                }
                else
                {
                    if (pozicijeBrodovaDrugog[broj].Item3)
                    {
                        for (int i = 0; i < duzina; i++) tablaDrugog[y, x + i] = (2, naziviBrodova[broj]);
                    }
                    else for (int i = 0; i < duzina; i++) tablaDrugog[y + i, x] = (2, naziviBrodova[broj]);
                }
            }
            else
            {
                brod.Left = pozicijeBrodovaZaPostavljanje[broj].Item1;
                brod.Top = pozicijeBrodovaZaPostavljanje[broj].Item2;
                if ((prviNaPotezu && !pozicijeBrodovaPrvog[broj].Item3) || (!prviNaPotezu && !pozicijeBrodovaDrugog[broj].Item3))
                {
                    brod.BackgroundImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
                    brod.Refresh();
                    int a = brod.Width;
                    brod.Width = pbx4a.Height;
                    brod.Height = a;
                    if (prviNaPotezu) pozicijeBrodovaPrvog[broj].Item3 = true;
                    else pozicijeBrodovaDrugog[broj].Item3 = true;
                }
            }
        }
        private void PomeriMis(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (pomeranjeBroda && c != null)
            {
                c.Top = e.Y + c.Top - pozY;
                c.Left = e.X + c.Left - pozX;
            }
        }
        private void SpustiMis(MouseEventArgs e)
        {
            pomeranjeBroda = true;
            pozX = e.X;
            pozY = e.Y;
        }
        private void RotirajBrod(PictureBox brod, int broj)
        {
            if (brod.Left != pozicijeBrodovaZaPostavljanje[broj].Item1)
            {
                int x = (brod.Left - pbxJa.Left) / sirinaPolja - 1;
                int y = (brod.Top - pbxJa.Top) / sirinaPolja - 1;
                if (MozeDaSePostaviBrod(x, y, int.Parse(naziviBrodova[broj][0].ToString()), prviNaPotezu ? !pozicijeBrodovaPrvog[broj].Item3 : !pozicijeBrodovaDrugog[broj].Item3, prviNaPotezu ? tablaPrvog : tablaDrugog, naziviBrodova[broj]))
                {
                    if ((prviNaPotezu && pozicijeBrodovaPrvog[broj].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[broj].Item3)) brod.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                    else brod.BackgroundImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
                    brod.Refresh();
                    int a = brod.Width;
                    brod.Width = brod.Height;
                    brod.Height = a;
                    if (prviNaPotezu) pozicijeBrodovaPrvog[broj].Item3 = !pozicijeBrodovaPrvog[broj].Item3;
                    else pozicijeBrodovaDrugog[broj].Item3 = !pozicijeBrodovaDrugog[broj].Item3;
                }
            }
        }
        private void pbx4a_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }
        private void pbx4a_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx4a, 9);
        }
        
        private void pbx4a_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx3a_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx3a_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx3a_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx3a, 7);
        }

        private void pbx3b_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx3b_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx3b_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx3b, 8);
        }

        private void pbx2a_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx2a_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx2a_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx2a, 4);
        }

        private void pbx2b_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx2b_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx2b_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx2b, 5);
        }

        private void pbx2c_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx2c_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx2c_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx2c, 6);
        }

        private void pbx1a_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx1a_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx1a_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx1a, 0);
        }

        private void pbx1b_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx1b_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx1b_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx1b, 1);
        }

        private void pbx1c_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx1c_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx1c_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx1c, 2);
        }

        private void pbx1d_MouseDown(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) SpustiMis(e);
        }

        private void pbx1d_MouseMove(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PomeriMis(sender, e);
        }

        private void pbx1d_MouseUp(object sender, MouseEventArgs e)
        {
            if (namestanjeBrodova) PostaviBrod(pbx1d, 3);
        }

        private void pbx4a_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx4a, 9);
        }

        private void pbx3a_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx3a, 7);
        }

        private void pbx3b_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx3b, 8);
        }

        private void pbx2a_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx2a, 4);
        }

        private void pbx2b_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx2b, 5);
        }

        private void pbx2c_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx2c, 6);
        }

        private void pbx1a_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx1a, 0);
        }

        private void pbx1b_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx1b, 1);
        }

        private void pbx1c_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx1c, 2);
        }

        private void pbx1d_DoubleClick(object sender, EventArgs e)
        {
            if (namestanjeBrodova) RotirajBrod(pbx1d, 3);
        }

        private void SledeciIgrac()
        {
            Stopwatch tajmer = new Stopwatch();
            tajmer.Start();
            while (tajmer.Elapsed.Seconds < 1) ;
            prviNaPotezu = !prviNaPotezu;
            lblIgrac1.Enabled = prviNaPotezu;
            lblIgrac1.Visible = prviNaPotezu;
            lblIgrac2.Enabled = !prviNaPotezu;
            lblIgrac2.Visible = !prviNaPotezu;
            pbxJa.Visible = false;
            pbxJa.Enabled = false;
            pbxProtivnik.Enabled = false;
            pbxProtivnik.Visible = false;
            btnHelp.Enabled = false;
            btnHelp.Visible = false;
            lblSledeci.Enabled = true;
            lblSledeci.Visible = true;
            for (int i = 0; i < 10; i++)
            {
                PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                a.Visible = false;
                a.Enabled = false;
            }
            while (tajmer.Elapsed.Seconds < 6) ;
            for (int i = 0; i < 10; i++)
            {
                PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                a.Visible = true;
                a.Enabled = true;
            }
            pbxJa.Visible = true;
            pbxJa.Enabled = true;
            pbxProtivnik.Enabled = true;
            pbxProtivnik.Visible = true;
            btnHelp.Enabled = true;
            btnHelp.Visible = true;
            lblSledeci.Enabled = false;
            lblSledeci.Visible = false;
            pbxJa.Refresh();
            pbxProtivnik.Refresh();
            PostaviBrodoveNaPozicije(false);
            tajmer.Stop();
        }
        private void pbxProtivnik_MouseClick(object sender, MouseEventArgs e)
        {
            sledeci = true;
            int x = e.X / sirinaPolja - 1;
            int y = e.Y / sirinaPolja - 1;
            if (!prviNaPotezu)
            {
                if (tablaPrvog[y, x].Item1 % 2 == 0) tablaPrvog[y, x].Item1++;
                if (tablaPrvog[y, x].Item1 == 3)
                {
                    int broj = tablaPrvog[y, x].Item2[1] - 96;
                    switch (int.Parse(tablaPrvog[y, x].Item2[0].ToString()))
                    {
                        case 2: broj += 3; break;
                        case 3: broj += 6; break;
                        case 4: broj += 8; break;
                        default: break;
                    }
                    PogodjenCeoBrod(tablaPrvog, tablaPrvog[y, x].Item2, pozicijeBrodovaPrvog[broj].Item3);
                    sledeci = false;
                }
            }
            else
            {
                if (tablaDrugog[y, x].Item1 % 2 == 0) tablaDrugog[y, x].Item1++;
                if (tablaDrugog[y, x].Item1 == 3)
                {
                    int broj = tablaDrugog[y, x].Item2[1] - 96;
                    switch(int.Parse(tablaDrugog[y, x].Item2[0].ToString()))
                    {
                        case 2: broj += 3; break;
                        case 3: broj += 6; break;
                        case 4: broj += 8; break;
                        default: break;
                    }
                    PogodjenCeoBrod(tablaDrugog, tablaDrugog[y, x].Item2, pozicijeBrodovaDrugog[broj].Item3);
                    sledeci = false;
                }
            }
            if (KrajIgre(prviNaPotezu ? tablaDrugog : tablaPrvog))
            {
                lblIspisPobednik.Visible = true;
                lblIspisPobednik.Enabled = true;
                lblIspisPobednik.Text = prviNaPotezu ? "Prvi igrac je pobedio" : "Drugi igrac je pobedio";
                lblIspisPobednik.ForeColor = Color.Black;
                lblIspisPobednik.Left = (int)(this.Width / 2 - lblIspisPobednik.Width / 2);
                lblIspisPobednik.Top = (int)(0.31 * this.Height);
                btnIzlaz.Enabled = true;
                btnIzlaz.Visible = true;
                btnIgrajOpet.Enabled = true;
                btnIgrajOpet.Visible = true;
                sledeci = false;
            }
            KretanjeAviona(x, y);
        }

        private void btnIgrajOpet_Click(object sender, EventArgs e)
        {
            namestanjeBrodova = true;
            prviNaPotezu = true;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tablaPrvog[i, j] = (0, null);
                    tablaDrugog[i, j] = (0, null);
                }
                pozicijeBrodovaPrvog[i] = (0, 0, true);
                pozicijeBrodovaDrugog[i] = (0, 0, true);
            }
            pbxJa.Refresh();
            pbxProtivnik.Enabled = false;
            pbxProtivnik.Visible = false;
            PostaviBrodoveNaPozicije(true);
            btnSpreman.Enabled = true;
            btnSpreman.Visible = true;
            btnRestartPozicije.Enabled = true;
            btnRestartPozicije.Visible = true;
            lblIgrac1.Enabled = true;
            lblIgrac1.Visible = true;
            lblIgrac2.Enabled = false;
            lblIgrac2.Visible = false;
            btnIzlaz.Enabled = false;
            btnIzlaz.Visible = false;
            btnIgrajOpet.Enabled = false;
            btnIgrajOpet.Visible = false;
            lblIspisPobednik.Visible = false;
            lblIspisPobednik.Enabled = false;
        }
    }
}
