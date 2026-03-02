//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Xml.Linq;
//using static System.Runtime.InteropServices.JavaScript.JSType;

using System.Diagnostics;
using CSVDataSrc;
using DTInterfaces;
using MySql.Data.MySqlClient;

namespace DataTransform
{
	public partial class CSVSourceForm : UserControl, IWizardPanelSource
	{
		public CSVSourceForm()
		{
			InitializeComponent();

			chkHasHeader.Checked = true; // Set default to indicate the first row has column names
			radCommaDelimited.Checked = true; // Set default selection

			SourceInfo srcInfo = new SourceInfo(string.Empty, DS_OPTIONS.DS_OPT_HAS_HEADER | DS_OPTIONS.DS_OPT_COMMA_DELIMITED);

			#if DEBUG
				string csCSVFileName = "D:\\Development\\Projects\\DataTransform\\Data\\CrossRef.csv";
				txtCSVFileName.Text = csCSVFileName;
				srcInfo.FilePath = csCSVFileName;
				InitSourceFile(srcInfo);
			#endif
		}

		public bool IsButtonEnabled(BT_TYPE btType)
		{
			bool bEnabled = false;
			switch (btType)
			{
				case BT_TYPE.BT_BACK:
					bEnabled = false;
					break;

				case BT_TYPE.BT_NEXT:
					bEnabled = true;
					break;
			}

			return bEnabled;
		}

		public bool ValidateInput()
		{
			DS_OPTIONS dsOptions = DS_OPTIONS.DS_OPT_NONE;
			dsOptions |= radCommaDelimited.Checked ? DS_OPTIONS.DS_OPT_COMMA_DELIMITED : DS_OPTIONS.DS_OPT_NONE;
			dsOptions |= chkHasHeader.Checked ? DS_OPTIONS.DS_OPT_HAS_HEADER : DS_OPTIONS.DS_OPT_NONE;

			SourceInfo srcInfo = new SourceInfo(txtCSVFileName.Text, dsOptions);
			return m_csvSource.TestConnection(srcInfo);
		}

		public void RefreshUI(WizardForm wizardForm)
		{
			Trace.WriteLine("SourceForm: RefreshUI called");
		}

		private void OnFindFile(object sender, EventArgs e)
		{
			OpenFileDialog openCSVFileDlg = new OpenFileDialog();

			string appBaseDirectory = System.AppContext.BaseDirectory;

			#if DEBUG
				Trace.WriteLine($"Application base directory: {appBaseDirectory}");
			#endif

			openCSVFileDlg.InitialDirectory = appBaseDirectory;
			openCSVFileDlg.Title = "Browse CSV Files";

			// Filter for CSV files (*.csv) and all files (*.*)
			openCSVFileDlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
			openCSVFileDlg.CheckFileExists = true;
			openCSVFileDlg.CheckPathExists = true;
			openCSVFileDlg.Multiselect = false;     // Only 1 file selection allowed

			if (openCSVFileDlg.ShowDialog() == DialogResult.OK)
			{
				// Get the path of the selected file
				string filePath = openCSVFileDlg.FileName;
				DS_OPTIONS dsOptions = DS_OPTIONS.DS_OPT_NONE;
				if (chkHasHeader.Checked == true)
				{
					dsOptions |= DS_OPTIONS.DS_OPT_HAS_HEADER;
				}

				if (radCommaDelimited.Checked == true)
				{
					dsOptions |= DS_OPTIONS.DS_OPT_COMMA_DELIMITED;
				}

				InitSourceFile(new SourceInfo(filePath, dsOptions));
			}
		}

		// Initialize the source file
		private bool InitSourceFile(SourceInfo srcInfo)
		{
			bool bError = false;
			m_nRecordCount = 0;

			bool areEqual = ((m_csvSource.DataSrcInfo is not null) && m_csvSource.DataSrcInfo.Equals(srcInfo));
			if (!areEqual)
			{
				try
				{
					if (m_csvSource.TestConnection(srcInfo))
					{
						m_csvSource.DataSrcInfo = srcInfo;

						Trace.WriteLine("Connection test successful.");
						m_nRecordCount = m_csvSource.GetRecordCount();
					}
					else
					{
						Trace.WriteLine("Connection test failed.");
						bError = true;
					}
				}
				catch (Exception ex)
				{
					Trace.WriteLine($"The file could not be read:");
					Trace.WriteLine(ex.Message);
					bError = true;
				}

				if (!bError)
				{
					txtRcdCount.Text = m_nRecordCount.ToString("N0");

					#if DEBUG
						Trace.WriteLine($"Total # of records for file {m_csvSource.DataSrcInfo.FilePath}: {m_nRecordCount:n0}");
					#endif
				}
			}

			return !bError;
		}

		public IDTDataSource? DataSource
		{
			get { return m_csvSource; }
		}

		public PAGE_TYPE PageType
		{
			get { return PAGE_TYPE.PAGE_SOURCE; }
		}

		public bool IsImplemented
		{
			get { return true; }
		}

		public string SourceFile 
		{ 
			get { return m_csvSource.DataSrcInfo?.FilePath ?? string.Empty; }
		}

		public int RecordCount 
		{ 
			get { return m_nRecordCount; }
		}

		private CSVDTDataSource m_csvSource = new CSVDTDataSource();
		private List<string> m_lstColumnNames = new List<string>();
		private int m_nRecordCount = 0;
	}
}
