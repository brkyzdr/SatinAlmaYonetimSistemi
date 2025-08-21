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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyRequisitions myRequisitions = new MyRequisitions();
            myRequisitions.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Requests requests = new Requests();
            requests.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stocks rtocks = new Stocks();
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
            PurchaseReq purchaseOrders = new PurchaseReq();
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
    }
}
