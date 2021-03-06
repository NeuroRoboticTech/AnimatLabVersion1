// RNeuron.cpp: implementation of the Neuron class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "Neuron.h"
#include "FastNeuralModule.h"

namespace FastNeuralNet
{
	namespace Neurons
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

Neuron::Neuron()
{
	m_bEnabled = TRUE;

	m_aryVn[0]=0.0;
	m_aryVn[1]=0.0;
	m_aryVth[0] = 0;
	m_aryVth[1] = 0;

	m_fltCn = (float) 75e-6;	//Membrane capacitance
	m_fltGn = (float) 0.1e-6;	//Membrane conductance
	m_fltVth = (float) -3e-3;	//Firing frequency voltage threshold
	m_fltVthi = m_fltVth;
	m_fltFmin = (float) 0.25;	//Minimum Firing frequency
	m_fltGain = (float) 0.1e-3;	//Firing frequency gain
	m_fltExternalI = 0;			//Externally injected current
	m_fltIntrinsicI = 0;
	m_fltSynapticI = 0;
	m_fltAdapterI = 0;
	m_fltAdapterMemoryI = 0;
	m_fltTotalMemoryI = 0;
	m_fltVn = 0;
	m_fltFiringFreq = 0;
	m_fltVndisp = 0;
	m_fltVthdisp = 0;
	m_fltVrest = 0;

	m_fltVNoiseMax = (float) 0.1e-4; //Max noise is 0.4 mV
	m_bUseNoise = TRUE;
	m_fltVNoise = 0;

	m_fltDCTH = 0;
	m_fltAccomTimeConst = (float) 100e-3;
	m_fltRelativeAccom = 0;
	m_bUseAccom = FALSE;

	m_bGainType = TRUE;
}

Neuron::~Neuron()
{

try
{
	m_arySynapses.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of Neuron\r\n", "", -1, FALSE, TRUE);}
}

float Neuron::Cn()
{return m_fltCn;}

void Neuron::Cn(float fltVal)
{m_fltCn=fltVal;}

float Neuron::Gn()
{return m_fltGn;}

void Neuron::Gn(float fltVal)
{m_fltGn=fltVal;}

float Neuron::Vth()
{return m_fltVth;}

void Neuron::Vth(float fltVal)
{m_fltVth=fltVal;}

float Neuron::Fmin()
{return m_fltFmin;}

void Neuron::Fmin(float fltVal)
{m_fltFmin=fltVal;}

float Neuron::Gain()
{return m_fltGain;}

void Neuron::Gain(float fltVal)
{
	if(fltVal==0) THROW_ERROR(Nl_Err_lInvalidGain, Nl_Err_strInvalidGain);
	m_fltGain=fltVal;
}

float Neuron::ExternalI()
{return m_fltExternalI;}

void Neuron::ExternalI(float fltVal)
{m_fltExternalI=fltVal;}

float Neuron::Vn()
{return m_fltVn;}

float Neuron::FiringFreq(FastNeuralModule *lpModule)
{
	return CalculateFiringFrequency(m_aryVn[lpModule->ActiveArray()], m_aryVth[lpModule->ActiveArray()]);
}

float Neuron::IntrinsicCurrent()
{return m_fltIntrinsicI;}

void Neuron::IntrinsicCurrent(float fltVal)
{m_fltIntrinsicI = fltVal;}

unsigned char Neuron::NeuronType()
{return RUGULAR_NEURON;}

CStdPtrArray<Synapse> *Neuron::GetSynapses()
{return &m_arySynapses;}

void Neuron::AddSynapse(Synapse *lpSynapse)
{
	if(!lpSynapse) 
		THROW_ERROR(Nl_Err_lSynapseToAddNull, Nl_Err_strSynapseToAddNull);
	m_arySynapses.Add(lpSynapse);
}

void Neuron::RemoveSynapse(int iIndex)
{
	if( iIndex<0 || iIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	m_arySynapses.RemoveAt(iIndex);
}

Synapse *Neuron::GetSynapse(int iIndex)
{
	if( iIndex<0 || iIndex>=m_arySynapses.GetSize() ) 
		THROW_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex);
	return m_arySynapses[iIndex];
}


int Neuron::TotalSynapses()
{return m_arySynapses.GetSize();}

void Neuron::ClearSynapses()
{m_arySynapses.RemoveAll();}


void Neuron::StepSimulation(Simulator *lpSim, Organism *lpOrganism, FastNeuralModule *lpModule, unsigned char iXPos, unsigned char iYPos, unsigned char iZPos)
{
	//float fltTime = lpSim->Time();
	//if( m_strName == "LU Leg Forward" ) //&& fltTime > 0.5
	//	iXPos = iXPos;

	if(m_bEnabled)
	{
		//Lets get the Summation of synaptic inputs
		m_fltSynapticI = CalculateSynapticCurrent(lpModule);
		m_fltIntrinsicI = CalculateIntrinsicCurrent(lpModule, m_fltExternalI+m_fltSynapticI);

		if(m_bUseNoise)
			m_fltVNoise = Std_FRand(-m_fltVNoiseMax, m_fltVNoiseMax);
		
		//Get the total current being applied to the neuron.
		m_fltTotalMemoryI = m_fltSynapticI + m_fltIntrinsicI + m_fltExternalI + m_fltAdapterI;

		m_aryVn[lpModule->InactiveArray()] = m_aryVn[lpModule->ActiveArray()] + m_fltVNoise + 
							(lpModule->TimeStep() * (1/m_fltCn) * 
							(m_fltSynapticI + m_fltIntrinsicI + m_fltExternalI + m_fltAdapterI - 
							(m_aryVn[lpModule->ActiveArray()]*m_fltGn)));

		m_fltVn = m_aryVn[lpModule->InactiveArray()];
		m_fltVndisp = m_fltVrest + m_fltVn;

		if(m_bUseAccom)
			m_aryVth[lpModule->InactiveArray()] = m_fltVthi + (m_aryVth[lpModule->ActiveArray()]-m_fltVthi)*m_fltDCTH + m_fltRelativeAccom*m_fltVn*(1-m_fltDCTH);
		else
			m_aryVth[lpModule->InactiveArray()] = m_fltVthi;

		m_fltVth = m_aryVth[lpModule->InactiveArray()];
		m_fltVthdisp = m_fltVrest + m_fltVth;

		m_fltFiringFreq = CalculateFiringFrequency(m_fltVn, m_fltVth);
		m_fltAdapterI = 0;  //Reset the adapter current for the next cycle.
	}
}


float Neuron::CalculateFiringFrequency(float fltVn, float fltVth)
{
	float fltFreq;
	
	if(m_bGainType)
	{
		if(fltVn<fltVth)
			fltFreq=0;
		else
			fltFreq = (m_fltGain*(fltVn-fltVth)) + m_fltFmin;

		//Final insurance
		if(fltFreq<1e-6) fltFreq = 0;
		if(fltFreq>1) fltFreq = 1;
	}
	else
	{
		if(fltVn<fltVth)
			fltFreq = 0;
		else
		{
			fltFreq = m_fltFmin - m_fltGain * fltVth;
			if (fltVn < (1 - fltFreq)/m_fltGain)
				fltFreq = (m_fltGain * fltVn) + fltFreq;
			else
				fltFreq = 1;
		}

		//Final insurance
		if(fltFreq<1e-6) fltFreq = 0;
		if(fltFreq>1) fltFreq = 1;
	}

	return fltFreq;
}


float Neuron::CalculateIntrinsicCurrent(FastNeuralModule *lpModule, float fltInputCurrent)
{return 0;}

float Neuron::CalculateSynapticCurrent(FastNeuralModule *lpModule)
{
	unsigned char iSynapse, iCount;
	float fltSynapticI=0;
	Synapse *lpSynapse=NULL;
	Neuron *lpNeuron=NULL;

	iCount = m_arySynapses.GetSize();
	for(iSynapse=0; iSynapse<iCount; iSynapse++)
	{
		lpSynapse = m_arySynapses[iSynapse];

		if(lpSynapse->Enabled())
		{
			lpNeuron = lpModule->GetNeuron(lpSynapse->FromX(), lpSynapse->FromY(), lpSynapse->FromZ()); 
			if(lpNeuron) 
				fltSynapticI+= (lpNeuron->FiringFreq(lpModule) * lpSynapse->Weight() * lpSynapse->CalculateModulation(lpModule) ); 
		}
	}

	return fltSynapticI;
}

void Neuron::InjectCurrent(float fltVal)
{m_fltExternalI+=fltVal;}

void Neuron::Initialize(Simulator *lpSim, Organism *lpOrganism, FastNeuralModule *lpModule)
{
	if(m_bUseAccom)
		m_fltDCTH = exp(-lpModule->TimeStep()/m_fltAccomTimeConst);
} 

void Neuron::AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput)
{
	m_fltAdapterI += fltInput;
	m_fltAdapterMemoryI = m_fltAdapterI;
}

float *Neuron::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);

	if(strType == "INTRINSICCURRENT")
		return &m_fltIntrinsicI;

	if(strType == "EXTERNALCURRENT")
		return &m_fltExternalI;

	if(strType == "SYNAPTICCURRENT")
		return &m_fltSynapticI;

	if(strType == "ADAPTERCURRENT")
		return &m_fltAdapterMemoryI;

	if(strType == "TOTALCURRENT")
		return &m_fltTotalMemoryI;

	if(strType == "MEMBRANEVOLTAGE")
		return &m_fltVndisp;

	if(strType == "FIRINGFREQUENCY")
		return &m_fltFiringFreq;

	if(strType == "NOISEVOLTAGE")
		return &m_fltVNoise;

	if(strType == "THRESHOLD")
		return &m_fltVthdisp;

	if(strType == "Gm")
		return &m_fltGn;

	if(strType == "VREST")
		return &m_fltVrest;

	//If it was not one of those above then we have a problem.
	THROW_PARAM_ERROR(Nl_Err_lInvalidNeuronDataType, Nl_Err_strInvalidNeuronDataType, "Neuron Data Type", strDataType);

	return NULL;
}


float *Neuron::GetSynapseDataPointer(short iSynapseIndex, short iCompoundIndex, short iDataType)
{
	Synapse *lpSynapse=NULL;
	int iCount;

	iCount = m_arySynapses.GetSize();
	if(iSynapseIndex<0 || iSynapseIndex>=iCount) 
		THROW_PARAM_ERROR(Nl_Err_lInvalidSynapseIndex, Nl_Err_strInvalidSynapseIndex, "SynapseIndex", (long) iSynapseIndex);

	lpSynapse = m_arySynapses[iSynapseIndex];
	return lpSynapse->GetDataPointer(iCompoundIndex, iDataType);
}

long Neuron::CalculateSnapshotByteSize()
{
	//We need bytes for the internal state variables for this neuron.
	return (sizeof(m_aryVn) + sizeof(m_fltExternalI) + sizeof(m_fltIntrinsicI) + sizeof(m_fltSynapticI) + sizeof(m_fltVn) + sizeof(m_fltFiringFreq));
}

void Neuron::SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	memcpy((void *) (aryBytes+lIndex), (void *)m_aryVn, sizeof(m_aryVn));
  lIndex += sizeof(m_aryVn);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltExternalI, sizeof(m_fltExternalI));
  lIndex += sizeof(m_fltExternalI);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltIntrinsicI, sizeof(m_fltIntrinsicI));
  lIndex += sizeof(m_fltIntrinsicI);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltSynapticI, sizeof(m_fltSynapticI));
  lIndex += sizeof(m_fltSynapticI);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltVn, sizeof(m_fltVn));
  lIndex += sizeof(m_fltVn);

	memcpy((void *) (aryBytes+lIndex), (void *)&m_fltFiringFreq, sizeof(m_fltFiringFreq));
  lIndex += sizeof(m_fltFiringFreq);
}

void Neuron::LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex)
{
	memcpy((void *)m_aryVn, (void *) (aryBytes+lIndex) , sizeof(m_aryVn));
  lIndex += sizeof(m_aryVn);

	memcpy((void *)&m_fltExternalI, (void *) (aryBytes+lIndex), sizeof(m_fltExternalI));
  lIndex += sizeof(m_fltExternalI);

	memcpy((void *)&m_fltIntrinsicI, (void *) (aryBytes+lIndex), sizeof(m_fltIntrinsicI));
  lIndex += sizeof(m_fltIntrinsicI);

	memcpy((void *)&m_fltSynapticI, (void *) (aryBytes+lIndex), sizeof(m_fltSynapticI));
  lIndex += sizeof(m_fltSynapticI);

	memcpy((void *)&m_fltVn, (void *) (aryBytes+lIndex), sizeof(m_fltVn));
  lIndex += sizeof(m_fltVn);

	memcpy((void *)&m_fltFiringFreq, (void *) (aryBytes+lIndex), sizeof(m_fltFiringFreq));
  lIndex += sizeof(m_fltFiringFreq);
}

void Neuron::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	int iCount, iIndex;

	oXml.IntoElem();  //Into Neuron Element

	m_arySynapses.RemoveAll();

	m_strName = oXml.GetChildString("Name", "");
	m_bEnabled = oXml.GetChildBool("Enabled", TRUE);

	m_fltCn = oXml.GetChildFloat("Cn");
	m_fltGn = oXml.GetChildFloat("Gn");
	m_fltVrest = oXml.GetChildFloat("Vrest", 0);
	m_fltVth = oXml.GetChildFloat("Vth");
	m_fltVthi = m_fltVth;
	m_fltFmin = oXml.GetChildFloat("Fmin");
	m_fltGain = oXml.GetChildFloat("Gain");
	m_fltExternalI = oXml.GetChildFloat("ExternalI");
	m_fltVNoiseMax = fabs(oXml.GetChildFloat("VNoiseMax", m_fltVNoiseMax));

	m_fltVndisp = m_fltVrest;
	m_fltVthdisp = m_fltVrest + m_fltVth;

	m_bGainType = oXml.GetChildBool("GainType", TRUE);

	m_aryVth[0] = m_aryVth[1] = m_fltVth;

	if(m_fltVNoiseMax != 0)
		m_bUseNoise = TRUE;
	else
		m_bUseNoise = FALSE;

	m_fltRelativeAccom = fabs(oXml.GetChildFloat("RelativeAccom", m_fltRelativeAccom));
	m_fltAccomTimeConst = fabs(oXml.GetChildFloat("AccomTimeConst", m_fltAccomTimeConst));

	if(m_fltRelativeAccom != 0)
		m_bUseAccom = TRUE;
	else
		m_bUseAccom = FALSE;

	//*** Begin Loading Synapses. *****
	if(oXml.FindChildElement("Synapses", FALSE))
	{
		oXml.IntoElem();  //Into Synapses Element

		iCount = oXml.NumberOfChildren();
		for(iIndex=0; iIndex<iCount; iIndex++)
		{
			oXml.FindChildByIndex(iIndex);
			LoadSynapse(lpSim, lpStructure, oXml);
		}

		oXml.OutOfElem();
	}
	//*** End Loading Synapses. *****


	oXml.OutOfElem(); //OutOf Neuron Element
}


Synapse *Neuron::LoadSynapse(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	string strType;
	Synapse *lpSynapse=NULL;

try
{
	oXml.IntoElem();  //Into Synapse Element
	strType = oXml.GetChildString("Type");
	oXml.OutOfElem(); //OutOf Synapse Element

	lpSynapse = dynamic_cast<Synapse *>(lpSim->CreateObject(Nl_NeuralModuleName(), "Synapse", strType));
	if(!lpSynapse)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Synapse");

	lpSynapse->Load(lpSim, lpStructure, this, oXml);
	AddSynapse(lpSynapse);
	return lpSynapse;
}
catch(CStdErrorInfo oError)
{
	if(lpSynapse) delete lpSynapse;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpSynapse) delete lpSynapse;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


void Neuron::Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	int iCount, iIndex;

	//Do NOT create and go into the nueron element here. We had to create
	//the neuron element in the brain object in order to save the x,y,z params.


	oXml.AddChildElement("NeuronType", NeuronType());
	oXml.AddChildElement("Cn", m_fltCn);
	oXml.AddChildElement("Gn", m_fltGn);
	oXml.AddChildElement("Vth", m_fltVth);
	oXml.AddChildElement("Fmin", m_fltFmin);
	oXml.AddChildElement("Gain", m_fltGain);
	oXml.AddChildElement("ExternalI", m_fltExternalI);

	//*** Begin Saving Synapses. *****
	oXml.AddChildElement("Synapses");
	oXml.IntoChildElement("Synapses");

	iCount = m_arySynapses.GetSize();
	for(iIndex=0; iIndex<iCount; iIndex++)
		m_arySynapses[iIndex]->Save(lpSim, lpStructure, this, oXml);

	oXml.OutOfElem();
	//*** End Saving Synapses. *****


	//Do NOT exit out of the neuron element here.
}


	}			//Neurons
}				//FastNeuralNet



