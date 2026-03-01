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
					bEnabled = true;
					break;

				case BT_TYPE.BT_NEXT:
					bEnabled = true;
					break;
			}

			return bEnabled;
		}

		public PAGE_TYPE GetPageType()
		{
			return PAGE_TYPE.PAGE_TRANSFORM;
		}

		public bool IsImplemented()
		{
			return true;
		}

		public bool ValidateInput()
		{
			return true;
		}

		public void RefreshUI(WizardForm wizardForm)
		{
			bool bTaskInProgress = ((m_tskImport != null) && !m_tskImport.IsCompleted);
			EnableButtons(wizardForm, bTaskInProgress);

			txtSource.Text = wizardForm.GetSourceFileName();

			// public (string csServer, string csDatabase, string csTable) GetTargetInfo()
			var targetInfo = wizardForm.GetTargetInfo();

			string csServer = targetInfo.Item1;
			string csDatabase = targetInfo.Item2;
			string csTable = targetInfo.Item3;

			txtTarget.Text = $"{csServer}.{csDatabase}.{csTable}";

			int nRecordCount = wizardForm.GetRecordCount();
			txtTotal.Text = $"{nRecordCount:n0}";
			txtRcdsImported.Text = "0";
			txtErrors.Text = "0";

			m_tskImport = null;
			m_cTknSource = null;
		}

		public void EnableButtons(WizardForm wizardForm, bool bTaskInProgress)
		{
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

		private Task? m_tskImport = null;
		private CancellationTokenSource? m_cTknSource = null;
	}
}
