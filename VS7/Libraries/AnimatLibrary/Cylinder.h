// Cylinder.h: interface for the Cylinder class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALCYLINDER_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_)
#define AFX_ALCYLINDER_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatLibrary
{
	namespace Environment
	{
		namespace Bodies
		{
			/*! \brief 
				A cylinder type of rigid body.

				\remarks
				This is a cylinder type of rigid body. You can specify the dimensions of 
				the radius and height for both the collision model and for the graphics model.

				\sa
				Body, Joint, CAlBox, CAlPlane, CAlCylinder, 
				CAlCone, CAlMuscle, CAlMuscleAttachment, CAlSphere                                
				 
				\ingroup AnimatLibrary
			*/

			class ANIMAT_PORT Cylinder : public RigidBody
			{
			protected:
				float m_fltRadius;
				float m_fltHeight;

				float m_fltCollisionRadius;
				float m_fltCollisionHeight;

			public:
				Cylinder();
				virtual ~Cylinder();

				virtual void Trace(ostream &oOs);
				virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatLibrary

#endif // !defined(AFX_ALCYLINDER_H__B6B13C0B_D733_44AF_917D_372FE21A4A2D__INCLUDED_)
