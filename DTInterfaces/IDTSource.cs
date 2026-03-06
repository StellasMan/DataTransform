namespace DTInterfaces
{
	public enum DS_TYPE
	{
		DST_CSV = 0,
		DST_EXCEL
	};

	// Bitmask options for data source
	public enum DS_OPTIONS
	{
		DS_OPT_NONE = 0,
		DS_OPT_HAS_HEADER = 1,
		DS_OPT_COMMA_DELIMITED = 2
	};

	public class SourceInfo : IEquatable<SourceInfo>
	{
		private string m_filePath;
		private DS_OPTIONS m_dsOptions;

		public SourceInfo() : this(string.Empty, DS_OPTIONS.DS_OPT_NONE) { }
		public SourceInfo(string filePath, DS_OPTIONS dsOptions) => (m_filePath, m_dsOptions) = (filePath, dsOptions);

		public string FilePath
		{
			get { return m_filePath; }
			set { if (value != null) m_filePath = value; }
		}

		public DS_OPTIONS Options
		{
			get { return m_dsOptions; }
			set { m_dsOptions = value; }
		}

		// ************** IEquatable<T> implementation for value equality **************

		// Overload the == operator
		public static bool operator ==(SourceInfo left, SourceInfo right)
		{
			bool bRetVal = false;
			// Handle nulls on either side
			if (ReferenceEquals(left, null))
			{
				bRetVal = ReferenceEquals(right, null);
			}
			else
			{
				bRetVal = left.Equals(right);
			}

			return bRetVal;
		}

		// Overload the != operator
		public static bool operator !=(SourceInfo left, SourceInfo right) => !(left == right);

		// Override System.Object.Equals
		public override bool Equals(object? obj)
		{
			bool bRetVal = false;
			if (obj is SourceInfo connectionParams)
			{
				bRetVal = Equals(connectionParams);
			}

			return bRetVal;
		}

		// Implement IEquatable<T>.Equals for strong typing and efficiency
		public bool Equals(SourceInfo? other)
		{
			bool bRetVal = false;
			if (!ReferenceEquals(other, null) && ReferenceEquals(this, other))
			{
				bRetVal = true;
			}
			else
			{
				// Value equality logic
				bRetVal = (string.Equals(m_filePath, other.m_filePath, StringComparison.OrdinalIgnoreCase) && (m_dsOptions == other.m_dsOptions));
			}

			return bRetVal;
		}

		// Override GetHashCode (essential for collections)
		public override int GetHashCode()
		{
			// Combine hash codes of the fields that determine equality
			return HashCode.Combine(m_filePath, m_dsOptions);
		}

		// ************** End of IEquatable<T> implementation **************
	}

	public delegate bool DTSrcRecordDelegate(Dictionary<string, string> dctDataCols, bool bError);

	public interface IDTDataSource
	{
		bool Initialize(SourceInfo srcInfo);
		bool EnumerateRecords(Dictionary<string, string> dctFieldMapping, DTSrcRecordDelegate dctDelegate);

		string Name { get; }
		List<string> ColumnNames { get; }
		uint RecordCount { get; }
		DS_TYPE DataSourceType { get; }
		SourceInfo? DataSrcInfo { get; set; }
	}
}
