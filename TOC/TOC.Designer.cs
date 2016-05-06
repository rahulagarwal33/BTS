namespace TOC
{
	partial class TOC
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
            this.components = new System.ComponentModel.Container();
            this.pnlMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.menToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hierarchyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 24);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(729, 450);
            this.pnlMain.TabIndex = 0;
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(729, 24);
            this.mnuMain.TabIndex = 3;
            this.mnuMain.Text = "menuStrip1";
            // 
            // menToolStripMenuItem
            // 
            this.menToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hierarchyToolStripMenuItem,
            this.propertyViewToolStripMenuItem,
            this.logViewToolStripMenuItem});
            this.menToolStripMenuItem.Name = "menToolStripMenuItem";
            this.menToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.menToolStripMenuItem.Text = "&View";
            // 
            // hierarchyToolStripMenuItem
            // 
            this.hierarchyToolStripMenuItem.Name = "hierarchyToolStripMenuItem";
            this.hierarchyToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.hierarchyToolStripMenuItem.Text = "&Hierarchy View";
            this.hierarchyToolStripMenuItem.Click += new System.EventHandler(this.hierarchyToolStripMenuItem_Click);
            this.hierarchyToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.hierarchyToolStripMenuItem_Paint);
            // 
            // propertyViewToolStripMenuItem
            // 
            this.propertyViewToolStripMenuItem.Name = "propertyViewToolStripMenuItem";
            this.propertyViewToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.propertyViewToolStripMenuItem.Text = "&Property View";
            this.propertyViewToolStripMenuItem.Click += new System.EventHandler(this.propertyViewToolStripMenuItem_Click);
            this.propertyViewToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.propertyViewToolStripMenuItem_Paint);
            // 
            // logViewToolStripMenuItem
            // 
            this.logViewToolStripMenuItem.Name = "logViewToolStripMenuItem";
            this.logViewToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.logViewToolStripMenuItem.Text = "&Log View";
            this.logViewToolStripMenuItem.Click += new System.EventHandler(this.logViewToolStripMenuItem_Click);
            this.logViewToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.logViewToolStripMenuItem_Paint);
            // 
            // timerStatus
            // 
            this.timerStatus.Enabled = true;
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // TOC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 474);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.mnuMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "TOC";
            this.Text = "TOC";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TOC_FormClosed);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private WeifenLuo.WinFormsUI.Docking.DockPanel pnlMain;
		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem menToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hierarchyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem propertyViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logViewToolStripMenuItem;
        private System.Windows.Forms.Timer timerStatus;
	}
}

