using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            ReadData();
        }

        private void ReadData()
        {
            DataTable dt = CRUD.Read("SELECT * FROM Users");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;
            

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Role"].HeaderText = "Rol";
            dataGridView1.Columns["Username"].HeaderText = "Kullanıcı Adı";
            dataGridView1.Columns["Passwordhash"].HeaderText = "Şifre";
            dataGridView1.Columns["Name"].HeaderText = "İsim";
            dataGridView1.Columns["Surname"].HeaderText = "Soyisim";
            dataGridView1.Columns["Email"].HeaderText = "E-posta";
            dataGridView1.Columns["PhoneNum"].HeaderText = "Telefon Numarası";
            dataGridView1.Columns["CreatedAt"].HeaderText = "Oluşturan Kişi";
            dataGridView1.Columns["IsActive"].HeaderText = "Aktiflik";
        }

        private void CreateData()
        {
            if (!string.IsNullOrEmpty(comboBoxRole.Text)
                && !string.IsNullOrEmpty(textBoxUsername.Text)
                && !string.IsNullOrEmpty(textBoxPassword.Text)
                && !string.IsNullOrEmpty(textBoxName.Text)
                && !string.IsNullOrEmpty(textBoxName.Text)
                && !string.IsNullOrEmpty(textBoxSurname.Text)
                && !string.IsNullOrEmpty(textBoxEmail.Text)
                && !string.IsNullOrEmpty(textBoxPhoneNumber.Text)
                && !string.IsNullOrEmpty(comboBoxIsActive.Text))
            {

                DialogResult result = MessageBox.Show(
                    "Kaydı oluşturmak istediğinize emin misiniz?",   // Mesaj
                    "Onay",                                          // Başlık
                    MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                    MessageBoxIcon.Question                          // Soru ikonu
                );

                if (result == DialogResult.Yes)
                {

                    var data = new Dictionary<string, object>
                    {
                        {"Role" ,comboBoxRole.Text },
                        {"Username" ,textBoxUsername.Text },
                        {"PasswordHash" ,textBoxPassword.Text },
                        {"Name" ,textBoxName.Text },
                        {"Surname", textBoxSurname.Text},
                        {"Email", textBoxEmail.Text},
                        {"PhoneNum", textBoxPhoneNumber.Text},
                        {"IsActive", 1},//değişecek
                        {"CreatedAt", 1},//değişecek!!!!
                    };
                    CRUD.Create("Users", data);

                    MessageBox.Show("Kayıt başarıyla oluşturuldu.", "Bilgi");
                    ReadData();
                }
                else
                {
                    MessageBox.Show("Kayıt işlemi iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
            }
        }

        private void UpdateData()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                "Kaydı güncellemek istediğinize emin misiniz?",   // Mesaj
                "Onay",                                          // Başlık
                MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                MessageBoxIcon.Question                          // Soru ikonu
                );

                // Kullanıcı "Yes" derse kayıt işlemi yapılır
                if (result == DialogResult.Yes)
                {
                    // Buraya kayıt kodlarını yaz
                    var data = new Dictionary<string, object>
                    {
                        {"Role" ,comboBoxRole.Text },
                        {"Username" ,textBoxUsername.Text },
                        {"PasswordHash" ,textBoxPassword.Text },
                        {"Name" ,textBoxName.Text },
                        {"Surname", textBoxSurname.Text},
                        {"Email", textBoxEmail.Text},
                        {"PhoneNum", textBoxPhoneNumber.Text},
                        {"IsActive", 1},//değişecek
                        {"CreatedAt", 1},//kullanıcı id kaydı

                    };
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Users", data, condition);
                    MessageBox.Show("Kayıt başarıyla güncellendi.", "Bilgi");
                    ReadData();
                }
                else
                {
                    MessageBox.Show("Kayıt güncelleme işlemi iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek bir satır seçin.");
            }
        }

        private void DeleteData()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                "Veriyi silmek istediğinize emin misiniz?",   // Mesaj
                "Onay",                                          // Başlık
                MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                MessageBoxIcon.Question                          // Soru ikonu
                );

                if (result == DialogResult.Yes)
                {
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Delete("Users", condition);
                    MessageBox.Show("Veri başarıyla silindi.");
                    ReadData();
                }
                else
                {
                    MessageBox.Show("Veri silme işlemi iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.");
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            CreateData();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                comboBoxRole.Text = row.Cells["Role"].Value.ToString();
                textBoxUsername.Text = row.Cells["Username"].Value.ToString();
                textBoxPassword.Text = row.Cells["PasswordHash"].Value.ToString();
                textBoxEmail.Text = row.Cells["Email"].Value.ToString();
                textBoxName.Text = row.Cells["Name"].Value.ToString();
                textBoxSurname.Text = row.Cells["Surname"].Value.ToString();
                textBoxPhoneNumber.Text = row.Cells["PhoneNum"].Value.ToString();
                comboBoxIsActive.Text = row.Cells["IsActive"].Value.ToString();
            }
        }
    }
}
