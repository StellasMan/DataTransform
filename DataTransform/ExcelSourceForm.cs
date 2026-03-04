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
	public partial class ExcelSourceForm : UserControl, IWizardPanelSource
	{
		public ExcelSourceForm()
		{
			InitializeComponent();
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
					bEnabled = IsImplemented;
					break;
			}

			return bEnabled;
		}

		public PAGE_TYPE PageType
		{
			get { return PAGE_TYPE.PAGE_SOURCE; }
		}

		public bool IsImplemented
		{
			get { return false; }
		}

		public bool ValidateInput()
		{
			return true;
		}

		public void RefreshUI(WizardForm wizardForm)
		{
			Trace.WriteLine("SourceForm: RefreshUI called");
		}

		public IDTDataSource? DataSource
		{
			get { return null; }
		}

		public string SourceFile
		{
			get { return string.Empty; }
		}

		public uint RecordCount
		{
			get { return 0; }
		}
	}
}
