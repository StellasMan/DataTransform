using MySql.Data.MySqlClient;
using System;

using DTInterfaces;

namespace MySQLDataTarget
{
	public enum KeyType
	{
		None,
		PrimaryKey,
		UniqueKey,
		MultiKey
	}

	public class MySQLDTDataTarget : IDTDataTarget
	{
		public MySQLDTDataTarget()
		{ 
		}

		public string Name { get { return "MySQL Database"; } }
		public DT_TYPE DataTargetType { get { return DT_TYPE.DTT_MYSQL; } }
		public List<string> FieldNames { get { return GetFieldNames(); } }
		public string TargetTable { get { return m_csTargetTable; } }

		public bool TestConnection(DBConnectInfo dbConnectInfo)
		{
			throw new NotImplementedException();
		}

		public bool Connect(DBConnectInfo dbConnectInfo)
		{
			throw new NotImplementedException();
		}

		public bool Close()
		{
			throw new NotImplementedException();
		}

		public bool WriteRecord(Dictionary<string, string> dctDataCols)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		// ************** MySQL-specific methods ****************

		private List<string> GetFieldNames()
		{
			return new List<string> { "Column1", "Column2", "Column3" };
		}

		MySqlConnection connection = new MySqlConnection(connectionString);
		private DBConnectInfo? dbConnectInfo;
		private string m_csTargetTable = String.Empty;
	}
}
