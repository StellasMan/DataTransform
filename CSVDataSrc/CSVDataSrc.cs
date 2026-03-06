using System.Diagnostics;
using DTInterfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace CSVDataSrc
{
	public class CSVDTDataSource : IDTDataSource
	{
		public CSVDTDataSource() { }
		~CSVDTDataSource()
		{
		}

		// ****** IDataSource-specific methods *****



		public bool Initialize(SourceInfo srcInfo)
		{
			bool bRetVal = true;
			if (!m_bInitialized || (m_srcInfo is null) || (m_srcInfo != srcInfo))
			{
				bRetVal = false;
				m_bInitialized = false;
				m_lstColumnNames.Clear();
				try
				{
					using (var reader = new StreamReader(srcInfo.FilePath))
					{

						var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
						if (!srcInfo.Options.HasFlag(DS_OPTIONS.DS_OPT_COMMA_DELIMITED))
							csvConfig.Delimiter = "\t";

						using (var csv = new CsvReader(reader, csvConfig))
						{
							if (csv.Read())
							{
								if (srcInfo.Options.HasFlag(DS_OPTIONS.DS_OPT_HAS_HEADER))
								{
									if (csv.ReadHeader())
									{
										foreach (string item in csv.HeaderRecord)
										{
											m_lstColumnNames.Add(item);
										}

										bRetVal = true;
									}
								}
								else
								{
									for (int i = 1; i <= csv.Parser.Count; i++)
									{
										m_lstColumnNames.Add($"Col{i}");
									}

									bRetVal = true;
								}
							}
							else
							{
								Trace.WriteLine("The file is empty.");
							}
						}
					}
				}
				catch (Exception ex)
				{
					Trace.WriteLine($"The file could not be read:");
					Trace.WriteLine(ex.Message);
				}

				if (bRetVal)
				{
					m_srcInfo = srcInfo;
					GetRecordCount(); // Cache the record count after successful initialization
				}
			}

			return bRetVal;
		}

		public bool EnumerateRecords(Dictionary<string, string> dctFieldMapping, DTSrcRecordDelegate dctDelegate)
		{
			bool bRetVal = true;
			try
			{
				using (var reader = new StreamReader(m_srcInfo.FilePath))
				{

					var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
					if (!m_srcInfo.Options.HasFlag(DS_OPTIONS.DS_OPT_COMMA_DELIMITED))
						csvConfig.Delimiter = "\t";

					using (var csv = new CsvReader(reader, csvConfig))
					{
						//var records = new List<string>();
						Dictionary<string, string> dctDataCols = new Dictionary<string, string>();

						if (m_srcInfo.Options.HasFlag(DS_OPTIONS.DS_OPT_HAS_HEADER))
						{
							csv.Read();
							csv.ReadHeader();
						}

						bool bContinue = true;
						while (csv.Read())
						{
							dctDataCols.Clear();

							if (csv.Parser.Record == null)
							{
								Trace.WriteLine("Warning: Reached end of file or encountered an empty line.");
								continue; // Skip to the next iteration if there's no record
							}

							string[] aszColValues = csv.Parser.Record;
							if (aszColValues.Length != m_lstColumnNames.Count)
							{
								string csError = $"Warning: Number of columns in the record ({aszColValues.Length}) does not match the number of column names ({m_lstColumnNames.Count}).";
								Trace.WriteLine(csError);
								bContinue = dctDelegate(dctDataCols, true); // Pass an empty dictionary and indicate an error
							}
							else
							{
								// public delegate void DTSrcRecordDelegate(Dictionary<string, string> dctDataCols, bool bError);
								for (int i = 0; i < m_lstColumnNames.Count; i++)
								{
									if (dctFieldMapping.TryGetValue(m_lstColumnNames[i], out string mappedField))
									{
										dctDataCols[mappedField] = aszColValues[i];
									}
								}

								bContinue = dctDelegate(dctDataCols, false);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine($"The file could not be read:");
				Trace.WriteLine(ex.Message);
				bRetVal = false;
			}

			return bRetVal;
		}

		public SourceInfo? DataSrcInfo 
		{ 
			get { return m_srcInfo; }
			set 
			{
				if ((value is not null) && ((m_srcInfo is null) || !value.Equals(m_srcInfo)))
				{
					m_srcInfo = value;
					m_uiRecordCount = 0; // Reset record count cache when source info changes
				}
			}
		}

		public string Name // public property
		{
			get { return "CSV Data Source"; }
		}

		public List<string> ColumnNames
		{
			get { return m_lstColumnNames; }
		}

		public DS_TYPE DataSourceType
		{
			get { return DS_TYPE.DST_CSV; }
		}

		public uint RecordCount
		{
			get { return m_uiRecordCount; }
		}

		// ****** End of IDTDataSource-specific methods *****

		private uint GetRecordCount()
		{
			if (!m_bInitialized)
			{
				m_uiRecordCount = 0;

				try
				{
					// Use a 'using' statement to ensure the StreamReader is properly closed
					using (StreamReader sr = new StreamReader(m_srcInfo.FilePath))
					{
						string? line;
						// Read the file line by line until the end (ReadLine returns null)
						while ((line = sr.ReadLine()) != null)
						{
							m_uiRecordCount++;
						}

						m_bInitialized = true; // Set the flag to indicate that we've initialized the record count
					}
				}
				catch (Exception ex)
				{
					Trace.WriteLine($"The file could not be read:");
					Trace.WriteLine(ex.Message);
					m_uiRecordCount = 0; // Set record count to 0 if there's an error reading the file
				}
			}

			return m_uiRecordCount;
		}

		private List<string> m_lstColumnNames = new List<string>();
		private SourceInfo? m_srcInfo = null;
		private uint m_uiRecordCount = 0;
		private bool m_bInitialized = false;
	}
}
