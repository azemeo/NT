using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using Blumind.Globalization;
using Blumind.Core;

namespace Blumind
{
    public partial class LicenseForm : Form
    {
        private string strLicenseKey = Security.FingerPrint.GetMD5Hash(Security.FingerPrint.Value() + Security.FingerPrint.GetMD5Hash("BowTie Presenter"));
        string appName = "BowTie Presenter";
        string serialKey = "SerialKey";

        public LicenseForm()
        {
            InitializeComponent();
            labelFingerPrint.Text = Lang._("Fingerprint");
            labelSerialKey.Text = Lang._("Serial Key");
            btnActive.Text = Lang._("Activate");
            btnQuit.Text = Lang._("Quit");
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            tbFingerPrint.Text = Security.FingerPrint.Value();

            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", false);
            if (key.ContainsSubKey(appName))
            {
                key = key.OpenSubKey(appName);
                object value = key.GetValue(serialKey);
                if (value != null)
                {
                    string strSavedLicenseKey = value.ToString();
                    if (strSavedLicenseKey == strLicenseKey)
                    {
                        this.DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        tbSerialKey.Text = strSavedLicenseKey;
                    }
                }
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            if (tbSerialKey.Text == strLicenseKey)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                key = !key.ContainsSubKey(appName) ? key.CreateSubKey(appName) : key.OpenSubKey(appName, true);
                key.SetValue(serialKey, strLicenseKey);

                MessageBox.Show(Lang._("Activated Successful!"), "BowTie Presenter");
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(Lang._("Invalid Serial Key!"), "BowTie Presenter");
            }
        }
    }
}
