// DataColumn.h: interface for the DataColumn class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_DATACOLUMN_H__391733E3_1E40_48EE_8ACF_BF4BFCE8C015__INCLUDED_)
#define AFX_DATACOLUMN_H__391733E3_1E40_48EE_8ACF_BF4BFCE8C015__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatLibrary
{
	namespace Charting
	{

		class ANIMAT_PORT DataColumn : public CStdSerialize  
		{
		protected:
			float *m_lpDataValue;
			int m_iAppendSpaces;
			string m_strID;
			string m_strColumnName;
			BOOL m_bInitialized;
			int m_iIndex;

			//These two indices are only used for 3D Array type charts. For normal line charts they are simply -1.
			//However, if they are not -1 then that means we want to specify the column and row where we should add the data for this column
			int m_iColumnIndex;
			int m_iRowIndex; 

		public:
			DataColumn();
			virtual ~DataColumn();

			virtual int ColumnCount();

			string ColumnName() {return m_strColumnName;}
			void ColumnName(string strName) {m_strColumnName = strName;}

			string ID() {return m_strID;}
			void ID(string strID) {m_strID = strID;}
			
			int Index() {return m_iIndex;}
			void Index(int iIndex) {m_iIndex = iIndex;}

			virtual string DataType() {return "";}
			virtual void DataType(string strType) {};

			BOOL BeenInitialized() {return m_bInitialized;}
			void BeenInitialized(BOOL bVal) {m_bInitialized = bVal;}

			float *DataValue() {return m_lpDataValue;};

			virtual void Initialize(Simulator *lpSim) = 0;
			virtual void ReInitialize(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim, DataChart *lpChart);
			virtual BOOL operator<(DataColumn *lpColumn);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void SaveColumnNames(ofstream &oStream);
		};

		BOOL LessThanDataColumnCompare(DataColumn *lpColumn1, DataColumn *lpColumn2);

	}			//Charting
}				//AnimatLibrary

#endif // !defined(AFX_DATACOLUMN_H__391733E3_1E40_48EE_8ACF_BF4BFCE8C015__INCLUDED_)
