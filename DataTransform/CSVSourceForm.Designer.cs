namespace DataTransform
{
	partial class CSVSourceForm
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
			label1 = new Label();
			txtCSVFileName = new TextBox();
			btnFindFile = new Button();
			radCommaDelimited = new RadioButton();
			radTabDelimited = new RadioButton();
			chkHasHeader = new CheckBox();
			label2 = new Label();
			txtRcdCount = new TextBox();
			SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(21, 96);
			label1.Name = "label1";
			label1.Size = new Size(91, 15);
			label1.TabIndex = 0;
			label1.Text = "CSV Source File:";
			// 
			// txtCSVFileName
			// 
			txtCSVFileName.Location = new Point(118, 92);
			txtCSVFileName.Name = "txtCSVFileName";
			txtCSVFileName.Size = new Size(477, 23);
			txtCSVFileName.TabIndex = 1;
			txtCSVFileName.Leave += OnCSVFileNameLeave;
			// 
			// btnFindFile
			// 
			btnFindFile.Location = new Point(601, 91);
			btnFindFile.Name = "btnFindFile";
			btnFindFile.Size = new Size(25, 25);
			btnFindFile.TabIndex = 2;
			btnFindFile.Text = "...";
			btnFindFile.UseVisualStyleBackColor = true;
			btnFindFile.Click += OnFindFile;
			// 
			// radCommaDelimited
			// 
			radCommaDelimited.AutoSize = true;
			radCommaDelimited.Location = new Point(285, 168);
			radCommaDelimited.Name = "radCommaDelimited";
			radCommaDelimited.Size = new Size(122, 19);
			radCommaDelimited.TabIndex = 3;
			radCommaDelimited.TabStop = true;
			radCommaDelimited.Text = "&Comma Delimited";
			radCommaDelimited.UseVisualStyleBackColor = true;
			// 
			// radTabDelimited
			// 
			radTabDelimited.AutoSize = true;
			radTabDelimited.Location = new Point(428, 168);
			radTabDelimited.Name = "radTabDelimited";
			radTabDelimited.Size = new Size(98, 19);
			radTabDelimited.TabIndex = 4;
			radTabDelimited.TabStop = true;
			radTabDelimited.Text = "&Tab Delimited";
			radTabDelimited.UseVisualStyleBackColor = true;
			// 
			// chkHasHeader
			// 
			chkHasHeader.AutoSize = true;
			chkHasHeader.Location = new Point(362, 132);
			chkHasHeader.Name = "chkHasHeader";
			chkHasHeader.Size = new Size(87, 19);
			chkHasHeader.TabIndex = 5;
			chkHasHeader.Text = "&Has Header";
			chkHasHeader.UseVisualStyleBackColor = true;
			chkHasHeader.CheckedChanged += OnHasHeaderClicked;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(638, 96);
			label2.Name = "label2";
			label2.Size = new Size(83, 15);
			label2.TabIndex = 6;
			label2.Text = "Record Count:";
			// 
			// txtRcdCount
			// 
			txtRcdCount.Location = new Point(727, 92);
			txtRcdCount.Name = "txtRcdCount";
			txtRcdCount.Size = new Size(62, 23);
			txtRcdCount.TabIndex = 7;
			txtRcdCount.Text = "0";
			txtRcdCount.TextAlign = HorizontalAlignment.Center;
			// 
			// CSVSourceForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(txtRcdCount);
			Controls.Add(label2);
			Controls.Add(chkHasHeader);
			Controls.Add(radTabDelimited);
			Controls.Add(radCommaDelimited);
			Controls.Add(btnFindFile);
			Controls.Add(txtCSVFileName);
			Controls.Add(label1);
			Name = "CSVSourceForm";
			Size = new Size(810, 402);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label label1;
		private TextBox txtCSVFileName;
		private Button btnFindFile;
		private RadioButton radCommaDelimited;
		private RadioButton radTabDelimited;
		private CheckBox chkHasHeader;
		private Label label2;
		private TextBox txtRcdCount;
	}
}
