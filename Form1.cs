using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;//acces kutuphanesi

namespace Emlak_Projesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection baglan = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Emlak_Bilgiler.accdb");
        OleDbDataAdapter okuvelistele;
        OleDbDataReader oku;
        DataTable tablo;
        OleDbCommand komutver;

        void listem()
        {
            baglan.Open();
            okuvelistele = new OleDbDataAdapter("select il_ilce AS[İL/İLÇE],Adres AS[ADRES],Kat_Sayisi AS[KAT SAYISI],Bulundugu_Kat AS[KAÇINCI KAT],Daire_No AS[DAİRE NO],isitma AS[ISITMA TÜRÜ],Alani AS[DAİRE M2],oda_Sayisi AS[ODA SAYISI],Bina_Yas AS[BİNA YAŞ],Ucret AS[FİYAT],Resim AS[RESİM],Tc_No AS[TC NO],AdiSoyadi AS[ADI SOYADI],Telefon AS[TELEFON] from Emlak_Kayitlar", baglan);
            tablo = new DataTable();
            okuvelistele.Fill(tablo);

            dataGridView1.DataSource = tablo;
            baglan.Close();
        }
        void sil()
        {
            textBox10Fiyat.Clear();
            textBox1AdiSoyadi.Clear();
            textBox1BinaYas.Clear();
            textBox1il_ilce.Clear();
            textBox1Resim.Clear();
            textBox2Adres.Clear();
            textBox3KatSayisi.Clear();
            textBox4Bu_Kat.Clear();
            textBox5DaireNo.Clear();
            textBox6isitma.Clear();
            textBox7M2.Clear();
            textBox8OdaSayisi.Clear();
            textBox2Tc.Clear();
            textBox1Telefon.Clear();
            pictureBox1.ImageLocation = null;
        }

        void toplam()
        {
            baglan.Open();
            komutver = new OleDbCommand("select sum(Ucret) from Emlak_Kayitlar", baglan);
            label17ToplamGelir.Text = komutver.ExecuteScalar().ToString() + " TL";

            komutver = new OleDbCommand("select count(AdiSoyadi) from Emlak_Kayitlar", baglan);
            label16KayitliSayisi.Text = komutver.ExecuteScalar().ToString();
            baglan.Close();
        }
        private void button1Ekle_Click(object sender, EventArgs e)
        {
            bool kayit_varmi = false;
            baglan.Open();
            komutver = new OleDbCommand("select *from Emlak_Kayitlar where Tc_No='" + textBox2Tc.Text + "'", baglan);
            oku = komutver.ExecuteReader();
            while (oku.Read())
            {
                kayit_varmi = true;
                MessageBox.Show(textBox2Tc.Text + " Numaralı Tc Kayıtta Var..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                break;
            }
            baglan.Close();

            if (kayit_varmi == false)
            {
                if (textBox1il_ilce.Text != "" && textBox2Adres.Text != "" && textBox3KatSayisi.Text != "" && textBox4Bu_Kat.Text != "" && textBox5DaireNo.Text != "" && textBox6isitma.Text != "" && textBox7M2.Text != "" && textBox8OdaSayisi.Text != "" && textBox1BinaYas.Text != "" && textBox10Fiyat.Text != "" && textBox2Tc.Text != "" && textBox1AdiSoyadi.Text != "")
                {
                    baglan.Open();
                    komutver = new OleDbCommand("insert into Emlak_Kayitlar(il_ilce,Adres,Kat_Sayisi,Bulundugu_Kat,Daire_No,isitma,Alani,oda_Sayisi,Bina_Yas,Ucret,Resim,Tc_No,AdiSoyadi,Telefon) values(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14)", baglan);
                    komutver.Parameters.AddWithValue("@p1", textBox1il_ilce.Text);
                    komutver.Parameters.AddWithValue("@p2", textBox2Adres.Text);
                    komutver.Parameters.AddWithValue("@p3", textBox3KatSayisi.Text);
                    komutver.Parameters.AddWithValue("@p4", textBox4Bu_Kat.Text);
                    komutver.Parameters.AddWithValue("@p5", textBox5DaireNo.Text);
                    komutver.Parameters.AddWithValue("@p6", textBox6isitma.Text);
                    komutver.Parameters.AddWithValue("@p7", textBox7M2.Text);
                    komutver.Parameters.AddWithValue("@p8", textBox8OdaSayisi.Text);
                    komutver.Parameters.AddWithValue("@p9", textBox1BinaYas.Text);
                    komutver.Parameters.AddWithValue("@p10", Convert.ToDouble(textBox10Fiyat.Text));
                    komutver.Parameters.AddWithValue("@p11", textBox1Resim.Text);
                    komutver.Parameters.AddWithValue("@p12", Convert.ToDouble(textBox2Tc.Text));
                    komutver.Parameters.AddWithValue("@p13", textBox1AdiSoyadi.Text);
                    komutver.Parameters.AddWithValue("@p14", textBox1Telefon.Text);
                    komutver.ExecuteNonQuery();
                    baglan.Close();

                    MessageBox.Show("Kayıtlar Listeye Başarıyla eklendi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listem();
                    sil();
                    toplam();

                }
                else
                {
                    MessageBox.Show("Eksik alanları doldurunuz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            listem();
            toplam();
        }

        private void textBox3KatSayisi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar > 126 || (int)e.KeyChar < 58)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void button5Resim_Click(object sender, EventArgs e)
        {
            OpenFileDialog resimac = new OpenFileDialog();
            resimac.Filter = "Resim Dosyası(*.JPG,*.PNG,*.BMP,*.GIF)| *.jpg; *.png; *.bmp; *.gif";
            if (resimac.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = resimac.FileName;
                textBox1Resim.Text = resimac.FileName;
            }
        }

        private void button2Sil_Click(object sender, EventArgs e)
        {
            bool silme_durum = false;
            baglan.Open();
            komutver = new OleDbCommand("select *from Emlak_Kayitlar where Tc_No='" + textBox2Tc.Text + "'", baglan);
            oku = komutver.ExecuteReader();
            while (oku.Read())
            {
                silme_durum = true;
                komutver = new OleDbCommand("delete from Emlak_Kayitlar where Tc_No=@p1", baglan);
                komutver.Parameters.AddWithValue("@p1", textBox2Tc.Text);
                komutver.ExecuteNonQuery();
                baglan.Close();

                MessageBox.Show(textBox2Tc.Text + " Tc No Kayıtta Silindi", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listem();
                sil();
                toplam();
                break;
            }
            if (silme_durum == false)
            {
                MessageBox.Show(textBox2Tc.Text + " Silinecek Tc Numarası bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                baglan.Close();
            }

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            textBox1il_ilce.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            textBox2Adres.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            textBox3KatSayisi.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            textBox4Bu_Kat.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            textBox5DaireNo.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            textBox6isitma.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            textBox7M2.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
            textBox8OdaSayisi.Text = dataGridView1.Rows[secilen].Cells[7].Value.ToString();
            textBox1BinaYas.Text = dataGridView1.Rows[secilen].Cells[8].Value.ToString();
            textBox10Fiyat.Text = dataGridView1.Rows[secilen].Cells[9].Value.ToString();
            textBox1Resim.Text = dataGridView1.Rows[secilen].Cells[10].Value.ToString();
            pictureBox1.ImageLocation = dataGridView1.Rows[secilen].Cells[10].Value.ToString();
            textBox2Tc.Text = dataGridView1.Rows[secilen].Cells[11].Value.ToString();
            textBox1AdiSoyadi.Text = dataGridView1.Rows[secilen].Cells[12].Value.ToString();
            textBox1Telefon.Text = dataGridView1.Rows[secilen].Cells[13].Value.ToString();
        }

        private void textBox1AdiSoyadi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void button4Arama_Click(object sender, EventArgs e)
        {
            bool varmi = false;
            if (textBox2Tc.Text != "")
            {
                baglan.Open();
                komutver = new OleDbCommand("select il_ilce AS[İL/İLÇE],Adres AS[ADRES],Kat_Sayisi AS[KAT SAYISI],Bulundugu_Kat AS[KAÇINCI KAT],Daire_No AS[DAİRE NO],isitma AS[ISITMA TÜRÜ],Alani AS[DAİRE M2],oda_Sayisi AS[ODA SAYISI],Bina_Yas AS[BİNA YAŞ],Ucret AS[FİYAT],Resim AS[RESİM],Tc_No AS[TC NO],AdiSoyadi AS[ADI SOYADI],Telefon AS[TELEFON] from Emlak_Kayitlar where Tc_No like '" + textBox2Tc.Text + "%'", baglan);
                oku = komutver.ExecuteReader();
                while (oku.Read())
                {
                    varmi = true;
                    okuvelistele = new OleDbDataAdapter("select il_ilce AS[İL/İLÇE],Adres AS[ADRES],Kat_Sayisi AS[KAT SAYISI],Bulundugu_Kat AS[KAÇINCI KAT],Daire_No AS[DAİRE NO],isitma AS[ISITMA TÜRÜ],Alani AS[DAİRE M2],oda_Sayisi AS[ODA SAYISI],Bina_Yas AS[BİNA YAŞ],Ucret AS[FİYAT],Resim AS[RESİM],Tc_No AS[TC NO],AdiSoyadi AS[ADI SOYADI],Telefon AS[TELEFON] from Emlak_Kayitlar where Tc_No like '" + textBox2Tc.Text + "%'", baglan);
                    tablo = new DataTable();
                    okuvelistele.Fill(tablo);
                    dataGridView1.DataSource = tablo;
                    break;
                }
                baglan.Close();
                if (varmi == false)
                {
                    MessageBox.Show("Aradıgınız " + textBox2Tc.Text + " Numara bulunamadı yada Kayıtlı degil..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    baglan.Close();
                }
            }
            else
            {
                MessageBox.Show("Lütfen Aranacak Tc Numarası Giriniz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            



        }

        private void button3Guncell_Click(object sender, EventArgs e)
        {
            baglan.Open();
            komutver = new OleDbCommand("update Emlak_Kayitlar set il_ilce=@p1,Adres=@p2,Kat_Sayisi=@p3,Bulundugu_Kat=@p4,Daire_No=@p5,isitma=@p6,Alani=@p7,oda_Sayisi=@p8,Bina_Yas=@p9,Ucret=@p10,Resim=@p11,AdiSoyadi=@p13,Telefon=@p14 where Tc_No=@p12", baglan);
            komutver.Parameters.AddWithValue("@p1", textBox1il_ilce.Text);
            komutver.Parameters.AddWithValue("@p2", textBox2Adres.Text);
            komutver.Parameters.AddWithValue("@p3", textBox3KatSayisi.Text);
            komutver.Parameters.AddWithValue("@p4", textBox4Bu_Kat.Text);
            komutver.Parameters.AddWithValue("@p5", textBox5DaireNo.Text);
            komutver.Parameters.AddWithValue("@p6", textBox6isitma.Text);
            komutver.Parameters.AddWithValue("@p7", textBox7M2.Text);
            komutver.Parameters.AddWithValue("@p8", textBox8OdaSayisi.Text);
            komutver.Parameters.AddWithValue("@p9", textBox1BinaYas.Text);
            komutver.Parameters.AddWithValue("@p10", Convert.ToDouble(textBox10Fiyat.Text));
            komutver.Parameters.AddWithValue("@p11", textBox1Resim.Text);
            komutver.Parameters.AddWithValue("@p13", textBox1AdiSoyadi.Text);
            komutver.Parameters.AddWithValue("@p14", textBox1Telefon.Text);
            komutver.Parameters.AddWithValue("@p12", Convert.ToDouble(textBox2Tc.Text));// where en son yazılır..
            komutver.ExecuteNonQuery();
            baglan.Close();

            MessageBox.Show("Güncellemeler Başarıyla gerçekleştirildi..", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listem();
            toplam();
        }

        private void button1Yenile_Click(object sender, EventArgs e)
        {
            listem();
        }

        private void button1Temizle_Click(object sender, EventArgs e)
        {
            textBox10Fiyat.Clear();
            textBox1AdiSoyadi.Clear();
            textBox1BinaYas.Clear();
            textBox1il_ilce.Clear();
            textBox1Resim.Clear();
            textBox2Adres.Clear();
            textBox3KatSayisi.Clear();
            textBox4Bu_Kat.Clear();
            textBox5DaireNo.Clear();
            textBox6isitma.Clear();
            textBox7M2.Clear();
            textBox8OdaSayisi.Clear();
            textBox2Tc.Clear();
            pictureBox1.ImageLocation = null;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/n.beyi");
        }
    }
}
