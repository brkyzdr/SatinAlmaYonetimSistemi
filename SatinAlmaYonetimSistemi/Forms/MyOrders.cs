using Data;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class MyOrders : Form
    {
        private int dataGridViewOrderID;

        public MyOrders()
        {
            InitializeComponent();
            ReadData();
            SetComboBoxData();
        }

        public void ReadData()
        {
            DataTable dt = CRUD.Read(
                "SELECT " +
                "o.ID, o.Item, o.Unit, o.Quantity, o.Price, o.Currency, o.Status, o.Date, " +
                "s.Name AS Supplier, " +
                "i.InvoiceNumber AS Invoice, " +
                "COALESCE(u.Name, '') + ' ' + COALESCE(u.Surname, '') + '(#' + CAST(u.ID AS varchar(20)) + ')' AS UserName, " +
                "COALESCE(u2.Name, '') + ' ' + COALESCE(u2.Surname, '') + '(#' + CAST(u2.ID AS varchar(20)) + ')' AS RequisitionsOwner " +
                "FROM Orders o " +
                "INNER JOIN Suppliers s ON o.SupplierID = s.ID " +
                "INNER JOIN Requisitions r ON o.RequisitionsID = r.ID " +
                "LEFT JOIN Invoices i ON o.ID = i.orderID " +
                "INNER JOIN Users u ON o.UserID = u.ID " +
                "INNER JOIN Users u2 ON r.UserID = u2.ID "+
                $"WHERE o.UserID={(int)Session.UserID} ");

            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["Supplier"].HeaderText = "Tedarikçi";
            dataGridView1.Columns["RequisitionsOwner"].HeaderText = "Talep Eden Kişi";
            dataGridView1.Columns["UserName"].HeaderText = "Satınalma Sorumlusu";
            dataGridView1.Columns["Invoice"].HeaderText = "Fatura";
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Item"].HeaderText = "Ürün";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView1.Columns["Price"].HeaderText = "Fiyat";
            dataGridView1.Columns["Currency"].HeaderText = "Para Birimi";
            dataGridView1.Columns["Status"].HeaderText = "Durum";
            dataGridView1.Columns["Date"].HeaderText = "Tarih";

        }

        private void CreateData()
        {
            if (!string.IsNullOrEmpty(comboBoxSuppliers.Text)
                && !string.IsNullOrEmpty(textBoxItem.Text)
                && !string.IsNullOrEmpty(comboBoxUnit.Text)
                && !string.IsNullOrEmpty(textBoxQuantity.Text)
                && !string.IsNullOrEmpty(textBoxPrice.Text)
                && !string.IsNullOrEmpty(comboBoxCurrency.Text))
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
                        {"UserID",Session.UserID  },
                        {"SupplierID",comboBoxSuppliers.SelectedValue },
                        {"RequisitionsID",4 },//talep eden kişi eklenecek
                        {"Item",textBoxItem.Text  },
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Price" ,textBoxPrice.Text },
                        {"Currency", comboBoxCurrency.Text},
                        {"Date", DateTime.Now},
                    };
                    CRUD.Create("Orders", data);

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
                        {"UserID",Session.UserID  },
                        {"SupplierID",comboBoxSuppliers.SelectedValue  },
                        {"RequisitionsID",4},//talep eden kişi eklenecek
                        {"Item",textBoxItem.Text  },
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Price" ,textBoxPrice.Text },
                        {"Currency", comboBoxCurrency.Text},
                        {"Date", DateTime.Now},
                    };
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Orders", data, condition);
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
                "Veriyi silmek istediğinize emin misiniz?",      // Mesaj
                "Onay",                                          // Başlık
                MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                MessageBoxIcon.Question                          // Soru ikonu
                );

                if (result == DialogResult.Yes)
                {
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Delete("Orders", condition);
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

        private void SetComboBoxData()
        {
            DataTable supplierName = CRUD.Read("SELECT ID, Name FROM Suppliers WHERE IsActive=1 ORDER BY Name");
            comboBoxSuppliers.DataSource = supplierName;
            comboBoxSuppliers.DisplayMember = "Name";
            comboBoxSuppliers.ValueMember = "ID";
            comboBoxSuppliers.SelectedItem = null;
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
                comboBoxSuppliers.Text = row.Cells["Supplier"].Value.ToString();
                textBoxItem.Text = row.Cells["Item"].Value.ToString();
                comboBoxUnit.Text = row.Cells["Unit"].Value.ToString();
                textBoxQuantity.Text = row.Cells["Quantity"].Value.ToString();
                textBoxPrice.Text = row.Cells["Price"].Value.ToString();
                comboBoxCurrency.Text = row.Cells["Currency"].Value.ToString();

                dataGridViewOrderID = (int)row.Cells["ID"].Value;

                buttonInvoice.Enabled = true;
            }
        }

        private void buttonInvoice_Click(object sender, EventArgs e)
        {
            Session.OrderID = dataGridViewOrderID;

            using (var f = new OrderInvoice())
            {
                f.Owner = this;
                f.ShowDialog();
            }
        }
    }
}
