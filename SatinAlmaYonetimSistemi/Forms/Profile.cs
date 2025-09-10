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
    public partial class Profile : Form
    {
        public Profile()
        {
            InitializeComponent();
            ReadData();
        }
        
        private void ReadData()
        {
            DataTable dt = CRUD.Read("SELECT " +
               "u.ID, u.Username, u.PasswordHash " +
               "FROM Users u " +
               $"WHERE u.ID = {Session.UserID}");


            labelusername.Text = dt.Rows[0]["Username"].ToString();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string current = (textBoxPassword.Text ?? string.Empty);
            string newPwd = (textBoxNewPassword.Text ?? string.Empty);
            string confirm = (textBoxNewPasswordConfirm.Text ?? string.Empty);

            // 0) Basit kontroller
            if (string.IsNullOrWhiteSpace(current) ||
                string.IsNullOrWhiteSpace(newPwd) ||
                string.IsNullOrWhiteSpace(confirm))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }
            if (newPwd.Length < 8)
            {
                MessageBox.Show("Yeni şifre en az 8 karakter olmalı.");
                return;
            }
            if (!string.Equals(newPwd, confirm, StringComparison.Ordinal))
            {
                MessageBox.Show("Yeni şifre ile doğrulama uyuşmuyor.");
                return;
            }

            int userId = Session.UserID; // giriş yapmış kullanıcının ID’si

            // 1) Kullanıcının mevcut hash’ini çek
            DataTable dt = CRUD.Read(
                "SELECT TOP 1 [PasswordHash]" +
                "FROM [Users] WITH (NOLOCK)" +
                $"WHERE [ID] = {Session.UserID}");

            if (dt == null || dt.Rows.Count != 1)
            {
                MessageBox.Show("Kullanıcı bulunamadı.");
                return;
            }

            string stored = Convert.ToString(dt.Rows[0]["PasswordHash"] ?? string.Empty);

            // 2) Mevcut şifre doğru mu?
            if (!PasswordHasher.Verify(current, stored))
            {
                MessageBox.Show("Mevcut şifre hatalı.");
                return;
            }

            // 3) Yeni şifre, eskisiyle aynı olmasın
            if (PasswordHasher.Verify(newPwd, stored))
            {
                MessageBox.Show("Yeni şifre, mevcut şifreyle aynı olamaz.");
                return;
            }

            // 4) Yeni hash üret
            string newHash = PasswordHasher.CreateHash(newPwd);

            // 5) DB’de güncelle (parametreli)
            int affected = CRUD.ExecuteNonQuery(@"
        UPDATE [Users]
           SET [PasswordHash] = @ph
         WHERE [ID] = @id",
                new Dictionary<string, object>
                {
            { "@ph", newHash },
            { "@id", userId }
                });

            if (affected == 1)
            {
                MessageBox.Show("Şifreniz başarıyla güncellendi.");
                textBoxPassword.Clear();
                textBoxNewPassword.Clear();
                textBoxNewPasswordConfirm.Clear();
            }
            else
            {
                MessageBox.Show("Güncelleme yapılamadı.");
            }
        }
    }
}
