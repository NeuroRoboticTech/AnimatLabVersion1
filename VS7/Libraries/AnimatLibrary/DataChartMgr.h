// DataChartMgr.h: interface for the DataChartMgr class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_DATACHARTMGR_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_)
#define AFX_DATACHARTMGR_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatLibrary
{
	namespace Charting
	{

		class ANIMAT_PORT DataChartMgr : public ActivatedItemMgr  
		{
		protected:

			DataChart *LoadDataChart(Simulator *lpSim, CStdXml &oXml);

		public:
			DataChartMgr();
			virtual ~DataChartMgr();

			virtual void AddDataColumn(string strChartKey, DataColumn *lpColumn);
			virtual void RemoveDataColumn(string strChartKey, string strColumnName, BOOL bThrowError = TRUE);
			virtual DataColumn *FindDataColumn(string strChartKey, string strColumnName, BOOL bThrowError = TRUE);
			virtual void ModifyDataColumn(string strChartKey, string strColumnName, string strDataType);
			virtual void SetDataColumnIndex(string strChartKey, string strColumnName, int iIndex);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
		};

	}			//Charting
}				//AnimatLibrary

#endif // !defined(AFX_DATACHARTMGR_H__FE7FF2D1_7842_4EB2_943D_DEF45D430958__INCLUDED_)
