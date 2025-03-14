using System; // Gerekli k�t�phaneleri ekliyoruz
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SansTopu.UI
{
    public partial class Form1 : Form
    {
        private Random rastgele = new Random(); // Rastgele say� �retici
        private int[] cekilisAna; // Ana �ekili� numaralar�
        private int cekilisEk; // Ek �ekili� numaras�

        public Form1()
        {
            InitializeComponent(); // Form bile�enlerini ba�lat
            ComboBoxlariBaslat(); // ComboBox'lar� ba�lat
        }

        private void ComboBoxlariBaslat()
        {
            ComboBox[] comboBoxlar = { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5 }; // ComboBox dizisi
            foreach (var cmb in comboBoxlar)
            {
                for (int i = 1; i <= 34; i++) // 1'den 34'e kadar say�lar� ekle
                {
                    cmb.Items.Add(i);
                }
                cmb.SelectedIndexChanged += ComboBox_SelectedIndexChanged; // Se�im de�i�ti�inde olay ekle
            }
            cmbSecim6.Items.Clear(); // Ek numara ComboBox'�n� temizle
            for (int i = 1; i <= 14; i++) // 1'den 14'e kadar say�lar� ekle
            {
                cmbSecim6.Items.Add(i);
            }
            cmbSecim6.SelectedIndexChanged += ComboBox_SelectedIndexChanged; // Se�im de�i�ti�inde olay ekle
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox secilenComboBox = sender as ComboBox; // Se�ilen ComboBox
            if (secilenComboBox == null) return;

            // �nce, daha �nce se�ilen say�y� geri ekleyelim
            foreach (var cmb in new ComboBox[] { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5, cmbSecim6 })
            {
                if (cmb != secilenComboBox && cmb.SelectedItem != null)
                {
                    int secilenDeger = (int)cmb.SelectedItem;
                    if (!secilenComboBox.Items.Contains(secilenDeger))
                    {
                        secilenComboBox.Items.Add(secilenDeger);
                    }
                }
            }

            // Yeni se�ilen say�y� di�er ComboBox'lardan kald�r
            if (secilenComboBox.SelectedItem != null)
            {
                int secilenNumara = (int)secilenComboBox.SelectedItem;
                foreach (var cmb in new ComboBox[] { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5, cmbSecim6 })
                {
                    if (cmb != secilenComboBox && cmb.Items.Contains(secilenNumara))
                    {
                        cmb.Items.Remove(secilenNumara);
                    }
                }
            }
        }

        private void btnCekilis_Click(object sender, EventArgs e)
        {
            // �ekili�i Ger�ekle�tir
            cekilisAna = RastgeleNumaraSec(34, 5); // 34 numara aras�ndan 5 tane rastgele se�
            cekilisEk = rastgele.Next(1, 15); // 1 ile 14 aras�nda rastgele bir ek numara se�

            // Kullan�c� Se�imlerini Al
            int[] kullaniciAna = new int[5]; // Kullan�c�n�n ana numaralar�
            ComboBox[] comboBoxlar = { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5 }; // ComboBox dizisi
            for (int i = 0; i < 5; i++)
            {
                if (comboBoxlar[i].SelectedItem == null)
                {
                    MessageBox.Show("L�tfen t�m numaralar� se�in!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyar� mesaj�
                    return;
                }
                kullaniciAna[i] = Convert.ToInt32(comboBoxlar[i].SelectedItem); // Se�ilen numaray� al
            }

            if (cmbSecim6.SelectedItem == null)
            {
                MessageBox.Show("L�tfen ek numaray� se�in!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyar� mesaj�
                return;
            }
            int kullaniciEk = Convert.ToInt32(cmbSecim6.SelectedItem); // Kullan�c�n�n ek numaras�

            // Sonu�lar� Kar��la�t�r
            int anaDogru = cekilisAna.Intersect(kullaniciAna).Count(); // Do�ru ana numaralar� say
            bool ekDogru = (cekilisEk == kullaniciEk); // Ek numara do�ru mu?

            // Kazanan say�lar� PictureBox'larda g�ster
            PictureBox[] pictureBoxlar = { pbTop1, pbTop2, pbTop3, pbTop4, pbTop5, pbTop6 }; // PictureBox dizisi
            for (int i = 0; i < 5; i++)
            {
                PictureBoxaNumaraCiz(pictureBoxlar[i], cekilisAna[i], Color.Magenta); // Ana numaralar� �iz
            }
            PictureBoxaNumaraCiz(pictureBoxlar[5], cekilisEk, Color.Blue); // Ek numaray� �iz

            // Kazan� Mesaj�n� Belirleme
            string sonucMesaji = "Sonu�: ";
            if (anaDogru == 5 && ekDogru) sonucMesaji += "5+1 bildiniz! 1.kategori ikramiye kazand�n�z.";
            else if (anaDogru == 5) sonucMesaji += "5 bildiniz! 2.kategori ikramiye kazand�n�z.";
            else if (anaDogru == 4 && ekDogru) sonucMesaji += "4+1 bildiniz! 3.kategori ikramiye kazand�n�z.";
            else if (anaDogru == 4) sonucMesaji += "4 bildiniz! 4.kategori ikramiye kazand�n�z.";
            else if (anaDogru == 3 && ekDogru) sonucMesaji += "3+1 bildiniz! 5.kategori ikramiye kazand�n�z.";
            else if (anaDogru == 3) sonucMesaji += "3 bildiniz! 6.kategori ikramiye kazand�n�z.";
            else if (anaDogru == 2 && ekDogru) sonucMesaji += "2+1 bildiniz! 7.kategori ikramiye kazand�n�z.";
            else if (anaDogru == 1 && ekDogru) sonucMesaji += "1+1 bildiniz! 8.kategori ikramiye kazand�n�z.";
            else if (anaDogru == 0 && ekDogru) sonucMesaji += "0+1 bildiniz! 9.kategori ikramiye kazand�n�z.";
            else sonucMesaji += "Maalesef ikramiye kazanamad�n�z, daha sonra tekrar deneyiniz.";

            lblSonuc.Text = sonucMesaji; // Sonu� mesaj�n� g�ster
            lblSonuc.ForeColor = Color.Black; // Rengi siyah yap

            // �ekili� Yap butonunu devre d��� b�rak
            btnCekilis.Enabled = false;

            // ComboBox'lar� devre d��� b�rak
            foreach (var cmb in new ComboBox[] { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5, cmbSecim6 })
            {
                cmb.Enabled = false;
            }
        }

        private void PictureBoxaNumaraCiz(PictureBox pictureBox, int numara, Color renk)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height); // Yeni bir bitmap olu�tur
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White); // Arka plan� beyaz yap
                using (SolidBrush firca = new SolidBrush(renk))
                {
                    g.FillEllipse(firca, 0, 0, pictureBox.Width, pictureBox.Height); // Daire �iz
                }
                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                using (SolidBrush firca = new SolidBrush(Color.White))
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center; // Metni ortala
                    format.LineAlignment = StringAlignment.Center; // Metni ortala
                    g.DrawString(numara.ToString(), font, firca, new RectangleF(0, 0, pictureBox.Width, pictureBox.Height), format); // Numaray� �iz
                }
            }
            pictureBox.Image = bmp; // PictureBox'a resmi ata
        }

        private int[] RastgeleNumaraSec(int maxNumara, int adet)
        {
            return Enumerable.Range(1, maxNumara).OrderBy(x => rastgele.Next()).Take(adet).ToArray(); // Rastgele numaralar se�
        }

        private void btnYeniOyun_Click(object sender, EventArgs e)
        {
            foreach (var cmb in new ComboBox[] { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5, cmbSecim6 })
            {
                cmb.SelectedIndex = -1; // Se�imi temizle
                cmb.Items.Clear(); // ��eleri temizle
                cmb.Enabled = true; // ComboBox'lar� etkinle�tir
            }
            ComboBoxlariBaslat(); // ComboBox'lar� ba�lat
            lblSonuc.Text = ""; // Sonu� etiketini temizle
            lblSonuc.ForeColor = Color.Black; // Rengi siyah yap
            foreach (var pb in new PictureBox[] { pbTop1, pbTop2, pbTop3, pbTop4, pbTop5, pbTop6 })
            {
                pb.Image = null; // PictureBox'lar� temizle
            }
            // �ekili� Yap butonunu etkinle�tir
            btnCekilis.Enabled = true;
        }
    }
}
