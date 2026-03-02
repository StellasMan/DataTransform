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
	public partial class SummaryForm : UserControl, IWizardPanel
	{
		public SummaryForm()
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
					bEnabled = false;
					break;
			}

			return bEnabled;
		}

		public PAGE_TYPE PageType
		{
			get { return PAGE_TYPE.PAGE_SUMMARY; }
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
			Trace.WriteLine("SummaryForm: RefreshUI called");
		}
	}
}
