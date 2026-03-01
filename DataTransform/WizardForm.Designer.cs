namespace DataTransform
{
    partial class WizardForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			panelContent = new Panel();
			btnBack = new Button();
			btnNext = new Button();
			cmbSrcType = new ComboBox();
			label1 = new Label();
			label2 = new Label();
			cmbDestType = new ComboBox();
			SuspendLayout();
			// 
			// panelContent
			// 
			panelContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			panelContent.Location = new Point(12, 60);
			panelContent.Name = "panelContent";
			panelContent.Size = new Size(810, 402);
			panelContent.TabIndex = 0;
			// 
			// btnBack
			// 
			btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnBack.Location = new Point(613, 484);
			btnBack.Name = "btnBack";
			btnBack.Size = new Size(75, 23);
			btnBack.TabIndex = 1;
			btnBack.Text = "&Back";
			btnBack.UseVisualStyleBackColor = true;
			btnBack.Click += OnBack;
			// 
			// btnNext
			// 
			btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnNext.Location = new Point(717, 484);
			btnNext.Name = "btnNext";
			btnNext.Size = new Size(75, 23);
			btnNext.TabIndex = 2;
			btnNext.Text = "&Next";
			btnNext.UseVisualStyleBackColor = true;
			btnNext.Click += OnNext;
			// 
			// cmbSrcType
			// 
			cmbSrcType.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbSrcType.FormattingEnabled = true;
			cmbSrcType.Location = new Point(166, 20);
			cmbSrcType.Name = "cmbSrcType";
			cmbSrcType.Size = new Size(121, 23);
			cmbSrcType.TabIndex = 3;
			cmbSrcType.DropDownClosed += OnSourceTypeChanged;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(46, 24);
			label1.Name = "label1";
			label1.Size = new Size(101, 15);
			label1.TabIndex = 4;
			label1.Text = "Source Date Type:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(356, 24);
			label2.Name = "label2";
			label2.Size = new Size(125, 15);
			label2.TabIndex = 6;
			label2.Text = "Destination Data Type:";
			// 
			// cmbDestType
			// 
			cmbDestType.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbDestType.FormattingEnabled = true;
			cmbDestType.Location = new Point(488, 20);
			cmbDestType.Name = "cmbDestType";
			cmbDestType.Size = new Size(121, 23);
			cmbDestType.TabIndex = 5;
			cmbDestType.DropDownClosed += OnDestTypeChanged;
			// 
			// WizardForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(834, 519);
			Controls.Add(label2);
			Controls.Add(cmbDestType);
			Controls.Add(label1);
			Controls.Add(cmbSrcType);
			Controls.Add(btnNext);
			Controls.Add(btnBack);
			Controls.Add(panelContent);
			MinimumSize = new Size(850, 500);
			Name = "WizardForm";
			Text = "Data Transform";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Panel panelContent;
		private Button btnBack;
		private Button btnNext;
		private ComboBox cmbSrcType;
		private Label label1;
		private Label label2;
		private ComboBox cmbDestType;
	}
}
