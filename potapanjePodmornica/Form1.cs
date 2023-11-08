﻿using System;
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
        int sirinaPolja;
        bool prviNaPotezu = true;
        (int, string)[,] tablaPrvog = new (int, string)[10, 10];
        (int, string)[,] tablaDrugog = new (int, string)[10, 10];
        (int, int, string)[] pozicijeBrodovaZaPostavljanje = new (int, int, string)[10];
        (int, int, string)[] pozicijeBrodovaPrvog = new (int, int, string)[10];
        (int, int, string)[] pozicijeBrodovaDrugog = new (int, int, string)[10];
        bool pomeranjeBroda = false;
        int pozX;
        int pozY;
        public frmPotop()
        {
            InitializeComponent();/*
            Rectangle intersection = Rectangle.Intersect(pbxProtivnik.Bounds, pbxAvion.Bounds);
            Region region = new Region(intersection);
            pbxAvion.Region = region;*/
        }
        //0 - prazno, 1 - pogodjeno prazno, 2 - brod, 3 - pogodjen brod
        //pictureBox1.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
        //pictureBox1.Refresh();
        //imena brodova: 1a,1b,1c,1d,2a,2b,2c,3a,3b,4a
        private bool MozeDaSePostaviBrod(int x, int y, int duzina, bool horizontalno, (int, string)[,] tabela)
        {
            if (horizontalno)
            {
                int krajX = x + duzina - 1;
                if (krajX > 9) return false;
                for (int i = x; i <= krajX; i++)
                {
                    if (tabela[y, i].Item1 != 0) return false;
                }
                if (y != 0)
                {
                    for (int i = x; i <= krajX; i++)
                    {
                        if (tabela[y - 1, i].Item1 != 0) return false;
                    }
                }
                if (y != 9)
                {
                    for (int i = x; i <= krajX; i++)
                    {
                        if (tabela[y + 1, i].Item1 != 0) return false;
                    }
                }
                if (x != 0 && tabela[y, x - 1].Item1 != 0) return false;
                if (krajX != 9 && tabela[y, krajX + 1].Item1 != 0) return false;
                if (x != 0 && y != 0 && tabela[y - 1, x - 1].Item1 != 0) return false;
                if (krajX != 9 && y != 0 && tabela[y - 1, krajX + 1].Item1 != 0) return false;
                if (x != 0 && y != 9 && tabela[y + 1, x - 1].Item1 != 0) return false;
                if (krajX != 9 && y != 9 && tabela[y + 1, krajX + 1].Item1 != 0) return false;
                return true;

            }
            else
            {
                int krajY = y + duzina - 1;
                if (krajY > 9) return false;
                for (int i = y; i <= krajY; i++)
                {
                    if (tabela[i, x].Item1 != 0) return false;
                }
                if (x != 0)
                {
                    for (int i = y; i <= krajY; i++)
                    {
                        if (tabela[i, x - 1].Item1 != 0) return false;
                    }
                }
                if (x != 9)
                {
                    for (int i = y; i <= krajY; i++)
                    {
                        if (tabela[i, x + 1].Item1 != 0) return false;
                    }
                }
                if (y != 0 && tabela[y - 1, x].Item1 != 0) return false;
                if (krajY != 9 && tabela[krajY + 1, x].Item1 != 0) return false;
                if (y != 0 && x != 0 && tabela[y - 1, x - 1].Item1 != 0) return false;
                if (krajY != 9 && x != 0 && tabela[krajY + 1, x - 1].Item1 != 0) return false;
                if (y != 0 && x != 9 && tabela[y - 1, x + 1].Item1 != 0) return false;
                if (krajY != 9 && x != 9 && tabela[krajY + 1, x + 1].Item1 != 0) return false;
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
            UpisiPocetnePozicije();
        }
        private void IscrtajTablu(PictureBox tabla, PaintEventArgs e)
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
        private void btnSpreman_Click(object sender, EventArgs e)
        {
            if (SviPostavljeni())
            {
                if (prviNaPotezu)
                {
                    prviNaPotezu = false;
                    lblIgrac1.Enabled = false;
                    lblIgrac1.Visible = false;
                    lblIgrac2.Enabled = true;
                    lblIgrac2.Visible = true;
                    pbxJa.Refresh();
                    for (int i = 0; i < pozicijeBrodovaPrvog.Length; i++)
                    {
                        PictureBox a = (PictureBox)this.Controls.Find(pozicijeBrodovaPrvog[i].Item3, true)[0];
                        pozicijeBrodovaPrvog[i].Item1 = a.Left;
                        pozicijeBrodovaPrvog[i].Item2 = a.Top;
                    }
                    PostaviNaPocetnePozicije();
                }

                else
                {
                    prviNaPotezu = true;
                    lblIgrac1.Enabled = true;
                    lblIgrac1.Visible = true;
                    btnSpreman.Visible = false;
                    btnSpreman.Enabled = false;
                    btnRestartPozicije.Visible = false;
                    btnRestartPozicije.Enabled = false;
                    pbxProtivnik.Visible = true;
                    pbxProtivnik.Enabled = true;
                    pbxJa.Refresh();
                    pbxProtivnik.Refresh();
                    for (int i = 0; i < pozicijeBrodovaDrugog.Length; i++)
                    {
                        PictureBox a = (PictureBox)this.Controls.Find(pozicijeBrodovaDrugog[i].Item3, true)[0];
                        pozicijeBrodovaDrugog[i].Item1 = a.Left;
                        pozicijeBrodovaDrugog[i].Item2 = a.Top;
                    }
                    PostaviNaPozicijeMojeBrodove();
                    //KretanjeAviona(5, 2, false);
                }
            }
            else
            {
                MessageBox.Show("Neophodno je postaviti sve brodove na tablu");
            }
        }
        private bool SviPostavljeni()
        {
            if (pbx1a.Left == pozicijeBrodovaZaPostavljanje[0].Item1) return false;
            if (pbx1b.Left == pozicijeBrodovaZaPostavljanje[1].Item1) return false;
            if (pbx1c.Left == pozicijeBrodovaZaPostavljanje[2].Item1) return false;
            if (pbx1d.Left == pozicijeBrodovaZaPostavljanje[3].Item1) return false;
            if (pbx2a.Left == pozicijeBrodovaZaPostavljanje[4].Item1) return false;
            if (pbx2b.Left == pozicijeBrodovaZaPostavljanje[5].Item1) return false;
            if (pbx2c.Left == pozicijeBrodovaZaPostavljanje[6].Item1) return false;
            if (pbx3a.Left == pozicijeBrodovaZaPostavljanje[7].Item1) return false;
            if (pbx3b.Left == pozicijeBrodovaZaPostavljanje[8].Item1) return false;
            if (pbx4a.Left == pozicijeBrodovaZaPostavljanje[9].Item1) return false;
            return true;
        }
        private void UpisiPocetnePozicije()
        {
            int poslednjeSlovo = 100, brVrste = 1;
            for (int i = 0, k = 97; i < pozicijeBrodovaZaPostavljanje.Length && k <= poslednjeSlovo; i++, k++)
            {
                string imeBroda = brVrste + Convert.ToString(Convert.ToChar(k));
                PictureBox a = (PictureBox)this.Controls.Find("pbx" + imeBroda, true)[0];
                pozicijeBrodovaZaPostavljanje[i] = (a.Left, a.Top, "pbx" + imeBroda);
                pozicijeBrodovaPrvog[i].Item3 = "pbx" + imeBroda;
                pozicijeBrodovaDrugog[i].Item3 = "pbx" + imeBroda;
                if (k == poslednjeSlovo) { k = 96; poslednjeSlovo--; brVrste++; }
            }
        }
        private void PostaviNaPocetnePozicije()
        {
            for (int i = 0; i < pozicijeBrodovaZaPostavljanje.Length; i++)
            {
                PictureBox a = (PictureBox)this.Controls.Find(pozicijeBrodovaZaPostavljanje[i].Item3, true)[0];
                a.Left = pozicijeBrodovaZaPostavljanje[i].Item1;
                a.Top = pozicijeBrodovaZaPostavljanje[i].Item2;
            }
        }
        private void PostaviNaPozicijeMojeBrodove()
        {
            if (prviNaPotezu)
            {
                for (int i = 0; i < pozicijeBrodovaPrvog.Length; i++)
                {
                    PictureBox a = (PictureBox)this.Controls.Find(pozicijeBrodovaPrvog[i].Item3, true)[0];
                    a.Left = pozicijeBrodovaPrvog[i].Item1;
                    a.Top = pozicijeBrodovaPrvog[i].Item2;
                }
            }
            else
            {
                for (int i = 0; i < pozicijeBrodovaDrugog.Length; i++)
                {
                    PictureBox a = (PictureBox)this.Controls.Find(pozicijeBrodovaDrugog[i].Item3, true)[0];
                    a.Left = pozicijeBrodovaDrugog[i].Item1;
                    a.Top = pozicijeBrodovaDrugog[i].Item2;
                }
            }
        }
        private void KretanjeAviona(int x, int y, bool JelBrod)
        {
            pbxAvion.Top = sirinaPolja * (1 + y) + pbxProtivnik.Top;
            pbxAvion.Left = pbxProtivnik.Left - pbxAvion.Width;
            pbxAvion.Visible = true;
            pbxAvion.Enabled = true;
            tajmer.Start();
        }

        private void frmPotop_Load(object sender, EventArgs e)
        {
            VelicinaLokacijaSvega();
            UnosPozicija(false);
        }

        private void pbxJa_Paint(object sender, PaintEventArgs e)
        {
            IscrtajTablu(pbxJa, e);
            PogodakPrazno(pbxJa, e, 3, 2);
        }

        private void btnRestartPozicije_Click(object sender, EventArgs e)
        {
            PostaviNaPocetnePozicije();
        }

        private void btnIzlaz_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
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
                pbxAvion.Left += 10;
            }
            else
            {
                pbxAvion.Visible = false;
                pbxAvion.Enabled = false;
            }
        }

        private void pbxProtivnik_Paint(object sender, PaintEventArgs e)
        {
            IscrtajTablu(pbxProtivnik, e);
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
            pbx3a.Width = (int)(3 * sirinaPolja - 6);
            pbx3a.Height = (int)(sirinaPolja - 4);
            //3b
            pbx3b.Left = (int)(0.82 * this.Width);
            pbx3b.Top = (int)(0.14 * this.Height);
            pbx3b.Width = (int)(3 * sirinaPolja - 6);
            pbx3b.Height = (int)(sirinaPolja - 4);
            //1c
            pbx1c.Left = (int)(0.67 * this.Width);
            pbx1c.Top = (int)(0.31 * this.Height);
            pbx1c.Width = (int)(sirinaPolja - 6);
            pbx1c.Height = (int)(sirinaPolja - 4);
            //1d
            pbx1d.Left = (int)(0.74 * this.Width);
            pbx1d.Top = (int)(0.31 * this.Height);
            pbx1d.Width = (int)(sirinaPolja - 6);
            pbx1d.Height = (int)(sirinaPolja - 4);
            //2a
            pbx2a.Left = (int)(0.83 * this.Width);
            pbx2a.Top = (int)(0.31 * this.Height);
            pbx2a.Width = (int)(2 * sirinaPolja - 6);
            pbx2a.Height = (int)(sirinaPolja - 4);
            //4a
            pbx4a.Left = (int)(0.64 * this.Width);
            pbx4a.Top = (int)(0.47 * this.Height);
            pbx4a.Width = (int)(4 * sirinaPolja - 6);
            pbx4a.Height = (int)(sirinaPolja - 4);
            //2b
            pbx2b.Left = (int)(0.83 * this.Width);
            pbx2b.Top = (int)(0.47 * this.Height);
            pbx2b.Width = (int)(2 * sirinaPolja - 6);
            pbx2b.Height = (int)(sirinaPolja - 4);
            //1a
            pbx1a.Left = (int)(0.64 * this.Width);
            pbx1a.Top = (int)(0.65 * this.Height);
            pbx1a.Width = (int)(sirinaPolja - 6);
            pbx1a.Height = (int)(sirinaPolja - 4);
            //1b
            pbx1b.Left = (int)(0.74 * this.Width);
            pbx1b.Top = (int)(0.65 * this.Height);
            pbx1b.Width = (int)(sirinaPolja - 6);
            pbx1b.Height = (int)(sirinaPolja - 4);
            //2c
            pbx2c.Left = (int)(0.83 * this.Width);
            pbx2c.Top = (int)(0.65 * this.Height);
            pbx2c.Width = (int)(2 * sirinaPolja - 6);
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
            //lblPobednik
            lblIspisPobednik.Left = (int)(0.46 * this.Width);
            lblIspisPobednik.Top = (int)(0.31 * this.Height);
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
            pbxAvion.Height = (int)(sirinaPolja - 3);
        }

        private void pbx4a_MouseDown(object sender, MouseEventArgs e)
        {
            pomeranjeBroda = true;
            pozX = e.X;
            pozY = e.Y;
        }

        private void pbx4a_MouseUp(object sender, MouseEventArgs e)
        {
            pomeranjeBroda = false;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (prviNaPotezu && tablaPrvog[i, j].Item2 == "4a") tablaPrvog[i, j] = (0, null);
                    else if (tablaDrugog[i, j].Item2 == "4a") tablaDrugog[i, j] = (0, null);
                }
            }
            if (pbx4a.Right <= pbxJa.Left + sirinaPolja || pbx4a.Left >= pbxJa.Right || pbx4a.Top >= pbxJa.Bottom || pbx4a.Bottom <= pbxJa.Top + sirinaPolja) 
            {
                pbx4a.Left = pozicijeBrodovaZaPostavljanje[9].Item1;
                pbx4a.Top = pozicijeBrodovaZaPostavljanje[9].Item2;
            }
            int x = 9, y = 9;
            for (int i = 1; i <= 9; i++)
            {
                if (pbx4a.Top <= pbxJa.Top + sirinaPolja * i + sirinaPolja / 2)
                {
                    y = i - 1;
                    break;
                }
            }
            for (int i = 1; i <= 9; i++)
            {
                if (pbx4a.Left <= pbxJa.Left + sirinaPolja * i + sirinaPolja / 2) 
                {
                    x = i - 1;
                    break;
                }
            }
            if (MozeDaSePostaviBrod(x, y, 4, true, prviNaPotezu ? tablaPrvog : tablaDrugog))
            {
                pbx4a.Left = pbxJa.Left + (x + 1) * sirinaPolja + 3;
                pbx4a.Top = pbxJa.Top + (y + 1) * sirinaPolja + 2;
                for (int i = 0; i < 4; i++)
                {
                    if (prviNaPotezu) tablaPrvog[y, x + i] = (2, "4a");
                    else tablaDrugog[y, x + i] = (2, "4a");
                }
            }
            else
            {
                pbx4a.Left = pozicijeBrodovaZaPostavljanje[9].Item1;
                pbx4a.Top = pozicijeBrodovaZaPostavljanje[9].Item2;
            }
        }

        private void pbx4a_MouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (pomeranjeBroda && c != null)
            {
                c.Top = e.Y + c.Top - pozY;
                c.Left = e.X + c.Left - pozX;
            }
        }
    }
}
