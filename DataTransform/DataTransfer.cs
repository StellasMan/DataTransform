using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTInterfaces;

// Declare callback delegate
public delegate void ProgressUpdateDelegate(int nPctComplete, int nRecordsWritten, int nRecordsFailed);

namespace DataTransform
{
	internal class DataTransfer
	{
		private IDTDataSource? m_idtDataSrc;
		private IDTDataTarget? m_idtDataTarget;
		private Dictionary<string, string>? m_dctFieldMapping = null;
		private CancellationToken m_token;
		private ProgressUpdateDelegate m_progressUpdateCallback;

		public DataTransfer(IDTDataSource? sourceData, IDTDataTarget? targetData, Dictionary<string, string>? dctFieldMapping, CancellationToken token, ProgressUpdateDelegate progressUpdateCallback)
		{
			(m_idtDataSrc, m_idtDataTarget, m_dctFieldMapping, m_token, m_progressUpdateCallback) = (sourceData, targetData, dctFieldMapping, token, progressUpdateCallback);
		}

		public void Execute()
		{
			uint uiTotalRecords = m_idtDataSrc.RecordCount;
			int nWritten = 0;
			int nErrors = 0;

			try
			{
				if ((m_idtDataSrc is not null) && (m_idtDataSrc.DataSrcInfo is not null) && (m_idtDataTarget is not null))
				{
					m_idtDataSrc.EnumerateRecords(m_dctFieldMapping, (dctDataCols, bError) =>
					{
						if (bError)
						{
							nErrors++;
							Trace.WriteLine($"Error processing record: {string.Join(", ", dctDataCols.Select(kv => $"{kv.Key}={kv.Value}"))}");
						}
						else
						{
							// bool bWriteSuccess = m_idtDataTarget.WriteRecord(dctDataCols);
							bool bWriteSuccess = true;
							if (bWriteSuccess)
							{
								nWritten++;
							}
							else
							{
								nErrors++;
								Trace.WriteLine($"Failed to write record: {string.Join(", ", dctDataCols.Select(kv => $"{kv.Key}={kv.Value}"))}");
							}
						}

						// Invoke the callback once every 100 records
						if ((nWritten + nErrors) % 100 == 0)
						{
							int nPctComplete = (int)Math.Round((double)(nWritten + nErrors) / uiTotalRecords * 100);
							m_progressUpdateCallback(nPctComplete, nWritten, nErrors);
						}
					});

					// Final callback invocation to ensure we report 100% completion at the end
					int nPctComplete = (int)Math.Round((double)(nWritten + nErrors) / uiTotalRecords * 100);
					m_progressUpdateCallback(nPctComplete, nWritten, nErrors);
				}
				else
				{
					Debug.WriteLine("Data source information is not available.");
				}
			}
			catch (System.IO.IOException ex)
			{
				// Handle general I/O errors (e.g., file in use by another process)
				Trace.WriteLine($"A general I/O error occurred: {ex.Message}");
			}
			catch (Exception ex) // Catch any other unexpected exceptions
			{
				Trace.WriteLine($"An unexpected error occurred: {ex.Message}");
			}
			//finally
			//{
			//	if (bSrcOpen)
			//	{
			//		m_idtDataSrc.Close();
			//		bSrcOpen = false;
			//	}
			//}

			//int nTotalRecords = _sourceData.GetRecordCount();
			//int nRecordsWritten = 0;
			//int nRecordsFailed = 0;
			//for (int i = 0; i < nTotalRecords; i++)
			//{
			//	var record = _sourceData.GetRecord(i);
			//	bool success = _targetData.WriteRecord(record);
			//	if (success)
			//		nRecordsWritten++;
			//	else
			//		nRecordsFailed++;
			//	int nPctComplete = (int)((double)(i + 1) / nTotalRecords * 100);
			//	_progressUpdateCallback(nPctComplete, nRecordsWritten, nRecordsFailed);
			//}
		}
	}
}
