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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.AcceptButton = button1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserAuth();           
        }

        private void UserAuth()
        {
            string userName = (textBoxUsername.Text ?? string.Empty).Trim();
            string password = textBoxPassword.Text ?? string.Empty;

            string uEsc = userName.Replace("'", "''");
            string pEsc = password.Replace("'", "''");

            DataTable dt = CRUD.Read($@"SELECT TOP 1 [ID], [Username] FROM [Users] WITH (NOLOCK) WHERE [Username] = '{uEsc}' AND [PasswordHash] = '{pEsc}';");

            if (!string.IsNullOrEmpty(textBoxUsername.Text) && !string.IsNullOrEmpty(textBoxPassword.Text))
            {
                if (dt != null && dt.Rows.Count == 1)
                {
                    var row = dt.Rows[0];
                   
                    Session.UserID = Convert.ToInt32(row["ID"]);
                    Session.Username = Convert.ToString(row["Username"]);
                    
                    var dashboard = new Dashboard();
                    dashboard.Show();
                    this.Hide();
                }            
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.");
            }
        }
    }
}
