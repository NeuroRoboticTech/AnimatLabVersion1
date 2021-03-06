// ContactAdapter.cpp: implementation of the ContactAdapter class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "Node.h"
#include "Gain.h"
#include "Adapter.h"
#include "ReceptiveField.h"
#include "ReceptiveFieldPair.h"
#include "ContactAdapter.h"
#include "Joint.h"
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
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
namespace AnimatLibrary
{
	namespace Adapters
	{

/*! \brief 
   Constructs an structure object..
   		
	 \return
	 No return value.

   \remarks
	 The constructor for a structure. 
*/

ContactAdapter::ContactAdapter()
{
}


/*! \brief 
   Destroys the structure object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the structure object..	 
*/

ContactAdapter::~ContactAdapter()
{

try
{
	m_aryFieldPairs.RemoveAll();
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of ContactAdapter\r\n", "", -1, FALSE, TRUE);}
}


void ContactAdapter::Initialize(Simulator *lpSim, Structure *lpStructure)
{
	Organism *lpOrganism = dynamic_cast<Organism *>(lpStructure);
	if(!lpOrganism)
		THROW_TEXT_ERROR(Al_Err_lConvertingClassToType, Al_Err_strConvertingClassToType, "Organism");

	m_lpSourceNode = lpOrganism->FindRigidBody(m_strSourceBodyID);
	m_lpSourceNode->AttachSourceAdapter(lpSim, lpStructure, this);
	lpSim->AttachTargetAdapter(lpStructure, this);

	int iCount = m_aryFieldPairs.GetSize();
	ReceptiveFieldPair *lpPair=NULL;
	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPair = m_aryFieldPairs[iIndex];
		lpPair->Initialize(lpSim, lpOrganism, m_lpSourceNode, m_strTargetModule);
	}
}

string ContactAdapter::SourceModule()
{return "AnimatLab";}

string ContactAdapter::TargetModule()
{return m_strTargetModule;}

//Node Overrides
void ContactAdapter::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	int iCount = m_aryFieldPairs.GetSize();
	for(int iIndex=0; iIndex<iCount; iIndex++)
		m_aryFieldPairs[iIndex]->StepSimulation(lpSim, lpStructure);
}

void ContactAdapter::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	oXml.IntoElem();  //Into Adapter Element
	m_strSourceBodyID = oXml.GetChildString("SourceBodyID");
	if(Std_IsBlank(m_strSourceBodyID)) 
		THROW_ERROR(Al_Err_lBodyIDBlank, Al_Err_strBodyIDBlank);

	m_strTargetModule = oXml.GetChildString("TargetModule");
	if(Std_IsBlank(m_strTargetModule)) 
		THROW_TEXT_ERROR(Al_Err_lModuleNameBlank, Al_Err_strModuleNameBlank, " Target Module");

	m_aryFieldPairs.RemoveAll();

	oXml.FindChildElement("FieldPairs");
	oXml.IntoElem(); //Into FieldPairs Element
	int iCount = oXml.NumberOfChildren();

	for(int iIndex=0; iIndex<iCount; iIndex++)
	{
		oXml.FindChildByIndex(iIndex);
		LoadFieldPair(lpSim, lpStructure, oXml);
	}
	oXml.OutOfElem(); //OutOf FieldPairs Element

	oXml.OutOfElem(); //OutOf Adapter Element
}

ReceptiveFieldPair *ContactAdapter::LoadFieldPair(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	ReceptiveFieldPair *lpPair = NULL;

try
{
	lpPair = new ReceptiveFieldPair();
	lpPair->Load(lpSim, lpStructure, oXml);
	m_aryFieldPairs.Add(lpPair);

	return lpPair;
}
catch(CStdErrorInfo oError)
{
	if(lpPair) delete lpPair;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpPair) delete lpPair;
	THROW_ERROR(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError);
	return NULL;
}
}

	}			//Adapters
}			//AnimatLibrary
