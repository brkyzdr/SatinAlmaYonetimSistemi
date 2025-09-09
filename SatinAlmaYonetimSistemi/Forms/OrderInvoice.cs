﻿using Data;
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
                "i.CreatedBy " +
                "FROM Invoices i " +
                "INNER JOIN Suppliers s ON i.SupplierID = s.ID " +
                $"WHERE i.OrderID = {Session.OrderID} ");

            if (dt.Rows.Count>0)
            {
                textBoxInvoiceNum.Text = dt.Rows[0]["InvoiceNumber"].ToString();
                dateTimePickerInvoice.Text = dt.Rows[0]["InvoiceDate"].ToString();
                comboBoxSupplier.Text = dt.Rows[0]["SupplierName"].ToString();
                textBoxTotalAmount.Text = dt.Rows[0]["TotalAmount"].ToString();
                comboBoxCurrency.Text = dt.Rows[0]["Currency"].ToString();
                textBoxTax.Text = dt.Rows[0]["TaxAmount"].ToString();
                if(dt.Rows[0]["ID"]!=null) invoiceID = (int)(dt.Rows[0]["ID"]);
            }         
        }

        private void SaveData()
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
                    CRUD.Create("Invoices", data);
                else
                {
                    string condition = $"ID = '{invoiceID}'";
                    CRUD.Update("Invoices", data, condition);
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
    }
}
