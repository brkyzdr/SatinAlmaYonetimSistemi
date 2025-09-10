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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            DashboardManager();
        }

        private void DashboardManager()
        {
            switch (Session.UserRole)
            {
                case "Admin":
                    buttonRequisitions.Enabled = true;
                    buttonRequisitions.Visible = true;

                    buttonOffers.Enabled = true;
                    buttonOffers.Visible = true;

                    buttonRequisitionsAnswer.Enabled = true;
                    buttonRequisitionsAnswer.Visible = true;

                    buttonOrders.Enabled = true;
                    buttonOrders.Visible = true;
                    buttonMyOrders.Enabled = true;
                    buttonMyOrders.Visible = true;

                    buttonInvoices.Enabled = true;
                    buttonInvoices.Visible = true;

                    buttonOffersAnswer.Enabled = true;
                    buttonOffersAnswer.Visible = true;

                    buttonSuppliers.Enabled = true;
                    buttonSuppliers.Visible = true;

                    buttonUsers.Enabled = true;
                    buttonUsers.Visible = true;
                    break;

                case "Normal Kullanıcı":
                    buttonRequisitions.Enabled = true;
                    buttonRequisitions.Visible = true;
                    break;

                case "Satınalma Sorumlusu":
                    buttonOffers.Enabled = true;
                    buttonOffers.Visible = true;

                    buttonRequisitionsAnswer.Enabled = true;
                    buttonRequisitionsAnswer.Visible = true;

                    buttonInvoices.Enabled = true;
                    buttonInvoices.Visible = true;

                    buttonSuppliers.Enabled = true;
                    buttonSuppliers.Visible = true;

                    buttonMyOrders.Enabled = true;
                    buttonMyOrders.Visible = true;
                    break;

                case "Patron":
                    buttonOffersAnswer.Enabled = true;
                    buttonOffersAnswer.Visible = true;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Requisitions myRequisitions = new Requisitions();
            myRequisitions.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RequisitionsAnswer requests = new RequisitionsAnswer();
            requests.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Invoices rtocks = new Invoices();
            rtocks.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Offers offers = new Offers();
            offers.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Suppliers suppliers = new Suppliers();
            suppliers.Show();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Orders orders = new Orders();
            orders.Show();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OffersAnswer purchaseOrders = new OffersAnswer();
            purchaseOrders.Show();
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.Show();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button99_Click(object sender, EventArgs e)
        {
            MyOrders myOrders = new MyOrders();
            myOrders.Show();
            this.Close();
        }

        private void buttonProfil_Click(object sender, EventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
        }
    }
}
