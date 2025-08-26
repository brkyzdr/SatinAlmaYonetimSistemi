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
    public partial class Offers : Form
    {
        public Offers()
        {
            InitializeComponent();
            ReadData();
        }
        private void ReadData()
        {
            DataTable dt = CRUD.Read("SELECT * FROM PurchaseOffers");
            dataGridView1.DataSource= dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["SupplierID"].HeaderText = "Tedarikçi";
            dataGridView1.Columns["UserID"].Visible = false;
            dataGridView1.Columns["ItemID"].HeaderText = "Ürün";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView1.Columns["Price"].HeaderText = "Fiyat";
            dataGridView1.Columns["Currency"].HeaderText = "Para Birimi";
            dataGridView1.Columns["Status"].HeaderText = "Durum";
            dataGridView1.Columns["Date"].HeaderText = "Tarih";
            dataGridView1.Columns["ApprovedAt"].Visible = false;

        }

        private void CreateData()
        {
            if (!string.IsNullOrEmpty(comboBoxSuppliers.Text)
                && !string.IsNullOrEmpty(comboBoxItem.Text)
                && !string.IsNullOrEmpty(comboBoxUnit.Text)
                && !string.IsNullOrEmpty(textBoxQuantity.Text)
                && !string.IsNullOrEmpty(textBoxPrice.Text)
                && !string.IsNullOrEmpty(comboBoxCurrency.Text))
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
                        {"SupplierID" ,1 },//!!!! Supplier id eklenecek
                        {"ItemID" ,1 },// item id eklencek
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Price" ,textBoxPrice.Text },
                        {"Currency" ,comboBoxCurrency.Text },
                        {"Date", DateTime.Now},
                    };
                    CRUD.Create("PurchaseOffers", data);

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
                    var data = new Dictionary<string, object>
                    {
                        {"UserID",1  },//!!!! Login yapan kullanıcı eklenecek
                        {"SupplierID" ,1 },//!!!! Supplier id eklenecek
                        {"ItemID" ,1 },// item id eklencek
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Price" ,textBoxPrice.Text },
                        {"Currency" ,comboBoxCurrency.Text },
                        {"Date", DateTime.Now},
                    };
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("PurchaseOffers", data, condition);
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
                    CRUD.Delete("PurchaseOffers", condition);
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
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                comboBoxSuppliers.Text = row.Cells["SupplierID"].Value.ToString();
                comboBoxItem.Text = row.Cells["ItemID"].Value.ToString();
                comboBoxUnit.Text = row.Cells["Unit"].Value.ToString();
                textBoxQuantity.Text = row.Cells["Quantity"].Value.ToString();
                textBoxPrice.Text = row.Cells["Price"].Value.ToString();
                comboBoxCurrency.Text = row.Cells["Currency"].Value.ToString();
            }
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
    }
}
