using System.Diagnostics;
using DTInterfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSVDataSrc
{
	public class CSVDTDataSource : IDTDataSource
	{
		private StreamReader? m_strmReader;

		public CSVDTDataSource() { }
		~CSVDTDataSource()
		{
			Dispose();
		}

		// ********* IDTDataSource implementation *********
		public string Name // public property
		{
			get { return "CSV Data Source"; }
		}

		public DS_TYPE DataSourceType
		{
			get { return DS_TYPE.DST_CSV; }
		}

		public bool Open(SourceInfo srcInfo)
		{
			if (m_strmReader is not null)
			{
				m_strmReader.Close();
				m_strmReader.Dispose();
			}
			else
			{
				m_strmReader = new StreamReader(srcInfo.FilePath);
				bool bRetVal = m_strmReader.ReadLine() != null; // Try to read the first line to test the connection
				if (bRetVal)
				{
					m_srcInfo = srcInfo; // Cache the source info if the connection test is successful

					if (m_strmReader.BaseStream.CanSeek) // Check if the stream supports seeking
					{
						m_strmReader.BaseStream.Seek(0, SeekOrigin.Begin); // Position to the start

						// Discard the character buffer of the StreamReader
						m_strmReader.DiscardBufferedData(); // Essential to synchronize the reader with the new stream position
					}
				}
			}

			return true;
		}

		public bool Close()
		{
			if (m_strmReader is not null)
			{
				m_strmReader.Close();
				m_strmReader.Dispose();
			}

			return true;
		}

		// ****** IDataSource-specific methods *****
		public List<string> GetColumnNames()
		{
			List<string> lstColumnNames = new List<string>();

			try
			{
				// Use a 'using' statement to ensure the StreamReader is properly closed
				using (StreamReader sr = new StreamReader(m_srcInfo.FilePath))
				{
					string? line = sr.ReadLine();
					if (line != null)
					{
						DS_OPTIONS dsOptions = m_srcInfo.Options;
						char delimiter = (((dsOptions & DS_OPTIONS.DS_OPT_COMMA_DELIMITED) != 0) ? ',' : '\t');
						string[] aszColumnNames = line.Split(delimiter);
						if ((dsOptions & DS_OPTIONS.DS_OPT_HAS_HEADER) != 0)
						{
							lstColumnNames.AddRange(aszColumnNames);
						}
						else
						{
							// If there's no header, generate generic column names
							// based on the number of columns in the first line
							for (int i = 0; i < aszColumnNames.Length; i++)
							{
								lstColumnNames.Add($"Column{i + 1}");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"The file could not be read:");
				Console.WriteLine(ex.Message);
			}

			return lstColumnNames;
		}

		public bool TestConnection(SourceInfo srcInfo)
		{
			bool bRetVal = false;

			// Use a 'using' statement to ensure the StreamReader is properly closed
			using (StreamReader sr = new StreamReader(srcInfo.FilePath))
			{
				bRetVal = sr.ReadLine() != null; // Try to read the first line to test the connection
				if (bRetVal)
				{
					m_srcInfo = srcInfo; // Cache the source info if the connection test is successful
				}
			}

			return bRetVal;
		}

		public int GetRecordCount()
		{
			if (m_nRecordCount < 0)
			{
				int nRecordCount = 0;

				try
				{
					// Use a 'using' statement to ensure the StreamReader is properly closed
					using (StreamReader sr = new StreamReader(m_srcInfo.FilePath))
					{
						string? line;
						// Read the file line by line until the end (ReadLine returns null)
						while ((line = sr.ReadLine()) != null)
						{
							nRecordCount++;
						}
					}
				}
				catch (Exception ex)
				{
					Trace.WriteLine($"The file could not be read:");
					Trace.WriteLine(ex.Message);
					nRecordCount = -1; // Set record count to 0 if there's an error reading the file
				}

				bool bHasHeader = (m_srcInfo.Options & DS_OPTIONS.DS_OPT_HAS_HEADER) != 0;
				m_nRecordCount = (bHasHeader) ? Math.Max(-1, (nRecordCount - 1)) : nRecordCount;
			}

			return m_nRecordCount;
		}

		public bool GetNextRecord(List<string> lstColumnNames, out Dictionary<string, string> dctDataCols)
		{
			bool bRetVal = false;
			dctDataCols = new Dictionary<string, string>();

			string? line = m_strmReader.ReadLine();
			if (line != null)
			{
				DS_OPTIONS dsOptions = m_srcInfo.Options;
				char delimiter = (((dsOptions & DS_OPTIONS.DS_OPT_COMMA_DELIMITED) != 0) ? ',' : '\t');
				string[] aszColValues = line.Split(delimiter);
				if (aszColValues.Length != lstColumnNames.Count)
				{
					Trace.WriteLine($"Warning: Number of columns in the record ({aszColValues.Length}) does not match the number of column names ({lstColumnNames.Count}).");
				}
				else
				{
					for (int i = 0; i < lstColumnNames.Count; i++)
					{
						dctDataCols[lstColumnNames[i]] = aszColValues[i];
					}

					bRetVal = true;
				}
			}

			return bRetVal; // Return true if a record was successfully read, false if there are no more records
		}

		public SourceInfo? DataSrcInfo 
		{ 
			get { return m_srcInfo; }
			set 
			{
				if ((value is not null) && ((m_srcInfo is null) || !value.Equals(m_srcInfo)))
				{
					m_srcInfo = value;
					m_nRecordCount = -1; // Reset record count cache when source info changes
				}
			}
		}
		// ****** End of IDTDataSource-specific methods *****


		// ***** IDisposable implementation *****
		public void Dispose()
		{
			Close();
		}
		// ***** End of IDisposable implementation *****

		private SourceInfo? m_srcInfo = null;
		private int m_nRecordCount = -1; // Cache record count after first retrieval
	}
}
