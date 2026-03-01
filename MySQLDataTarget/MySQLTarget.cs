using DTInterfaces;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using MySql.Data;

namespace MySQLDataTarget
{
	public enum KeyType
	{
		None,
		PrimaryKey,
		UniqueKey,
		MultiKey
	}

	public class DBConnectInfo : IEquatable<DBConnectInfo>
	{
		public string Server { get; set; }
		public string Database { get; set; }
		public string UserID { get; set; }
		public string Password { get; set; }
		public DBConnectInfo() : this(string.Empty, string.Empty, string.Empty, string.Empty) { }
		public DBConnectInfo(string server, string database, string userID, string password) => (Server, Database, UserID, Password) = (server, database, userID, password);

		public bool Equals(DBConnectInfo? other)
		{
			bool bRetVal = false;
			if ((other is not null) && GetType() == other.GetType())
			{
				bRetVal =
				(
					(string.Equals(Server, other.Server, StringComparison.Ordinal)) &&
					(string.Equals(Database, other.Database, StringComparison.Ordinal)) &&
					(string.Equals(UserID, other.UserID, StringComparison.Ordinal)) &&
					(string.Equals(Password, other.Password, StringComparison.Ordinal))
				);
			}

			return bRetVal;
		}
	}

	public class MySQLFieldInfo
	{
		public MySQLFieldInfo(string name, string type, bool bAllowsNulls, KeyType keyType, string defaultValue)
		{
			Name = name;
			Type = type;
			AllowsNulls = bAllowsNulls;
			KeyType = keyType;
			DefaultValue = defaultValue;
		}

		public string Name { get; set; }
		public string Type { get; set; }
		public bool AllowsNulls { get; set; }
		public KeyType KeyType { get; set; }
		public string DefaultValue { get; set; }
	}

	public class MySQLDTDataTarget : IDTDataTarget
	{
		public MySQLDTDataTarget() 
		{ 
			Trace.WriteLine("MySQLDTDataTarget instance created.");
		}

		~MySQLDTDataTarget()
		{ 
			Dispose(); 
		}

		public string Server 
		{ 
			get 
			{
				return (m_dbConnectInfo != null) ? m_dbConnectInfo.Server : string.Empty;
			} 
		}

		public string Database 
		{
			get
			{
				return (m_dbConnectInfo != null) ? m_dbConnectInfo.Database : string.Empty;
			}
		}

		//************** IDTDataTarget implementation ****************
		public string Name { get { return "MySQL Database"; } }
		public DT_TYPE DataTargetType { get { return DT_TYPE.DTT_MYSQL; } }
		public List<string> TableNames { get { return GetTableNames(); } }
		public string TargetTable 
		{ 
			get { return m_csTargetTable; } 
			set { if (value is not null) m_csTargetTable = value; }
		}

		public bool Connect(Object objConnectInfo)
		{
			bool bRetVal = true;

			try
			{
				Close();

				DBConnectInfo dbConnectInfo = objConnectInfo as DBConnectInfo ?? throw new ArgumentException("Invalid connection info object. Expected type: DBConnectInfo", nameof(objConnectInfo));
				string connectionString = $"Server={dbConnectInfo.Server};User ID={dbConnectInfo.UserID};Password={dbConnectInfo.Password};Database={dbConnectInfo.Database}";

				// Establish Connection
				m_mySQLConnection = new MySqlConnection(connectionString);
				m_mySQLConnection.Open();
				if (m_mySQLConnection.State == System.Data.ConnectionState.Open)
				{
					m_dbConnectInfo = dbConnectInfo;
					Trace.WriteLine("Connected successfully!");
				}
				else
				{
					Trace.WriteLine("Failed to connect.");
					bRetVal = false;
				}
			}
			catch (MySqlException ex)
			{
				Trace.WriteLine($"MySQL error: {ex.Message}");
				bRetVal = false;
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"General error: {ex.Message}");
				bRetVal = false;
			}

			return bRetVal;
		}

		public bool Close()
		{
			Dispose();
			return true;
		}

		public void Dispose()
		{
			if (m_mySQLConnection is not null)
			{
				m_mySQLConnection.Close();
				m_mySQLConnection.Dispose();
				m_mySQLConnection = null;
			}
		}

		public bool TestConnection(Object objConnectInfo)
		{
			bool bRetVal = true;

			DBConnectInfo dbConnectInfo = objConnectInfo as DBConnectInfo ?? throw new ArgumentException("Invalid connection info object. Expected type: DBConnectInfo", nameof(objConnectInfo));
			string connectionString = $"Server={dbConnectInfo.Server};User ID={dbConnectInfo.UserID};Password={dbConnectInfo.Password};Database={dbConnectInfo.Database}";

			try
			{
				// Establish temporary connection
				using MySqlConnection mySQLConnection = new MySqlConnection(connectionString);
				mySQLConnection.Open();
				if (mySQLConnection.State == System.Data.ConnectionState.Open)
				{
					Trace.WriteLine("Connected successfully!");
				}
				else
				{
					Trace.WriteLine("Failed to connect.");
					bRetVal = false;
				}
			}
			catch (MySqlException ex)
			{
				Trace.WriteLine($"MySQL error: {ex.Message}");
				bRetVal = false;
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"General error: {ex.Message}");
				bRetVal = false;
			}

			return bRetVal;
		}

		public Dictionary<string, Object> GetTableColumnInfo(string tableName)
		{
			Dictionary<string, Object> dctColumnInfo = new Dictionary<string, Object>();

			try
			{
				if ((m_dbConnectInfo != null) && Connect(m_dbConnectInfo))
				{
					System.Diagnostics.Debug.Assert((m_mySQLConnection is not null) && (m_mySQLConnection.State == System.Data.ConnectionState.Open));

					string sqlCmd = $"DESCRIBE {tableName}";
					MySqlCommand cmd = new MySqlCommand(sqlCmd, m_mySQLConnection);

					// Execute the query and get the results in a data reader
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						// Read rows one by one
						while (reader.Read())
						{
							// Access data by column name or index
							string fieldName = GetStringValue(reader, "Field");
							string fieldType = GetStringValue(reader, "Type");
							bool bNullAllowed = GetStringValue(reader, "Null").Equals("YES", StringComparison.OrdinalIgnoreCase) ? true : false;
							string csKeyType = GetStringValue(reader, "Key");
							KeyType keyType = (csKeyType.Equals("PRI", StringComparison.OrdinalIgnoreCase)) ? KeyType.PrimaryKey :
												(csKeyType.Equals("UNI", StringComparison.OrdinalIgnoreCase)) ? KeyType.UniqueKey :
												(csKeyType.Equals("MUL", StringComparison.OrdinalIgnoreCase)) ? KeyType.MultiKey : KeyType.None;

							string fieldDefault = GetStringValue(reader, "Default");

							MySQLFieldInfo mySqlFieldInfo = new MySQLFieldInfo(fieldName, fieldType, bNullAllowed, keyType, fieldDefault);
							dctColumnInfo.Add(fieldName, mySqlFieldInfo as Object);
						}
					}
				}
			}
			catch (MySqlException ex)
			{
				Trace.WriteLine($"MySQL error: {ex.Message}");
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"General error: {ex.Message}");
			}

			return dctColumnInfo;
		}

		public bool WriteRecord(Dictionary<string, string> dctDataCols)
		{
			throw new NotImplementedException();
		}

		// ************** MySQL-specific methods ****************

		private List<string> GetTableNames()
		{
			List<string> lstTables = new List<string>();
			try
			{
				if ((m_mySQLConnection is not null) && (m_mySQLConnection.State == System.Data.ConnectionState.Open))
				{
					MySqlCommand cmd = new MySqlCommand("SHOW TABLES", m_mySQLConnection);

					// Execute the query and get the results in a data reader
					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						// Read rows one by one
						while (reader.Read())
						{
							// Access data by column name or index
							string tableName = reader.GetString(0);
							lstTables.Add(tableName);
						}
					}
				}
			}
			catch (MySqlException ex)
			{
				Trace.WriteLine($"MySQL error: {ex.Message}");
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"General error: {ex.Message}");
			}

			return lstTables;
		}

		private string GetStringValue(MySqlDataReader reader, string csColumn)
		{
			string csColValue;
			if (!reader.IsDBNull(reader.GetOrdinal(csColumn)))
			{
				csColValue = reader.GetString(reader.GetOrdinal(csColumn));
			}
			else
			{
				csColValue = string.Empty;
			}

			return csColValue;
		}

		DBConnectInfo? m_dbConnectInfo;
		MySqlConnection? m_mySQLConnection;
		private string m_csTargetTable = string.Empty;
	}
}
