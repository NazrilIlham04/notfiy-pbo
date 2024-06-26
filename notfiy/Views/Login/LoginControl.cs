﻿using Krypton.Toolkit;
using Newtonsoft.Json;
using notfiy.Controllers;
using notfiy.Core;
using notfiy.Helpers;
using notfiy.Views.Homepage;
using notfiy.Views.Register;
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
using NotifyViewManager = notfiy.Core.ViewManager;

namespace notfiy.Views.Login
{
    public partial class LoginControl : UserControl
    {
        private UserController UserController;
        private MessageBoxHelper MessageBoxHelper;
        private System.Windows.Forms.Timer timer;
        public LoginControl()
        {
            InitializeComponent();
            UserController = new UserController();
            MessageBoxHelper = new MessageBoxHelper();
        }
        private void LoginControl_Load(object sender, EventArgs e)
        {
            this.Width = this.ClientSize.Width;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 2000;
            timer.Tick += Timer_Tick;
            EventHandler eventHandler = ControlLoaded;
            this.Load += eventHandler;
        }

        private void ControlLoaded(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            //pictureBox1.Hide();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (UserController.AuthAttempt(UsernameTextbox.Text, kryptonTextBox1.Text))
            {
                HomepageControl homepage = new HomepageControl();
                NotifyViewManager.MoveView(homepage);
            }
            else
            {
                if (UsernameTextbox.Text == "Username" || kryptonTextBox1.Text == "Password" || string.IsNullOrWhiteSpace(kryptonTextBox1.Text) || string.IsNullOrWhiteSpace(UsernameTextbox.Text))
                {
                    MessageBoxHelper.ShowInfoMessageBox("Mohon lengkapi data terlebih dahulu!");
                    UsernameTextbox.Text = "Username";
                    kryptonTextBox1.Text = "Password";
                    kryptonTextBox1.PasswordChar = '\0';
                }
                else
                {
                    MessageBoxHelper.ShowCustomMessageBox("Username atau Password Salah", "Login Gagal!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    kryptonTextBox1.Text = "Password";
                    kryptonTextBox1.PasswordChar = '\0';
                }
            }
        }
        private void kryptonTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (UserController.AuthAttempt(UsernameTextbox.Text, kryptonTextBox1.Text))
                {
                    HomepageControl homepage = new HomepageControl();
                    NotifyViewManager.MoveView(homepage);
                }
                else
                {
                    if (UsernameTextbox.Text == "Username" || kryptonTextBox1.Text == "Password" || string.IsNullOrWhiteSpace(kryptonTextBox1.Text) || string.IsNullOrWhiteSpace(UsernameTextbox.Text))
                    {
                        MessageBoxHelper.ShowInfoMessageBox("Mohon lengkapi data terlebih dahulu!");
                        UsernameTextbox.Text = "Username";
                        kryptonTextBox1.Text = "Password";
                        kryptonTextBox1.PasswordChar = '\0';
                    }
                    else
                    {
                        MessageBoxHelper.ShowCustomMessageBox("Username atau Password Salah", "Login Gagal!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        kryptonTextBox1.Text = "Password";
                        kryptonTextBox1.PasswordChar = '\0';
                    }
                }
            }
        }
        private void UsernameTextbox_Enter(object sender, EventArgs e)
        {
            if (UsernameTextbox.Text == "Username")
            {
                UsernameTextbox.Text = "";
            }
        }

        private void UsernameTextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextbox.Text))
            {
                UsernameTextbox.Text = "Username";
            }
        }

        private void kryptonTextBox1_Enter(object sender, EventArgs e)
        {
            if (kryptonTextBox1.Text == "Password")
            {
                kryptonTextBox1.Text = "";
                kryptonTextBox1.PasswordChar = '●';
            }
        }
        private void kryptonTextBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(kryptonTextBox1.Text))
            {
                kryptonTextBox1.Text = "Password";
                kryptonTextBox1.PasswordChar = '\0';
            }
        }

        private void BuatAkunLabel_Click(object sender, EventArgs e)
        {
            RegisterControl reg = new RegisterControl();
            NotifyViewManager.MoveView(reg);
        }

        private void kryptonPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                kryptonTextBox1.Focus();
            }
        }

        private void UsernameTextbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
