// NlClassFactory.cpp: implementation of the ClassFactory class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "Synapse.h"
#include "CompoundSynapse.h"
#include "GatedSynapse.h"
#include "ModulatedSynapse.h"
#include "Neuron.h"
#include "PacemakerNeuron.h"
#include "TonicNeuron.h"
#include "RandomNeuron.h"
#include "BistableNeuron.h"
#include "FastNeuralModule.h"
#include "NeuronData.h"
#include "CurrentInjection.h"
#include "ClassFactory.h"

namespace FastNeuralNet
{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

ClassFactory::ClassFactory()
{

}

ClassFactory::~ClassFactory()
{

}

// ************* Neuron Type Conversion functions ******************************

Neuron *ClassFactory::CreateNeuron(string strType, BOOL bThrowError)
{
	Neuron *lpNeuron=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "REGULAR")
		lpNeuron = new Neuron;
	else if(strType == "PACEMAKER")
		lpNeuron = new PacemakerNeuron;
	else if(strType == "TONIC")
		lpNeuron = new TonicNeuron;
	else if(strType == "RANDOM")
		lpNeuron = new RandomNeuron;
	else if(strType == "BISTABLE")
		lpNeuron = new BistableNeuron;
	else 
	{
		lpNeuron = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Nl_Err_lInvalidNeuronType, Nl_Err_strInvalidNeuronType, "NeuronType", strType);
	}

	return lpNeuron;
}
catch(CStdErrorInfo oError)
{
	if(lpNeuron) delete lpNeuron;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpNeuron) delete lpNeuron;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}


// ************* Neuron Type Conversion functions ******************************


// ************* Synapse Type Conversion functions ******************************

Synapse *ClassFactory::CreateSynapse(string strType, BOOL bThrowError)
{
	Synapse *lpSynapse=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "REGULAR")
		lpSynapse = new Synapse;
	else if(strType == "COMPOUND")
		lpSynapse = new CompoundSynapse;
	else if(strType == "GATED")
		lpSynapse = new GatedSynapse;
	else if(strType == "MODULATED")
		lpSynapse = new ModulatedSynapse;
	else
	{
		lpSynapse = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Nl_Err_lInvalidSynapseType, Nl_Err_strInvalidSynapseType, "SynapseType", strType);
	}

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

// ************* Synapse Type Conversion functions ******************************


// ************* DataColumn Type Conversion functions ******************************

DataColumn *ClassFactory::CreateDataColumn(string strType, BOOL bThrowError)
{
	DataColumn *lpColumn=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "NEURONDATA")
		lpColumn = new NeuronData;
	else
	{
		lpColumn = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidDataColumnType, Al_Err_strInvalidDataColumnType, "DataColumnType", strType);
	}

	return lpColumn;
}
catch(CStdErrorInfo oError)
{
	if(lpColumn) delete lpColumn;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpColumn) delete lpColumn;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* DataColumn Type Conversion functions ******************************


// ************* External Stimulus Type Conversion functions ******************************

ExternalStimulus *ClassFactory::CreateExternalStimulus(string strType, BOOL bThrowError)
{
	ExternalStimulus *lpStimulus=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "CURRENTINJECTION")
		lpStimulus = new CurrentInjection;
	else if(strType == "CURRENT")
		lpStimulus = new AnimatLibrary::ExternalStimuli::CurrentStimulus;
	else if(strType == "VOLTAGECLAMP")
		lpStimulus = new AnimatLibrary::ExternalStimuli::VoltageClamp;
	else
	{
		lpStimulus = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Nl_Err_lInvalidExternalStimulusType, Nl_Err_strInvalidExternalStimulusType, "ExternalStimulusType", strType);
	}

	return lpStimulus;
}
catch(CStdErrorInfo oError)
{
	if(lpStimulus) delete lpStimulus;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpStimulus) delete lpStimulus;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* External Stimulus Type Conversion functions ******************************


// ************* External Neural Module Conversion functions ******************************

NeuralModule *ClassFactory::CreateNeuralModule(string strType, BOOL bThrowError)
{
	NeuralModule *lpModule=NULL;

try
{
	strType = Std_ToUpper(Std_Trim(strType));

	if(strType == "FASTNEURALMODULE")
		lpModule = new FastNeuralModule;
	else
	{
		lpModule = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Al_Err_lInvalidNeuralModuleType, Al_Err_strInvalidNeuralModuleType, "NeuralModule", strType);
	}

	return lpModule;
}
catch(CStdErrorInfo oError)
{
	if(lpModule) delete lpModule;
	RELAY_ERROR(oError); 
	return NULL;
}
catch(...)
{
	if(lpModule) delete lpModule;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

// ************* Neural Module Type Conversion functions ******************************


// ************* IStdClassFactory functions ******************************

CStdSerialize *ClassFactory::CreateObject(string strClassType, string strObjectType, BOOL bThrowError)
{
	CStdSerialize *lpObject=NULL;

	strClassType = Std_ToUpper(Std_Trim(strClassType));

	if(strClassType == "DATACOLUMN")
		lpObject = CreateDataColumn(strObjectType, bThrowError);
	else if(strClassType == "EXTERNALSTIMULUS")
		lpObject = CreateExternalStimulus(strObjectType, bThrowError);
	else if(strClassType == "NEURON")
		lpObject = CreateNeuron(strObjectType, bThrowError);
	else if(strClassType == "SYNAPSE")
		lpObject = CreateSynapse(strObjectType, bThrowError);
	else if(strClassType == "NEURALMODULE")
		lpObject = CreateNeuralModule(strObjectType, bThrowError);
	else
	{
		lpObject = NULL;
		if(bThrowError)
			THROW_PARAM_ERROR(Std_Err_lInvalidClassType, Std_Err_strInvalidClassType, "ClassType", strClassType);
	}

	return lpObject;
}

// ************* IStdClassFactory functions ******************************

}				//FastNeuralNet
