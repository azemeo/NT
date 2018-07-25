namespace Blumind
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.taskBar1 = new Blumind.Controls.TaskBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.mdiWorkSpace1 = new Blumind.Controls.MdiWorkSpace();
            this.StartMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRecentFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelps = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuShortcuts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuMailToMe = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHomePage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuCheckUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.accelaratorKeyMap1 = new Blumind.Core.HotKeyMap();
            this.HelpMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuQuickHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDonation = new System.Windows.Forms.ToolStripMenuItem();
            this.StartMenu.SuspendLayout();
            this.HelpMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // taskBar1
            // 
            this.taskBar1.AeroBackground = false;
            this.taskBar1.BaseLineSize = 3;
            this.taskBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.taskBar1.Location = new System.Drawing.Point(0, 0);
            this.taskBar1.Name = "taskBar1";
            this.taskBar1.Size = new System.Drawing.Size(658, 30);
            this.taskBar1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.ShowReadOnly = true;
            // 
            // mdiWorkSpace1
            // 
            this.mdiWorkSpace1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mdiWorkSpace1.Location = new System.Drawing.Point(0, 30);
            this.mdiWorkSpace1.Name = "mdiWorkSpace1";
            this.mdiWorkSpace1.Size = new System.Drawing.Size(658, 367);
            this.mdiWorkSpace1.TabIndex = 3;
            // 
            // StartMenu
            // 
            this.StartMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuNew,
            this.MenuOpen,
            this.MenuRecentFiles,
            this.toolStripSeparator1,
            this.MenuSave,
            this.MenuSaveAs,
            this.toolStripSeparator8,
            this.toolStripMenuItem1,
            this.toolStripSeparator5,
            //this.MenuPreview,
            //this.MenuPrint,
            //this.toolStripSeparator6,
            this.MenuOptions,
            //this.MenuHelps,
            //this.toolStripSeparator2,
            this.MenuExit});
            this.StartMenu.Name = "MenuStart";
            this.StartMenu.Size = new System.Drawing.Size(137, 276);
            this.StartMenu.Opening += new System.ComponentModel.CancelEventHandler(this.StartMenu_Opening);
            // 
            // MenuNew
            // 
            this.MenuNew.Image = global::Blumind.Properties.Resources._new;
            this.MenuNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuNew.Name = "MenuNew";
            this.MenuNew.Size = new System.Drawing.Size(136, 22);
            this.MenuNew.Text = "&New";
            this.MenuNew.Click += new System.EventHandler(this.MenuNew_Click);
            // 
            // MenuOpen
            // 
            this.MenuOpen.Image = global::Blumind.Properties.Resources.open;
            this.MenuOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.Size = new System.Drawing.Size(136, 22);
            this.MenuOpen.Text = "&Open";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuRecentFiles
            // 
            this.MenuRecentFiles.Name = "MenuRecentFiles";
            this.MenuRecentFiles.Size = new System.Drawing.Size(136, 22);
            this.MenuRecentFiles.Text = "&Recent Files";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // MenuSave
            // 
            this.MenuSave.Image = global::Blumind.Properties.Resources.save;
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.Size = new System.Drawing.Size(136, 22);
            this.MenuSave.Text = "Save";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuSaveAs
            // 
            this.MenuSaveAs.Name = "MenuSaveAs";
            this.MenuSaveAs.Size = new System.Drawing.Size(136, 22);
            this.MenuSaveAs.Text = "Save &as...";
            this.MenuSaveAs.Click += new System.EventHandler(this.MenuSaveAs_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(133, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem1.Text = "Export...";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.MenuExportDocument_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(133, 6);
            // 
            // MenuPreview
            // 
            this.MenuPreview.Name = "MenuPreview";
            this.MenuPreview.Size = new System.Drawing.Size(136, 22);
            this.MenuPreview.Text = "Pre&view";
            this.MenuPreview.Click += new System.EventHandler(this.MenuPreview_Click);
            // 
            // MenuPrint
            // 
            this.MenuPrint.Image = global::Blumind.Properties.Resources.print;
            this.MenuPrint.Name = "MenuPrint";
            this.MenuPrint.Size = new System.Drawing.Size(136, 22);
            this.MenuPrint.Text = "&Print";
            this.MenuPrint.Click += new System.EventHandler(this.MenuPrint_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(133, 6);
            // 
            // MenuOptions
            // 
            this.MenuOptions.Image = global::Blumind.Properties.Resources.preferences;
            this.MenuOptions.Name = "MenuOptions";
            this.MenuOptions.Size = new System.Drawing.Size(136, 22);
            this.MenuOptions.Text = "Options...";
            this.MenuOptions.Click += new System.EventHandler(this.MenuOptions_Click);
            // 
            // MenuHelps
            // 
            this.MenuHelps.Name = "MenuHelps";
            this.MenuHelps.Size = new System.Drawing.Size(136, 22);
            this.MenuHelps.Text = "&Help";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(133, 6);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(136, 22);
            this.MenuExit.Text = "E&xit";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // MenuImport
            // 
            this.MenuImport.Name = "MenuImport";
            this.MenuImport.Size = new System.Drawing.Size(136, 22);
            this.MenuImport.Text = "Import";
            // 
            // MenuShortcuts
            // 
            this.MenuShortcuts.Image = global::Blumind.Properties.Resources.keyboard;
            this.MenuShortcuts.Name = "MenuShortcuts";
            this.MenuShortcuts.Size = new System.Drawing.Size(192, 22);
            this.MenuShortcuts.Text = "Accelerator Keys Table";
            this.MenuShortcuts.Click += new System.EventHandler(this.MenuShortcuts_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 6);
            // 
            // MenuMailToMe
            // 
            this.MenuMailToMe.Name = "MenuMailToMe";
            this.MenuMailToMe.Size = new System.Drawing.Size(32, 19);
            // 
            // MenuHomePage
            // 
            this.MenuHomePage.Name = "MenuHomePage";
            this.MenuHomePage.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 6);
            // 
            // MenuCheckUpdate
            // 
            this.MenuCheckUpdate.Name = "MenuCheckUpdate";
            this.MenuCheckUpdate.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 6);
            // 
            // MenuAbout
            // 
            this.MenuAbout.Name = "MenuAbout";
            this.MenuAbout.Size = new System.Drawing.Size(32, 19);
            // 
            // HelpMenu
            // 
            this.HelpMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuShortcuts});
            this.HelpMenu.Name = "contextMenuStrip1";
            this.HelpMenu.Size = new System.Drawing.Size(193, 26);
            // 
            // MenuQuickHelp
            // 
            this.MenuQuickHelp.Name = "MenuQuickHelp";
            this.MenuQuickHelp.Size = new System.Drawing.Size(32, 19);
            // 
            // MenuDonation
            // 
            this.MenuDonation.Name = "MenuDonation";
            this.MenuDonation.Size = new System.Drawing.Size(32, 19);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 397);
            this.Controls.Add(this.mdiWorkSpace1);
            this.Controls.Add(this.taskBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "BowTie Presenter";
            this.StartMenu.ResumeLayout(false);
            this.HelpMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Blumind.Controls.TaskBar taskBar1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Blumind.Controls.MdiWorkSpace mdiWorkSpace1;
        private System.Windows.Forms.ContextMenuStrip StartMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuNew;
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuRecentFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        private System.Windows.Forms.ToolStripMenuItem MenuSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem MenuPreview;
        private System.Windows.Forms.ToolStripMenuItem MenuPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem MenuOptions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem MenuMailToMe;
        private System.Windows.Forms.ToolStripMenuItem MenuHomePage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem MenuAbout;
        private System.Windows.Forms.ToolStripMenuItem MenuHelps;
        private System.Windows.Forms.ToolStripMenuItem MenuShortcuts;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem MenuCheckUpdate;
        private Blumind.Core.HotKeyMap accelaratorKeyMap1;
        private System.Windows.Forms.ContextMenuStrip HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuQuickHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuDonation;
        private System.Windows.Forms.ToolStripMenuItem MenuImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}

