// EnablerStimulus.h: interface for the EnablerStimulus class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ENABLER_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_ENABLER_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatLibrary
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT EnablerStimulus  : public ExternalStimulus
		{
		protected:
			Structure *m_lpStructure;
			string m_strStructureID;

			Node *m_lpNode;

			string m_strBodyID;
			BOOL m_bEnableWhenActive;

		public:
			EnablerStimulus();
			virtual ~EnablerStimulus();

			virtual void Activate(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, CStdXml &oXml);
			virtual void Trace(ostream &oOs);

			//ActiveItem overrides
			virtual string Type() {return "EnablerInput";};
			virtual void Initialize(Simulator *lpSim);
		};

	}			//ExternalStimuli
}				//VortexAnimatLibrary

#endif // !defined(AFX_ENABLER_STIMULUS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
