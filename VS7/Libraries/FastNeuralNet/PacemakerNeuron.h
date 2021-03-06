// PacemakerNeuron.h: interface for the PacemakerNeuron class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_PACEMAKERNEURON_H__7517B0B1_A6A9_433F_B7D0_759913935693__INCLUDED_)
#define AFX_PACEMAKERNEURON_H__7517B0B1_A6A9_433F_B7D0_759913935693__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace FastNeuralNet
{
	namespace Neurons
	{ 

		class FAST_NET_PORT PacemakerNeuron  : public Neuron
		{
		protected:
			float m_fltIl;
			float m_fltIh;
			float m_fltVssm;
			float m_fltMtl;
			float m_fltBtl;
			float m_fltTh;
			float m_fltITime;
			float m_fltInterburstInterval;
			float m_fltVss;
			unsigned char m_iIntrinsicType;

			void HighCurrentOn();
			void LowCurrentOn(float fltVss);

			virtual float CalculateIntrinsicCurrent(FastNeuralModule *lpModule, float fltInputCurrent);

		public:
			PacemakerNeuron();
			virtual ~PacemakerNeuron();

			float Il();
			void Il(float fltVal);

			float Ih();
			void Ih(float fltVal);

			float Vssm();
			void Vssm(float fltVal);

			float Mtl();
			void Mtl(float fltVal);

			float Btl();
			void Btl(float fltVal);

			float Th();
			void Th(float fltVal);

			float ITime();
			void ITime(float fltVal);

			unsigned char IntrinsicType();
			void IntrinsicType(unsigned char iVal);

			virtual unsigned char NeuronType();

			virtual void StepSimulation(Simulator *lpSim, Organism *lpOrganism, FastNeuralModule *lpModule, unsigned char iXPos, unsigned char iYPos, unsigned char iZPos);
			virtual float *GetDataPointer(string strDataType);

			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			//Neurons
}				//FastNeuralNet

#endif // !defined(AFX_PACEMAKERNEURON_H__7517B0B1_A6A9_433F_B7D0_759913935693__INCLUDED_)
