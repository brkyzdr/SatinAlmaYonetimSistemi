using Data;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class Offers : Form
    {
        private int RequisitionsOwnerID;

        public Offers()
        {
            InitializeComponent();
            SetComboBoxData();
            ReadData();           
        }

        private void ReadData()
        {
            DataTable dt = CRUD.Read
                ("SELECT " +
                "s.Name AS SupplierName," +
                "COALESCE(u.Name, '') + ' ' + COALESCE(u.Surname, '') + '(#' + CAST(u.ID AS varchar(20)) + ')' AS UserName," +
                "COALESCE(u2.Name, '') + ' ' + COALESCE(u2.Surname, '') + '(#' + CAST(u2.ID AS varchar(20)) + ')' AS RequisitionsOwner," +
                "u2.ID AS RequisitionsOwnerID," +
                "o.ID, o.Item, o.Unit, o.Quantity, o.Price, o.Currency, o.Status, o.Date, r.Item AS RItem, " +
                "CASE WHEN o.ApprovedByID = 0 THEN 'Beklemede' END AS ApprovedBy " +
                "FROM Offers o " +
                "INNER JOIN Suppliers s ON o.SupplierID = s.ID " +
                "INNER JOIN Requisitions r ON o.RequisitionsID = r.ID " +
                "INNER JOIN Users u ON o.UserID = u.ID " +
                "INNER JOIN Users u2 ON r.UserID = u2.ID ");
            
            dataGridView1.DataSource= dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["ApprovedBy"].Visible = false;
            dataGridView1.Columns["SupplierName"].HeaderText = "Tedarikçi";
            dataGridView1.Columns["RequisitionsOwner"].HeaderText = "Ürünü Talep Eden Kişi";
            dataGridView1.Columns["UserName"].HeaderText = "Satınalma Sorumlusu";
            dataGridView1.Columns["Item"].HeaderText = "Ürün";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView1.Columns["Price"].HeaderText = "Fiyat";
            dataGridView1.Columns["Currency"].HeaderText = "Para Birimi";
            dataGridView1.Columns["Status"].HeaderText = "Durum";
            dataGridView1.Columns["Date"].HeaderText = "Tarih";                            
            dataGridView1.Columns["RequisitionsOwnerID"].Visible = false;                            
            dataGridView1.Columns["RItem"].Visible = false;                            
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
                        {"SupplierID" ,comboBoxSuppliers.SelectedValue },
                        {"RequisitionsID" ,comboBoxRequisitionOwner.SelectedValue },
                        {"Item" ,textBoxItem.Text },
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Price" ,textBoxPrice.Text },
                        {"Currency" ,comboBoxCurrency.Text },
                        {"Date", DateTime.Now},
                    };
                    CRUD.Create("Offers", data);

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
                        {"SupplierID" ,comboBoxSuppliers.SelectedValue },
                        {"RequisitionsID" ,comboBoxRequisitionOwner.SelectedValue },
                        {"Item" ,textBoxItem.Text },
                        {"Unit" ,comboBoxUnit.Text },
                        {"Quantity" ,textBoxQuantity.Text },
                        {"Price" ,textBoxPrice.Text },
                        {"Currency" ,comboBoxCurrency.Text },
                        {"Date", DateTime.Now},
                    };
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Offers", data, condition);
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
                    CRUD.Delete("Offers", condition);
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
            DataTable supplierName = CRUD.Read("SELECT ID, Name FROM Suppliers WHERE IsActive=1 ORDER BY Name  ");
            comboBoxSuppliers.DataSource = supplierName;
            comboBoxSuppliers.DisplayMember = "Name";
            comboBoxSuppliers.ValueMember = "ID";
            comboBoxSuppliers.SelectedItem = null;


            DataTable RequisitionOwner = CRUD.Read
               ("SELECT " +
               "r.UserID, r.ID, " +
               "COALESCE(u.Name, '') + ' ' + COALESCE(u.Surname, '') + '(#' + CAST(u.ID AS varchar(20)) + ')' + ', ' + COALESCE(r.Item, '')  AS RequisitionsOwner " +
               "FROM Requisitions r " +
               "INNER JOIN Users u ON r.UserID=u.ID " +
               "WHERE r.Status LIKE '%Onayland%' ");
            comboBoxRequisitionOwner.DataSource = RequisitionOwner;
            comboBoxRequisitionOwner.DisplayMember = "RequisitionsOwner";
            comboBoxRequisitionOwner.ValueMember = "ID";
            comboBoxRequisitionOwner.SelectedItem = null;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                comboBoxSuppliers.Text = row.Cells["SupplierName"].Value.ToString();
                comboBoxUnit.Text = row.Cells["Unit"].Value.ToString();
                textBoxQuantity.Text = row.Cells["Quantity"].Value.ToString();                
                comboBoxCurrency.Text = row.Cells["Currency"].Value.ToString();
                textBoxItem.Text = row.Cells["Item"].Value.ToString();
                RequisitionsOwnerID=(int)row.Cells["RequisitionsOwnerID"].Value;
                comboBoxRequisitionOwner.Text= (row.Cells["RequisitionsOwner"].Value.ToString()) + ", " + (row.Cells["RItem"].Value.ToString());

                var value = row.Cells["Price"].Value.ToString();
                if (value != null && decimal.TryParse(value.ToString(), out decimal price))
                {
                    // Binlik ayraçlı olarak TextBox’a ata
                    textBoxPrice.Text = price.ToString("N0"); // 1.000
                                                                    // veya
                                                                    // textBoxPrice.Text = price.ToString("N2"); // 1.000,00
                }
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

        private void textBoxQuantity_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!decimal.TryParse(textBoxQuantity.Text, out decimal value) || value <= 0)
            {
                MessageBox.Show("Lütfen sıfırdan büyük bir sayı giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // focus TextBox'ta kalır
            }
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

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxPrice_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!decimal.TryParse(textBoxPrice.Text, out decimal value) || value <= 0)
            {
                MessageBox.Show("Lütfen sıfırdan büyük bir sayı giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // focus TextBox'ta kalır
            }
        }

        private void textBoxPrice_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxPrice.Text, out decimal value) && value > 0)
            {
                // Sayıyı binlik ayraçlı formatla (ör. 1.000 veya 1.000,00)
                textBoxPrice.Text = value.ToString("N0"); // 1.000
                                                                // textBoxPrice.Text = value.ToString("N2"); // 1.000,00
            }
            else
            {
                // Geçersiz giriş olursa temizle veya uyarı ver
                textBoxPrice.Text = string.Empty;
            }
        }
    }
}
