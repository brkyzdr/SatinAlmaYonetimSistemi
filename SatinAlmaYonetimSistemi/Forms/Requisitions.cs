using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Data;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class Requisitions : Form
    {//Create & update için userid işlemleri tamamlanacak
        public Requisitions()
        {
            InitializeComponent();
            ReadData();
        }

        private void ReadData()
        {
            DataTable dt = CRUD.Read("SELECT " +
                "r.Item, r.Unit, r.Quantity, r.Description, r.Status, r.Date," +
                "CASE WHEN r.ApprovedByID = 0 THEN 'Beklemede' END AS ApprovedBy " +
                "FROM Requisitions r");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["Item"].HeaderText = "Ürün";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView1.Columns["Description"].HeaderText = "Açıklama";
            dataGridView1.Columns["Status"].HeaderText = "Durum";
            dataGridView1.Columns["ApprovedBy"].HeaderText = "Onaylayan Kişi";
            dataGridView1.Columns["Date"].HeaderText = "Talep Tarihi";
        }

        private void CreateData()
        {
            if (!string.IsNullOrEmpty(textBoxItem.Text)
                && !string.IsNullOrEmpty(comboBoxUnit.Text)
                && !string.IsNullOrEmpty(textBoxQuantity.Text)
                && !string.IsNullOrEmpty(textBoxDescription.Text))
            {
                // Kullanıcıya emin misiniz? sorusu sorulur
                DialogResult result = MessageBox.Show(
                    "Kaydı oluşturmak istediğinize emin misiniz?",   // Mesaj
                    "Onay",                                          // Başlık
                    MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                    MessageBoxIcon.Question                          // Soru ikonu
                );

                // Kullanıcı "Yes" derse kayıt işlemi yapılır
                if (result == DialogResult.Yes)
                {

                    var data = new Dictionary<string, object>
                    {
                        {"UserID",1  },//!!!! Login yapan kullanıcı eklenecek
                        {"Item" ,textBoxItem.Text },
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Description" ,textBoxDescription.Text },
                        {"Date", DateTime.Now},
                    };
                    CRUD.Create("Requisitions", data);

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

                
                if (result == DialogResult.Yes)
                {
                    var data = new Dictionary<string, object>
                    {
                        {"Item" ,textBoxItem.Text },
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Description" ,textBoxDescription.Text },
                    };
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Requisitions", data, condition);
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
                "Veriyi silmek istediğinize emin misiniz?",     // Mesaj
                "Onay",                                          // Başlık
                MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                MessageBoxIcon.Question                          // Soru ikonu
                );

                if (result == DialogResult.Yes)
                {
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Delete("Requisitions", condition);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBoxItem.Text = row.Cells["Item"].Value.ToString();
                comboBoxUnit.Text = row.Cells["Unit"].Value.ToString();
                textBoxQuantity.Text = row.Cells["Quantity"].Value.ToString();
                textBoxDescription.Text = row.Cells["Description"].Value.ToString();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }
    }
}
