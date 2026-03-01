namespace DataTransform
{
	partial class TransformForm
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
			txtTarget = new TextBox();
			label2 = new Label();
			label1 = new Label();
			txtSource = new TextBox();
			chkClearTable = new CheckBox();
			label3 = new Label();
			txtTotal = new TextBox();
			txtRcdsImported = new TextBox();
			label4 = new Label();
			txtErrors = new TextBox();
			label5 = new Label();
			grpProgress = new GroupBox();
			btnStart = new Button();
			btnCancel = new Button();
			grpProgress.SuspendLayout();
			SuspendLayout();
			// 
			// txtTarget
			// 
			txtTarget.Location = new Point(226, 86);
			txtTarget.Name = "txtTarget";
			txtTarget.ReadOnly = true;
			txtTarget.Size = new Size(240, 23);
			txtTarget.TabIndex = 3;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(134, 90);
			label2.Name = "label2";
			label2.Size = new Size(74, 15);
			label2.TabIndex = 2;
			label2.Text = "Target Table:";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(141, 40);
			label1.Name = "label1";
			label1.Size = new Size(67, 15);
			label1.TabIndex = 0;
			label1.Text = "Source File:";
			// 
			// txtSource
			// 
			txtSource.Location = new Point(226, 36);
			txtSource.Name = "txtSource";
			txtSource.ReadOnly = true;
			txtSource.Size = new Size(470, 23);
			txtSource.TabIndex = 1;
			// 
			// chkClearTable
			// 
			chkClearTable.AutoSize = true;
			chkClearTable.Location = new Point(484, 88);
			chkClearTable.Name = "chkClearTable";
			chkClearTable.Size = new Size(160, 19);
			chkClearTable.TabIndex = 4;
			chkClearTable.Text = "&Clear Table Before Import";
			chkClearTable.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(111, 29);
			label3.Name = "label3";
			label3.Size = new Size(105, 15);
			label3.TabIndex = 5;
			label3.Text = "Total # of Records:";
			// 
			// txtTotal
			// 
			txtTotal.Location = new Point(227, 25);
			txtTotal.Name = "txtTotal";
			txtTotal.ReadOnly = true;
			txtTotal.Size = new Size(109, 23);
			txtTotal.TabIndex = 6;
			// 
			// txtRcdsImported
			// 
			txtRcdsImported.Location = new Point(227, 59);
			txtRcdsImported.Name = "txtRcdsImported";
			txtRcdsImported.ReadOnly = true;
			txtRcdsImported.Size = new Size(109, 23);
			txtRcdsImported.TabIndex = 8;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(112, 63);
			label4.Name = "label4";
			label4.Size = new Size(104, 15);
			label4.TabIndex = 7;
			label4.Text = "Records Imported:";
			// 
			// txtErrors
			// 
			txtErrors.Location = new Point(227, 93);
			txtErrors.Name = "txtErrors";
			txtErrors.ReadOnly = true;
			txtErrors.Size = new Size(109, 23);
			txtErrors.TabIndex = 10;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(98, 97);
			label5.Name = "label5";
			label5.Size = new Size(118, 15);
			label5.TabIndex = 9;
			label5.Text = "Errors During Import:";
			// 
			// grpProgress
			// 
			grpProgress.Controls.Add(label3);
			grpProgress.Controls.Add(txtErrors);
			grpProgress.Controls.Add(txtTotal);
			grpProgress.Controls.Add(label5);
			grpProgress.Controls.Add(label4);
			grpProgress.Controls.Add(txtRcdsImported);
			grpProgress.Location = new Point(134, 139);
			grpProgress.Name = "grpProgress";
			grpProgress.Size = new Size(562, 147);
			grpProgress.TabIndex = 11;
			grpProgress.TabStop = false;
			grpProgress.Text = "Import Progress";
			// 
			// btnStart
			// 
			btnStart.Location = new Point(307, 318);
			btnStart.Name = "btnStart";
			btnStart.Size = new Size(103, 23);
			btnStart.TabIndex = 12;
			btnStart.Text = "&Start Import";
			btnStart.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			btnCancel.Location = new Point(416, 318);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(103, 23);
			btnCancel.TabIndex = 13;
			btnCancel.Text = "&Cancel Import";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// TransformForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(btnCancel);
			Controls.Add(btnStart);
			Controls.Add(grpProgress);
			Controls.Add(chkClearTable);
			Controls.Add(txtTarget);
			Controls.Add(label2);
			Controls.Add(txtSource);
			Controls.Add(label1);
			Name = "TransformForm";
			Size = new Size(810, 402);
			grpProgress.ResumeLayout(false);
			grpProgress.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private TextBox txtTarget;
		private Label label2;
		private Label label1;
		private TextBox txtSource;
		private CheckBox chkClearTable;
		private Label label3;
		private TextBox txtTotal;
		private TextBox txtRcdsImported;
		private Label label4;
		private TextBox txtErrors;
		private Label label5;
		private GroupBox grpProgress;
		private Button btnStart;
		private Button btnCancel;
	}
}
