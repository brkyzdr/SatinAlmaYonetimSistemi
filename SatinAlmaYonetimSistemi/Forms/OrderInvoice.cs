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
    public partial class OrderInvoice : Form
    {
        public OrderInvoice()
        {
            InitializeComponent();
            ReadData();
            FillComboBox();
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

            textBoxInvoiceNum.Text = dt.Rows[0]["InvoiceNumber"].ToString();
            dateTimePickerInvoice.Text = dt.Rows[0]["InvoiceDate"].ToString();
            comboBoxSupplier.Text = dt.Rows[0]["SupplierName"].ToString();
            textBoxTotalAmount.Text = dt.Rows[0]["TotalAmount"].ToString();
            comboBoxCurrency.Text = dt.Rows[0]["Currency"].ToString();
            textBoxTax.Text = dt.Rows[0]["TaxAmount"].ToString();
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
            this.Close();
        }
    }
}
