using DTInterfaces;
using MySQLDataTarget;
using System.Diagnostics;

namespace DataTransform
{
	public partial class WizardForm : Form
	{
		enum FT_INDEX
		{
			FT_SOURCE = 0,
			FT_DEST,
			FT_MAPPING,
			FT_TRANSFORM,
			FT_SUMMARY
		}

		private CSVSourceForm csvSourceForm = new CSVSourceForm();
		private ExcelSourceForm excelSourceForm = new ExcelSourceForm();
		private MySQLDestForm mySQLDestForm = new MySQLDestForm();
		private PostgresDestForm postgresDestForm = new PostgresDestForm();
		private MappingForm mappingForm = new MappingForm();
		private TransformForm transformForm = new TransformForm();
		private SummaryForm summaryForm = new SummaryForm();

		private UserControl[] wizardPages;
		private int currentPageIndex = 0;

		public bool m_bAutoMapEnabled = true;

		public List<string> m_lstColumnNames = new List<string>();

		// TODO: Make this more generic to support different types of data targets, not just MySQL
		public Dictionary<string, MySQLFieldInfo> m_mapColToFieldInfo = new Dictionary<string, MySQLFieldInfo>();

		public WizardForm()
		{
			InitializeComponent();

			// Initialize pages array with instances of your UserControls

			cmbSrcType.Items.Add(new ComboBoxItem(".CSV File", csvSourceForm));
			cmbSrcType.Items.Add(new ComboBoxItem("Excel File", excelSourceForm));
			cmbSrcType.SelectedIndex = 0; // Set default selection

			cmbDestType.Items.Add(new ComboBoxItem("MySQL Database", mySQLDestForm));
			cmbDestType.Items.Add(new ComboBoxItem("Postgres Database", postgresDestForm));
			cmbDestType.SelectedIndex = 0; // Set default selection

			wizardPages = new UserControl[] { csvSourceForm, mySQLDestForm, mappingForm, transformForm, summaryForm };
			LoadCurrentPage();
		}

		public void EnableButton(BT_TYPE btType, bool bEnabled)
		{
			Button btnButton = (btType == BT_TYPE.BT_BACK) ? btnBack : btnNext;
			if (btnButton.InvokeRequired)
			{
				btnButton.Invoke
				(
					new Action(
						() =>
						{
							btnButton.Enabled = bEnabled;
						}
					)
				);
			}
			else
			{
				btnButton.Enabled = bEnabled;
			}
		}

		public string GetSourceFileName()
		{
			string csFileName = string.Empty;
			IWizardPanelSource isrcForm = wizardPages[(int)FT_INDEX.FT_SOURCE] as IWizardPanelSource;
			if (isrcForm != null)
			{
				csFileName = isrcForm.SourceFile;
			}

			return csFileName;
		}

		public uint GetRecordCount()
		{
			uint uiRecordCount = 0;
			IWizardPanelSource isrcForm = wizardPages[(int)FT_INDEX.FT_SOURCE] as IWizardPanelSource;
			if (isrcForm != null)
			{
				uiRecordCount = isrcForm.RecordCount;
			}

			return uiRecordCount;
		}

		public (string csServer, string csDatabase, string csTable) GetTargetInfo()
		{
			string csServer = string.Empty;
			string csDatabase = string.Empty;
			string csTable = string.Empty;
			IWizardPanelTarget idtPanelTarget = wizardPages[(int)FT_INDEX.FT_DEST] as IWizardPanelTarget;
			if (idtPanelTarget != null)
			{
				IDTDataTarget idtTarget = idtPanelTarget.GetDataTarget();
				if (idtTarget != null)
				{
					csServer = idtTarget.Server;
					csDatabase = idtTarget.Database;
				}

				csTable = idtPanelTarget.TargetTable;
			}

			var retVal = (csServer,	csDatabase, csTable);
			return retVal;
		}

		public IDTDataSource? GetDataSource()
		{
			IDTDataSource? dataSource = null;
			IWizardPanelSource isrcForm = wizardPages[(int)FT_INDEX.FT_SOURCE] as IWizardPanelSource;
			if (isrcForm != null)
			{
				dataSource = isrcForm.DataSource;
			}
			return dataSource;
		}

		public IDTDataTarget? GetDataTarget()
		{
			IDTDataTarget? dataTarget = null;
			IWizardPanelTarget idtPanelTarget = wizardPages[(int)FT_INDEX.FT_DEST] as IWizardPanelTarget;
			if (idtPanelTarget != null)
			{
				dataTarget = idtPanelTarget.GetDataTarget();
			}
			return dataTarget;
		}

		public Dictionary<string, string> GetFieldMapping()
		{
			Dictionary<string, string> mapFieldMapping = new Dictionary<string, string>();
			MappingForm mappingForm = wizardPages[(int)FT_INDEX.FT_MAPPING] as MappingForm;
			if (mappingForm != null)
			{
				mapFieldMapping = mappingForm.FieldMapping;
			}
			return mapFieldMapping;
		}

		private void LoadCurrentPage()
		{
			// Clear existing page and add the new one
			panelContent.Controls.Clear();
			UserControl currentPage = wizardPages[currentPageIndex];
			currentPage.Dock = DockStyle.Fill;
			panelContent.Controls.Add(currentPage);
			IWizardPanel? wizardPanel = currentPage as IWizardPanel;
			if (wizardPanel != null)
			{
				wizardPanel.RefreshUI(this);
				btnBack.Enabled = wizardPanel.IsButtonEnabled(BT_TYPE.BT_BACK);
				btnNext.Enabled = wizardPanel.IsButtonEnabled(BT_TYPE.BT_NEXT);
			}
		}

		private void OnBack(object sender, EventArgs e)
		{
			if (currentPageIndex > 0)
			{
				IWizardPanel? IWizardPanel = wizardPages[currentPageIndex] as IWizardPanel;
				if ((IWizardPanel != null) && (IWizardPanel.IsButtonEnabled(BT_TYPE.BT_BACK)))
				{
					currentPageIndex--;
					LoadCurrentPage();
				}
			}
		}

		private void OnNext(object sender, EventArgs e)
		{
			// Add validation logic here before moving to the next step
			if (currentPageIndex < wizardPages.Length - 1)
			{
				IWizardPanel? IWizardPanel = wizardPages[currentPageIndex] as IWizardPanel;
				if ((IWizardPanel != null) && IWizardPanel.IsButtonEnabled(BT_TYPE.BT_NEXT) && IWizardPanel.ValidateInput())
				{
					if (currentPageIndex == (int)FT_INDEX.FT_SOURCE)
					{
						// If we're moving forward from the source page, capture the column names for mapping
						if (cmbSrcType.SelectedItem is ComboBoxItem objSelected)
						{
							UserControl newControl = objSelected.UserControlData;
							IWizardPanelSource? idtPanelSrc = newControl as IWizardPanelSource;
							if (idtPanelSrc != null)
							{
								IDTDataSource? idtDataSrc = idtPanelSrc.DataSource;
								if (idtDataSrc != null)
								{
									m_lstColumnNames = idtDataSrc.ColumnNames;
								}
							}
						}
					} 
					else if (currentPageIndex == (int)FT_INDEX.FT_DEST)
					{
						// If we're moving forward from the destination page, capture the table column names for mapping
						if (cmbDestType.SelectedItem is ComboBoxItem objSelected)
						{
							m_mapColToFieldInfo.Clear();

							UserControl newControl = objSelected.UserControlData;
							IWizardPanelTarget? idtPanelTarget = newControl as IWizardPanelTarget;
							if (idtPanelTarget != null)
							{
								IDTDataTarget? idtDataTarget = idtPanelTarget.GetDataTarget();
								if (idtDataTarget != null)
								{
									string csTargetTable = idtPanelTarget.TargetTable;
									Dictionary<string, object> dctColInfo = idtDataTarget.GetTableColumnInfo(csTargetTable);

									m_mapColToFieldInfo = dctColInfo.ToDictionary(kvp => kvp.Key, kvp => (MySQLFieldInfo)kvp.Value);
								}
							}
						}
					}

					currentPageIndex++;
					LoadCurrentPage();
				}
			}
		}

		private void OnSourceTypeChanged(object sender, EventArgs e)
		{
			m_lstColumnNames.Clear();
			if (cmbSrcType.SelectedItem is ComboBoxItem objSelected)
			{
				UserControl currentControl = wizardPages[(int)FT_INDEX.FT_SOURCE];
				UserControl newControl = objSelected.UserControlData;
				if ((newControl != null) && (currentControl != newControl))
				{
					Trace.WriteLine($"Source type changed to: {objSelected.DisplayText}");
					wizardPages[(int)FT_INDEX.FT_SOURCE] = newControl;
					if (currentPageIndex == (int)FT_INDEX.FT_SOURCE)
					{
						LoadCurrentPage();
						IWizardPanelSource? dtPanelSrc = newControl as IWizardPanelSource;
						if (dtPanelSrc != null)
						{
							IDTDataSource? idtDataSource = dtPanelSrc.DataSource;
							if (idtDataSource != null)
							{
								m_lstColumnNames = idtDataSource.ColumnNames;
							}
						}
					}
				}
			}
			else
			{
				Trace.WriteLine("Source type changed to: null");
			}
		}

		private void OnDestTypeChanged(object sender, EventArgs e)
		{
			if (cmbDestType.SelectedItem is ComboBoxItem objSelected)
			{
				UserControl currentControl = wizardPages[(int)FT_INDEX.FT_DEST];
				UserControl newControl = objSelected.UserControlData;
				if ((currentControl != null) && (currentControl != newControl))
				{
					Trace.WriteLine($"Destination type changed to: {objSelected.DisplayText}");
					wizardPages[(int)FT_INDEX.FT_DEST] = newControl;
					if (currentPageIndex == (int)FT_INDEX.FT_DEST)
					{
						LoadCurrentPage();
					}
				}
			}
			else
			{
				Trace.WriteLine("Destination type changed to: null");
			}
		}
	}

	public class ComboBoxItem
	{
		public ComboBoxItem(string displayText, UserControl userControl)
		{
			DisplayText = displayText;
			UserControlData = userControl;
		}

		public string DisplayText { get; set; }
		public UserControl UserControlData { get; set; }

		// Override ToString() so the ComboBox displays the correct text by default
		public override string ToString()
		{
			return DisplayText;
		}
	}

}

