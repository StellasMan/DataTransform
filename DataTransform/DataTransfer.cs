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
		private IDTDataSource m_idtDataSrc;
		private IDTDataTarget m_idtDataTarget;
		private CancellationToken m_token;
		private ProgressUpdateDelegate m_progressUpdateCallback;

		public DataTransfer(IDTDataSource sourceData, IDTDataTarget targetData, CancellationToken token, ProgressUpdateDelegate progressUpdateCallback)
		{
			(m_idtDataSrc, m_idtDataTarget, m_token, m_progressUpdateCallback) = (sourceData, targetData, token, progressUpdateCallback);
		}

		public void Execute()
		{
			int nTotalRecords = m_idtDataSrc.GetRecordCount();
			int nWritten = 0;
			int nErrors = 0;

			for (int nIx = 0; nIx < nTotalRecords; nIx+=100)
			{
				if (m_token.IsCancellationRequested)
				{
					Debug.WriteLine("Data transfer cancelled by user.");
					break;
				}
				//var record = m_idtDataSrc.GetRecord(nIx);
				//bool success = m_idtDataTarget.WriteRecord(record);

				System.Threading.Thread.Sleep(50); // Simulate time-consuming operation

				int nPctComplete = (int)((double)(nIx + 1) / nTotalRecords * 100);
				nWritten += 100;
				m_progressUpdateCallback(nPctComplete, Math.Min(nWritten, nTotalRecords), nErrors);
			}

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
