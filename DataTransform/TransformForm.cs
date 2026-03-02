using DTInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTransform
{
	public partial class TransformForm : UserControl, IWizardPanel
	{
		public TransformForm()
		{
			InitializeComponent();
		}

		public bool IsButtonEnabled(BT_TYPE btType)
		{
			bool bEnabled = false;
			switch (btType)
			{
				case BT_TYPE.BT_BACK:
					bEnabled = ((m_tskImport == null) || m_tskImport.IsCompleted);
					break;

				case BT_TYPE.BT_NEXT:
					bEnabled = false;
					break;
			}

			return bEnabled;
		}

		public PAGE_TYPE PageType
		{
			get { return PAGE_TYPE.PAGE_TRANSFORM; }
		}

		public bool IsImplemented
		{
			get { return true; }
		}

		public bool ValidateInput()
		{
			return true;
		}

		public void RefreshUI(WizardForm wizardForm)
		{
			bool bTaskInProgress = ((m_tskImport != null) && !m_tskImport.IsCompleted);
			EnableButtons(bTaskInProgress);

			txtSource.Text = wizardForm.GetSourceFileName();

			// public (string csServer, string csDatabase, string csTable) GetTargetInfo()
			var targetInfo = wizardForm.GetTargetInfo();

			string csServer = targetInfo.Item1;
			string csDatabase = targetInfo.Item2;
			string csTable = targetInfo.Item3;

			txtTarget.Text = $"{csServer}.{csDatabase}.{csTable}";

			chkClearTable.Checked = true;

			int nRecordCount = wizardForm.GetRecordCount();
			txtTotal.Text = $"{nRecordCount:n0}";
			txtRcdsImported.Text = "0";
			txtErrors.Text = "0";

			if (m_tskImport is not null)
				m_tskImport.Dispose();

			if (m_cTknSource is not null)
				m_cTknSource.Dispose();

			m_tskImport = null;
			m_cTknSource = null;
		}

		public void EnableButtons(bool bTaskInProgress)
		{
			WizardForm wizardForm = this.Parent.Parent as WizardForm;

			wizardForm.EnableButton(BT_TYPE.BT_BACK, !bTaskInProgress);
			wizardForm.EnableButton(BT_TYPE.BT_NEXT, false);

			if (btnCancel.InvokeRequired)
			{
				btnCancel.Invoke(new Action(() =>
				{
					btnCancel.Enabled = bTaskInProgress;
					btnCancel.Visible = bTaskInProgress;

					btnStart.Enabled = !bTaskInProgress;
					btnStart.Visible = !bTaskInProgress;
				}));
			}
			else
			{
				btnCancel.Enabled = bTaskInProgress;
				btnCancel.Visible = bTaskInProgress;

				btnStart.Enabled = !bTaskInProgress;
				btnStart.Visible = !bTaskInProgress;
			}
		}

		private void ProgressUpdate(int nPctComplete, int nRecordsWritten, int nRecordsFailed)
		{
			if (txtRcdsImported.InvokeRequired)
			{
				txtRcdsImported.Invoke(new Action(() =>
				{
					txtRcdsImported.Text = $"{nRecordsWritten:n0}";
					txtErrors.Text = $"{nRecordsFailed:n0}";
				}));
			}
			else
			{
				txtRcdsImported.Text = $"{nRecordsWritten:n0}";
				txtErrors.Text = $"{nRecordsFailed:n0}";
			}
			Debug.WriteLine($"Progress: {nPctComplete}% - Imported: {nRecordsWritten} - Errors: {nRecordsFailed}");
		}

		private async void OnStart(object sender, EventArgs e)
		{
			bool bClearTable = chkClearTable.Checked;
			bool bOK = true;

			WizardForm wizardForm = this.Parent.Parent as WizardForm;

			if (bClearTable)
			{
				int nRecordCount = wizardForm.GetRecordCount();
				if (nRecordCount > 0)
				{
					// public (string csServer, string csDatabase, string csTable) GetTargetInfo()
					var targetInfo = wizardForm.GetTargetInfo();
					string csTable = targetInfo.Item3;

					DialogResult result = MessageBox.Show($"Table {csTable} currently has {nRecordCount} records.\nAre you sure you want to clear this table before importing?", "Confirm Clear Table", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (result != DialogResult.Yes)
					{
						bOK = false;
					}
				}
			}

			if (bOK)
			{
				// Set button states - Only the 'Cancel' button is enabled during import
				EnableButtons(true);

				// Create a CancellationTokenSource (CTS)
				m_cTknSource = new CancellationTokenSource();
				CancellationToken token = m_cTknSource.Token;

				IDTDataSource idtDataSource = wizardForm.GetDataSource();
				IDTDataTarget idtDataTarget = wizardForm.GetDataTarget();

				DataTransfer dtaXFer = new DataTransfer(idtDataSource, idtDataTarget, token, ProgressUpdate);

				// Start the import, asynchronously so that the UI remains responsive.
				// Store the Task object so we can check its status later.
				m_tskImport = Task.Run(() => dtaXFer.Execute());
				await m_tskImport;
				EnableButtons(false);
			}
		}

		private void OnCancel(object sender, EventArgs e)
		{
			// Request cancellation from the main thread
			m_cTknSource.Cancel();
		}

		private Task? m_tskImport = null;
		private CancellationTokenSource? m_cTknSource = null;
	}
}
