namespace DataTransform
{
	partial class MappingForm
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
			lstViewColumns = new ListView();
			columnName = new ColumnHeader();
			label1 = new Label();
			label2 = new Label();
			lstViewFields = new ListView();
			columnFields = new ColumnHeader();
			label3 = new Label();
			lstViewMapped = new ListView();
			colName = new ColumnHeader();
			colField = new ColumnHeader();
			btnMap = new Button();
			btnUnMap = new Button();
			SuspendLayout();
			// 
			// lstViewColumns
			// 
			lstViewColumns.Columns.AddRange(new ColumnHeader[] { columnName });
			lstViewColumns.FullRowSelect = true;
			lstViewColumns.GridLines = true;
			lstViewColumns.Location = new Point(70, 39);
			lstViewColumns.Name = "lstViewColumns";
			lstViewColumns.Size = new Size(274, 169);
			lstViewColumns.TabIndex = 0;
			lstViewColumns.UseCompatibleStateImageBehavior = false;
			lstViewColumns.View = View.Details;
			// 
			// columnName
			// 
			columnName.Text = "File Column Name";
			columnName.Width = 270;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(132, 17);
			label1.Name = "label1";
			label1.Size = new Size(150, 15);
			label1.TabIndex = 1;
			label1.Text = "Source File Column Names";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(447, 17);
			label2.Name = "label2";
			label2.Size = new Size(139, 15);
			label2.TabIndex = 3;
			label2.Text = "Target Table Field Names";
			// 
			// lstViewFields
			// 
			lstViewFields.Columns.AddRange(new ColumnHeader[] { columnFields });
			lstViewFields.FullRowSelect = true;
			lstViewFields.GridLines = true;
			lstViewFields.Location = new Point(379, 39);
			lstViewFields.Name = "lstViewFields";
			lstViewFields.Size = new Size(274, 169);
			lstViewFields.TabIndex = 2;
			lstViewFields.UseCompatibleStateImageBehavior = false;
			lstViewFields.View = View.Details;
			// 
			// columnFields
			// 
			columnFields.Text = "Table Field Name";
			columnFields.Width = 270;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(219, 229);
			label3.Name = "label3";
			label3.Size = new Size(284, 15);
			label3.TabIndex = 5;
			label3.Text = "Mapping of Source File Column to Target Table Field";
			// 
			// lstViewMapped
			// 
			lstViewMapped.Columns.AddRange(new ColumnHeader[] { colName, colField });
			lstViewMapped.FullRowSelect = true;
			lstViewMapped.GridLines = true;
			lstViewMapped.Location = new Point(70, 247);
			lstViewMapped.Name = "lstViewMapped";
			lstViewMapped.Size = new Size(583, 138);
			lstViewMapped.TabIndex = 4;
			lstViewMapped.UseCompatibleStateImageBehavior = false;
			lstViewMapped.View = View.Details;
			// 
			// colName
			// 
			colName.Text = "File Column Name";
			colName.Width = 290;
			// 
			// colField
			// 
			colField.Text = "Table Field Name";
			colField.Width = 288;
			// 
			// btnMap
			// 
			btnMap.Location = new Point(693, 112);
			btnMap.Name = "btnMap";
			btnMap.Size = new Size(75, 23);
			btnMap.TabIndex = 6;
			btnMap.Text = "&Map";
			btnMap.UseVisualStyleBackColor = true;
			btnMap.Click += OnMap;
			// 
			// btnUnMap
			// 
			btnUnMap.Location = new Point(693, 305);
			btnUnMap.Name = "btnUnMap";
			btnUnMap.Size = new Size(75, 23);
			btnUnMap.TabIndex = 7;
			btnUnMap.Text = "&UnMap";
			btnUnMap.UseVisualStyleBackColor = true;
			btnUnMap.Click += OnUnMap;
			// 
			// MappingForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(btnUnMap);
			Controls.Add(btnMap);
			Controls.Add(label3);
			Controls.Add(lstViewMapped);
			Controls.Add(label2);
			Controls.Add(lstViewFields);
			Controls.Add(label1);
			Controls.Add(lstViewColumns);
			Name = "MappingForm";
			Size = new Size(810, 402);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ListView lstViewColumns;
		private Label label1;
		private ColumnHeader columnName;
		private Label label2;
		private ListView lstViewFields;
		private ColumnHeader columnFields;
		private Label label3;
		private ListView lstViewMapped;
		private ColumnHeader colName;
		private ColumnHeader colField;
		private Button btnMap;
		private Button btnUnMap;
	}
}
