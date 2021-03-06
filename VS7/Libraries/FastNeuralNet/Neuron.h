// Neuron.h: interface for the Neuron class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_RCNEURON_H__85C3FD55_D527_409C_ABA5_B3B18D225846__INCLUDED_)
#define AFX_RCNEURON_H__85C3FD55_D527_409C_ABA5_B3B18D225846__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace FastNeuralNet
{
	namespace Neurons
	{

		class FAST_NET_PORT Neuron : public AnimatLibrary::Node   
		{
		protected:

			float m_fltCn;			//Membrane capacitance
			float m_fltGn;			//Membrane conductance
			float m_fltFmin;		//Minimum Firing frequency
			float m_fltGain;		//Firing frequency gain
			float m_fltExternalI;	//Externally injected current
			float m_fltIntrinsicI; //Intrinsic current.
			float m_fltSynapticI;	//Current synaptic current.
			float m_fltAdapterI; //current added from all of the adapters.
			float m_fltAdapterMemoryI;  //Used to allow datacharts to track current input from adapters.
			float m_fltVNoiseMax; //Tells the maximum noise to use when running sim
			BOOL m_bUseNoise;    //Tells if we should use noise or not.
			float m_fltTotalMemoryI; //Total current applied to the neuron

			BOOL m_bGainType; 

			float m_fltDCTH;      // expon decline working factor for thresh accomm
			float m_fltAccomTimeConst;
			float m_fltRelativeAccom;
			BOOL m_bUseAccom;

			float m_fltVn;      //Current membrane voltage.
			float m_fltFiringFreq;  //Current firing frequency.
			float m_aryVn[2];		//Current and next Membrane voltage. Vn
			float m_fltVNoise;	//This is the random noise that should be added to the membrane voltage at a timestep

			float m_fltVth;			//Firing frequency voltage threshold
			float m_fltVthi;    //Initial firing frequency voltage threshold
			float m_aryVth[2];		//Current and next threshold voltage. Vth


			float m_fltVrest;      // this is the resting potential of the neuron.
			float m_fltVndisp;      // this is the membrane voltage that is reported back to animatlab.
			float m_fltVthdisp;		// this is the theshold voltage that is reported back to animatlab.

			CStdPtrArray<Synapse> m_arySynapses;

			virtual float CalculateFiringFrequency(float fltVn, float fltVth);
			virtual float CalculateSynapticCurrent(FastNeuralModule *lpModule);
			virtual float CalculateIntrinsicCurrent(FastNeuralModule *lpModule, float fltInputCurrent);

			Synapse *LoadSynapse(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);

		public:
			Neuron();
			virtual ~Neuron();

			float Cn();
			void Cn(float fltVal);

			float Gn();
			void Gn(float fltVal);

			float Vth();
			void Vth(float fltVal);

			float Fmin();
			void Fmin(float fltVal);

			float Gain();
			void Gain(float fltVal);

			float ExternalI();
			void ExternalI(float fltVal);

			float IntrinsicCurrent();
			void IntrinsicCurrent(float fltVal);

			float Vn();
			float FiringFreq(FastNeuralModule *lpModule);

			virtual unsigned char NeuronType();

			virtual CStdPtrArray<Synapse> *GetSynapses();
			virtual void AddSynapse(Synapse *lpSynapse);
			virtual void RemoveSynapse(int iIndex);
			virtual Synapse *GetSynapse(int iIndex);
			virtual int TotalSynapses();
			virtual void ClearSynapses();

			virtual void Initialize(Simulator *lpSim, Organism *lpOrganism, FastNeuralModule *lpModule);
			virtual void StepSimulation(Simulator *lpSim, Organism *lpOrganism, FastNeuralModule *lpModule, unsigned char iXPos, unsigned char iYPos, unsigned char iZPos);

			virtual void InjectCurrent(float fltVal);

			virtual float *GetSynapseDataPointer(short iSynapseIndex, short iCompoundIndex, short iDataType);

			virtual long CalculateSnapshotByteSize();
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

			//Node Overrides
			virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
			virtual float *GetDataPointer(string strDataType);

			//This is not used. The one above is used because we have to pass in the neuron indexes
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure) {};
			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			//Neurons
}				//FastNeuralNet

#endif // !defined(AFX_RCNEURON_H__85C3FD55_D527_409C_ABA5_B3B18D225846__INCLUDED_)
