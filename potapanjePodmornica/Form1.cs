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
        bool pomeranjeBroda = false;//pomeranje brodova misem
        bool namestanjeBrodova = false;//unos brodova oba igraca
        bool sledeci;//ako je brod pogodjen, trenutni igrac ostaje na potezu
        bool pocetak = true;//flag za resize koji se desava prilikom loadovanja forme
        int brSek;//prelaz sa jednog na drugog igraca
        int pozX;
        int pozY;
        public frmPotop()
        {
            InitializeComponent();
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
                //provera da li su prazna mesta do krajeva broda i dijagonalno od krajeva broda
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
                //provera da li su prazna mesta do krajeva broda i dijagonalno od krajeva broda
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
            bool nadjeno = false;//da li na polju ima pogodjen brod ili ne
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
                    if (y != 0)
                    {
                        for (int k = 0; k < duzina; k++)
                        {
                            tabla[y - 1, x + k].Item1 = 1;
                        }
                    }
                    if (y != 9)
                    {
                        for (int k = 0; k < duzina; k++)
                        {
                            tabla[y + 1, x + k].Item1 = 1;
                        }
                    }
                    if (x != 0) tabla[y, x - 1].Item1 = 1;
                    if (x + duzina <= 9) tabla[y, x + duzina].Item1 = 1;
                    if (x != 0 && y != 0) tabla[y - 1, x - 1].Item1 = 1;
                    if (x + duzina <= 9 && y != 0) tabla[y - 1, x + duzina].Item1 = 1;
                    if (x != 0 && y != 9) tabla[y + 1, x - 1].Item1 = 1;
                    if (x + duzina <= 9 && y != 9) tabla[y + 1, x + duzina].Item1 = 1;
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
                    if (x != 0)
                    {
                        for (int k = 0; k < duzina; k++)
                        {
                            tabla[y + k, x - 1].Item1 = 1;
                        }
                    }
                    if (x != 9)
                    {
                        for (int k = 0; k < duzina; k++)
                        {
                            tabla[y + k, x + 1].Item1 = 1;
                        }
                    }
                    if (y != 0) tabla[y - 1, x].Item1 = 1;
                    if (y + duzina <= 9) tabla[y + duzina, x].Item1 = 1;
                    if (y != 0 && x != 0) tabla[y - 1, x - 1].Item1 = 1;
                    if (y + duzina <= 9 && x != 0) tabla[y + duzina, x - 1].Item1 = 1;
                    if (y != 0 && x != 9) tabla[y - 1, x + 1].Item1 = 1;
                    if (y + duzina <= 9 && x != 9) tabla[y + duzina, x + 1].Item1 = 1;
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
        }
        private void IscrtajTablu(PictureBox tabla, PaintEventArgs e, (int, string)[,] igrac)
        {
            sirinaPolja = tabla.Width / 11;
            Font fontKoordinata = new Font("Georgia", (int)(sirinaPolja*0.4));
            for (int i = 1; i < 11; i++)
            {
                e.Graphics.DrawString(Convert.ToString(i), fontKoordinata, Brushes.Blue, new Point(sirinaPolja * i + (int)(sirinaPolja * 0.2), 5));
                e.Graphics.DrawString(Convert.ToString((char)(64 + i)), fontKoordinata, Brushes.Blue, new Point(5, sirinaPolja * i+(int)(sirinaPolja*0.25)));
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
                    else if (igrac[i - 1, j - 1].Item1 == 3) PogodakBrod(tabla, e, j, i, 0);
                    else if (igrac[i - 1, j - 1].Item1 == 5) PogodakCeoBrod(e, j, i, 2);
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
        private void PogodakBrod(PictureBox tabla, PaintEventArgs e, int x, int y, int smanjenje)
        {
            Pen olovka = new Pen(Color.Red, (float)(tabla.Width * 0.005 + 1));
            e.Graphics.DrawLine(olovka, sirinaPolja * x + (smanjenje + 1) * 2, sirinaPolja * y + (smanjenje + 1) * 2, (x + 1) * sirinaPolja + (smanjenje - 1) * 2, (y + 1) * sirinaPolja + (smanjenje - 1) * 2);
            e.Graphics.DrawLine(olovka, sirinaPolja * x + (smanjenje + 1) * 2, sirinaPolja * (y + 1) + (smanjenje - 1) * 2, (x + 1) * sirinaPolja + (smanjenje - 1) * 2, y * sirinaPolja + (smanjenje + 1) * 2);
        }
        private void PogodakCeoBrod(PaintEventArgs e, int x, int y, int smanjenje)
        {
            Brush cetkica = new SolidBrush(Color.Red);
            e.Graphics.FillRectangle(cetkica, sirinaPolja * x + smanjenje, sirinaPolja * y + smanjenje, sirinaPolja - 4, sirinaPolja - 4);
        }
        private void btnSpreman_Click(object sender, EventArgs e)
        {
            if (SviPostavljeni())
            {
                if (prviNaPotezu)
                {
                    lblIgrac1.Enabled = false;
                    lblIgrac1.Visible = false;
                    lblIgrac2.Enabled = true;
                    lblIgrac2.Visible = true;
                    PostaviBrodoveNaPozicije(true);
                    prviNaPotezu = false;
                }

                else
                {
                    btnSpreman.Visible = false;
                    btnSpreman.Enabled = false;
                    btnRestartPozicije.Visible = false;
                    btnRestartPozicije.Enabled = false;
                    SledeciIgrac(true);
                    PostaviBrodoveNaPozicije(false);
                    namestanjeBrodova = false;
                }
            }
            else
            {
                MessageBox.Show("Neophodno je postaviti sve brodove na tablu");
            }
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
                        (a.Height, a.Width) = (a.Width, a.Height);
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
                            (a.Height, a.Width) = (a.Width, a.Height);
                        }
                        else if(!pozicijeBrodovaDrugog[i].Item3 && pozicijeBrodovaPrvog[i].Item3)
                        {
                            a.BackgroundImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
                            a.Refresh();
                            (a.Height, a.Width) = (a.Width, a.Height);
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
                            (a.Height, a.Width) = (a.Width, a.Height);
                        }
                        else if (!pozicijeBrodovaDrugog[i].Item3 && pozicijeBrodovaPrvog[i].Item3)
                        {
                            a.BackgroundImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                            a.Refresh();
                            (a.Height, a.Width) = (a.Width, a.Height);
                        }
                    }
                }
            }
        }
        private void KretanjeAviona(int y)
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
                if (prviNaPotezu) pozicijeBrodovaPrvog[i] = (0, 0, true);
                else pozicijeBrodovaDrugog[i] = (0, 0, true);
            }
        }

        private void btnIzlaz_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.Exit();
            this.Close();
        }

        private void frmPotop_SizeChanged(object sender, EventArgs e)
        {
            if (pocetak)
            {
                for (int i = 0; i < 10; i++)
                {
                    pozicijeBrodovaDrugog[i].Item3 = true;
                    pozicijeBrodovaPrvog[i].Item3 = true;
                }
                pocetak = false;
            }
            VelicinaLokacijaSvega();
            this.Refresh();
            //UpisiPocetnePozicije();
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
                if (sledeci) SledeciIgrac(false);
            }
        }

        private void pbxProtivnik_Paint(object sender, PaintEventArgs e)
        {
            IscrtajTablu(pbxProtivnik, e, prviNaPotezu ? tablaDrugog : tablaPrvog);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Igra se na tabli veličine 10x10. Kolone su označene slovima abecede A-J, dok su vrste označene brojevima 1-10. Pre nego što počne igra, svaki igrač postavi brodove na svojoj tabli. Postoje četiri broda duzine 1, tri broda duzine 2, dva broda duzine 3 i jedan brod duzine 4. Brodovi se postavljaju tako sto se mišem prevuku na tablu i tako da se nijedna dva ne dodiruju ni dijagonalno. Svaki od njih može biti postavljen horizontalno ili vertikalno. Brodovi se rotiraju tako što se postave na tablu i onda dvokliknu i tada se rotiraju tako da im gornji/levi kraj ostane na istom mestu. Igra kreće kada oba igrača završe sa postavljanjem svojih brodova. Svaki igrač, kada je njegov red, gađa jedno od protivničkih polja tako što klikne na jedno od polja na desnoj tabli. Ako ne pogodi ništa to polje će biti šrafirano plavo, a u suprotnom na njemu će se ispisati crveno X i gađaće opet. U slučaju da je igrač pogodio ceo brod polja na kojima se taj brod nalazio će cela biti obojena u crveno. Igrač na levoj table može da vidi svoje brodove i sva polja koja je protivnik gađao, označena istim simbolima. Igra se završava kada jedan igrač pogodi sve protivnikove brodove i on tada postaje pobednik.", "Pravila");
        }
        private void VelicinaLokacijaSvega()
        {
            int stariX = pbxJa.Left, stariY = pbxJa.Top, staraSirina = sirinaPolja;
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
            pozicijeBrodovaZaPostavljanje[7] = ((int)(0.67 * this.Width), (int)(0.14 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[7].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[7].Item3)) 
            {
                pbx3a.Width = (int)(3 * sirinaPolja - 4);
                pbx3a.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx3a.Width = (int)(sirinaPolja - 4);
                pbx3a.Height = (int)(3 * sirinaPolja - 4);
            }
            //3b
            pozicijeBrodovaZaPostavljanje[8] = ((int)(0.82 * this.Width), (int)(0.14 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[8].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[8].Item3))
            {
                pbx3b.Width = (int)(3 * sirinaPolja - 4);
                pbx3b.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx3b.Width = (int)(sirinaPolja - 4);
                pbx3b.Height = (int)(3 * sirinaPolja - 4);
            }
            //1c
            pozicijeBrodovaZaPostavljanje[2] = ((int)(0.67 * this.Width), (int)(0.31 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[2].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[2].Item3))
            {
                pbx1c.Width = (int)(sirinaPolja - 4);
                pbx1c.Height = (int)(sirinaPolja - 4);
            }
            else
            { 
                pbx1c.Width = (int)(sirinaPolja - 4);
                pbx1c.Height = (int)(sirinaPolja - 4);
            }
            //1d
            pozicijeBrodovaZaPostavljanje[3] = ((int)(0.74 * this.Width), (int)(0.31 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[3].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[3].Item3))
            {
                pbx1d.Width = (int)(sirinaPolja - 4);
                pbx1d.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx1d.Width = (int)(sirinaPolja - 4);
                pbx1d.Height = (int)(sirinaPolja - 4);
            }
            //2a
            pozicijeBrodovaZaPostavljanje[4] = ((int)(0.83 * this.Width), (int)(0.31 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[4].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[4].Item3))
            {
                pbx2a.Width = (int)(2 * sirinaPolja - 4);
                pbx2a.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx2a.Width = (int)(sirinaPolja - 4);
                pbx2a.Height = (int)(2 * sirinaPolja - 4);
            }
            //4a
            pozicijeBrodovaZaPostavljanje[9] = ((int)(0.64 * this.Width), (int)(0.47 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[9].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[9].Item3))
            {
                pbx4a.Width = (int)(4 * sirinaPolja - 4);
                pbx4a.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx4a.Width = (int)(sirinaPolja - 4);
                pbx4a.Height = (int)(4 * sirinaPolja - 4);
            }
            //2b
            pozicijeBrodovaZaPostavljanje[5] = ((int)(0.83 * this.Width), (int)(0.47 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[5].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[5].Item3))
            {
                pbx2b.Width = (int)(2 * sirinaPolja - 4);
                pbx2b.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx2b.Width = (int)(sirinaPolja - 4);
                pbx2b.Height = (int)(2 * sirinaPolja - 4);
            }
            //1a
            pozicijeBrodovaZaPostavljanje[0] = ((int)(0.64 * this.Width), (int)(0.65 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[0].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[0].Item3))
            {
                pbx1a.Width = (int)(sirinaPolja - 4);
                pbx1a.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx1a.Width = (int)(sirinaPolja - 4);
                pbx1a.Height = (int)(sirinaPolja - 4);
            }
            //1b
            pozicijeBrodovaZaPostavljanje[1] = ((int)(0.74 * this.Width), (int)(0.65 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[1].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[1].Item3))
            {
                pbx1b.Width = (int)(sirinaPolja - 4);
                pbx1b.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx1b.Width = (int)(sirinaPolja - 4);
                pbx1b.Height = (int)(sirinaPolja - 4);
            }
            //2c
            pozicijeBrodovaZaPostavljanje[6] = ((int)(0.83 * this.Width), (int)(0.65 * this.Height));
            if ((prviNaPotezu && pozicijeBrodovaPrvog[6].Item3) || (!prviNaPotezu && pozicijeBrodovaDrugog[6].Item3))
            {
                pbx2c.Width = (int)(2 * sirinaPolja - 4);
                pbx2c.Height = (int)(sirinaPolja - 4);
            }
            else
            {
                pbx2c.Width = (int)(sirinaPolja - 4);
                pbx2c.Height = (int)(2 * sirinaPolja - 4);
            }
            for (int i = 0; i < 10; i++)
            {
                PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                PromenaVelicineBroda(a, i, stariX, stariY, staraSirina);
            }
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
            lblIgrac1.Left = this.Width / 2 - lblIgrac1.Width / 2;
            lblIgrac1.Top = (int)(0.02 * this.Height);
            //lblIgrac2
            lblIgrac2.Left = this.Width / 2 - lblIgrac2.Width / 2;
            lblIgrac2.Top = (int)(0.02 * this.Height);
            //lblSledeci
            lblSledeci.Left = this.Width / 2 - lblSledeci.Width / 2;
            lblSledeci.Top = this.Height / 2 - lblSledeci.Height / 2;
            //lblPobednik
            lblIspisPobednik.Left = this.Width / 2 - lblIspisPobednik.Width / 2;
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
            pbxAvion.Height = sirinaPolja - 4;
        }
        private void PromenaVelicineBroda(PictureBox brod, int broj, int x, int y, int staraSirina)
        {
            int x1, y1;
            if (prviNaPotezu)
            {
                if (pozicijeBrodovaPrvog[broj].Item1 == 0)
                {
                    brod.Left = pozicijeBrodovaZaPostavljanje[broj].Item1;
                    brod.Top = pozicijeBrodovaZaPostavljanje[broj].Item2;
                }
                else
                {
                    x1 = (pozicijeBrodovaPrvog[broj].Item1 - x) / staraSirina;
                    y1 = (pozicijeBrodovaPrvog[broj].Item2 - y) / staraSirina;
                    pozicijeBrodovaPrvog[broj].Item1 = pbxJa.Left + x1 * sirinaPolja + 2;
                    pozicijeBrodovaPrvog[broj].Item2 = pbxJa.Top + y1 * sirinaPolja + 2;
                    brod.Left = pozicijeBrodovaPrvog[broj].Item1;
                    brod.Top = pozicijeBrodovaPrvog[broj].Item2;
                    if (pozicijeBrodovaDrugog[broj].Item1 != 0)
                    {
                        x1 = (pozicijeBrodovaDrugog[broj].Item1 - x) / staraSirina;
                        y1 = (pozicijeBrodovaDrugog[broj].Item2 - y) / staraSirina;
                        pozicijeBrodovaDrugog[broj].Item1 = pbxJa.Left + x1 * sirinaPolja + 2;
                        pozicijeBrodovaDrugog[broj].Item2 = pbxJa.Top + y1 * sirinaPolja + 2;
                    }
                }
            }
            else
            {
                if (pozicijeBrodovaDrugog[broj].Item1 == 0)
                {
                    brod.Left = pozicijeBrodovaZaPostavljanje[broj].Item1;
                    brod.Top = pozicijeBrodovaZaPostavljanje[broj].Item2;
                }
                else
                {
                    x1 = (pozicijeBrodovaDrugog[broj].Item1 - x) / staraSirina;
                    y1 = (pozicijeBrodovaDrugog[broj].Item2 - y) / staraSirina;
                    brod.Left = pbxJa.Left + x1 * sirinaPolja + 2;
                    brod.Top = pbxJa.Top + y1 * sirinaPolja + 2;
                    pozicijeBrodovaDrugog[broj].Item1 = brod.Left;
                    pozicijeBrodovaDrugog[broj].Item2 = brod.Top;
                }
                x1 = (pozicijeBrodovaPrvog[broj].Item1 - x) / staraSirina;
                y1 = (pozicijeBrodovaPrvog[broj].Item2 - y) / staraSirina;
                pozicijeBrodovaPrvog[broj].Item1 = pbxJa.Left + x1 * sirinaPolja + 2;
                pozicijeBrodovaPrvog[broj].Item2 = pbxJa.Top + y1 * sirinaPolja + 2;
            }
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
                    (brod.Height, brod.Width) = (brod.Width, brod.Height);
                    if (prviNaPotezu) pozicijeBrodovaPrvog[broj] = (0, 0, true);
                    else pozicijeBrodovaDrugog[broj] = (0, 0, true);
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
                    pozicijeBrodovaPrvog[broj].Item1 = brod.Left;
                    pozicijeBrodovaPrvog[broj].Item2 = brod.Top;
                }
                else
                {
                    if (pozicijeBrodovaDrugog[broj].Item3)
                    {
                        for (int i = 0; i < duzina; i++) tablaDrugog[y, x + i] = (2, naziviBrodova[broj]);
                    }
                    else for (int i = 0; i < duzina; i++) tablaDrugog[y + i, x] = (2, naziviBrodova[broj]);
                    pozicijeBrodovaDrugog[broj].Item1 = brod.Left;
                    pozicijeBrodovaDrugog[broj].Item2 = brod.Top;
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
                    (brod.Height, brod.Width) = (brod.Width, brod.Height);
                    if (prviNaPotezu) pozicijeBrodovaPrvog[broj] = (0, 0, true); 
                    else pozicijeBrodovaDrugog[broj] = (0, 0, true);
                }
            }
        }
        private void PomeriMis(object sender, MouseEventArgs e)
        {
            if (pomeranjeBroda && sender is Control c)
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
                    (brod.Height, brod.Width) = (brod.Width, brod.Height);
                    if (prviNaPotezu) pozicijeBrodovaPrvog[broj].Item3 = !pozicijeBrodovaPrvog[broj].Item3;
                    else pozicijeBrodovaDrugog[broj].Item3 = !pozicijeBrodovaDrugog[broj].Item3;
                }
            }
        }
        private void IscrtajNaBrodu(PaintEventArgs e, int broj, (int, string)[,] tabla)
        {
            int x = 0, y = 0;
            bool kraj = false;
            for (int i = 0; i < 10 && !kraj; i++)
            {
                for (int j = 0; j < 10 && !kraj; j++)
                {
                    if (tabla[i, j].Item2 == naziviBrodova[broj])
                    {
                        x = j;
                        y = i;
                        kraj = true;
                    }
                }
            }
            int duzina = int.Parse(naziviBrodova[broj][0].ToString());
            bool horizontalno;
            if (prviNaPotezu) horizontalno = pozicijeBrodovaPrvog[broj].Item3;
            else horizontalno = pozicijeBrodovaDrugog[broj].Item3;
            if (horizontalno)
            {
                for (int k = 0; k < duzina; k++)
                {
                    if (tabla[y, x + k].Item1 == 3) PogodakBrod(pbxJa, e, k, 0, - 1);
                    else if (tabla[y, x + k].Item1 == 5) PogodakCeoBrod(e, k, 0, 0);
                }
            }
            else
            {
                for (int k = 0; k < duzina; k++)
                {
                    if (tabla[y + k, x].Item1 == 3) PogodakBrod(pbxJa, e, 0, k, -1);
                    else if (tabla[y + k, x].Item1 == 5) PogodakCeoBrod(e, 0, k, 0);
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

        private void pbx4a_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 9, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx3a_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu( e, 7, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx3b_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 8, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx2c_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 6, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx2b_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 5, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx2a_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 4, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx1d_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 3, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx1c_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 2, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx1b_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 1, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void pbx1a_Paint(object sender, PaintEventArgs e)
        {
            if (!namestanjeBrodova) IscrtajNaBrodu(e, 0, prviNaPotezu ? tablaPrvog : tablaDrugog);
        }

        private void SledeciIgrac(bool pocinjeIgra)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            if(!pocinjeIgra) while (timer.Elapsed.Seconds < 1) ;
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
            if (pocinjeIgra) lblSledeci.Text = "Pocinje igra!";
            else lblSledeci.Text = "Sledeci igrac";
            lblSledeci.Enabled = true;
            lblSledeci.Visible = true;
            for (int i = 0; i < 10; i++)
            {
                PictureBox a = (PictureBox)this.Controls.Find("pbx" + naziviBrodova[i], true)[0];
                a.Visible = false;
                a.Enabled = false;
            }
            timer.Stop();
            brSek = 0;
            tajmer.Start();
            PostaviBrodoveNaPozicije(false);
        }
        private void pbxProtivnik_MouseClick(object sender, MouseEventArgs e)
        {
            sledeci = true;
            int x = e.X / sirinaPolja - 1;
            int y = e.Y / sirinaPolja - 1;
            if (!prviNaPotezu)
            {
                if (tablaPrvog[y, x].Item1 % 2 == 0)
                {
                    tablaPrvog[y, x].Item1++;
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
                else return;
            }
            else
            {
                if (tablaDrugog[y, x].Item1 % 2 == 0)
                {
                    tablaDrugog[y, x].Item1++;
                    if (tablaDrugog[y, x].Item1 == 3)
                    {
                        int broj = tablaDrugog[y, x].Item2[1] - 96;
                        switch (int.Parse(tablaDrugog[y, x].Item2[0].ToString()))
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
                else return;
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
            KretanjeAviona(y);
        }

        private void btnIgrajOpet_Click(object sender, EventArgs e)
        {
            namestanjeBrodova = true;
            PostaviBrodoveNaPozicije(true);
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
            UnosPozicija(true);
        }

        private void tajmer_Tick_1(object sender, EventArgs e)
        {
            brSek++;
            if (brSek >= 2)
            {
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
                tajmer.Stop();
            }
        }
    }
}
