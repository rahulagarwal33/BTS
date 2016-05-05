namespace TOC
{
	partial class Hierarchy
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
            this.treeCtrl = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeCtrl
            // 
            this.treeCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeCtrl.Location = new System.Drawing.Point(0, 0);
            this.treeCtrl.Name = "treeCtrl";
            this.treeCtrl.Size = new System.Drawing.Size(282, 381);
            this.treeCtrl.TabIndex = 0;
            this.treeCtrl.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeCtrl_NodeMouseClick);
            this.treeCtrl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeCtrl_MouseUp);
            // 
            // Hierarchy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 381);
            this.Controls.Add(this.treeCtrl);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Hierarchy";
            this.Text = "Hierarchy";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView treeCtrl;
	}
}