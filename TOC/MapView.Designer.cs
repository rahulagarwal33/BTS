namespace TOC
{
	partial class MapView
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
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.btnGenLL = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gmap
            // 
            this.gmap.Bearing = 0F;
            this.gmap.CanDragMap = true;
            this.gmap.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gmap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap.GrayScaleMode = false;
            this.gmap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap.LevelsKeepInMemmory = 5;
            this.gmap.Location = new System.Drawing.Point(0, 28);
            this.gmap.MarkersEnabled = true;
            this.gmap.MaxZoom = 18;
            this.gmap.MinZoom = 2;
            this.gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmap.Name = "gmap";
            this.gmap.NegativeMode = false;
            this.gmap.PolygonsEnabled = true;
            this.gmap.RetryLoadTile = 0;
            this.gmap.RoutesEnabled = true;
            this.gmap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap.ShowTileGridLines = false;
            this.gmap.Size = new System.Drawing.Size(357, 311);
            this.gmap.TabIndex = 0;
            this.gmap.Zoom = 12D;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnWrite);
            this.panel1.Controls.Add(this.barProgress);
            this.panel1.Controls.Add(this.btnGenLL);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(357, 28);
            this.panel1.TabIndex = 1;
            // 
            // barProgress
            // 
            this.barProgress.Dock = System.Windows.Forms.DockStyle.Right;
            this.barProgress.Location = new System.Drawing.Point(114, 0);
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(243, 28);
            this.barProgress.TabIndex = 1;
            // 
            // btnGenLL
            // 
            this.btnGenLL.Location = new System.Drawing.Point(3, 3);
            this.btnGenLL.Name = "btnGenLL";
            this.btnGenLL.Size = new System.Drawing.Size(58, 23);
            this.btnGenLL.TabIndex = 0;
            this.btnGenLL.Text = "Generate Lat Lon";
            this.btnGenLL.UseVisualStyleBackColor = true;
            this.btnGenLL.Click += new System.EventHandler(this.btnGenLL_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(59, 3);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(58, 23);
            this.btnWrite.TabIndex = 2;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // MapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 339);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gmap);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MapView";
            this.Text = "MapView";
            this.Load += new System.EventHandler(this.MapView_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar barProgress;
        private System.Windows.Forms.Button btnGenLL;
        private System.Windows.Forms.Button btnWrite;
	}
}