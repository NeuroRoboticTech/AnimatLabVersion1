#ifndef __ANIMAT_CONSTANTS_H__
#define __ANIMAT_CONSTANTS_H__

#define Al_Err_lInvalidPartType -3000
#define Al_Err_strInvalidPartType "Invalid body part type specified."

#define Al_Err_lInvalidJointType -3001
#define Al_Err_strInvalidJointType "Invalid body joint type specified."

#define Al_Err_lLayoutBlank -3002
#define Al_Err_strLayoutBlank "The body plan file name is blank."

#define Al_Err_lNeuralNetworkBlank -3003
#define Al_Err_strNeuralNetworkBlank "The neural network file name is blank."

#define Al_Err_lOrganismIDBlank -3004
#define Al_Err_strOrganismIDBlank "The organism ID is blank."

#define Al_Err_lProjectPathBlank -3005
#define Al_Err_strProjectPathBlank "The project path is blank."

#define Al_Err_lParentNotDefined -3006
#define Al_Err_strParentNotDefined "The parent part is not defined."

#define Al_Err_lChildNotDefined -3007
#define Al_Err_strChildNotDefined "The child part is not defined."

#define Al_Err_lRenderContextNotDefined -3008
#define Al_Err_strRenderContextNotDefined "The render context is not defined."

#define Al_Err_lSimNotDefined -3008
#define Al_Err_strSimNotDefined "The simulation is not defined."

#define Al_Err_lJointParentMismatch -3009
#define Al_Err_strJointParentMismatch "A parent part is defined for this part, but no joint is defined between them!"

#define Al_Err_lJointIDNotFound -3010
#define Al_Err_strJointIDNotFound "The specified joint was not found."

#define Al_Err_lBodyNotDefined -3011
#define Al_Err_strBodyNotDefined "The rigid body was not defined."

#define Al_Err_lOrganismIDNotFound -3012
#define Al_Err_strOrganismIDNotFound "The specified organism was not found."

#define Al_Err_lLimitNotDefined -3013
#define Al_Err_strLimitNotDefined "The specified constraint limit was not defined."

#define Al_Err_lClassFactoryNotDefined -3014
#define Al_Err_strClassFactoryNotDefined "The class factory was not defined."

#define Al_Err_lInvalidOrganismType -3015
#define Al_Err_strInvalidOrganismType "An invalid organism type was specified."

#define Al_Err_lForceLessThanZero -3016
#define Al_Err_strForceLessThanZero "The specified muscle force was less than zero."

#define Al_Err_lPartNotAttachment -3017
#define Al_Err_strPartNotAttachment "The body part was not a muscle attachment part type."

#define Al_Err_lRigidBodyIDNotFound -3018
#define Al_Err_strRigidBodyIDNotFound "The specified rigid body was not found."

#define Al_Err_lStructureIDBlank -3019
#define Al_Err_strStructureIDBlank "The structure ID is blank."

#define Al_Err_lStructureIDNotFound -3020
#define Al_Err_strStructureIDNotFound "The specified structure was not found."

#define Al_Err_lJointNotDefined -3021
#define Al_Err_strJointNotDefined "The joint was not defined."

#define Al_Err_lStructureNotDefined -3022
#define Al_Err_strStructureNotDefined "Structure not defined."

#define Al_Err_lUnableToCastBodyToDesiredType -3023
#define Al_Err_strUnableToCastBodyToDesiredType "Unable to cast rigid body to desired type."

#define Al_Err_lMuscleNotFound -3024
#define Al_Err_strMuscleNotFound "The specified muscle was not found."

#define Al_Err_lUnableToCastSimToDesiredType -3025
#define Al_Err_strUnableToCastSimToDesiredType "Unable to cast simulator to desired type."

#define Al_Err_lInvalidSurceContactCount -3026
#define Al_Err_strInvalidSurceContactCount "Invalid surface contact count. You are attempting to reset the contact count below zero."

#define Al_Err_lMustBeContactBodyToGetCount -3027
#define Al_Err_strMustBeContactBodyToGetCount "The specified rigid body must be a contact sensor in order to get the surface contact count data."

#define Al_Err_lUnableToCastJointToDesiredType -3028
#define Al_Err_strUnableToCastJointToDesiredType "Unable to cast joint to desired type."

#define Al_Err_lBodyOrJointIDNotFound -3028
#define Al_Err_strBodyOrJointIDNotFound "No body or joint was found with the specified ID."

#define Al_Err_lParentHwndNotDefined -3029
#define Al_Err_strParentHwndNotDefined "No Parent HWND was defined."

#define Al_Err_lSimFileBlank -3030
#define Al_Err_strSimFileBlank "The simulation file name is blank."

#define Al_Err_lModuleFactoryNotNeural -3031
#define Al_Err_strModuleFactoryNotNeural "The class factory loaded from the specified module is not a neural class factory type."

#define Al_Err_lModuleFactoryNotAnimat -3032
#define Al_Err_strModuleFactoryNotAnimat "The class factory loaded from the specified module is not an animat class factory type."

#define Al_Err_lNoProjectParamOnCommandLine -3033
#define Al_Err_strNoProjectParamOnCommandLine "There was no project parameter defined on the command line."

#define Al_Err_lInvalidSimulatorType -3034
#define Al_Err_strInvalidSimulatorType "Invalid simulator type."

#define Al_Err_lFullPathNotSpecified -3035
#define Al_Err_strFullPathNotSpecified "A full path to the project simulation file was not given."

#define Err_lSimulationWndNotDefined -3036
#define Err_strSimulationWndNotDefined "A full path to the project simulation file was not given."

#define Al_Err_lItemNotKeyFrameType -3037
#define Al_Err_strItemNotKeyFrameType "The activated item is not a key frame type of object."

#define Al_Err_lInvalidKeyFrameType -3038
#define Al_Err_strInvalidKeyFrameType "Invalid Keyframe type."

#define Al_Err_lNoRecorderDefined -3039
#define Al_Err_strNoRecorderDefined "You can not perform actions on recorder keyframes when no simulation recorder has been defined."

#define Al_Err_lKeyFrameOverlap -3040
#define Al_Err_strKeyFrameOverlap "The keyframe with the following ID overlaps with this keyframe."

#define Al_Err_lOpNotDefinedForAdapter -3041
#define Al_Err_strOpNotDefinedForAdapter "The following opertion is not defined for an adapter object."

#define Al_Err_lModuleNameBlank -3042
#define Al_Err_strModuleNameBlank "The name for the following module is blank."

#define Al_Err_lDataTypeBlank -3043
#define Al_Err_strDataTypeBlank "The data type is blank."

#define Al_Err_lConvertingClassToType -3044
#define Al_Err_strConvertingClassToType "A failure occured while trying to convert a newly created object from the class factory to the type specified."

#define Al_Err_lModuleNameNotFound -3044
#define Al_Err_strModuleNameNotFound "The specified neural module name was not found in the list of modules."

#define Al_Err_lNeuralModuleNotDefined -3045
#define Al_Err_strNeuralModuleNotDefined "The neural module was not define."

#define Al_Err_lInvalidAdapterType -3046
#define Al_Err_strInvalidAdapterType "Invalid adapter type."

#define Al_Err_lInvalidGainType -3046
#define Al_Err_strInvalidGainType "Invalid gain type."

#define Al_Err_lBodyTypeBlank -3047
#define Al_Err_strBodyTypeBlank "The body type is blank."

#define Al_Err_lBodyIDBlank -3047
#define Al_Err_strBodyIDBlank "The body ID is blank."

#define Al_Err_lJointIDBlank -3048
#define Al_Err_strJointIDBlank "The joint ID is blank."

#define Al_Err_lModuleClassFactoryNotDefined -3048
#define Al_Err_strModuleClassFactoryNotDefined "The class factory for the specified neural module is not defined."

#define Al_Err_lSimulationNotDefined -3024
#define Al_Err_strSimulationNotDefined "Simulation has not been defined."

#define Al_Err_lInvalidDataType -3022
#define Al_Err_strInvalidDataType "Invalid data type."

#define Al_Err_lIDBlank -3020
#define Al_Err_strIDBlank "ID is blank."

#define Al_Err_lItemNotStimulusType -3028
#define Al_Err_strItemNotStimulusType "Activated item is not an external stimulus type."

#define Al_Err_lItemNotDataChartType -3026
#define Al_Err_strItemNotDataChartType "The activated object is not a data chart type."

#define Al_Err_lExceededMaxBuffer -3025
#define Al_Err_strExceededMaxBuffer "Data Buffer size has been exceeded."

#define Al_Err_lInvalidDataColumnType -3021
#define Al_Err_strInvalidDataColumnType "Invalid data column type."

#define Al_Err_lFilenameBlank -3019
#define Al_Err_strFilenameBlank "Filename is blank."

#define Al_Err_lActivatedItemIDNotFound -3031
#define Al_Err_strActivatedItemIDNotFound "The specified activated item was not found."

#define Al_Err_lInvalidExternalStimulusType -3027
#define Al_Err_strInvalidExternalStimulusType "Invalid external stimulus type."

#define Al_Err_lInvalidDataChartType -3023
#define Al_Err_strInvalidDataChartType "Invalid data chart type."

#define Al_Err_lInvalidNeuralModuleType -3032
#define Al_Err_strInvalidNeuralModuleType "An invalid neural module type was specified."

#define Al_Err_lInvalidMassUnits -3035
#define Al_Err_strInvalidMassUnits "Invalid data column type."

#define Al_Err_lInvalidDistanceUnits -3036
#define Al_Err_strInvalidDistanceUnits "Invalid data column type."

#define Al_Err_lNeuralModuleNameBlank -3037
#define Al_Err_strNeuralModuleNameBlank "The neural module name is blank."

#define Al_Err_lMuscleRestPercExceed100 -3038
#define Al_Err_strMuscleRestPercExceed100 "The total resting length for the elements of the muscle exceed 100%."

#define Al_Err_lMuscleRestPercLessThan100 -3039
#define Al_Err_strMuscleRestPercLessThan100 "The total resting length for the elements of the muscle not be less than 100%."

#define Al_Err_lTLCurveEndPointsNotZero -3040
#define Al_Err_strTLCurveEndPointsNotZero "The y values for the end points of the tension length curve must be 0."

#define Al_Err_lObtainingCriticalSection -3050
#define Al_Err_strObtainingCriticalSection "Unable to obtain a critical section for the memory data chart."

#define Al_Err_lDataColumnIDNotFound -3051
#define Al_Err_strDataColumnIDNotFound "The specified data column was not found."

#define Al_Err_lInvalidPlaybackRate -3052
#define Al_Err_strInvalidPlaybackRate "An invalid playback rate was specified."

#define Al_Err_lActivatedItemNull -3053
#define Al_Err_strActivatedItemNull "The activated item is null."

#define Al_Err_lDataPointNotFound -3054
#define Al_Err_strDataPointNotFound "The data pointer for the following type was not found."

#define Al_Err_lVortexSimulationError -3055
#define Al_Err_strVortexSimulationError "An error occured in the vortex simulator."

#define Al_Err_lInvalidConeRadius -3056
#define Al_Err_strInvalidConeRadius "Both the upper and lower radius of a cone can not be zero."

#define Al_Err_lNeedTwoMuscleAttachments -3057
#define Al_Err_strNeedTwoMuscleAttachments "At least two muscle attachments are required."

#define Al_Err_lNoVerticesDefined -3058
#define Al_Err_strNoVerticesDefined "No vertices have been defined for this body."

#define Al_Err_lGraphicsMeshNotDefined -3059
#define Al_Err_strGraphicsMeshNotDefined "No graphics mesh was defined for this body."

#define Al_Err_lInvalidCollisionMeshType -3060
#define Al_Err_strInvalidCollisionMeshType "An invalid collision mesh type was specified."

#define Al_Err_lReceptiveFieldVertexNotFound -3061
#define Al_Err_strReceptiveFieldVertexNotFound "The specified receptive field index was not found."

#define Al_Err_lOdorNotDefined -3062
#define Al_Err_strOdorNotDefined "The odor was not defined."

#define Al_Err_lOdorIDNotFound -3063
#define Al_Err_strOdorIDNotFound "The specified odor ID was not found."

#define Al_Err_lUnableToConvertToCeSimulator -3064
#define Al_Err_strUnableToConvertToCeSimulator "Unable to convert simulator pointer to CeSimulator type."

#define Al_Err_lKeyFramNotDefinedForCE -3065
#define Al_Err_strKeyFramNotDefinedForCE "Keyframes are not defined for the CeSimulator type."

#define Al_Err_lInvalidMicrocontrollerType -3066
#define Al_Err_strInvalidMicrocontrollerType "An invalid microcontroller type was specified."

#define Al_Err_lOpenCommPort -3067
#define Al_Err_strOpenCommPort "Unable to open the comm port for communications."

#define Al_Err_lGetCommState -3068
#define Al_Err_strGetCommState "Unable to get the comm state of the serial port."

#define Al_Err_lSetCommState -3069
#define Al_Err_strSetCommState "Unable to set the comm state of the serial port."

#define Al_Err_lUnableToCreateProcessThread -3070
#define Al_Err_strUnableToCreateProcessThread "Unable to create the processing IO thread for the microcontroller."

#define Al_Err_lGraphicsMeshNoExtension -3071
#define Al_Err_strGraphicsMeshNoExtension "No extension was defined for the graphics mesh."

#define Al_Err_lInvalidSpringForceType -3072
#define Al_Err_strInvalidSpringForceType "Invalid spring force type."

#define Al_Err_lInvalidItemType -3073
#define Al_Err_strInvalidItemType "Invalid Data Item Type."

#define Al_Err_lInvalidMusc_Vel_Avg -3073
#define Al_Err_strInvalidMusc_Vel_Avg "Invalid average muscle velocity count value."

#define MAX_DATA_CHART_BUFFER 10485760

#define PI 3.14159
#define PI_2 6.28319
#define ERROR_THRESHOLD 0.0001

#define AL_INFINITY ((float) 0x7fffffff)

//Spring Force Types
#define AL_SPRING_COMPRESSION_ONLY 0
#define AL_SPRING_EXTENSION_ONLY 1
#define AL_SPRING_BOTH 2

#define AL_TONIC_CURRENT 0
#define AL_REPETITIVE_CURRENT 1
#define AL_BURST_CURRENT 2

#endif // __ANIMAT_CONSTANTS_H__
