using System.Data.SqlClient;

namespace Proje1
{
    public partial class Giris : Form
    {
        
        public Giris()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=MusteriTakip;Integrated Security=True");

        private void btnGiris_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd7 = new SqlCommand("SELECT MAX(siparisID) FROM siparis", baglanti);
            SqlDataReader rd7 = cmd7.ExecuteReader();
            while (rd7.Read()) // reader Okuyabiliyorsa
            {
                new urun(txtKullaniciAdi1.Text, Convert.ToInt16(rd7[0]) + 1);
            }
            baglanti.Close();
            try
            {
                baglanti.Open();
                SqlCommand cmd3 = new SqlCommand("SELECT * FROM kullaniciGiris WHERE TC=@TC AND sifre=@sifre", baglanti);
                cmd3.Parameters.AddWithValue("@TC", txtKullaniciAdi1.Text);
                cmd3.Parameters.AddWithValue("@sifre", txtSifre1.Text);
                SqlDataReader rd3 = cmd3.ExecuteReader();
                
                if (rd3.HasRows) // Girilen K.Ad� ve K.Parola Dahilinde Gelen Data var ise 
                {
                    while (rd3.Read()) // reader Okuyabiliyorsa
                    {
                        if (Convert.ToInt16(rd3["kullaniciTuru"])==1) // 1 Rol� Admin'e ait olarak Ayarlanm��d�r
                        {
                            // Kullan�c� Rol� 1 ise Admin Ekran� A� 
                            AnaSayfa formGiris = new AnaSayfa();
                            formGiris.Show();
                            this.Hide();
                        }
                        else
                        {
                            // Kullan�c� Rol� 1 d���nda ise Kullan�c� Ekran� A�
                            siparisIslemleri formGiris = new siparisIslemleri();
                            formGiris.Show();
                            this.Hide();
                        }
                    }
                }
                else /// Reader SATIR d�nd�remiyorsa K.Ad� Parola Yanl�� Demekdir
                {
                    rd3.Close();
                    MessageBox.Show("Kullan�c� Ad� veya Parola Ge�ersizdir", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            baglanti.Close();
            }

            catch // Ba�lant� a�amay�p Sorgu �al��t�ram�yorsa Veritaban�na Ula�am�yor Demekdir
            {
                MessageBox.Show("DB ye ula��lamad�", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Giris_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnUyeol_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }

        private void btnUyelikTamamla_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTC.Text.Trim() != "" && txtAdi.Text.Trim() != "" && txtSoyadi.Text.Trim() != "" && txtTelefon.Text.Trim() != "" && txtAdres.Text.Trim() != "" && textSifre.Text.Trim() != "")
                {
                    baglanti.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO musteri(TC,mstrAdi,mstrSoyadi,mstrTelefon,mstrAdres) VALUES (@TC,@mstrAdi,@mstrSoyadi,@mstrTelefon,@mstrAdres)", baglanti);
                    cmd.Parameters.AddWithValue("@TC", txtTC.Text);
                    cmd.Parameters.AddWithValue("@mstrAdi", txtAdi.Text);
                    cmd.Parameters.AddWithValue("@mstrSoyadi", txtSoyadi.Text);
                    cmd.Parameters.AddWithValue("@mstrTelefon", txtTelefon.Text);
                    cmd.Parameters.AddWithValue("@mstrAdres", txtAdres.Text);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    baglanti.Close();
                    baglanti.Open();
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO kullaniciGiris(TC,sifre,kullaniciTuru) VALUES(@TC,@sifre,@kullaniciTuru)", baglanti);
                    cmd2.Parameters.AddWithValue("@TC", txtTC.Text);
                    cmd2.Parameters.AddWithValue("@sifre", textSifre.Text);
                    cmd2.Parameters.AddWithValue("@kullaniciTuru", false);
                    cmd2.ExecuteNonQuery();
                    cmd2.Dispose();
                    baglanti.Close();
                    foreach (Control kontrol in this.Controls)
                    {
                        if (kontrol is TextBox)
                            kontrol.Text = "";
                    }
                    MessageBox.Show("EKLEND�");
                }
                else
                {
                    MessageBox.Show("T�m Alanlar� Doldorunuz...!!!");
                }
        }
            catch (Exception)
            {
                MessageBox.Show("TC bilginiz kay�tl� ise kay�t olamazs�n�z");
            }
    groupBox1.Visible = false;
        }
    }
}