// EnablerStimulus.cpp: implementation of the EnablerStimulus class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "Node.h"
#include "Gain.h"
#include "Adapter.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "IODataEntry.h"
#include "Microcontroller.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "ExternalStimulus.h"
#include "ExternalInputStimulus.h"
#include "EnablerStimulus.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatLibrary
{
	namespace ExternalStimuli
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

EnablerStimulus::EnablerStimulus()
{
	m_bEnableWhenActive = TRUE;
}

EnablerStimulus::~EnablerStimulus()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of EnablerStimulus\r\n", "", -1, FALSE, TRUE);}
}

void EnablerStimulus::Initialize(Simulator *lpSim)
{
	if(!lpSim)
		THROW_ERROR(Al_Err_lSimulationNotDefined, Al_Err_strSimulationNotDefined);

	ExternalStimulus::Initialize(lpSim);

	//Lets try and get the node we will dealing with.
	m_lpStructure = lpSim->FindStructureFromAll(m_strStructureID);

	m_lpNode = m_lpStructure->FindNode(m_strBodyID);
}

void EnablerStimulus::Activate(Simulator *lpSim)
{
	if(m_bEnableWhenActive)
		m_lpNode->Enabled(TRUE);
	else
		m_lpNode->Enabled(FALSE);
}

void EnablerStimulus::StepSimulation(Simulator *lpSim)
{
}

void EnablerStimulus::Deactivate(Simulator *lpSim)
{
	if(m_bEnableWhenActive)
		m_lpNode->Enabled(FALSE);
	else
		m_lpNode->Enabled(TRUE);
}

void EnablerStimulus::Load(Simulator *lpSim, CStdXml &oXml)
{
	ActivatedItem::Load(lpSim, oXml);

	oXml.IntoElem();  //Into Simulus Element

	m_strStructureID = oXml.GetChildString("StructureID");
	if(Std_IsBlank(m_strStructureID)) 
		THROW_ERROR(Al_Err_lIDBlank, Al_Err_strIDBlank);

	m_strBodyID = oXml.GetChildString("BodyID");
	if(Std_IsBlank(m_strBodyID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);

	m_bEnableWhenActive = oXml.GetChildBool("EnableWhenActive", m_bEnableWhenActive);

	oXml.OutOfElem(); //OutOf Simulus Element

}

void EnablerStimulus::Save(Simulator *lpSim, CStdXml &oXml)
{
}

void EnablerStimulus::Trace(ostream &oOs)
{
	oOs << "EnablerStimulus"  << ", Name: " << m_strName << " Time (" << m_fltStartTime << ", " << m_fltEndTime << ") Slice: (" << m_lStartSlice << ", " << m_lEndSlice << ")";
}

	}			//ExternalStimuli
}				//VortexAnimatLibrary




