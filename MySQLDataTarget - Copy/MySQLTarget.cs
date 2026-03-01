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

	public class DBClass
	{
		public MySql.Data.MySqlClient.MySqlConnection myConnection;
		public string? myConnectionString;
	}
}
