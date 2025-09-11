using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Data;
using Data.Services;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class Requisitions : Form
    {
        public Requisitions()
        {
            InitializeComponent();
            ReadData();
        }

        private void ReadData()
        {
            DataTable dt = CRUD.Read("SELECT " +
                "r.ID, r.Item, r.Quantity, r.Unit, r.Description, r.Status, r.Date, " +
                "CASE WHEN r.ApprovedByID = 0 THEN 'Beklemede' END AS ApprovedBy " +
                "FROM Requisitions r " +
                $"WHERE r.UserID ={Session.UserID} ");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Item"].HeaderText = "Ürün";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView1.Columns["Description"].HeaderText = "Açıklama";
            dataGridView1.Columns["Status"].HeaderText = "Durum";
            dataGridView1.Columns["ApprovedBy"].Visible = false;
            dataGridView1.Columns["Date"].HeaderText = "Talep Tarihi";
        }

        private void CreateData()
        {
            if (!string.IsNullOrEmpty(textBoxItem.Text)
                && !string.IsNullOrEmpty(comboBoxUnit.Text)
                && !string.IsNullOrEmpty(textBoxQuantity.Text)
                && !string.IsNullOrEmpty(textBoxDescription.Text))
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
                        {"UserID",Session.UserID },
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

                if ((dataGridView1.SelectedRows[0].Cells["Status"].Value.ToString()) == "Onaylandı")
                {
                    MessageBox.Show("Onaylanmış bir talebi düzenleyemezsiniz!", "Bilgi");
                    return;
                }

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
                        {"UserID",Session.UserID },
                        {"Item" ,textBoxItem.Text },
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Description" ,textBoxDescription.Text },
                        {"Date", DateTime.Now},
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

        private void textBoxQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sayı, kontrol (backspace vb.) veya nokta/virgül dışındaki her şeyi engelle
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Hem nokta hem virgül girişini tek bir "decimal ayırıcı" kabul et (noktayı tercih edelim)
            if (e.KeyChar == ',')
            {
                e.KeyChar = '.'; // virgül yazılırsa nokta yap
            }

            // Birden fazla nokta olmasını engelle
            if (e.KeyChar == '.' && (sender as System.Windows.Forms.TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxQuantity_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!decimal.TryParse(textBoxQuantity.Text, out decimal value) || value <= 0)
            {
                MessageBox.Show("Lütfen sıfırdan büyük bir sayı giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // focus TextBox'ta kalır
            }
        }
    }
}
