using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTInterfaces
{
	public enum DT_TYPE
	{
		DTT_MYSQL = 0,
		DTT_POSTGRES
	}

	public interface IDTDataTarget : IDisposable
	{
		string Name { get; }
		DT_TYPE DataTargetType { get; }
		List<string> TableNames { get; }
		string Server { get; }
		string Database { get;  }

		bool TestConnection(Object objConnectInfo);
		bool Connect(Object objConnectInfo);
		bool Close();
		bool WriteRecord(Dictionary<string, string> dctDataCols);
		Dictionary<string, Object> GetTableColumnInfo(string tableName);
	}
}
