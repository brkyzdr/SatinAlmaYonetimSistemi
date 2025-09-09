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
using System.Windows.Forms.VisualStyles;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class OffersAnswer : Form
    {
        int ReqID;

        public OffersAnswer()
        {
            InitializeComponent();
            SetCombobox();
        }

        private void ReadData(int id)
        {
            DataTable dt = CRUD.Read(
                "SELECT " +
                "o.ID, s.Name AS SupplierName, o.Item, o.Quantity, o.Unit, o.Price, o.Currency, o.Date, o.RequisitionsID, " +
                "COALESCE(u.Name, '') + ' ' + COALESCE(u.Surname, '') + '(#' + CAST(u.ID AS varchar(20)) + ')' AS PurchaseName " +
                "FROM Offers o " +
                "INNER JOIN Suppliers s ON o.SupplierID = s.ID " +
                "INNER JOIN Users u ON o.UserID = u.ID " +
                $"WHERE o.RequisitionsID = {id} ");

            dataGridView.DataSource = dt;
            dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView.AutoGenerateColumns = true;

            dataGridView.Columns["ID"].Visible = false;
            dataGridView.Columns["RequisitionsID"].Visible = false;
            dataGridView.Columns["SupplierName"].HeaderText = "Tedarikçi";
            dataGridView.Columns["Item"].HeaderText = "Ürün";
            dataGridView.Columns["Unit"].HeaderText = "Birim";
            dataGridView.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView.Columns["Price"].HeaderText = "Fiyat";
            dataGridView.Columns["Currency"].HeaderText = "Para Birimi";
            dataGridView.Columns["PurchaseName"].HeaderText = "Satınalma Sorumlusu";
            dataGridView.Columns["Date"].HeaderText = "Teklif Tarihi";
        }

        //private void UpdateData()
        //{
        //    if (dataGridView.SelectedRows.Count > 0)
        //    {
        //        DialogResult result = MessageBox.Show(
        //        "Kayıt etmek istediğinize emin misiniz?",       // Mesaj
        //        "Onay",                                          // Başlık
        //        MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
        //        MessageBoxIcon.Question                          // Soru ikonu
        //        );


        //        if (result == DialogResult.Yes)
        //        {
        //            var data = new Dictionary<string, object>
        //            {
        //                {"ApprovedByID",Session.UserID },
        //                {"Status" , "Onaylandı"},
        //            };
        //            string condition = $"ID = '{dataGridView.SelectedRows[0].Cells["ID"].Value}'";
        //            CRUD.Update("Offers", data, condition);
        //            MessageBox.Show("Kayıt başarıyla yapıldı.", "Bilgi");
        //            ReadData(ReqID);
        //        }
        //        else
        //        {
        //            MessageBox.Show("İşlem iptal edildi.", "Bilgi");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Lütfen satır seçin.");
        //    }
        //}

        private void UpdateData()
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Onaylamak istediğinize emin misiniz?",
                    "Onay",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // Seçilen satırın ID ve RequisitionsID değerleri
                    int selectedOfferID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["ID"].Value);
                    int requisitionID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["RequisitionsID"].Value);

                    // 1. Seçili satır → Onaylandı
                    var approveData = new Dictionary<string, object>
            {
                { "ApprovedByID", Session.UserID },
                { "Status", "Onaylandı" }
            };
                    string approveCondition = $"ID = {selectedOfferID}";
                    CRUD.Update("Offers", approveData, approveCondition);

                    // 2. Aynı RequisitionsID’deki diğer satırlar → Reddedildi
                    var rejectData = new Dictionary<string, object>
            {
                { "ApprovedByID", Session.UserID },
                { "Status", "Reddedildi" }
            };
                    string rejectCondition = $"RequisitionsID = {requisitionID} AND ID <> {selectedOfferID}";
                    CRUD.Update("Offers", rejectData, rejectCondition);

                    MessageBox.Show("Onaylama işlemi başarılı.", "Bilgi");
                    SetCombobox();
                    dataGridView.DataSource = null;

                    textBoxSupplier.Enabled = false;
                    textBoxCurrency.Enabled = false;
                    textBoxItem.Enabled = false;
                    textBoxPrice.Enabled = false;
                    textBoxQuantity.Enabled = false;
                    textBoxUnit.Enabled = false;
                    label1.Enabled = false;

                    textBoxSupplier.Visible = false;
                    textBoxCurrency.Visible = false;
                    textBoxItem.Visible = false;
                    textBoxPrice.Visible = false;
                    textBoxQuantity.Visible = false;
                    textBoxUnit.Visible = false;
                    label1.Visible = false;
                }
                else
                {
                    MessageBox.Show("İşlem iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen satır seçin.");
            }
        }

        private void SetCombobox()
        {
            comboBoxOffersAnswer.Enabled = true;
            DataTable dt = CRUD.Read(
                "SELECT " +
                "o.RequisitionsID, " +
                "COALESCE(o.Item, '') + ' | ' + COALESCE(u.Name, '') + ' ' + COALESCE(u.Surname, '') + '(#' + CAST(u.ID AS varchar(20)) +')' AS RequisitionsOwner " +
                "FROM Offers o " +
                "INNER JOIN Requisitions r  ON o.RequisitionsID = r.ID " +
                "INNER JOIN Users u ON o.UserID=u.ID " +
                "WHERE o.Status='Onay Bekliyor' ");

            DataView dv = new DataView(dt);
            DataTable distinct = dv.ToTable(true, "RequisitionsID", "RequisitionsOwner");

            comboBoxOffersAnswer.DataSource = distinct;
            comboBoxOffersAnswer.DisplayMember = "RequisitionsOwner";
            comboBoxOffersAnswer.ValueMember = "RequisitionsID";
            comboBoxOffersAnswer.SelectedIndex = -1;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonSave.Enabled = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                textBoxSupplier.Text = row.Cells["SupplierName"].Value.ToString();
                textBoxItem.Text = row.Cells["Item"].Value.ToString();
                textBoxQuantity.Text = row.Cells["Quantity"].Value.ToString();
                textBoxUnit.Text = row.Cells["Unit"].Value.ToString();
                textBoxPrice.Text = row.Cells["Price"].Value.ToString();
                textBoxCurrency.Text = row.Cells["Currency"].Value.ToString();
            }

            textBoxSupplier.Enabled = true;
            textBoxCurrency.Enabled = true;
            textBoxItem.Enabled = true;
            textBoxPrice.Enabled = true;
            textBoxQuantity.Enabled = true;
            textBoxUnit.Enabled = true;
            label1.Enabled = true;

            textBoxSupplier.Visible = true;
            textBoxCurrency.Visible = true;
            textBoxItem.Visible = true;
            textBoxPrice.Visible = true;
            textBoxQuantity.Visible = true;
            textBoxUnit.Visible = true;
            label1.Visible = true;
        }

        private void comboBoxOffersAnswer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOffersAnswer.SelectedValue != null && int.TryParse(comboBoxOffersAnswer.SelectedValue.ToString(), out int reqID) && reqID != 0)
            {
                ReqID = reqID;
                ReadData(ReqID);
            }


        }

        //private void buttonRefuse_Click(object sender, EventArgs e)
        //{
        //    if (dataGridView.SelectedRows.Count > 0)
        //    {
        //        DialogResult result = MessageBox.Show(
        //            "Tüm teklifleri reddetmek istediğinize emin misiniz?",
        //            "Onay",
        //            MessageBoxButtons.YesNo,
        //            MessageBoxIcon.Question
        //        );

        //        if (result == DialogResult.Yes)
        //        {
        //            // Seçilen satırın ID ve RequisitionsID değerleri
        //            int selectedOfferID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["ID"].Value);
        //            int requisitionID = Convert.ToInt32(dataGridView.SelectedRows[0].Cells["RequisitionsID"].Value);

        //            // 1. Seçili satır → Onaylandı
        //            var approveData = new Dictionary<string, object>
        //    {
        //        { "ApprovedByID", Session.UserID },
        //        { "Status", "Onaylandı" }
        //    };
        //            string approveCondition = $"ID = {selectedOfferID}";
        //            CRUD.Update("Offers", approveData, approveCondition);

        //            // 2. Aynı RequisitionsID’deki diğer satırlar → Reddedildi
        //            var rejectData = new Dictionary<string, object>
        //    {
        //        { "ApprovedByID", Session.UserID },
        //        { "Status", "Reddedildi" }
        //    };
        //            string rejectCondition = $"RequisitionsID = {requisitionID} AND ID <> {selectedOfferID}";
        //            CRUD.Update("Offers", rejectData, rejectCondition);

        //            MessageBox.Show("Onaylama işlemi başarılı.", "Bilgi");
        //            SetCombobox();
        //            dataGridView.DataSource = null;

        //            textBoxSupplier.Enabled = false;
        //            textBoxCurrency.Enabled = false;
        //            textBoxItem.Enabled = false;
        //            textBoxPrice.Enabled = false;
        //            textBoxQuantity.Enabled = false;
        //            textBoxUnit.Enabled = false;
        //            label1.Enabled = false;

        //            textBoxSupplier.Visible = false;
        //            textBoxCurrency.Visible = false;
        //            textBoxItem.Visible = false;
        //            textBoxPrice.Visible = false;
        //            textBoxQuantity.Visible = false;
        //            textBoxUnit.Visible = false;
        //            label1.Visible = false;
        //        }
        //        else
        //        {
        //            MessageBox.Show("İşlem iptal edildi.", "Bilgi");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Lütfen satır seçin.");
        //    }
        //}

        private void buttonRefuse_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Tüm teklifleri reddetmek istediğinize emin misiniz?",
                    "Onay",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // Griddeki ilk satırdan RequisitionsID alıyoruz (hepsi aynı ReqID'ye bağlı zaten)
                    int requisitionID = Convert.ToInt32(dataGridView.Rows[0].Cells["RequisitionsID"].Value);

                    // Tüm satırlar için → Reddedildi
                    var rejectData = new Dictionary<string, object>
            {
                { "ApprovedByID", Session.UserID },
                { "Status", "Reddedildi" }
            };

                    string rejectCondition = $"RequisitionsID = {requisitionID}";
                    CRUD.Update("Offers", rejectData, rejectCondition);

                    MessageBox.Show("Tüm teklifler reddedildi.", "Bilgi");

                    // Grid ve combobox yenileniyor
                    SetCombobox();
                    dataGridView.DataSource = null;

                    // Textbox ve label kontrollerini pasif yap
                    textBoxSupplier.Enabled = false;
                    textBoxCurrency.Enabled = false;
                    textBoxItem.Enabled = false;
                    textBoxPrice.Enabled = false;
                    textBoxQuantity.Enabled = false;
                    textBoxUnit.Enabled = false;
                    label1.Enabled = false;

                    textBoxSupplier.Visible = false;
                    textBoxCurrency.Visible = false;
                    textBoxItem.Visible = false;
                    textBoxPrice.Visible = false;
                    textBoxQuantity.Visible = false;
                    textBoxUnit.Visible = false;
                    label1.Visible = false;
                }
                else
                {
                    MessageBox.Show("İşlem iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Tabloda hiç satır yok.");
            }
        }

    }
}
