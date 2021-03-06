// VsBoxContactSensor.h: interface for the VsBoxContactSensor class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSBOX_CONTACT_SENSOR_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_)
#define AFX_VSBOX_CONTACT_SENSOR_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatLibrary
{
	namespace Environment
	{
		namespace Bodies
		{

			class VORTEX_PORT VsBoxContactSensor : public Box, public VsRigidBody
			{
			protected:
				MeMatrix4 m_aryBodyTM;
				MeMatrix4 m_aryRelativeTM;

				void UpdatePosition(Simulator *lpSim);
				virtual void ResetNode(Simulator *lpSim, Structure *lpStructure);

			public:
				VsBoxContactSensor();
				virtual ~VsBoxContactSensor();

				virtual CStdFPoint GetCurrentPosition();

				virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
				virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);
				virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
				virtual float *GetDataPointer(string strDataType);

				virtual void EnableCollision(RigidBody *lpBody);
				virtual void DisableCollision(RigidBody *lpBody);
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatLibrary

#endif // !defined(AFX_VSBOX_CONTACT_SENSOR_H__F74F0855_9701_4D03_82C4_EA3E5755910A__INCLUDED_)
