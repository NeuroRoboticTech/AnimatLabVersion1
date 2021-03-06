// RigidBody.h: interface for the RigidBody class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALRIGIDBODY_H__BE00E72D_B205_450A_9A20_58752ED37EED__INCLUDED_)
#define AFX_ALRIGIDBODY_H__BE00E72D_B205_450A_9A20_58752ED37EED__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatLibrary
{
	namespace Environment
	{
		class Odor;

		/*! \brief 
			The base class for all of the basic rigid body type of
			objects.
		   
			\remarks
			This class provides the base functionality for a rigid body.
			Each structure/Organism is made up of a heirarchcal tree of
			rigid bodies that are connected by Joint objects. The base
			structure has a root rigid body. That root body has a list of
			child rigid bodies that are connected to the root through a
			joint. Each of those child bodies can then have other
			children connected to them and so on.
		   
		   
		   
			Each rigid body within a structure/organism has a unique
			string identifier that is specifed in the configuration file.
			Whenever you are attempting to find a given part through a
			function like Simulator::FindRigidBody it is this
			identifier that is used as the key in the search. This class
			also contains a number of parameters that are common for all
			rigid bodies. These include things like the relative position
			of the center of the object and the uniform density of the
			body. All rigid bodies have these properties.
		   
		   
		   
			This is a virtual base class and can not be directly created.
			You must override this class to provide specific
			functionality in order to create a real rigid body. For
			example, the CAlBox and CAlCylinder are two examples of
			subclasses of a rigid body that add more parameters specific
			for their body type. Box has the x,y, and z widths for the
			box, whereas cylinder has the radius and height of the
			cylinder to create. The purpose of Body and its
			subclasses like CAlBox are to try and provide implement as
			much of the functionality as can be accomplished in this
			library, yet leave it flexible enough that a new physics
			engine could be easily swapped in for use. This means that a
			lot of the mundane things like loading the data for the
			different rigid body and joint types is done for you
			automatically by the Body and Joint classes. But
			these classes can still not really implement all of the
			functionality necessary for the rigid body. The reason for
			this is that the overridable functions to create the actual
			implementation for those parts in your chosen physics engine
			must still be implemented. The two main functions related to
			that are CreateParts and CreateJoints. Since AnimatLibrary is
			generalized so that it is not tightly coupled to any one
			physics engine then that coupling must be done in the layer
			above this library where the chosen physics engine like
			Vortex is actually used.
		   
			\sa
			Body, Joint, CAlBox, CAlPlane, CAlCylinder, 
			CAlCone, CAlMuscle, CAlMuscleAttachment, CAlSphere                                
			 
			\ingroup AnimatLibrary
		*/

		class ANIMAT_PORT RigidBody : public Node    
		{
		protected:
			///The relative position of the center of this rigid body
			///in relation to the center of its parent rigid body. 
			CStdFPoint m_oRelPosition;

			///The absolute position of the rigid body in world coordinates.
			///This is calcualted during loading of the part using the position of 
			///the parent part and the relative position specified in the configuration file.
			CStdFPoint m_oAbsPosition;

			//These are rotation and position coords relative to the parent. Not in world coords.
			//This is only used for a few special cases like contact sensors.
			CStdFPoint m_oLocalPosition;
			CStdFPoint m_oLocalRotation;

			///The rotation to apply to this rigid body. It is defined by the three
			///euler angles in radians.
			CStdFPoint m_oRotation;

			///The center of mass of this body relative to the center of the object. 
			CStdFPoint m_oCenterOfMass;

			///The color to apply to this part. It is specified as red, green, blue, and alpha.
			float m_aryColor[4];

			///An optional texture to apply to the rigid body.
			string m_strTexture;

			///Specifies if the part should frozen in place to the world. If a rigid body 
			///is frozen then it is as if it is nailed in place and can not move. Gravity and 
			///and other forces will not act on it.
			BOOL m_bFreeze;

			///Uniform density for the rigid body.
			float m_fltDensity;

			///Drag Coefficient
			float m_fltCd;   
			float m_fltCdr;   
			float m_fltCa;   //Alpha (added mass coefficient for linear) 
			float m_fltCar; //gamma (added mass coefficient for angular)  

			///Total volume for the rigid body. This is used in calculating the buoyancy
			float m_fltVolume;

			///The area of this rigid body in the x direction. This is used to calculate the
			///drag force in this direction.
			float m_fltXArea;

			///The area of this rigid body in the y direction. This is used to calculate the
			///drag force in this direction.
			float m_fltYArea;

			///The area of this rigid body in the z direction. This is used to calculate the
			///drag force in this direction.
			float m_fltZArea;

			///The parent rigid body of this part. If this value is null
			///then it is assumed that this is the root part of a structure.
			RigidBody *m_lpParent;

			///A list of child parts that are connected to this part through
			///different joints. 
			CStdPtrArray<RigidBody> m_aryChildParts;

			///A pointer to the joint that connects this rigid body to its parent.
			///If this is the root object then this pointer is null. However, All child parts
			///must be connected to their parent.
			Joint *m_lpJointToParent;

			//Some body parts like contact sensors and muscle attachments do not have joints. If not then we should not 
			//attempt to load them.
			BOOL m_bUsesJoint;

			///Determines if this body is physically seen or not. If this is FALSE then 
			///whatever geometry this is, like a box, is not seen in the graphics.
			BOOL m_bIsVisible;

			///This determines whether or not this is a contact sensor. If this is TRUE then
			///this object does not take part in collisions and such, it is a contact sensor only.
			BOOL m_bIsContactSensor;

			///This keeps track of the current number of surface contacts that are occuring for this
			///contact sensor. This is only used for sensors.
			float m_fltSurfaceContactCount;

			ContactSensor *m_lpContactSensor;

			float m_fltLinearVelocityDamping;
			float m_fltAngularVelocityDamping;

			CStdPtrMap<string, Odor> m_aryOdorSources;
			
			BOOL m_bFoodSource;  //Tells if this body is considered a food source.
			float m_fltFoodQuantity;
			float m_fltFoodEaten;  //Tells how much food is being eaten.
			float m_fltMaxFoodQuantity;
			float m_fltFoodReplenishRate;
			float m_fltFoodEnergyContent;
			long m_lEatTime;

			void Copy(RigidBody *lpOrig);
			RigidBody *LoadRigidBody(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			float *LoadMeshVertices(CStdXml &oXml, string strTagName, int &iVertCount, BOOL bThrowError = TRUE);

			string TextureFile(Simulator *lpSim, string strTexture);

			Odor *LoadOdor(Simulator *lpSim, CStdXml &oXml);
			void AddOdor(Odor *lpOdor);

		public:
			RigidBody();
			virtual ~RigidBody();

			string Texture() {return m_strTexture;};
			void Texture(string strValue) {m_strTexture = strValue;};

			CStdFPoint RelativePosition() {return m_oRelPosition;};
			void RelativePosition(CStdFPoint &oPoint) {m_oRelPosition = oPoint;};

			CStdFPoint AbsolutePosition() {return m_oAbsPosition;};
			void AbsolutePosition(CStdFPoint &oPoint) {m_oAbsPosition = oPoint;};

			CStdFPoint Rotation()	{return m_oRotation;};
			void Rotation(CStdFPoint &oPoint) {m_oRotation = oPoint;};

			CStdFPoint LocalPosition() {return m_oLocalPosition;};
			CStdFPoint LocalRotation() {return m_oLocalRotation;};

			CStdPtrArray<RigidBody>* ChildParts() {return &m_aryChildParts;};

			RigidBody *Parent() {return m_lpParent;};
			void Parent(RigidBody *lpValue) {m_lpParent = lpValue;};

			Joint *JointToParent() {return m_lpJointToParent;};
			void JointToParent(Joint *lpValue) {m_lpJointToParent = lpValue;};

			ContactSensor *ContactSensor() {return m_lpContactSensor;};

			float Density() {return m_fltDensity;};
			void Density(float fltVal) {m_fltDensity = fltVal;};

			float Cd() {return m_fltCd;};
			void Cd(float fltVal) {m_fltCd = fltVal;};

			float Cdr() {return m_fltCdr;};
			void Cdr(float fltVal) {m_fltCdr = fltVal;};

			float Ca() {return m_fltCa;};
			void Ca(float fltVal) {m_fltCa = fltVal;};

			float Car() {return m_fltCar;};
			void Car(float fltVal) {m_fltCar = fltVal;};

			float Volume() {return m_fltVolume;};
			float XArea() {return m_fltXArea;};
			float YArea() {return m_fltYArea;};
			float ZArea() {return m_fltZArea;};

			BOOL Freeze() {return m_bFreeze;};
			void Freeze(BOOL bVal) {m_bFreeze = bVal;};

			BOOL IsVisible() {return m_bIsVisible;};
			void IsVisible(BOOL bVal) {m_bIsVisible = bVal;};

			BOOL IsContactSensor() {return m_bIsContactSensor;};
			void IsContactSensor(BOOL bVal) {m_bIsContactSensor = bVal;};

			BOOL IsFoodSource() {return m_bFoodSource;};
			void IsFoodSource(BOOL bVal) {m_bFoodSource = bVal;};

			float FoodQuantity() {return m_fltFoodQuantity;};
			void FoodQuantity(float fltVal) {m_fltFoodQuantity = fltVal;};

			float FoodEaten() {return m_fltFoodEaten;};
			void FoodEaten(float fltVal) {m_fltFoodEaten = fltVal;};

			void Eat(float fltVal, long lTimeSlice);
			
			float FoodReplenishRate() {return m_fltFoodReplenishRate;};
			void FoodReplenishRate(float fltVal) {m_fltFoodReplenishRate = fltVal;};

			float FoodEnergyContent() {return m_fltFoodEnergyContent;};
			void FoodEnergyContent(float fltVal) {m_fltFoodEnergyContent = fltVal;};

			virtual CStdFPoint GetCurrentPosition() {return m_oAbsPosition;};

			virtual float SurfaceContactCount() {return m_fltSurfaceContactCount;};
			virtual void AddSurfaceContact(Simulator *lpSim, RigidBody *lpContactedSurface);
			virtual void RemoveSurfaceContact(Simulator *lpSim, RigidBody *lpContactedSurface);
			virtual void AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz);
			virtual void AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz);

			virtual void EnableCollision(RigidBody *lpBody);
			virtual void DisableCollision(RigidBody *lpBody);

			virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
			virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);

			virtual void Trace(ostream &oOs);
			virtual void CompileIDLists(Simulator *lpSim, Structure *lpStructure);

			//Node Overrides
			virtual void Kill(Simulator *lpSim, Organism *lpOrganism, BOOL bState = TRUE);
			virtual void Reset(Simulator *lpSim, Structure *lpStructure);
			virtual void AddExternalNodeInput(Simulator *lpSim, Structure *lpStructure, float fltInput);
			virtual float *GetDataPointer(string strDataType);
			virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
			virtual void Load(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
			virtual void Save(Simulator *lpSim, Structure *lpStructure, CStdXml &oXml);
		};

	}			// Environment
}				//AnimatLibrary

#endif // !defined(AFX_ALRIGIDBODY_H__BE00E72D_B205_450A_9A20_58752ED37EED__INCLUDED_)
