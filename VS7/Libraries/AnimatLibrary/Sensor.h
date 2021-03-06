// AlSensor.h: interface for the CAlSensor class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALSENSOR_H__41341DA4_DDA9_4B00_9198_27B628231EE2__INCLUDED_)
#define AFX_ALSENSOR_H__41341DA4_DDA9_4B00_9198_27B628231EE2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

namespace AnimatLibrary
{
	namespace Environment
	{
		namespace Bodies
		{

				class ANIMAT_PORT Sensor : public RigidBody   
				{
				public:
					Sensor();
					virtual ~Sensor();

					virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
					virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);

					virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
				};

		}		//Bodies
	}			// Environment
}				//AnimatLibrary

#endif // !defined(AFX_ALSENSOR_H__41341DA4_DDA9_4B00_9198_27B628231EE2__INCLUDED_)
