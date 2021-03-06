// VsBox.cpp: implementation of the VsBox class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsBox.h"
#include "VsSimulator.h"

namespace VortexAnimatLibrary
{
	namespace Environment
	{
		namespace Bodies
		{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsBox::VsBox()
{
}

VsBox::~VsBox()
{
}

CStdFPoint VsBox::GetCurrentPosition()
{
	m_oAbsPosition.x = m_lpBodyID->bodyTM[3][0];
	m_oAbsPosition.y = m_lpBodyID->bodyTM[3][1];
	m_oAbsPosition.z = m_lpBodyID->bodyTM[3][2];

	return m_oAbsPosition;
}

void VsBox::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	TRACE_DEBUG("Statring Parts Creation for " + m_strName);
	TRACE_DETAIL_NS("PartType: Rectangle");

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	MeMatrix4 aryRotationTM, aryBodyTM;
	MeMatrix3 aryRotation;

	MeMatrix4TMSetRotationFromEulerAngles(aryRotationTM, m_oRotation.x, m_oRotation.y, m_oRotation.z);
	MeMatrix4TMGetRotation(aryRotation, aryRotationTM) ;
	MeMatrix4TMMakeFromRotationAndPosition(aryBodyTM, aryRotation, m_oAbsPosition.x, m_oAbsPosition.y, m_oAbsPosition.z);

	m_lpGeometryID = McdBoxCreate(m_oCollisionBoxSize.x, m_oCollisionBoxSize.y, m_oCollisionBoxSize.z);
	if(!m_lpGeometryID)
		THROW_PARAM_ERROR(Vs_Err_lCreatingGeometry, Vs_Err_strCreatingGeometry, "BodyID", m_strName);

	m_lpModelID = MstModelAndBodyCreate(lpVsSim->Universe(), m_lpGeometryID, m_fltDensity);

	m_lpBodyID = McdModelGetBody(m_lpModelID);
	MdtBodySetTransform (m_lpBodyID, aryBodyTM);

	MeMatrix4Copy(m_aryInitialBodyTM, aryBodyTM);

	if(m_oCenterOfMass.x != 0 || m_oCenterOfMass.y != 0 || m_oCenterOfMass.z != 0)
	{
		MeMatrix4 aryCenterOfMassTM;
		MeVector3 aryFinalPos, aryCOM;
		aryCOM[0] = m_oCenterOfMass.x; aryCOM[1] = m_oCenterOfMass.y; aryCOM[2] = m_oCenterOfMass.z;

		//First lets account for any rotations of this body.
		MeMatrix4TMMakeIdentity(aryRotationTM);
		MeMatrix4TMSetRotationFromEulerAngles(aryRotationTM, m_oRotation.x, m_oRotation.y, m_oRotation.z);
		MeMatrix4TMTransformVector3(aryFinalPos, aryRotationTM, aryCOM);

		//Now create the center of mass transformation matrix and set the center of mass
		MeMatrix4TMSetRotationFromEulerAngles(aryRotationTM, 0, 0, 0);
		MeMatrix4TMGetRotation(aryRotation, aryRotationTM) ;
		MeMatrix4TMMakeFromRotationAndPosition(aryCenterOfMassTM, aryRotation, aryFinalPos[0], aryFinalPos[1], aryFinalPos[2]);
		MdtBodySetCenterOfMassRelativeTransform(m_lpBodyID, aryCenterOfMassTM);
	}

	MdtBodyEnable(m_lpBodyID);

	if(m_bIsVisible)
	  m_lpGraphic = RGraphicBoxCreate(lpVsSim->RenderContext(), m_oGraphicsBoxSize.x, m_oGraphicsBoxSize.y, m_oGraphicsBoxSize.z, m_aryColor, MdtBodyGetTransformPtr(m_lpBodyID));

	//Lets get the volume and areas
	m_fltVolume = m_oCollisionBoxSize.x * m_oCollisionBoxSize.y * m_oCollisionBoxSize.z;
	m_fltXArea = m_oCollisionBoxSize.y * m_oCollisionBoxSize.z;
	m_fltYArea = m_oCollisionBoxSize.x * m_oCollisionBoxSize.z;
	m_fltZArea = m_oCollisionBoxSize.y * m_oCollisionBoxSize.x;

	if(m_bFreeze)
		MstModelFreeze(m_lpModelID);

	if(!Std_IsBlank(m_strTexture))
		RGraphicSetTexture(lpVsSim->RenderContext(), m_lpGraphic, TextureFile(lpSim, m_strTexture).c_str());

	if(m_fltLinearVelocityDamping > 0)
		MdtBodySetLinearVelocityDamping(m_lpBodyID, m_fltLinearVelocityDamping);

	if(m_fltAngularVelocityDamping > 0)
		MdtBodySetAngularVelocityDamping(m_lpBodyID, m_fltAngularVelocityDamping);

	TRACE_DETAIL_NS("Volume: " + STR(m_fltVolume) + "   XArea: " + STR(m_fltXArea) + "  YArea: " + STR(m_fltYArea) + "  Z: " + STR(m_fltZArea));

	Box::CreateParts(lpSim, lpStructure);

	TRACE_DEBUG("Ending Parts Creation for " + m_strName);
}

void VsBox::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
	TRACE_DEBUG("Statring Joints Creation for Part " + m_strName);
	TRACE_DETAIL_NS("PartType: Rectangle");

	if(m_lpParent && !m_lpJointToParent)
		THROW_PARAM_ERROR(Al_Err_lJointParentMismatch, Al_Err_strJointParentMismatch, "Part", m_strName);

	if(m_lpJointToParent)
		m_lpJointToParent->CreateJoint(lpSim, lpStructure);

	Box::CreateJoints(lpSim, lpStructure);

	if(m_bIsContactSensor)
		McdModelSetResponseID(m_lpModelID, kMstResponseSensor);

	McdModelSetUserData(m_lpModelID, (void*) this); 

	VsRigidBody::Initialize(lpSim, lpStructure);

	TRACE_DEBUG("Ending Joints Creation for Part " + m_strName);
}

void VsBox::ResetNode(Simulator *lpSim, Structure *lpStructure)
{VsRigidBody::ResetNode(lpSim, lpStructure);}

void VsBox::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	Box::StepSimulation(lpSim, lpStructure);
	VsRigidBody::CollectBodyData(lpSim);

	//Only do this code if we are tracing details
#ifdef STD_TRACE_DETAIL
	MeVector3 aryPosition, aryRotation, aryLinear, aryAngular;
	MeMatrix3 aryOrientation;

	MdtBodyGetPosition(m_lpBodyID, aryPosition);
	MdtBodyGetLinearVelocity(m_lpBodyID, aryLinear);
	MdtBodyGetAngularVelocity(m_lpBodyID, aryAngular);

	MdtBodyGetOrientation(m_lpBodyID, aryOrientation);
	MeMatrix3GetEulerAngles(aryOrientation, aryRotation);

	TRACE_DETAIL_NS("TimeSlice: " + STR(lpSim->TimeSlice()) + "  Part: " + m_strName);
	TRACE_DETAIL_NS("Position: (" + STR(aryPosition[0]) + ", " + STR(aryPosition[1]) + ", " + STR(aryPosition[2]) + ")");
	TRACE_DETAIL_NS("Rotation: (" + STR(aryRotation[0]) + ", " + STR(aryRotation[1]) + ", " + STR(aryRotation[2]) + ")");
	TRACE_DETAIL_NS("LinearVelocity: (" + STR(aryLinear[0]) + ", " + STR(aryLinear[1]) + ", " + STR(aryLinear[2]) + ")");
	TRACE_DETAIL_NS("AngularVelocity: (" + STR(aryAngular[0]) + ", " + STR(aryAngular[1]) + ", " + STR(aryAngular[2]) + ")");
#endif

	if(lpSim->SimulateHydrodynamics())
		SimulateHydrodynamics(lpSim, lpStructure);

	TRACE_DETAIL_NS("");
}

	
void VsBox::EnableCollision(RigidBody *lpBody)
{VsRigidBody::EnableCollision(lpBody);}

void VsBox::DisableCollision(RigidBody *lpBody)
{VsRigidBody::DisableCollision(lpBody);}

float *VsBox::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	lpData = Box::GetDataPointer(strDataType);
	if(lpData) return lpData;

	lpData = VsRigidBody::GetVortexDataPointer(strDataType);
	if(lpData) return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

void VsBox::AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz)
{VsRigidBody::AddBodyForce(lpSim, fltPx, fltPy, fltPz, fltFx, fltFy, fltFz);}

void VsBox::AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz)
{VsRigidBody::AddBodyTorque(lpSim, fltTx, fltTy, fltTz);}

/*
void VsBox::operator=(VsBox &oOrig)
{
	VsRigidBody::Copy(&oOrig);
	m_oCollisionBoxSize = oOrig.m_oCollisionBoxSize;
	m_oGraphicsBoxSize = oOrig.m_oGraphicsBoxSize;
}


CStdSerialize *VsBox::Clone()
{
	VsBox *lpPart = NULL;

try
{
	lpPart = new VsBox(m_lpParent);
	lpPart->operator=(*this);

	return lpPart;
}
catch(CStdErrorInfo oError)
{
	if(lpPart) delete lpPart;
	RELAY_ERROR(oError);
	return NULL;
}
catch(...)
{
	if(lpPart) delete lpPart;
	THROW_ERROR(Err_lUnspecifiedError, Err_strUnspecifiedError);
	return NULL;
}
}


void VsBox::Trace(ostream &oOs)
{
//	oOs << "{ Name: " << m_strName  << ", Size: " << m_oSize ;
//	oOs <<", RelPos: " << m_oRelPosition << ", AbsPos: " << m_oAbsPosition;
//	oOs << ", " << m_fltDensity << "}";
}
*/

		}		//Bodies
	}			// Environment
}				//VortexAnimatLibrary
