// VsBoxContactSensor.cpp: implementation of the VsBoxContactSensor class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsJoint.h"
#include "VsRigidBody.h"
#include "VsBoxContactSensor.h"
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

VsBoxContactSensor::VsBoxContactSensor()
{
	m_bUsesJoint = FALSE;
	m_bIsContactSensor = TRUE;
}

VsBoxContactSensor::~VsBoxContactSensor()
{
}

CStdFPoint VsBoxContactSensor::GetCurrentPosition()
{
	m_oAbsPosition.x = m_aryBodyTM[3][0];
	m_oAbsPosition.y = m_aryBodyTM[3][1];
	m_oAbsPosition.z = m_aryBodyTM[3][2];

	return m_oAbsPosition;
}

void VsBoxContactSensor::UpdatePosition(Simulator *lpSim)
{
		VsRigidBody *lpParent = dynamic_cast<VsRigidBody *>(this->Parent());

		MeMatrix4 aryParentTM;
		MdtBodyGetTransform(lpParent->BodyID(), aryParentTM);
		MeMatrix4MultiplyMatrix(m_aryBodyTM, m_aryRelativeTM, aryParentTM);

		if(m_bCollectData)
		{
			m_vPosition[0] = m_aryBodyTM[3][0] * lpSim->DistanceUnits();
			m_vPosition[1] = m_aryBodyTM[3][1] * lpSim->DistanceUnits();
			m_vPosition[2] = m_aryBodyTM[3][2] * lpSim->DistanceUnits();
		}
}

void VsBoxContactSensor::CreateParts(Simulator *lpSim, Structure *lpStructure)
{
	TRACE_DEBUG("Statring Parts Creation for " + m_strName);
	TRACE_DETAIL_NS("PartType: Box Sensor");

	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(lpSim);
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	MeMatrix4 aryRotationTM;
	MeMatrix3 aryRotation;

	//Setup the initial body tranformation matrix using global coordinates
	MeMatrix4TMSetRotationFromEulerAngles(aryRotationTM, m_oRotation.x, m_oRotation.y, m_oRotation.z);
	MeMatrix4TMGetRotation(aryRotation, aryRotationTM) ;
	MeMatrix4TMMakeFromRotationAndPosition(m_aryBodyTM, aryRotation, m_oAbsPosition.x, m_oAbsPosition.y, m_oAbsPosition.z);

	//Setup the relative transformation matrix that will be used to recalculate the body matrix at each time step
	MeMatrix4TMSetRotationFromEulerAngles(aryRotationTM, m_oLocalRotation.x, m_oLocalRotation.y, m_oLocalRotation.z);
	MeMatrix4TMGetRotation(aryRotation, aryRotationTM) ;
	MeMatrix4TMMakeIdentity(m_aryRelativeTM);
	MeMatrix4TMMakeFromRotationAndPosition(m_aryRelativeTM, aryRotation, m_oLocalPosition.x, m_oLocalPosition.y, m_oLocalPosition.z);

	MeMatrix4Copy(m_aryInitialBodyTM, m_aryBodyTM);

	//Create the geometry and model
	m_lpGeometryID = McdBoxCreate(m_oCollisionBoxSize.x, m_oCollisionBoxSize.y, m_oCollisionBoxSize.z);
	if(!m_lpGeometryID)
		THROW_PARAM_ERROR(Vs_Err_lCreatingGeometry, Vs_Err_strCreatingGeometry, "BodyID", m_strName);

	m_lpModelID = MstSensorModelCreate(lpVsSim->Universe(), m_lpGeometryID, m_aryBodyTM);
	McdModelSetUserData(m_lpModelID, (void*) this); 

	if(m_bIsVisible)
	  m_lpGraphic = RGraphicBoxCreate(lpVsSim->RenderContext(), m_oGraphicsBoxSize.x, m_oGraphicsBoxSize.y, m_oGraphicsBoxSize.z, m_aryColor, m_aryBodyTM);

	if(!Std_IsBlank(m_strTexture))
		RGraphicSetTexture(lpVsSim->RenderContext(), m_lpGraphic, TextureFile(lpSim, m_strTexture).c_str());

	//disable collisions between this sensor and its parents.
	VsRigidBody *lpVsParent = dynamic_cast<VsRigidBody *>(m_lpParent);
	if(!lpVsParent)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsRigidBody, Vs_Err_strUnableToConvertToVsRigidBody);

	McdSpaceDisablePair(lpVsParent->ModelID(), m_lpModelID);

	TRACE_DETAIL_NS("Volume: " + STR(m_fltVolume) + "   XArea: " + STR(m_fltXArea) + "  YArea: " + STR(m_fltYArea) + "  Z: " + STR(m_fltZArea));

	Box::CreateParts(lpSim, lpStructure);

	TRACE_DEBUG("Ending Parts Creation for " + m_strName);
}

void VsBoxContactSensor::ResetNode(Simulator *lpSim, Structure *lpStructure)
{
	//MeMatrix4Copy(m_aryBodyTM, m_aryInitialBodyTM);
	VsRigidBody::ResetNode(lpSim, lpStructure);
}

void VsBoxContactSensor::CreateJoints(Simulator *lpSim, Structure *lpStructure)
{
}

void VsBoxContactSensor::StepSimulation(Simulator *lpSim, Structure *lpStructure)
{
	Box::StepSimulation(lpSim, lpStructure);
	UpdatePosition(lpSim);
	
	TRACE_DETAIL_NS("");
}

void VsBoxContactSensor::EnableCollision(RigidBody *lpBody)
{VsRigidBody::EnableCollision(lpBody);}

void VsBoxContactSensor::DisableCollision(RigidBody *lpBody)
{VsRigidBody::DisableCollision(lpBody);}

float *VsBoxContactSensor::GetDataPointer(string strDataType)
{
	string strType = Std_CheckString(strDataType);
	float *lpData = NULL;

	if(strType == "CONTACTCOUNT")
		return &m_fltSurfaceContactCount;

	lpData = Box::GetDataPointer(strDataType);
	if(lpData) return lpData;

	lpData = VsRigidBody::GetVortexDataPointer(strDataType);
	if(lpData) return lpData;

	THROW_TEXT_ERROR(Al_Err_lInvalidDataType, Al_Err_strInvalidDataType, "RigidBodyID: " + STR(m_strName) + "  DataType: " + strDataType);

	return NULL;
}

		}		//Bodies
	}			// Environment
}				//VortexAnimatLibrary
