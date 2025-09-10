using Data;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class OrderInvoice : Form
    {
        int invoiceID=0;
        int orderID=0;
        public OrderInvoice()
        {
            InitializeComponent();
            FillComboBox();
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
                "i.CreatedBy," +
                "i.OrderID " +
                "FROM Invoices i " +
                "INNER JOIN Suppliers s ON i.SupplierID = s.ID " +
                $"WHERE i.OrderID = {Session.OrderID} ");

            if (dt.Rows.Count>0)
            {
                textBoxInvoiceNum.Text = dt.Rows[0]["InvoiceNumber"].ToString();
                dateTimePickerInvoice.Text = dt.Rows[0]["InvoiceDate"].ToString();
                comboBoxSupplier.Text = dt.Rows[0]["SupplierName"].ToString();
                comboBoxCurrency.Text = dt.Rows[0]["Currency"].ToString();


                var value = dt.Rows[0]["TotalAmount"].ToString();
                if (value != null && decimal.TryParse(value.ToString(), out decimal price))
                {
                    // Binlik ayraçlı olarak TextBox’a ata
                    textBoxTotalAmount.Text = price.ToString("N0"); // 1.000
                                                                    // veya
                                                                    // textBoxPrice.Text = price.ToString("N2"); // 1.000,00
                }

                var value2 = dt.Rows[0]["TaxAmount"].ToString();
                if (value2 != null && decimal.TryParse(value2.ToString(), out decimal price2))
                {
                    // Binlik ayraçlı olarak TextBox’a ata
                    textBoxTax.Text = price2.ToString("N0"); // 1.000
                                                                    // veya
                                                                    // textBoxPrice.Text = price.ToString("N2"); // 1.000,00
                }


                if (dt.Rows[0]["ID"]!=null) invoiceID = (int)(dt.Rows[0]["ID"]);
                if(dt.Rows[0]["OrderID"]!=null) orderID = (int)(dt.Rows[0]["OrderID"]);
            }         
        }

        private void SaveData()
        {
            DataTable dtOrder = CRUD.Read(
                "SELECT " +
                "o.ID, o.Item, o.Unit, o.Quantity, o.Price, o.Currency, o.Status, o.Date, o.RequisitionsID " +
                "FROM Orders o "+
                $"WHERE o.ID ={orderID}  ");

            DialogResult result = MessageBox.Show(
                "Kaydı oluşturmak istediğinize emin misiniz?",   // Mesaj
                "Onay",                                          // Başlık
                MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                MessageBoxIcon.Question                          // Soru ikonu
            );

            if (result == DialogResult.Yes)
            {
                var dataOrder = new Dictionary<string, object>
                    {
                        {"UserID",Session.UserID  },
                        {"SupplierID",comboBoxSupplier.SelectedValue  },
                        {"RequisitionsID", dtOrder.Rows[0]["RequisitionsID"].ToString()},
                        {"Item",dtOrder.Rows[0]["Item"].ToString() },
                        {"Unit" ,dtOrder.Rows[0]["Unit"].ToString() },
                        {"Quantity" ,dtOrder.Rows[0]["Quantity"].ToString() },
                        {"Price" ,textBoxTotalAmount.Text },
                        {"Currency", comboBoxCurrency.Text},
                        {"Date", DateTime.Now},
                    };

                var dataInvoice = new Dictionary<string, object>
                    {
                        {"InvoiceNumber" ,textBoxInvoiceNum.Text },
                        {"InvoiceDate" ,dateTimePickerInvoice.Value },
                        {"SupplierID" ,comboBoxSupplier.SelectedValue },
                        {"TotalAmount" ,textBoxTotalAmount.Text },
                        {"Currency" ,comboBoxCurrency.Text },
                        {"TaxAmount" ,textBoxTax.Text },
                        {"CreatedDate" , DateTime.Now},
                        {"CreatedBy" ,Session.UserID},
                        {"OrderID" ,Session.OrderID},
                    };
                if (invoiceID == 0)
                {
                    CRUD.Create("Invoices", dataInvoice);
                    CRUD.Create("Orders", dataOrder);
                }
                else
                {
                    string condition = $"ID = '{invoiceID}'";
                    string condition2 = $"ID = '{orderID}'";

                    CRUD.Update("Invoices", dataInvoice, condition);
                    CRUD.Update("Orders", dataOrder,condition2 );
                }
                MessageBox.Show("Kayıt başarıyla oluşturuldu.", "Bilgi");
                Session.OrderID = 0;

                if (this.Owner is Orders parent)
                {
                    parent.ReadData();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Kayıt işlemi iptal edildi.", "Bilgi");
            }

        }

        private void FillComboBox()
        {
            DataTable supplierName = CRUD.Read("SELECT ID, Name FROM Suppliers ORDER BY Name");
            comboBoxSupplier.DataSource = supplierName;
            comboBoxSupplier.DisplayMember = "Name";
            comboBoxSupplier.ValueMember = "ID";
            comboBoxSupplier.SelectedItem = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Session.OrderID = 0;
            this.Close();
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

        private void textBoxTotalAmount_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!decimal.TryParse(textBoxTotalAmount.Text, out decimal value) || value <= 0)
            {
                MessageBox.Show("Lütfen sıfırdan büyük bir sayı giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // focus TextBox'ta kalır
            }
        }

        private void textBoxTax_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxTax_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!decimal.TryParse(textBoxTax.Text, out decimal value) || value <= 0)
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

        private void textBoxTax_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxTax.Text, out decimal value) && value > 0)
            {
                // Sayıyı binlik ayraçlı formatla (ör. 1.000 veya 1.000,00)
                textBoxTax.Text = value.ToString("N0"); // 1.000
                                                                // textBoxPrice.Text = value.ToString("N2"); // 1.000,00
            }
            else
            {
                // Geçersiz giriş olursa temizle veya uyarı ver
                textBoxTax.Text = string.Empty;
            }
        }
    }
}
