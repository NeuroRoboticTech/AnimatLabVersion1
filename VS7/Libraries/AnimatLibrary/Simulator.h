// Simulator.h: interface for the Simulator class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ALSIMULATOR_H__2CAD431A_1D6E_446F_90CD_728509081187__INCLUDED_)
#define AFX_ALSIMULATOR_H__2CAD431A_1D6E_446F_90CD_728509081187__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace AnimatLibrary
{

		/*! \brief 
			The main simulator system.

			\remarks
			This is the root object used to create and manage the entire
			simulation. You tell it the the path to the simulation file
			to use and it takes care of loading the files and creating all
			of the objects like organisms and structures. Then at each time
			step you need to step the simulation by one slice.

			\sa
			Organism, Structure, Body, Joint, CNlSimulator
			 
			\ingroup AnimatLibrary
		*/
		typedef void (*ManagedCallbackPtr)(void *);
		//void *lpInstance

		class ANIMAT_PORT Simulator : public CStdSerialize
		{
		protected:
			///The directory path where the simulation configuration files are located.
			string m_strProjectPath;

			///The directory path where the executable is located.
			string m_strExecutablePath;

			///The name of the Animat Simulation (ASIM) file.
			string m_strSimulationFile;

			///A list of all Organisms in this simulation. This is not a reference list
			///It is an actual list that destroys its objects when the list is destroyed.
			CStdPtrMap<string, Organism> m_aryOrganisms;

			///A list of structures in this simulation that are not organisms. 
			///This is not a reference list. It is an actual list that destroys
			///its objects when the list is destroyed.
			CStdPtrMap<string, Structure> m_aryStructures;

			///A list of all structures, both regular structures and organisms. This is
			///a reference list.
			CStdMap<string, Structure *> m_aryAllStructures;

			///A list of all odors in this simulation. This is not a reference list
			///It is an actual list that destroys its objects when the list is destroyed.
			CStdPtrMap<string, OdorType> m_aryOdorTypes;

			CStdArray<RigidBody *> m_aryFoodSources;

			///A list of class factories for neural modules that have been loaded by any organisms in the
			///environment. Each factory is only added to the list once even if the 
			///the same module is loaded seperately for each organism. 
			CStdMap<string, IStdClassFactory *> m_aryNeuralModuleFactories;

			///This is a copy of the class factory associated with the animat module. 
			///This is the default class factory that is used if no specific module name
			///is specified. If a specific name is given then it looks through the list
			///of modules that were loaded for neural modules. 
			///All class factories should have have NO state.
			IStdClassFactory *m_lpAnimatClassFactory;

			///The acceleration of gravity to use in the simulation.
			float m_fltGravity;

			float m_fltDistanceUnits;
			float m_fltInverseDistanceUnits;
			float m_fltDenominatorDistanceUnits;
			float m_fltMassUnits;
			float m_fltInverseMassUnits;
			float m_fltDensityMassUnits;

			float m_fltLinearCompliance;
			float m_fltAngularCompliance;
			float m_fltLinearDamping;
			float m_fltAngularDamping;
			float m_fltLinearKineticLoss;
			float m_fltAngularKineticLoss;

			///Tells whether or not we will be doing hydrodynamic simulations.
			///If you are not doing stuff underwater then be sure this is set to
			///FALSE. The hydrodynamics adds extra overhead that can slow the
			///simulation down slightly.
			BOOL m_bSimulateHydrodynamics;

			///Density of the fluid for hydrodynamic simulations. This is not
			///used if m_bSimulateHydrodynamics is FALSE.
			float m_fltFluidDensity;

			float m_fltMaxHydroForce;
			float m_fltMaxHydroTorque;

			///A pointer to the water surface structure. If no water surface is specified
			///then this pointer remains NULL.
			Structure *m_lpWaterSurface;

			///A pointer to the ground surface structure. If no ground surface is specified
			///then this pointer remains NULL.
			Structure *m_lpGroundSurface;

			///The value of the maximum surface friction force that can be applied.
			float m_fltMaxSurfaceFriction;

			///This is the minimum integratin time step taken for all neural modules and the physics engine.
			float m_fltTimeStep;

			///The current time slice. This a long value.
			long m_lTimeSlice;

			///The time when the simulation should end.
			float m_fltEndSimTime;

			///The time slice when the simulation should end
			float m_lEndSimTimeSlice;

			///The tick count for when the simulation first begins running.
			DWORD m_iStartSimTick;

			BOOL m_bStopSimulation;

			///The playback rate for the simulation. This is a multiplying factor. A setting of 2 would mean
			///that 2 seconds of simulation time passes for each second of real time.
			float m_fltPlaybackRate;

			///The number of slices that the physics engine has been updated.
			long m_lPhysicsSliceCount;

			long m_lVideoSliceCount;

			int m_iVideoLoops;

			///This tells how many time slices occur between each interval where the 
			///body physics is updated. There could be multiple neural updates in between
			///one physics update.
			short m_iPhysicsStepInterval;

			///The time increment for each time slice of the physics simulation. 
			float m_fltPhysicsTimeStep;

			///This keeps track of the number of steps since the physics system was
			///last updated. By using this we do not have to do a bunch of divisions
			///at each time step to determine if it is time to update the physics engine.
			int m_iPhysicsStepCount;

			CStdArray<Adapter *> m_arySourcePhysicsAdapters;
			CStdArray<Adapter *> m_aryTargetPhysicsAdapters;
			int m_iTargetAdapterCount;

			///The rotation of the camera.
			float m_fltCameraRotation;

			///The elevation of the camera.
			float m_fltCameraElevation;

			///The offset of the camera.
			float m_fltCameraOffset;

			BOOL m_bUseAlphaBlending;
			BOOL m_bUseWireframe;

			///The ID of the structure that the camera should look at initially. If this
			///is blank or missing then the camera looks in its default direction. If 
			///m_bTrackCamera is TRUE then it will follow the structure identfied here.
			string m_strLookAtStructureID;

			///The ID of the body item within the structure that the camera should look at initially. If this
			///is blank or missing then the camera looks in its default direction. If 
			///m_bTrackCamera is TRUE then it will follow the structure identfied here.
			string m_strLookAtBodyID;

			///If this is TRUE then the camera will update its position and orientation so
			///that it stays focues on the structure specified by the m_strLookAtID element.
			///The camera always looks at the center of the root rigid body object of the 
			///structure.
			BOOL m_bTrackCamera;

			HWND m_hSimulationWnd;
			HWND m_hParentWnd;
			LONG_PTR m_PrevWndProc;
			BOOL m_bManualStepSimulation;
			BOOL m_bPaused;
			BOOL m_bInitialized;
			long m_lUpdateDataInterval;
			long m_lUpdateDataCount;

			BOOL m_bEnableSimRecording;
			long m_lSnapshotByteSize;

			//Temp Variables
			CStdMap<string, Organism *>::iterator m_oOrganismIterator;
			CStdMap<string, Structure *>::iterator m_oStructureIterator;
			Organism *m_lpSelOrganism;
			Structure *m_lpSelStructure;

			KeyFrame *m_lpVideoRecorder;
			KeyFrame *m_lpVideoPlayback;
			KeyFrame *m_lpSimStopPoint;

			///Determines whether a random seed is automatically generated when the 
			///simulation is initialized, or if a manual seed is used.
			BOOL m_bAutoGenerateRandomSeed;

			///If the AutoGenerateRandomSeed variable is false then this manual seed is
			///used for the random number generator.
			int m_iManualRandomSeed;

			///This is a void pointer to the gc handle of a managed simulator wrapper class from the
			///ManagedAnimatTools project. It is used in the callback routines. If certain callback 
			///routines are defined, like UpdateDataCallback, then when the simulator has collected
			///the desired amount of information it will invoke this callback and pass the ManagedInstance
			///pointer back as a param. Inside that callback function the instance pointer will be cast 
			///back into the appropraite managed Simulator object and then the UpdateData even will be
			///fired. Any forms, like graphs, that have subscribed to this event will be notified that 
			///they need to query the simulation object for the latest data and update their graphs.
			///This system allows the unmanaged animat simulation to provide notifications to the
			///managed editor system, while maintaining strict STL compliance.
			void *m_lpManagedInstance;

			///Callback Function pointer that will be called each time the simulator needs to notify
			///any subscribing applications to update their data. This will only be fired if it is set.
			ManagedCallbackPtr m_lpUpdateDataCallback;
			ManagedCallbackPtr m_lpStartSimulationCallback;
			ManagedCallbackPtr m_lpPauseSimulationCallback;
			ManagedCallbackPtr m_lpEndingSimulationCallback;

			DataChartMgr m_oDataChartMgr;
			ExternalStimuliMgr m_oExternalStimuliMgr;
			SimulationRecorder *m_lpSimRecorder;

			BOOL m_bHasConvexMesh;
			BOOL m_bHasTriangleMesh;
			BOOL m_bHasHeightField;

			float m_fltMouseSpringStiffness;
			float m_ftlMouseSpringDamping;

			//Video variables
			BOOL m_bRecordVideo;
			string m_strVideoFilename;
			float m_fltVideoRecordFrameTime;
			float m_fltVideoPlaybackFrameTime;
			float m_fltVideoStartTime;
			float m_fltVideoEndTime;
			int m_iVideoStepSize;
			int m_iVideoStep;
			long m_lVideoStartSlice;
			long m_lVideoEndSlice;
			long m_lVideoFrame;
			CStdAvi *m_lpAvi;
			AVICOMPRESSOPTIONS m_aviOpts; 

			virtual void LoadEnvironment(CStdXml &oXml);
			Structure *LoadStructure(CStdXml &oXml);
			Organism *LoadOrganism(CStdXml &oXml);
			virtual void InitializeStructures();

			void GenerateAutoSeed();

			void AddOrganism(Organism *lpOrganism);
			void AddStructure(Structure *lpStructure);

			void AddOdorType(OdorType *lpOdorType);
			OdorType *LoadOdorType(CStdXml &oXml);

			void StepNeuralEngine();
			void StepPhysicsEngine();
			void Step();

			static void LoadAnimatModuleName(string strFile, string &strAnimatModule);
			static void LoadAnimatModuleName(CStdXml &oXml, string &strAnimatModule);

			virtual SimulationRecorder *CreateSimulationRecorder() = 0;
			virtual long CalculateSnapshotByteSize();
			virtual void SnapshotStopFrame() = 0;

			float ConvertDistanceUnits(string strUnits);
			float ConvertDenominatorDistanceUnits(string strUnits);
			float ConvertMassUnits(string strUnits);
			float ConvertDensityMassUnits(string strUnits);

			void RecordVideoFrame();

		public:
			Simulator();
			virtual ~Simulator();

			//////Temp Property!!!!
			virtual long UpdateCount() {return 0;};

			virtual float Time() {return m_lTimeSlice * m_fltTimeStep;};
			virtual long Millisecond() {return (long) (Time() * 1000);};
			virtual long MillisecondToSlice(long lMillisecond) {return (long) (lMillisecond / (m_fltTimeStep * 1000));};
			virtual long SliceToMillisecond(long lSlice) {return (long) (lSlice * m_fltTimeStep * 1000);};
			virtual DWORD StartSimTick() {return m_iStartSimTick;};

			virtual long TimeSlice() {return m_lTimeSlice;};
			virtual void TimeSlice(long lVal) {m_lTimeSlice = lVal;};

			virtual long PhysicsSliceCount() {return m_lPhysicsSliceCount;};
			virtual void PhysicsSliceCount(long lVal) {m_lPhysicsSliceCount = lVal;};

			virtual long VideoSliceCount() {return m_lVideoSliceCount;};
			virtual void VideoSliceCount(long lVal) {m_lVideoSliceCount = lVal;};

			virtual int VideoLoops() {return m_iVideoLoops;};
			virtual void VideoLoops(int iVal) {m_iVideoLoops = iVal;};

			virtual BOOL ManualStepSimulation() {return m_bManualStepSimulation;};
			virtual void ManualStepSimulation(BOOL bVal) {m_bManualStepSimulation = bVal;};

			virtual string ProjectPath() {return m_strProjectPath;};
			virtual void ProjectPath(string strPath) {m_strProjectPath = strPath;};

			virtual string ExecutablePath() {return m_strExecutablePath;};
			virtual void ExecutablePath(string strPath) {m_strExecutablePath = strPath;};

			virtual string SimulationFile() {return m_strSimulationFile;};
			virtual void SimulationFile(string strFile) {m_strSimulationFile = strFile;};

			BOOL Paused() {return m_bPaused;};
			void Paused(BOOL bVal) {m_bPaused = bVal;};

			BOOL Initialized() {return m_bInitialized;};
			void Initialized(BOOL bVal) {m_bInitialized = bVal;};

			BOOL UseAlphaBlending() {return m_bUseAlphaBlending;};
			virtual void UseAlphaBlending(BOOL bVal) {m_bUseAlphaBlending = bVal;};

			virtual BOOL UseMicrocontrollers() {return FALSE;};

			BOOL AutoGenerateRandomSeed() {return m_bAutoGenerateRandomSeed;};
			int ManualRandomSeed() {return m_iManualRandomSeed;};

			HWND ParentHwnd() {return m_hParentWnd;};
			void ParentHwnd(HWND hParentWnd) {m_hParentWnd = hParentWnd;};
			void ParentHwnd(void *hParentWnd) 
			{
				m_hParentWnd = (HWND) hParentWnd;
			};

			HWND SimulationHwnd() {return m_hSimulationWnd;};
			void SimulationHwnd(HWND hWnd) {m_hSimulationWnd = hWnd;};

			virtual float TimeStep();
			virtual void TimeStep(float fltVal);

			float EndSimTime() {return m_fltEndSimTime;};
			void EndSimTime(float fltVal) {m_fltEndSimTime = fltVal;};

			long EndSimTimeSlice() {return m_lEndSimTimeSlice;};
			void EndSimTimeSlice(long lVal) {m_lEndSimTimeSlice = lVal;};

			BOOL Stopped() {return m_bStopSimulation;};

			virtual float PlaybackRate() {return m_fltPlaybackRate;};
			virtual void PlaybackRate(float fltVal) {m_fltPlaybackRate = fltVal;};
		 
			virtual short PhysicsStepInterval() {return m_iPhysicsStepInterval;};
			virtual void PhysicsStepInterval(short iVal) {m_iPhysicsStepInterval = iVal;};
		 
			virtual float PhysicsTimeStep() {return m_fltPhysicsTimeStep;};
			virtual long PhysicsStepCount() {return m_iPhysicsStepCount;};

			float Gravity() {return m_fltGravity;};
			void Gravity(float fltVal) {m_fltGravity = fltVal;};

			float DistanceUnits() {return m_fltDistanceUnits;};
			void DistanceUnits(float fltVal) {m_fltDistanceUnits = fltVal;};
			float InverseDistanceUnits() {return m_fltInverseDistanceUnits;};
			//For items that use distance unit measures in the denominator we may want to use a differnt
			//scale that that used for the whole app. For example, if we are using a distance scale of decimeters
			//we will want to use centimeters for the density instead. This allows us to do that.
			float DenominatorDistanceUnits() {return m_fltDenominatorDistanceUnits;};

			float MassUnits() {return m_fltMassUnits;};
			void MassUnits(float fltVal) {m_fltMassUnits = fltVal;};
			float InverseMassUnits() {return m_fltInverseMassUnits;};
			//The editor will save out 1 Kg as 1000. So we need to convert 1000 to 1. We use this
			//density mass unit value to do this.
			float DensityMassUnits() {return m_fltDensityMassUnits;};

			BOOL SimulateHydrodynamics() {return m_bSimulateHydrodynamics;};
			void SimulateHydrodynamics(BOOL bVal) {m_bSimulateHydrodynamics = bVal;};

			float FluidDensity() {return m_fltFluidDensity;};
			void FluidDensity(float fltVal) {m_fltFluidDensity = fltVal;};

			BOOL IsInWater(float fltHeight);

			float MaxHydroForce() {return m_fltMaxHydroForce;};
			void MaxHydroForce(float fltVal) {m_fltMaxHydroForce = fltVal;};

			float MaxHydroTorque() {return m_fltMaxHydroTorque;};
			void MaxHydroTorque(float fltVal) {m_fltMaxHydroTorque = fltVal;};

			BOOL HasConvexMesh() {return m_bHasConvexMesh;};
			void HasConvexMesh(BOOL bVal) {m_bHasConvexMesh = bVal;};

			BOOL HasTriangleMesh() {return m_bHasTriangleMesh;};
			void HasTriangleMesh(BOOL bVal) {m_bHasTriangleMesh = bVal;};

			BOOL HasHeightField() {return m_bHasHeightField;};
			void HasHeightField(BOOL bVal) {m_bHasHeightField = bVal;};

			virtual KeyFrame *VideoRecorder() {return m_lpVideoRecorder;};
			virtual void VideoRecorder(KeyFrame *lpFrame) {m_lpVideoRecorder = lpFrame;};

			virtual KeyFrame *VideoPlayback() {return m_lpVideoPlayback;};
			virtual void VideoPlayback(KeyFrame *lpFrame) {m_lpVideoPlayback = lpFrame;};

			string LookAtStructureID() {return m_strLookAtStructureID;};
			string LookAtBodyID() {return m_strLookAtBodyID;};
			BOOL TrackCamera() {return m_bTrackCamera;};
			virtual void TrackCamera(BOOL bTrackCamera, string strLookAtStructureID, string strLookAtBodyID);

			BOOL EnableSimRecording() {return m_bEnableSimRecording;};
			void EnableSimRecording(BOOL bVal) {m_bEnableSimRecording = bVal;};

			long SnapshotByteSize() {return m_lSnapshotByteSize;};

			DataChartMgr *DataChartMgr() {return &m_oDataChartMgr;};
			ExternalStimuliMgr *ExternalStimuliMgr() {return &m_oExternalStimuliMgr;};
			SimulationRecorder *SimulationRecorder() {return m_lpSimRecorder;};

			ManagedCallbackPtr UpdateDataCallback() {return m_lpUpdateDataCallback;};
			void UpdateDataCallback(ManagedCallbackPtr lpFunc) {m_lpUpdateDataCallback = lpFunc;};

			ManagedCallbackPtr StartSimulationCallback() {return m_lpStartSimulationCallback;};
			void StartSimulationCallback(ManagedCallbackPtr lpFunc) {m_lpStartSimulationCallback = lpFunc;};

			ManagedCallbackPtr PauseSimulationCallback() {return m_lpPauseSimulationCallback;};
			void PauseSimulationCallback(ManagedCallbackPtr lpFunc) {m_lpPauseSimulationCallback = lpFunc;};

			ManagedCallbackPtr EndingSimulationCallback() {return m_lpEndingSimulationCallback;};
			void EndingSimulationCallback(ManagedCallbackPtr lpFunc) {m_lpEndingSimulationCallback = lpFunc;};

			void *ManagedInstance() {return m_lpManagedInstance;};
			void ManagedInstance(void *lpVal) {m_lpManagedInstance = lpVal;};

			//NeuralModule methods.
			IStdClassFactory *FindNeuralModuleFactory(string strModuleName, BOOL bThrowError = FALSE);
			void AddNeuralModuleFactory(string strModuleName, NeuralModule *lpModule);

			void AttachSourceAdapter(Structure *lpStructure, Adapter *lpAdapter);
			void AttachTargetAdapter(Structure *lpStructure, Adapter *lpAdapter);

			//ClassFactory methods.
			//virtual CStdSerialize *CreateObject(string strModule, string strClassName, unsigned char iDataType, BOOL bThrowError = TRUE);
			virtual CStdSerialize *CreateObject(string strModule, string strClassName, string strType, BOOL bThrowError = TRUE);
			//virtual BOOL StringToType(string strModule, string strClassName, string strType, unsigned char &iType, BOOL bThrowError = TRUE);
			//virtual BOOL TypeToString(string strModule, string strClassName, unsigned char iType, string &strType, BOOL bThrowError = TRUE);
			//virtual BOOL IsValidType(string strModule, string strClassName, string strType);

			//Main Simulator methods
			virtual void Reset();
			virtual void ResetStructures();
			virtual void Initialize(int argc, const char **argv) = 0;
			virtual void StepSimulation();
			virtual void Simulate() = 0;
			virtual void ShutdownSimulation() = 0;
			virtual void TogglePauseSimulation() = 0;
			virtual void StopSimulation() = 0;
			virtual BOOL StartSimulation() = 0;
			virtual BOOL PauseSimulation() = 0;
			virtual void RunSimulation();
			virtual void Load(string strFileName = "");
			virtual void Load(CStdXml &oXml);

			static IStdClassFactory *LoadClassFactory(string strModuleName);
			static Simulator *CreateSimulator(string strSimulationFile);
			static Simulator *CreateSimulator(CStdXml &oXml);
			static Simulator *CreateSimulator(int argc, const char **argv);
			
			Organism *FindOrganism(string strOrganismID, BOOL bThrowError = TRUE);
			Structure *FindStructure(string strStructureID, BOOL bThrowError = TRUE);
			Structure *FindStructureFromAll(string strStructureID, BOOL bThrowError = TRUE);
			Joint *FindJoint(string strStructureID, string strJointID, BOOL bThrowError = TRUE);
			RigidBody *FindRigidBody(string strStructureID, string strBodyID, BOOL bThrowError = TRUE);
			OdorType *FindOdorType(string strOdorID, BOOL bThrowError = TRUE);
			void AddFoodSource(RigidBody *lpFood);
			RigidBody *FindClosestFoodSource(CStdFPoint &oMouthPos, float fltMinRadius);

			virtual void EnableCollision(RigidBody *lpBody);
			virtual void DisableCollision(RigidBody *lpBody);

			virtual void EnableVideoPlayback(string strKeyFrameID);
			virtual void DisableVideoPlayback();
			virtual void StartVideoPlayback();
			virtual void StopVideoPlayback();
			virtual void StepVideoPlayback(int iFrameCount = 1);
			virtual void SaveVideo(string strPath);

			virtual string AddKeyFrame(string strType, long lStart, long lEnd);
			virtual void RemoveKeyFrame(string strID);
			virtual string MoveKeyFrame(string strID, long lStart, long lEnd);
			virtual void MoveSimulationToKeyFrame(string strKeyFrameID);
			virtual void SaveKeyFrameSnapshot(byte *aryBytes, long &lIndex);
			virtual void LoadKeyFrameSnapshot(byte *aryBytes, long &lIndex);

			//Simulator overrides
			//virtual	CNlClassFactory *NeuralClassFactory();
 			//virtual CNlNeuron *GetNeuron(string strOrganismID, short iXPos, 
			//														short iYPos, short iZPos);
			virtual BOOL IsPhysicsBeingUpdated();	
		};

}			//AnimatLibrary

#endif // !defined(AFX_ALSIMULATOR_H__2CAD431A_1D6E_446F_90CD_728509081187__INCLUDED_)
