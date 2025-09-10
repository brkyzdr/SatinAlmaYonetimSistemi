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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class Invoices : Form
    {
        public Invoices()
        {
            InitializeComponent();
            SetComboBoxData();
            ReadData();   
        }

        private void ReadData()
        {
            DataTable dt = CRUD.Read
                ("SELECT " +
                "i.InvoiceNumber," +
                "i.ID," +
                "i.InvoiceDate," +
                "s.Name AS SupplierName," +
                "i.TotalAmount," +
                "i.Currency," +
                "i.TaxAmount," +
                "i.CreatedDate," +
                "i.CreatedBy " +
                "FROM Invoices i " +
                "INNER JOIN Suppliers s ON i.SupplierID = s.ID");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            
            dataGridView1.Columns["InvoiceNumber"].HeaderText = "Fatura Numarası";
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["InvoiceDate"].HeaderText = "Fatura Tarihi";
            dataGridView1.Columns["SupplierName"].HeaderText = "Tedarikçi";
            dataGridView1.Columns["TotalAmount"].HeaderText = "Toplam Tutar";
            dataGridView1.Columns["Currency"].HeaderText = "Para Birimi";
            dataGridView1.Columns["TaxAmount"].HeaderText = "Toplam Vergi";
            dataGridView1.Columns["CreatedDate"].HeaderText = "Oluşturma Tarihi";
            dataGridView1.Columns["CreatedBy"].HeaderText = "Oluşturan Kişi";
        }
        
        private void CreateData()
        {
            if (!string.IsNullOrEmpty(textBoxInvoiceNum.Text)
                && !string.IsNullOrEmpty(dateTimeInvoice.Text)
                && !string.IsNullOrEmpty(comboBoxSuppliers.Text)
                && !string.IsNullOrEmpty(textBoxTotalAmount.Text)
                && !string.IsNullOrEmpty(comboBoxCurrency.Text)
                && !string.IsNullOrEmpty(textBoxTotalTax.Text))
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
                        {"InvoiceNumber" ,textBoxInvoiceNum.Text },
                        {"InvoiceDate" ,dateTimeInvoice.Value },
                        {"SupplierID" ,comboBoxSuppliers.SelectedValue },
                        {"TotalAmount" ,textBoxTotalAmount.Text },
                        {"Currency" ,comboBoxCurrency.Text },
                        {"TaxAmount" ,textBoxTotalTax.Text },
                        {"CreatedDate" , DateTime.Now},
                        {"CreatedBy" ,Session.UserID},
                    };
                    CRUD.Create("Invoices", data);

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
                        {"InvoiceNumber" ,textBoxInvoiceNum.Text },
                        {"InvoiceDate" ,dateTimeInvoice.Value },
                        {"SupplierID" ,comboBoxSuppliers.SelectedValue },
                        {"TotalAmount" ,textBoxTotalAmount.Text },
                        {"Currency" ,comboBoxCurrency.Text },
                        {"TaxAmount" ,textBoxTotalTax.Text },
                        {"CreatedDate" , DateTime.Now},
                        {"CreatedBy" ,Session.UserID},
                    };
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Invoices", data, condition);
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
                    CRUD.Delete("Invoices", condition);
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
            DataTable supplierName = CRUD.Read("SELECT ID, Name FROM Suppliers ORDER BY Name");
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
                textBoxInvoiceNum.Text = row.Cells["InvoiceNumber"].Value.ToString();
                dateTimeInvoice.Text = row.Cells["InvoiceDate"].Value.ToString();
                comboBoxSuppliers.Text = row.Cells["SupplierName"].Value.ToString();

                var value = row.Cells["TotalAmount"].Value.ToString();
                if (value != null && decimal.TryParse(value.ToString(), out decimal price))
                {
                    // Binlik ayraçlı olarak TextBox’a ata
                    textBoxTotalAmount.Text = price.ToString("N0"); // 1.000
                                                              // veya
                                                              // textBoxPrice.Text = price.ToString("N2"); // 1.000,00
                }

                comboBoxCurrency.Text = row.Cells["Currency"].Value.ToString();

                var value2 = row.Cells["TaxAmount"].Value.ToString();
                if (value2 != null && decimal.TryParse(value2.ToString(), out decimal price2))
                {
                    // Binlik ayraçlı olarak TextBox’a ata
                    textBoxTotalTax.Text = price2.ToString("N0"); // 1.000
                                                                    // veya
                                                                    // textBoxPrice.Text = price.ToString("N2"); // 1.000,00
                }
            }
        }

        private void textBoxTotalAmount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxTotalAmount_Validating(object sender, CancelEventArgs e)
        {
            if (!decimal.TryParse(textBoxTotalAmount.Text, out decimal value) || value <= 0)
            {
                MessageBox.Show("Lütfen sıfırdan büyük bir sayı giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // focus TextBox'ta kalır
            }
        }

        private void textBoxTotalTax_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxTotalTax_Validating(object sender, CancelEventArgs e)
        {
            if (!decimal.TryParse(textBoxTotalTax.Text, out decimal value) || value <= 0)
            {
                MessageBox.Show("Lütfen sıfırdan büyük bir sayı giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // focus TextBox'ta kalır
            }
        }

        private void textBoxTotalAmount_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxTotalAmount.Text, out decimal value) && value > 0)
            {
                // Sayıyı binlik ayraçlı formatla (ör. 1.000 veya 1.000,00)
                textBoxTotalAmount.Text = value.ToString("N0"); // 1.000
                                                          // textBoxPrice.Text = value.ToString("N2"); // 1.000,00
            }
            else
            {
                // Geçersiz giriş olursa temizle veya uyarı ver
                textBoxTotalAmount.Text = string.Empty;
            }
        }

        private void textBoxTotalTax_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxTotalTax.Text, out decimal value) && value > 0)
            {
                // Sayıyı binlik ayraçlı formatla (ör. 1.000 veya 1.000,00)
                textBoxTotalTax.Text = value.ToString("N0"); // 1.000
                                                                // textBoxPrice.Text = value.ToString("N2"); // 1.000,00
            }
            else
            {
                // Geçersiz giriş olursa temizle veya uyarı ver
                textBoxTotalTax.Text = string.Empty;
            }
        }
    }
}
