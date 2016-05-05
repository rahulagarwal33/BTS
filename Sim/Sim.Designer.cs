namespace Sim
{
	partial class Sim
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
			this.connect = new System.Windows.Forms.Button();
			this.connectionStatus = new System.Windows.Forms.CheckBox();
			this.clientReceiveData = new System.Windows.Forms.TextBox();
			this.clientSendData = new System.Windows.Forms.TextBox();
			this.sendClientData = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.sendServerData = new System.Windows.Forms.Button();
			this.serverSendData = new System.Windows.Forms.TextBox();
			this.serverReceivedData = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lstSensorData = new System.Windows.Forms.ListView();
			this.clmSensorName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.clmSensorIdx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.clmSensorValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chkRandomFill = new System.Windows.Forms.CheckBox();
			this.tmrUpdateData = new System.Windows.Forms.Timer(this.components);
			this.btnSendSensorData = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// connect
			// 
			this.connect.Location = new System.Drawing.Point(197, 12);
			this.connect.Name = "connect";
			this.connect.Size = new System.Drawing.Size(75, 23);
			this.connect.TabIndex = 0;
			this.connect.Text = "Connect";
			this.connect.UseVisualStyleBackColor = true;
			this.connect.Click += new System.EventHandler(this.connect_Click);
			// 
			// connectionStatus
			// 
			this.connectionStatus.AutoSize = true;
			this.connectionStatus.Location = new System.Drawing.Point(13, 17);
			this.connectionStatus.Name = "connectionStatus";
			this.connectionStatus.Size = new System.Drawing.Size(92, 17);
			this.connectionStatus.TabIndex = 1;
			this.connectionStatus.Text = "Disconnected";
			this.connectionStatus.UseVisualStyleBackColor = true;
			// 
			// clientReceiveData
			// 
			this.clientReceiveData.Location = new System.Drawing.Point(13, 79);
			this.clientReceiveData.Multiline = true;
			this.clientReceiveData.Name = "clientReceiveData";
			this.clientReceiveData.Size = new System.Drawing.Size(260, 116);
			this.clientReceiveData.TabIndex = 2;
			// 
			// clientSendData
			// 
			this.clientSendData.Location = new System.Drawing.Point(13, 53);
			this.clientSendData.Name = "clientSendData";
			this.clientSendData.Size = new System.Drawing.Size(178, 20);
			this.clientSendData.TabIndex = 3;
			// 
			// sendClientData
			// 
			this.sendClientData.Location = new System.Drawing.Point(197, 52);
			this.sendClientData.Name = "sendClientData";
			this.sendClientData.Size = new System.Drawing.Size(75, 23);
			this.sendClientData.TabIndex = 4;
			this.sendClientData.Text = "Send";
			this.sendClientData.UseVisualStyleBackColor = true;
			this.sendClientData.Click += new System.EventHandler(this.sendClientData_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 37);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Client";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 204);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Server";
			// 
			// sendServerData
			// 
			this.sendServerData.Location = new System.Drawing.Point(199, 219);
			this.sendServerData.Name = "sendServerData";
			this.sendServerData.Size = new System.Drawing.Size(75, 23);
			this.sendServerData.TabIndex = 8;
			this.sendServerData.Text = "Send";
			this.sendServerData.UseVisualStyleBackColor = true;
			this.sendServerData.Click += new System.EventHandler(this.sendServerData_Click);
			// 
			// serverSendData
			// 
			this.serverSendData.Location = new System.Drawing.Point(15, 220);
			this.serverSendData.Name = "serverSendData";
			this.serverSendData.Size = new System.Drawing.Size(178, 20);
			this.serverSendData.TabIndex = 7;
			// 
			// serverReceivedData
			// 
			this.serverReceivedData.Location = new System.Drawing.Point(15, 246);
			this.serverReceivedData.Multiline = true;
			this.serverReceivedData.Name = "serverReceivedData";
			this.serverReceivedData.Size = new System.Drawing.Size(260, 116);
			this.serverReceivedData.TabIndex = 6;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnSendSensorData);
			this.groupBox1.Controls.Add(this.chkRandomFill);
			this.groupBox1.Controls.Add(this.lstSensorData);
			this.groupBox1.Location = new System.Drawing.Point(281, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(416, 350);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sensor Data";
			// 
			// lstSensorData
			// 
			this.lstSensorData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmSensorName,
            this.clmSensorIdx,
            this.clmSensorValue});
			this.lstSensorData.Location = new System.Drawing.Point(6, 19);
			this.lstSensorData.Name = "lstSensorData";
			this.lstSensorData.Size = new System.Drawing.Size(306, 330);
			this.lstSensorData.TabIndex = 0;
			this.lstSensorData.UseCompatibleStateImageBehavior = false;
			this.lstSensorData.View = System.Windows.Forms.View.Details;
			// 
			// clmSensorName
			// 
			this.clmSensorName.Text = "Sensor Name";
			this.clmSensorName.Width = 100;
			// 
			// clmSensorIdx
			// 
			this.clmSensorIdx.Text = "Sensor Index";
			this.clmSensorIdx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.clmSensorIdx.Width = 100;
			// 
			// clmSensorValue
			// 
			this.clmSensorValue.Text = "Value";
			this.clmSensorValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.clmSensorValue.Width = 100;
			// 
			// chkRandomFill
			// 
			this.chkRandomFill.AutoSize = true;
			this.chkRandomFill.Location = new System.Drawing.Point(333, 19);
			this.chkRandomFill.Name = "chkRandomFill";
			this.chkRandomFill.Size = new System.Drawing.Size(81, 17);
			this.chkRandomFill.TabIndex = 1;
			this.chkRandomFill.Text = "Random Fill";
			this.chkRandomFill.UseVisualStyleBackColor = true;
			this.chkRandomFill.CheckedChanged += new System.EventHandler(this.chkRandomFill_CheckedChanged);
			// 
			// tmrUpdateData
			// 
			this.tmrUpdateData.Enabled = true;
			this.tmrUpdateData.Interval = 1000;
			this.tmrUpdateData.Tick += new System.EventHandler(this.tmrUpdateData_Tick);
			// 
			// btnSendSensorData
			// 
			this.btnSendSensorData.Location = new System.Drawing.Point(335, 39);
			this.btnSendSensorData.Name = "btnSendSensorData";
			this.btnSendSensorData.Size = new System.Drawing.Size(75, 23);
			this.btnSendSensorData.TabIndex = 2;
			this.btnSendSensorData.Text = "Send";
			this.btnSendSensorData.UseVisualStyleBackColor = true;
			this.btnSendSensorData.Click += new System.EventHandler(this.btnSendSensorData_Click);
			// 
			// Sim
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(709, 373);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.sendServerData);
			this.Controls.Add(this.serverSendData);
			this.Controls.Add(this.serverReceivedData);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.sendClientData);
			this.Controls.Add(this.clientSendData);
			this.Controls.Add(this.clientReceiveData);
			this.Controls.Add(this.connectionStatus);
			this.Controls.Add(this.connect);
			this.Name = "Sim";
			this.Text = "Form1";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Sim_FormClosed);
			this.Load += new System.EventHandler(this.Sim_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button connect;
		private System.Windows.Forms.CheckBox connectionStatus;
		private System.Windows.Forms.TextBox clientReceiveData;
		private System.Windows.Forms.TextBox clientSendData;
		private System.Windows.Forms.Button sendClientData;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button sendServerData;
		private System.Windows.Forms.TextBox serverSendData;
		private System.Windows.Forms.TextBox serverReceivedData;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView lstSensorData;
		private System.Windows.Forms.ColumnHeader clmSensorName;
		private System.Windows.Forms.ColumnHeader clmSensorIdx;
		private System.Windows.Forms.ColumnHeader clmSensorValue;
		private System.Windows.Forms.CheckBox chkRandomFill;
		private System.Windows.Forms.Timer tmrUpdateData;
		private System.Windows.Forms.Button btnSendSensorData;
	}
}

