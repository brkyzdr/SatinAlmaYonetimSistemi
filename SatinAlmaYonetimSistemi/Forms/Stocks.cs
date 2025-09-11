using Data;
using Data.Services;
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
    public partial class Stocks : Form
    {
        string itemCode;
        public Stocks()
        {
            InitializeComponent();
            SetCombobox();
            ReadData();
        }

        private void ReadData()
        {
            DataTable dt = CRUD.Read(
                "SELECT " +
                "s.ID, s.ItemCode, s.ItemName,c.CategoryName,s.Unit,s.CreatedDate,s.CreatedBy,s.ModifiedBy,s.IsActive, " +
                "CASE WHEN s.IsActive = 1 THEN 'Aktif' ELSE 'Pasif' END AS Status " +
                "FROM Stocks s " +
                "INNER JOIN Categories c ON s.Category = c.ID ");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["ItemCode"].HeaderText = "Ürün Kodu";
            dataGridView1.Columns["ItemName"].HeaderText = "Ürün Adı";
            dataGridView1.Columns["CategoryName"].HeaderText = "Kategori";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["CreatedDate"].HeaderText = "Oluşturulma Tarihi";
            dataGridView1.Columns["CreatedBy"].HeaderText ="Oluşturan Kişi" ;
            dataGridView1.Columns["ModifiedBy"].HeaderText = "Değiştiren Kişi";
            dataGridView1.Columns["IsActive"].HeaderText = "Aktiflik";
        }

        private void CreateData()
        {
            if (!string.IsNullOrEmpty(textBoxItemName.Text)
                && !string.IsNullOrEmpty(comboBoxCategory.Text)
                && !string.IsNullOrEmpty(comboBoxUnit.Text)
                && !string.IsNullOrEmpty(comboBoxIsActive.Text)
                )
            {
                DialogResult result = MessageBox.Show(
                    "Kaydı oluşturmak istediğinize emin misiniz?",   // Mesaj
                    "Onay",                                          // Başlık
                    MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                    MessageBoxIcon.Question                          // Soru ikonu
                );

                if (result == DialogResult.Yes)
                {
                    int isActive;
                    if (comboBoxIsActive.Text == "Aktif")
                    {
                        isActive = 1;
                    }
                    else
                    {
                        isActive = 0;
                    }

                    var data = new Dictionary<string, object>
                    {
                        {"ItemCode" ,itemCode},
                        {"ItemName" ,textBoxItemName.Text },
                        {"Unit" ,comboBoxUnit.Text },
                        {"CreatedDate" ,DateTime.Now },
                        {"CreatedBy" ,Session.UserID },
                        {"Category" ,comboBoxCategory.Text },
                        {"IsActive", isActive},
                    };
                    CRUD.Create("Stocks", data);

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
                    int isActive;
                    if (comboBoxIsActive.Text == "Aktif")
                    {
                        isActive = 1;
                    }
                    else
                    {
                        isActive = 0;
                    }

                    var data = new Dictionary<string, object>
                    {
                        {"ItemCode" ,itemCode},
                        {"ItemName" ,textBoxItemName.Text },
                        {"Unit" ,comboBoxUnit.Text },
                        {"CreatedDate" ,DateTime.Now },
                        {"ModifiedBy" ,Session.UserID },
                        {"Category" ,comboBoxCategory.Text },
                        {"IsActive", isActive},
                    };
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Stocks", data, condition);
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
                    CRUD.Delete("Stocks", condition);
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

        private void SetCombobox()
        {
            DataTable categoryName = CRUD.Read("SELECT ID, CategoryName FROM Categories ORDER BY CategoryName");
            comboBoxCategory.DataSource = categoryName;
            comboBoxCategory.DisplayMember = "CategoryName";
            comboBoxCategory.ValueMember = "ID";
            comboBoxCategory.SelectedItem = null;
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

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
