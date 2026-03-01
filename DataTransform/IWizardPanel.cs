using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTInterfaces;

namespace DataTransform
{
	public enum BT_TYPE
	{
		BT_BACK,
		BT_NEXT
	};

	public enum PAGE_TYPE
	{
		PAGE_SOURCE,
		PAGE_DEST,
		PAGE_MAPPING,
		PAGE_TRANSFORM,
		PAGE_SUMMARY
	};

	internal interface IWizardPanelSource : IWizardPanel
	{
		IDTDataSource? GetDataSource();
		string SourceFile { get; }
		int RecordCount { get; }
	}

	internal interface IWizardPanelTarget : IWizardPanel
	{
		IDTDataTarget? GetDataTarget();
		string TargetTable { get; }
	}

	internal interface IWizardPanel
	{
		void RefreshUI(WizardForm wizardForm);
		bool IsButtonEnabled(BT_TYPE btType);
		PAGE_TYPE GetPageType();
		bool IsImplemented();
		bool ValidateInput();
	}
}
