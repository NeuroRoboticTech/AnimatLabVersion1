// Hinge.cpp: implementation of the Hinge class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "Node.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "Hinge.h"
#include "IODataEntry.h"
#include "Microcontroller.h"
#include "Structure.h"
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

namespace AnimatLibrary
{
	namespace Environment
	{
		namespace Joints
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

/*! \brief 
   Constructs a hinge joint.
   		
   \param lpParent This is a pointer to the parent rigid body of this joint. 
   \param lpChild This is a pointer to the child rigid body of this joint. 

	 \return
	 No return value.

   \remarks
	 The constructor for a hinge joint. 
*/

Hinge::Hinge()
{
	m_fltConstraintLow = (float) (0.25*PI);
	m_fltConstraintHigh = (float) (-0.25*PI);
	m_fltMaxTorque = 1000;
	m_bServoMotor = FALSE;
	m_ftlServoGain = 100;
	m_fltTorque = 0;
	m_fltRotation = 0;
	m_fltVelocity = 0;
}


/*! \brief 
   Destroys the hinge joint object..
   		
	 \return
	 No return value.

   \remarks
   Destroys the hinge joint object..	 
*/

Hinge::~Hinge()
{

}

void Hinge::Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml)
{
	Joint::Load(lpSim, lpStructure, oXml);

	oXml.IntoElem();  //Into Joint Element
	Std_LoadPoint(oXml, "RotationAxis", m_oRotationAxis);

	oXml.IntoChildElement("Constraint");  //Into Constraint Element
	m_fltConstraintLow = oXml.GetAttribFloat("Low");
	m_fltConstraintHigh = oXml.GetAttribFloat("High");
	oXml.OutOfElem(); //OutOf Constraint Element

	if(oXml.FindChildElement("MaxTorque", false))
		m_fltMaxTorque = AnimatLibrary::LoadScaledNumber(oXml, "MaxTorque", FALSE, m_fltMaxTorque) * lpSim->InverseMassUnits() * lpSim->InverseDistanceUnits() * lpSim->InverseDistanceUnits();

	//If max torque is over 1000 N then assume we mean infinity.
	if(m_fltMaxTorque >= 1000)
		m_fltMaxTorque = 1e35f;

	m_bServoMotor = oXml.GetChildBool("ServoMotor", m_bServoMotor);
	m_ftlServoGain = oXml.GetChildFloat("ServoGain", m_ftlServoGain);

	oXml.OutOfElem(); //OutOf Joint Element
}

		}		//Joints
	}			//Environment
}				//AnimatLibrary
