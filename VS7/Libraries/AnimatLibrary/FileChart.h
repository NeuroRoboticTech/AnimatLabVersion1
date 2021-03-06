// FileChart.h: interface for the FileChart class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_FILECHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_)
#define AFX_FILECHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatLibrary
{
	namespace Charting
	{

		class ANIMAT_PORT FileChart : public DataChart  
		{
		protected:
			string m_strOutputFilename;
			ofstream oStream;

		public:
			FileChart();
			virtual ~FileChart();

			string OutputFilename() {return m_strOutputFilename;};
			void OutputFilename(string strVal) {m_strOutputFilename = strVal;};

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void SaveOutput(Simulator *lpSim);

			//ActiveItem overrides
			virtual string Type() {return "FileChart";};

			virtual void Initialize(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);
		};

	}			//Charting
}				//AnimatLibrary

#endif // !defined(AFX_FILECHART_H__D91DC66E_01F1_47FC_AB62_766BA63FCEF0__INCLUDED_)
