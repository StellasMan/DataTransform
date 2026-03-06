using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;

using MySQLDataTarget;
using DTInterfaces;

namespace DataTransform
{
	public partial class MySQLDestForm : UserControl, IWizardPanelTarget
	{
		public MySQLDestForm()
		{
			InitializeComponent();

			#if DEBUG
				txtServer.Text = "localhost";
				txtDatabase.Text = "aes";
				txtUserName.Text = "root";
				txtPassword.Text = "password";
			#endif
		}

		~MySQLDestForm()
		{
			m_mySqlTarget.Close();
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

		public void RefreshUI(WizardForm wizardForm)
		{
			Trace.WriteLine("DestForm: RefreshUI called");
		}

		private void OnConnect(object sender, EventArgs e)
		{
			cmbTargetTable.Items.Clear();

			string server = txtServer.Text;
			string database = txtDatabase.Text;
			string userID = txtUserName.Text;
			string password = txtPassword.Text;

			DBConnectInfo connectInfo = new DBConnectInfo(server, database, userID, password);
			try
			{
				if (m_mySqlTarget.Open(connectInfo))
				{
					m_connectInfo = connectInfo;
					List<string> lstTableNames = m_mySqlTarget.TableNames;
					foreach (string table in lstTableNames)
					{
						cmbTargetTable.Items.Add(table);
					}

					cmbTargetTable.SelectedIndex = (lstTableNames.Count > 0) ? 0 : -1;
				}
			}
			finally
			{
				m_mySqlTarget.Close();
			}
		}

		private void OnTableSelectChanged(object sender, EventArgs e)
		{
			int nselIndx = Math.Max(0, cmbTargetTable.SelectedIndex);
			if (cmbTargetTable.Items.Count > 0)
			{
				var cmbItem = cmbTargetTable.Items[nselIndx];
				if (cmbItem != null)
				{
					string csTableName = cmbItem.ToString() ?? string.Empty;
					if (!String.IsNullOrEmpty(csTableName) && (m_csTableName != csTableName))
					{
						m_csTableName = csTableName;
						m_mapColToFieldInfo = m_mySqlTarget.GetTableColumnInfo(csTableName);
					}
				}
			}
		}

		public IDTDataTarget? GetDataTarget()
		{
			return m_mySqlTarget;
		}

		public string TargetTable
		{
			get { return m_csTableName; }
		}

		public Object ConnectInfo
		{
			get { return m_connectInfo ?? new DBConnectInfo(); }
		}

		MySQLDTDataTarget m_mySqlTarget = new MySQLDTDataTarget();

		public string m_csTableName = string.Empty;
		public Dictionary<string, Object> m_mapColToFieldInfo = new Dictionary<string, Object>();
		private DBConnectInfo? m_connectInfo;
	}
}
