namespace TOC
{
	partial class Site
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
            this.connectionStatus = new System.Windows.Forms.CheckBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtClientSendData = new System.Windows.Forms.TextBox();
            this.btnclientSendData = new System.Windows.Forms.Button();
            this.btnServerSendData = new System.Windows.Forms.Button();
            this.txtServerSendData = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtClientReceiveData = new System.Windows.Forms.TextBox();
            this.txtServerReceiveData = new System.Windows.Forms.TextBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.barProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // connectionStatus
            // 
            this.connectionStatus.AutoSize = true;
            this.connectionStatus.Location = new System.Drawing.Point(13, 13);
            this.connectionStatus.Name = "connectionStatus";
            this.connectionStatus.Size = new System.Drawing.Size(92, 17);
            this.connectionStatus.TabIndex = 0;
            this.connectionStatus.Text = "Disconnected";
            this.connectionStatus.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(123, 9);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Client";
            // 
            // txtClientSendData
            // 
            this.txtClientSendData.Location = new System.Drawing.Point(16, 66);
            this.txtClientSendData.Name = "txtClientSendData";
            this.txtClientSendData.Size = new System.Drawing.Size(330, 20);
            this.txtClientSendData.TabIndex = 3;
            // 
            // btnclientSendData
            // 
            this.btnclientSendData.Location = new System.Drawing.Point(352, 63);
            this.btnclientSendData.Name = "btnclientSendData";
            this.btnclientSendData.Size = new System.Drawing.Size(75, 23);
            this.btnclientSendData.TabIndex = 4;
            this.btnclientSendData.Text = "Send";
            this.btnclientSendData.UseVisualStyleBackColor = true;
            this.btnclientSendData.Click += new System.EventHandler(this.btnclientSendData_Click);
            // 
            // btnServerSendData
            // 
            this.btnServerSendData.Location = new System.Drawing.Point(352, 232);
            this.btnServerSendData.Name = "btnServerSendData";
            this.btnServerSendData.Size = new System.Drawing.Size(75, 23);
            this.btnServerSendData.TabIndex = 7;
            this.btnServerSendData.Text = "Send";
            this.btnServerSendData.UseVisualStyleBackColor = true;
            this.btnServerSendData.Click += new System.EventHandler(this.btnServerSendData_Click);
            // 
            // txtServerSendData
            // 
            this.txtServerSendData.Location = new System.Drawing.Point(16, 235);
            this.txtServerSendData.Name = "txtServerSendData";
            this.txtServerSendData.Size = new System.Drawing.Size(330, 20);
            this.txtServerSendData.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 218);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server";
            // 
            // txtClientReceiveData
            // 
            this.txtClientReceiveData.Location = new System.Drawing.Point(16, 92);
            this.txtClientReceiveData.Multiline = true;
            this.txtClientReceiveData.Name = "txtClientReceiveData";
            this.txtClientReceiveData.Size = new System.Drawing.Size(411, 112);
            this.txtClientReceiveData.TabIndex = 8;
            // 
            // txtServerReceiveData
            // 
            this.txtServerReceiveData.Location = new System.Drawing.Point(16, 261);
            this.txtServerReceiveData.Multiline = true;
            this.txtServerReceiveData.Name = "txtServerReceiveData";
            this.txtServerReceiveData.Size = new System.Drawing.Size(411, 112);
            this.txtServerReceiveData.TabIndex = 9;
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(204, 9);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 10;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // barProgress
            // 
            this.barProgress.Location = new System.Drawing.Point(204, 34);
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(218, 23);
            this.barProgress.TabIndex = 11;
            // 
            // Site
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 377);
            this.Controls.Add(this.barProgress);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.txtServerReceiveData);
            this.Controls.Add(this.txtClientReceiveData);
            this.Controls.Add(this.btnServerSendData);
            this.Controls.Add(this.txtServerSendData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnclientSendData);
            this.Controls.Add(this.txtClientSendData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.connectionStatus);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Site";
            this.Text = "Site";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox connectionStatus;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtClientSendData;
		private System.Windows.Forms.Button btnclientSendData;
		private System.Windows.Forms.Button btnServerSendData;
		private System.Windows.Forms.TextBox txtServerSendData;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtClientReceiveData;
		private System.Windows.Forms.TextBox txtServerReceiveData;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.ProgressBar barProgress;
	}
}