using DTInterfaces;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using MySql.Data;
using System.Text;
using Org.BouncyCastle.Security;

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
			(Name, Type, AllowsNulls, KeyType, DefaultValue) = (name, type, bAllowsNulls, keyType, defaultValue);
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

		public bool Open(Object objConnectInfo)
		{
			bool bRetVal = true;

			try
			{
				Close();

				DBConnectInfo dbConnectInfo = objConnectInfo as DBConnectInfo ?? throw new ArgumentException("Invalid connection info object. Expected type: DBConnectInfo", nameof(objConnectInfo));
				string connectionString = $"Server={dbConnectInfo.Server};User ID={dbConnectInfo.UserID};Password={dbConnectInfo.Password};Database={dbConnectInfo.Database}";

				// Establish Connection
				MySqlConnection mySQLConnection = new MySqlConnection(connectionString);
				mySQLConnection.Open();
				if (mySQLConnection.State == System.Data.ConnectionState.Open)
				{
					m_mySQLConnection = mySQLConnection;
					m_dbConnectInfo = dbConnectInfo;
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
			Dictionary<string, Object> dctColumnInfo = null;

			try
			{
				if ((m_dbConnectInfo != null) && Open(m_dbConnectInfo))
				{
					dctColumnInfo = new Dictionary<string, Object>();
					GetTableColumnInfoInternal(tableName, dctColumnInfo);
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
			finally
			{
				Close();
			}

			return dctColumnInfo ?? new Dictionary<string, object>();
		}

		private void GetTableColumnInfoInternal(string csTableName, Dictionary<string, Object> dctColumnInfo)
		{
			System.Diagnostics.Debug.Assert((m_mySQLConnection is not null) && (m_mySQLConnection.State == System.Data.ConnectionState.Open));

			string sqlCmd = $"DESCRIBE {csTableName}";
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

		public bool WriteRecord(Dictionary<string, string> dctDataCols)
		{
			bool bRetVal = false;

			// Prepare the insert statement
			string csPreparedStmt = CreatePreparedStatement(dctDataCols);

			// Do this once per table, since the column info won't change during the import process.
			// This is needed to determine the data type of each column, so we can format the value
			// correctly for MySQL (e.g. dates need to be formatted as YYYY-MM-DD).
			if (m_dctColumnInfo is null)
			{
				m_dctColumnInfo = new Dictionary<string, Object>();
				GetTableColumnInfoInternal(m_csTargetTable, m_dctColumnInfo);
			}
			
			//#if DEBUG
			//	Trace.WriteLine(csPreparedStmt);
			//#endif

			using (var command = new MySqlCommand(csPreparedStmt, m_mySQLConnection))
			{
				bRetVal = OutputRecord(csPreparedStmt, dctDataCols, command);
			}

			return bRetVal;
		}

		public uint GetRecordCount(Object objConnectInfo, string tableName)
		{
			uint uiCount = 0;
			try
			{
				DBConnectInfo dbConnectInfo = objConnectInfo as DBConnectInfo ?? throw new ArgumentException("Invalid connection info object. Expected type: DBConnectInfo", nameof(objConnectInfo));
				if ((dbConnectInfo != null) && Open(dbConnectInfo))
				{
					string sqlCmd = $"SELECT COUNT(*) FROM {dbConnectInfo.Database}.{tableName}";
					MySqlCommand cmd = new MySqlCommand(sqlCmd, m_mySQLConnection);
					Object objValue = cmd.ExecuteScalar();
					if (!(objValue != null && uint.TryParse(objValue.ToString(), out uiCount)))
					{
						Trace.WriteLine($"Failed to get record count for table {tableName}. ExecuteScalar returned null or non-integer value.");
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
			finally
			{
				Close();
			}

			return uiCount;
		}

		public void ClearTable(Object objConnectInfo, string tableName)
		{
			try
			{
				DBConnectInfo dbConnectInfo = objConnectInfo as DBConnectInfo ?? throw new ArgumentException("Invalid connection info object. Expected type: DBConnectInfo", nameof(objConnectInfo));
				if ((dbConnectInfo != null) && Open(dbConnectInfo))
				{
					string sqlCmd = $"TRUNCATE {dbConnectInfo.Database}.{tableName}";
					MySqlCommand cmd = new MySqlCommand(sqlCmd, m_mySQLConnection);
					cmd.ExecuteNonQuery();
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
			finally
			{
				Close();
			}
		}

		// ************** MySQL-specific methods ****************
		private string CreatePreparedStatement(Dictionary<string, string> dctDataCols)
		{
			StringBuilder sb = new StringBuilder($"INSERT INTO {m_csTargetTable} (");

			foreach (KeyValuePair<string, string> kvPair in dctDataCols)
			{
				string csColumn = $"{kvPair.Key},";
				sb.Append(csColumn);
			}

			sb.Remove(sb.Length - 1, 1);    // Remove trailing ','
			sb.Append(") VALUES (");

			foreach (KeyValuePair<string, string> kvPair in dctDataCols)
			{
				sb.Append($"@{kvPair.Key},");
			}

			sb.Remove(sb.Length - 1, 1);    // Remove trailing ','
			sb.Append(")");

			return sb.ToString();
		}

		// Process the line and insert into the database, using the prepared statement
		private bool OutputRecord(string csLine, Dictionary<string, string> dctDataCols, MySqlCommand mySqlCmd)
		{
			bool bRetVal = false;

			try
			{
				mySqlCmd.Parameters.Clear();    // Clear previous values
				foreach (KeyValuePair<string, string> kvPair in dctDataCols)
				{
					string csDBColumn = kvPair.Key;
					string csRawValue = kvPair.Value;
					string csParamValue;
					string csColName = $"@{csDBColumn}";
					if (csRawValue.Length == 0)
					{
						//#if DEBUG
						//	Trace.WriteLine($"Setting parameter {csColName} to value 'DBNull.Value'");
						//#endif
						mySqlCmd.Parameters.AddWithValue(csColName, DBNull.Value);
					}
					else
					{
						// Remove any extranneous spaces or double quotes
						csRawValue = csRawValue.Trim();
						csRawValue = csRawValue.Trim('"');
						csParamValue = GetColumnValue(csRawValue, csDBColumn);

						//#if DEBUG
						//	Trace.WriteLine($"Setting parameter {csColName} to value {csParamValue}");
						//#endif

						mySqlCmd.Parameters.AddWithValue(csColName, csParamValue);
					}
				}

				int nRows = mySqlCmd.ExecuteNonQuery();  // Should return 1, since we're inserting 1 row
				bRetVal = (nRows == 1);
			}
			catch (Exception ex)
			{
				Trace.WriteLine(csLine);
				Trace.WriteLine(ex.Message);
				Trace.WriteLine(ex.StackTrace);
			}

			return bRetVal;
		}

		// Return a value that can be input into MySQL.
		// A date is formatted as YYYY-MM-DD
		// All other types are returned as-is.
		private string GetColumnValue(string csValue, string csColumnName)
		{
			string csMySQL = csValue;

			Object objColType;
			if (m_dctColumnInfo.TryGetValue(csColumnName, out objColType))
			{
				MySQLFieldInfo mySqlColType = objColType as MySQLFieldInfo ?? throw new InvalidCastException($"Column info for column {csColumnName} cannot be cast to MySQLFieldInfo.");
				string csSqlColType = mySqlColType.Type.ToLower();
				int nParen = csSqlColType.IndexOf("(");
				if (nParen > 0)
				{
					csSqlColType = csSqlColType.Substring(0, nParen);
				}

				if (IsDateType(csSqlColType))
				{
					csMySQL = GetDateType(csValue, csSqlColType);
				}
			}
			else
			{
				Debug.Assert(false, $"Cannot find column info for column {csColumnName}");
			}

			return csMySQL;
		}

		bool IsStringType(string szColumnType)
		{
			string szType = szColumnType.ToLower();
			string[] aszStringTypes =
			{
				"char",
				"varchar",
				"binary",
				"varbinary",
				"tinytext",
				"text",
				"mediumtext",
				"longtext"
			};

			return aszStringTypes.Contains(szType);
		}

		private bool IsDateType(string csColumnType)
		{
			string csType = csColumnType.ToLower();
			string[] acsDateTypes =
			{
				"date",
				"time",
				"year",
				"datetime"
			};
			return acsDateTypes.Contains(csType);
		}

		private string GetDateType(string csValue, string csColumnType)
		{
			string csDateValue = csValue;
			DateTime dtDateTime;
			if (DateTime.TryParse(csValue, out dtDateTime))
			{
				switch (csColumnType)
				{
					case "date":
						csDateValue = dtDateTime.ToString("yyyy-MM-dd");
						break;

					case "time":
						csDateValue = dtDateTime.ToString("HH-mm-ss");
						break;

					case "year":
						csDateValue = dtDateTime.ToString("yyyy");
						break;

					case "datetime":
						csDateValue = dtDateTime.ToString("yyyy-MM-dd HH:mm:ss");
						break;
				}
				;
			}

			return csDateValue;
		}

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
		private Dictionary<string, Object>? m_dctColumnInfo = null;
		private string m_csTargetTable = string.Empty;
	}
}
