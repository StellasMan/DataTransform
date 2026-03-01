namespace DataTransform
{
	partial class MySQLDestForm
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			groupBox1 = new GroupBox();
			btnConnect = new Button();
			txtPassword = new TextBox();
			label4 = new Label();
			txtUserName = new TextBox();
			label3 = new Label();
			txtDatabase = new TextBox();
			label2 = new Label();
			txtServer = new TextBox();
			label1 = new Label();
			groupBox2 = new GroupBox();
			cmbTargetTable = new ComboBox();
			label5 = new Label();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(btnConnect);
			groupBox1.Controls.Add(txtPassword);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(txtUserName);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(txtDatabase);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(txtServer);
			groupBox1.Controls.Add(label1);
			groupBox1.Location = new Point(95, 55);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(308, 267);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "MySQL Connection Parameters";
			// 
			// btnConnect
			// 
			btnConnect.Location = new Point(117, 211);
			btnConnect.Name = "btnConnect";
			btnConnect.Size = new Size(75, 23);
			btnConnect.TabIndex = 8;
			btnConnect.Text = "&Connect";
			btnConnect.UseVisualStyleBackColor = true;
			btnConnect.Click += OnConnect;
			// 
			// txtPassword
			// 
			txtPassword.Location = new Point(96, 161);
			txtPassword.Name = "txtPassword";
			txtPassword.PasswordChar = '*';
			txtPassword.Size = new Size(167, 23);
			txtPassword.TabIndex = 7;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(25, 165);
			label4.Name = "label4";
			label4.Size = new Size(60, 15);
			label4.TabIndex = 6;
			label4.Text = "&Password:";
			// 
			// txtUserName
			// 
			txtUserName.Location = new Point(96, 119);
			txtUserName.Name = "txtUserName";
			txtUserName.Size = new Size(167, 23);
			txtUserName.TabIndex = 5;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(22, 123);
			label3.Name = "label3";
			label3.Size = new Size(63, 15);
			label3.TabIndex = 4;
			label3.Text = "&Username:";
			// 
			// txtDatabase
			// 
			txtDatabase.Location = new Point(96, 77);
			txtDatabase.Name = "txtDatabase";
			txtDatabase.Size = new Size(167, 23);
			txtDatabase.TabIndex = 3;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(27, 81);
			label2.Name = "label2";
			label2.Size = new Size(58, 15);
			label2.TabIndex = 2;
			label2.Text = "&Database:";
			// 
			// txtServer
			// 
			txtServer.Location = new Point(96, 35);
			txtServer.Name = "txtServer";
			txtServer.Size = new Size(167, 23);
			txtServer.TabIndex = 1;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(43, 39);
			label1.Name = "label1";
			label1.Size = new Size(42, 15);
			label1.TabIndex = 0;
			label1.Text = "&Server:";
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(cmbTargetTable);
			groupBox2.Controls.Add(label5);
			groupBox2.Location = new Point(438, 55);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new Size(308, 267);
			groupBox2.TabIndex = 9;
			groupBox2.TabStop = false;
			groupBox2.Text = "Target Table";
			// 
			// cmbTargetTable
			// 
			cmbTargetTable.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbTargetTable.FormattingEnabled = true;
			cmbTargetTable.Location = new Point(35, 85);
			cmbTargetTable.Name = "cmbTargetTable";
			cmbTargetTable.Size = new Size(190, 23);
			cmbTargetTable.TabIndex = 1;
			cmbTargetTable.SelectedIndexChanged += OnTableSelectChanged;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(35, 55);
			label5.Name = "label5";
			label5.Size = new Size(112, 15);
			label5.TabIndex = 0;
			label5.Text = "Target MySQL Table";
			// 
			// MySQLDestForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(groupBox2);
			Controls.Add(groupBox1);
			Name = "MySQLDestForm";
			Size = new Size(810, 402);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private GroupBox groupBox1;
		private TextBox txtPassword;
		private Label label4;
		private TextBox txtUserName;
		private Label label3;
		private TextBox txtDatabase;
		private Label label2;
		private TextBox txtServer;
		private Label label1;
		private Button btnConnect;
		private GroupBox groupBox2;
		private ComboBox cmbTargetTable;
		private Label label5;
	}
}
