namespace Blumind
{
    partial class LicenseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelFingerPrint = new System.Windows.Forms.Label();
            this.labelSerialKey = new System.Windows.Forms.Label();
            this.tbFingerPrint = new System.Windows.Forms.TextBox();
            this.tbSerialKey = new System.Windows.Forms.TextBox();
            this.btnActive = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelFingerPrint
            // 
            this.labelFingerPrint.AutoSize = true;
            this.labelFingerPrint.Location = new System.Drawing.Point(12, 16);
            this.labelFingerPrint.Name = "labelFingerPrint";
            this.labelFingerPrint.Size = new System.Drawing.Size(82, 13);
            this.labelFingerPrint.TabIndex = 0;
            this.labelFingerPrint.Text = "Activation Code";
            // 
            // labelSerialKey
            // 
            this.labelSerialKey.AutoSize = true;
            this.labelSerialKey.Location = new System.Drawing.Point(12, 45);
            this.labelSerialKey.Name = "labelSerialKey";
            this.labelSerialKey.Size = new System.Drawing.Size(54, 13);
            this.labelSerialKey.TabIndex = 1;
            this.labelSerialKey.Text = "Serial Key";
            // 
            // tbFingerPrint
            // 
            this.tbFingerPrint.Location = new System.Drawing.Point(100, 13);
            this.tbFingerPrint.Name = "tbFingerPrint";
            this.tbFingerPrint.ReadOnly = true;
            this.tbFingerPrint.Size = new System.Drawing.Size(410, 20);
            this.tbFingerPrint.TabIndex = 2;
            // 
            // tbSerialKey
            // 
            this.tbSerialKey.Location = new System.Drawing.Point(100, 42);
            this.tbSerialKey.Name = "tbSerialKey";
            this.tbSerialKey.Size = new System.Drawing.Size(410, 20);
            this.tbSerialKey.TabIndex = 3;
            // 
            // btnActive
            // 
            this.btnActive.Location = new System.Drawing.Point(354, 75);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(75, 23);
            this.btnActive.TabIndex = 4;
            this.btnActive.Text = "Activate";
            this.btnActive.UseVisualStyleBackColor = true;
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(435, 75);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 5;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // LicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 110);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnActive);
            this.Controls.Add(this.tbSerialKey);
            this.Controls.Add(this.tbFingerPrint);
            this.Controls.Add(this.labelSerialKey);
            this.Controls.Add(this.labelFingerPrint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "LicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BowTie Presenter";
            this.Load += new System.EventHandler(this.LicenseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFingerPrint;
        private System.Windows.Forms.Label labelSerialKey;
        private System.Windows.Forms.TextBox tbFingerPrint;
        private System.Windows.Forms.TextBox tbSerialKey;
        private System.Windows.Forms.Button btnActive;
        private System.Windows.Forms.Button btnQuit;
    }
}