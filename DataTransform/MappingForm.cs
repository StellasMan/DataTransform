using MySQLDataTarget;
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
	public partial class MappingForm : UserControl, IWizardPanel
	{
		public MappingForm()
		{
			InitializeComponent();
		}

		public void RefreshUI(WizardForm wizardForm)
		{
			DisplayMapping();
		}

		public void DisplayMapping()
		{
			WizardForm wizardForm = this.Parent.Parent as WizardForm;

			// Automatically map the columns and fields that have the same name (case-insensitive)
			// We only do this once for each file/table pair, so we don't overwrite any mapping changes the user may have made already.
			if (wizardForm.m_bAutoMapEnabled)
			{
				AutoMapColumnsToFields(wizardForm);
				wizardForm.m_bAutoMapEnabled = false;
			}

			// Display the source file columns that are not yet mapped to target fields
			lstViewColumns.Items.Clear();

			// Get a list of all the mapped target fields so we can exclude them from the list of available fields
			List<string> lstFields = m_mapColToField.Values.ToList();

			foreach (string csColName in wizardForm.m_lstColumnNames)
			{
				if (!m_mapColToField.TryGetValue(csColName, out string csFieldName))
				{
					// Column is not mapped, so add it to the list of available columns
					ListViewItem item = new ListViewItem(csColName);
					lstViewColumns.Items.Add(item);
				}
			}

			// Display the target fields that are not yet mapped to source columns
			lstViewFields.Items.Clear();
			foreach (KeyValuePair<string, MySQLFieldInfo> kvp in wizardForm.m_mapColToFieldInfo)
			{
				if (!lstFields.Contains(kvp.Key))
				{
					ListViewItem item = new ListViewItem(kvp.Key);
					lstViewFields.Items.Add(item);
				}
			}

			// Display the existing mappings
			DisplayMappedFields();
		}

		private void DisplayMappedFields()
		{
			WizardForm wizardForm = this.Parent.Parent as WizardForm;

			lstViewMapped.Items.Clear();
			foreach (string csColName in wizardForm.m_lstColumnNames)
			{
				if (m_mapColToField.TryGetValue(csColName, out string csFieldName))
				{
					ListViewItem item = new ListViewItem(csColName);
					item.SubItems.Add(csFieldName);
					lstViewMapped.Items.Add(item);
				}
			}

			EnableMappingButtons();
		}

		public void EnableMappingButtons()
		{
			// Enable the 'Map' button only if there are available columns and fields to map
			btnMap.Enabled = (lstViewColumns.Items.Count > 0) && (lstViewFields.Items.Count > 0);
			btnUnMap.Enabled = (lstViewMapped.Items.Count > 0);
		}

		public void AutoMapColumnsToFields(WizardForm wizardForm)
		{
			foreach (string csColName in wizardForm.m_lstColumnNames)
			{
				if (!m_mapColToField.TryGetValue(csColName, out string csFieldName))
				{
					// Column is not mapped, so check if there is a field with the same name (case-insensitive)
					string csMatchingField = wizardForm.m_mapColToFieldInfo.Keys.FirstOrDefault(f => string.Equals(f, csColName, StringComparison.OrdinalIgnoreCase));
					if (!string.IsNullOrEmpty(csMatchingField))
					{
						// Found a matching field, so create the mapping
						m_mapColToField[csColName] = csMatchingField;

						// Update the display to show the mapping
						ListViewItem item = new ListViewItem(csColName);
						item.SubItems.Add(csMatchingField);
						lstViewMapped.Items.Add(item);
					}
				}
			}

			EnableMappingButtons();
		}

		// Check to see if the given table field is mapped to a source column. 
		private bool IsFieldMapped(string csFieldName)
		{
			WizardForm wizardForm = this.Parent.Parent as WizardForm;
			return m_mapColToField.Values.Contains(csFieldName);
		}

		private bool ValidateField(MySQLFieldInfo mySQLFieldInfo, out string csError)
		{
			bool bIsValid = true;
			csError = string.Empty;
			if ((mySQLFieldInfo != null) && !IsFieldMapped(mySQLFieldInfo.Name))
			{
				if (mySQLFieldInfo.KeyType == KeyType.UniqueKey)
				{
					// Unique key fields must be mapped to a source column.
					bIsValid = false;
					csError = $"Field {mySQLFieldInfo.Name} is not mapped.\nUnique key fields must be mapped.";
				}
				else if (mySQLFieldInfo.KeyType == KeyType.PrimaryKey)
				{
					// Primary key fields must be mapped to a source column.
					bIsValid = false;
					csError = $"Field {mySQLFieldInfo.Name} is not mapped.\nPrimary key fields must be mapped.";
				}
				else if (!mySQLFieldInfo.AllowsNulls && string.IsNullOrEmpty(mySQLFieldInfo.DefaultValue))
				{
					// Field doesn't allow NULLs and doesn't have a default value, so it
					// must be mapped to a source column.
					bIsValid = false;
					csError = $"Field {mySQLFieldInfo.Name} does not allow NULL values, has no default value, and is not mapped.\nFields without a default value that do not allow NULL values must be mapped to a source column.";
				}
			}

			return bIsValid;
		}

		private bool ValidateMapping()
		{
			bool bValid = true;

			WizardForm wizardForm = this.Parent.Parent as WizardForm;
			Dictionary<string, MySQLFieldInfo> mapColToFieldInfo = wizardForm.m_mapColToFieldInfo;
			if ((mapColToFieldInfo != null) && (mapColToFieldInfo.Count > 0))
			{
				foreach (KeyValuePair<string, MySQLFieldInfo> kvp in mapColToFieldInfo)
				{
					MySQLFieldInfo mySQLFieldInfo = kvp.Value;
					if (!ValidateField(mySQLFieldInfo, out string csError))
					{
						MessageBox.Show($"The field '{mySQLFieldInfo.Name}' has an invalid mapping:\n{csError}", "Mapping Validation Error");
						bValid = false;
						break;
					}
				}
			}
			else
			{
				MessageBox.Show("No target fields have been selected for mapping.", "Mapping Validation Error");
				bValid = false;
			}

			return bValid;
		}

		private void OnMap(object sender, EventArgs e)
		{
			if ((lstViewColumns.SelectedItems.Count > 0) && (lstViewFields.SelectedItems.Count > 0))
			{
				string csColName = lstViewColumns.SelectedItems[0].SubItems[0].Text;
				string csFieldName = lstViewFields.SelectedItems[0].SubItems[0].Text;
				WizardForm wizardForm = this.Parent.Parent as WizardForm;
				m_mapColToField[csColName] = csFieldName;

				// Remove the mapped column and field from the available lists
				lstViewColumns.Items.Remove(lstViewColumns.SelectedItems[0]);
				lstViewFields.Items.Remove(lstViewFields.SelectedItems[0]);

				// Display mapped fields in source file column order
				DisplayMappedFields();
			}
			else
			{
				MessageBox.Show("Please select both a source column and a target field to create a mapping.", "Error");
			}

			EnableMappingButtons();
		}

		private void OnUnMap(object sender, EventArgs e)
		{
			if (lstViewMapped.SelectedItems.Count > 0)
			{
				foreach (ListViewItem listViewItem in lstViewMapped.SelectedItems)
				{
					string csColName = listViewItem.SubItems[0].Text;
					string csFieldName = listViewItem.SubItems[1].Text;

					// Remove the mapping from the wizard form's mapping dictionary
					WizardForm wizardForm = this.Parent.Parent as WizardForm;
					m_mapColToField.Remove(csColName);

					// Add the column and field back to the available lists
					ListViewItem item = new ListViewItem(csColName);
					lstViewColumns.Items.Add(item);

					item = new ListViewItem(csFieldName);
					lstViewFields.Items.Add(item);

					// Remove the mapping from the display
					lstViewMapped.Items.Remove(listViewItem);
				}

				EnableMappingButtons();
			}
			else
			{
				MessageBox.Show("Please select a mapped pair to unmap.", "Error");
			}
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

		public PAGE_TYPE PageType
		{
			get { return PAGE_TYPE.PAGE_DEST; }
		}

		public bool IsImplemented
		{
			get { return true; }
		}

		public bool ValidateInput()
		{
			return true;
		}

		public Dictionary<string, string> FieldMapping
		{
			get { return m_mapColToField; }
		}

		// Mapping information
		public Dictionary<string, string> m_mapColToField = new Dictionary<string, string>();   // Maps source column name to target field name
	}
}
