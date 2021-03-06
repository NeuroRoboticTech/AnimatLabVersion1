// VoltageClamp.h: interface for the VoltageClamp class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VOLTAGE_CLAMP_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
#define AFX_VOLTAGE_CLAMP_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatLibrary
{
	namespace ExternalStimuli
	{

		class ANIMAT_PORT VoltageClamp  : public ExternalStimulus
		{
		protected:
			Organism *m_lpOrganism;
			string m_strOrganismID;

			string m_strNeuralModule;
			long m_lTargetNodeID;

			Node *m_lpNode;
			float *m_lpExternalCurrent;
			float *m_lpTotalCurrent;
			float *m_lpVrest;
			float *m_lpGm;

			float m_fltVtarget;
			float m_fltTargetCurrent;
			float m_fltActiveCurrent;

		public:
			VoltageClamp();
			virtual ~VoltageClamp();

			virtual void Load(Simulator *lpSim, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, CStdXml &oXml);
			virtual void Trace(ostream &oOs);

			//ActiveItem overrides
			virtual string Type() {return "VoltageClamp";};
			virtual void Initialize(Simulator *lpSim);  
			virtual void Activate(Simulator *lpSim);
			virtual void StepSimulation(Simulator *lpSim);
			virtual void Deactivate(Simulator *lpSim);
			virtual float *GetDataPointer(string strDataType);

			virtual void Modify(Simulator *lpSim, ActivatedItem *lpItem);
		};

	}			//ExternalStimuli
}				//VortexAnimatLibrary

#endif // !defined(AFX_VOLTAGE_CLAMPS_H__AEBF2DF9_E7A0_4ED2_83CD_BE74B7D74E59__INCLUDED_)
