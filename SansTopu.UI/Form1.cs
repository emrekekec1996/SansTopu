using System; // Gerekli kütüphaneleri ekliyoruz
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SansTopu.UI
{
    public partial class Form1 : Form
    {
        private Random rastgele = new Random(); // Rastgele sayý üretici
        private int[] cekilisAna; // Ana çekiliþ numaralarý
        private int cekilisEk; // Ek çekiliþ numarasý

        public Form1()
        {
            InitializeComponent(); // Form bileþenlerini baþlat
            ComboBoxlariBaslat(); // ComboBox'larý baþlat
        }

        private void ComboBoxlariBaslat()
        {
            ComboBox[] comboBoxlar = { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5 }; // ComboBox dizisi
            foreach (var cmb in comboBoxlar)
            {
                for (int i = 1; i <= 34; i++) // 1'den 34'e kadar sayýlarý ekle
                {
                    cmb.Items.Add(i);
                }
                cmb.SelectedIndexChanged += ComboBox_SelectedIndexChanged; // Seçim deðiþtiðinde olay ekle
            }
            cmbSecim6.Items.Clear(); // Ek numara ComboBox'ýný temizle
            for (int i = 1; i <= 14; i++) // 1'den 14'e kadar sayýlarý ekle
            {
                cmbSecim6.Items.Add(i);
            }
            cmbSecim6.SelectedIndexChanged += ComboBox_SelectedIndexChanged; // Seçim deðiþtiðinde olay ekle
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox secilenComboBox = sender as ComboBox; // Seçilen ComboBox
            if (secilenComboBox == null) return;

            // Önce, daha önce seçilen sayýyý geri ekleyelim
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

            // Yeni seçilen sayýyý diðer ComboBox'lardan kaldýr
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
            // Çekiliþi Gerçekleþtir
            cekilisAna = RastgeleNumaraSec(34, 5); // 34 numara arasýndan 5 tane rastgele seç
            cekilisEk = rastgele.Next(1, 15); // 1 ile 14 arasýnda rastgele bir ek numara seç

            // Kullanýcý Seçimlerini Al
            int[] kullaniciAna = new int[5]; // Kullanýcýnýn ana numaralarý
            ComboBox[] comboBoxlar = { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5 }; // ComboBox dizisi
            for (int i = 0; i < 5; i++)
            {
                if (comboBoxlar[i].SelectedItem == null)
                {
                    MessageBox.Show("Lütfen tüm numaralarý seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyarý mesajý
                    return;
                }
                kullaniciAna[i] = Convert.ToInt32(comboBoxlar[i].SelectedItem); // Seçilen numarayý al
            }

            if (cmbSecim6.SelectedItem == null)
            {
                MessageBox.Show("Lütfen ek numarayý seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Uyarý mesajý
                return;
            }
            int kullaniciEk = Convert.ToInt32(cmbSecim6.SelectedItem); // Kullanýcýnýn ek numarasý

            // Sonuçlarý Karþýlaþtýr
            int anaDogru = cekilisAna.Intersect(kullaniciAna).Count(); // Doðru ana numaralarý say
            bool ekDogru = (cekilisEk == kullaniciEk); // Ek numara doðru mu?

            // Kazanan sayýlarý PictureBox'larda göster
            PictureBox[] pictureBoxlar = { pbTop1, pbTop2, pbTop3, pbTop4, pbTop5, pbTop6 }; // PictureBox dizisi
            for (int i = 0; i < 5; i++)
            {
                PictureBoxaNumaraCiz(pictureBoxlar[i], cekilisAna[i], Color.Magenta); // Ana numaralarý çiz
            }
            PictureBoxaNumaraCiz(pictureBoxlar[5], cekilisEk, Color.Blue); // Ek numarayý çiz

            // Kazanç Mesajýný Belirleme
            string sonucMesaji = "Sonuç: ";
            if (anaDogru == 5 && ekDogru) sonucMesaji += "5+1 bildiniz! 1.kategori ikramiye kazandýnýz.";
            else if (anaDogru == 5) sonucMesaji += "5 bildiniz! 2.kategori ikramiye kazandýnýz.";
            else if (anaDogru == 4 && ekDogru) sonucMesaji += "4+1 bildiniz! 3.kategori ikramiye kazandýnýz.";
            else if (anaDogru == 4) sonucMesaji += "4 bildiniz! 4.kategori ikramiye kazandýnýz.";
            else if (anaDogru == 3 && ekDogru) sonucMesaji += "3+1 bildiniz! 5.kategori ikramiye kazandýnýz.";
            else if (anaDogru == 3) sonucMesaji += "3 bildiniz! 6.kategori ikramiye kazandýnýz.";
            else if (anaDogru == 2 && ekDogru) sonucMesaji += "2+1 bildiniz! 7.kategori ikramiye kazandýnýz.";
            else if (anaDogru == 1 && ekDogru) sonucMesaji += "1+1 bildiniz! 8.kategori ikramiye kazandýnýz.";
            else if (anaDogru == 0 && ekDogru) sonucMesaji += "0+1 bildiniz! 9.kategori ikramiye kazandýnýz.";
            else sonucMesaji += "Maalesef ikramiye kazanamadýnýz, daha sonra tekrar deneyiniz.";

            lblSonuc.Text = sonucMesaji; // Sonuç mesajýný göster
            lblSonuc.ForeColor = Color.Black; // Rengi siyah yap

            // Çekiliþ Yap butonunu devre dýþý býrak
            btnCekilis.Enabled = false;

            // ComboBox'larý devre dýþý býrak
            foreach (var cmb in new ComboBox[] { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5, cmbSecim6 })
            {
                cmb.Enabled = false;
            }
        }

        private void PictureBoxaNumaraCiz(PictureBox pictureBox, int numara, Color renk)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height); // Yeni bir bitmap oluþtur
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White); // Arka planý beyaz yap
                using (SolidBrush firca = new SolidBrush(renk))
                {
                    g.FillEllipse(firca, 0, 0, pictureBox.Width, pictureBox.Height); // Daire çiz
                }
                using (Font font = new Font("Arial", 24, FontStyle.Bold))
                using (SolidBrush firca = new SolidBrush(Color.White))
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center; // Metni ortala
                    format.LineAlignment = StringAlignment.Center; // Metni ortala
                    g.DrawString(numara.ToString(), font, firca, new RectangleF(0, 0, pictureBox.Width, pictureBox.Height), format); // Numarayý çiz
                }
            }
            pictureBox.Image = bmp; // PictureBox'a resmi ata
        }

        private int[] RastgeleNumaraSec(int maxNumara, int adet)
        {
            return Enumerable.Range(1, maxNumara).OrderBy(x => rastgele.Next()).Take(adet).ToArray(); // Rastgele numaralar seç
        }

        private void btnYeniOyun_Click(object sender, EventArgs e)
        {
            foreach (var cmb in new ComboBox[] { cmbSecim1, cmbSecim2, cmbSecim3, cmbSecim4, cmbSecim5, cmbSecim6 })
            {
                cmb.SelectedIndex = -1; // Seçimi temizle
                cmb.Items.Clear(); // Öðeleri temizle
                cmb.Enabled = true; // ComboBox'larý etkinleþtir
            }
            ComboBoxlariBaslat(); // ComboBox'larý baþlat
            lblSonuc.Text = ""; // Sonuç etiketini temizle
            lblSonuc.ForeColor = Color.Black; // Rengi siyah yap
            foreach (var pb in new PictureBox[] { pbTop1, pbTop2, pbTop3, pbTop4, pbTop5, pbTop6 })
            {
                pb.Image = null; // PictureBox'larý temizle
            }
            // Çekiliþ Yap butonunu etkinleþtir
            btnCekilis.Enabled = true;
        }
    }
}
