/* -*- mode: C++; -*- */

/*
   Copyright (c) 2000-2002 CMLabs Simulations Inc. 
   
   http://www.cm-labs.com

   $Name:  $
   
   Date: $Date: 2004/07/09 16:14:20 $
   
   This software and its accompanying manuals have been developed by CMLabs
   Simulations Inc. ("CMLabs") and MathEngine PLC ("MathEngine").  The
   copyright to the Vortex Toolkits and all associated materials belongs to
   CMLabs.
   
   All intellectual property rights in the software belong to CMLabs,
   except for portions that have been licensed from MathEngine.

   All rights conferred by law (including rights under international
   copyright conventions) are reserved to CMLabs. This software may also
   incorporate information which is confidential to CMLabs.

   Save to the extent permitted by law, or as otherwise expressly permitted
   by CMLabs, this software and the manuals must not be copied (in whole or
   in part), re-arranged, altered or adapted in any way without the prior
   written consent of CMLabs. In addition, the information contained in the
   software may not be disseminated without the prior written consent of
   CMLabs.

 */

ERR(kMcdErrorNum_GenericFatal, kMcdErrorTypeFatal, \
    ""),
ERR(kMcdErrorNum_GenericWarning, kMcdErrorTypeWarning, \
    ""),
ERR(kMcdErrorNum_EvalTimedOut, kMcdErrorTypeFatal, \
    "Your evaluation version of the Collision library has timed out.\n" \
    "Please contact Critical Mass Labs for a full development version.\n"),
ERR(kMcdErrorNum_InvalidPtr, kMcdErrorTypeFatal, \
    "Function called with null pointer as argument "),
ERR(kMcdErrorNum_BadCast, kMcdErrorTypeFatal, \
    "object not of correct derived type: expecting object of type "),
ERR(kMcdErrorNum_AllocFailure, kMcdErrorTypeFatal, \
    "Allocation failed for object of type: "),
ERR(kMcdErrorNum_AssertionFailed, kMcdErrorTypeFatal, \
    "Assertion failed: "),
ERR(kMcdErrorNum_BadSetTransInBridge, kMcdErrorTypeWarning, \
    "Invalid use of McdModelSetTransform(),: This McdModel's transform is automatically updated from a MdtBody transform "),
ERR(kMcdErrorNum_InvalidPtr_CM, kMcdErrorTypeFatal, \
    "Invalid McdModelID."),
ERR(kMcdErrorNum_InvalidPtr_Space, kMcdErrorTypeFatal, \
    "Invalid McdSpaceID."),
ERR(kMcdErrorNum_InvalidPtr_Geometry, kMcdErrorTypeFatal, \
    "Invalid McdGeometryID."),
ERR(kMcdErrorNum_InvalidPtr_SPID, kMcdErrorTypeFatal, \
    "Invalid McdSpacePairID."),
ERR(kMcdErrorNum_InvalidPtr_SP, kMcdErrorTypeFatal, \
    "Invalid McdModelPair pointer."),
ERR(kMcdErrorNum_InvalidPtr_IR, kMcdErrorTypeFatal, \
    "Invalid McdIntersectResult pointer."),
ERR(kMcdErrorNum_InvalidPair, kMcdErrorTypeFatal, \
    "McdModelPair contains null model pointer(s),."),
ERR(kMcdErrorNum_InvalidPtr_CDDT, kMcdErrorTypeFatal, \
    "Invalid McdDtBridgeID."),
ERR(kMcdErrorNum_InvalidPtr_RWW, kMcdErrorTypeFatal, \
    "Invalid McdRwWorldID."),
ERR(kMcdErrorNum_InvalidPtr_RWBSP, kMcdErrorTypeFatal, \
    "Invalid McdRwBSPID."),
ERR(kMcdErrorNum_InvalidPtr_TM, kMcdErrorTypeFatal, \
    "Expected non null transform pointer."),
ERR(kMcdErrorNum_InvalidFuncOrder_Space, kMcdErrorTypeFatal, \
    "SpaceUpdate called on unbuilt space.  Invoke SpaceBuild(),"),
ERR(kMcdErrorNum_InvalidFuncOrder_Bridge, kMcdErrorTypeFatal, \
    "Attempt to SetBody(), without Initializing dynamics bridge (BridgeInit(),),"),
ERR(kMcdErrorNum_InvalidGeoType, kMcdErrorTypeFatal, \
    "Failed to register new geometry type. Ensure sufficient number of geometries in McdInit()."),
ERR(kMcdErrorNum_InvalidIR, kMcdErrorTypeFatal, \
    "Invalid Intersect Result. Output contact array pointer must exist."),
ERR(kMcdErrorNum_InvalidRequest, kMcdErrorTypeFatal, \
    "Invalid Intersect Request (%s),."),

ERR(kMcdErrorNum_Mem_SpaceModelPairPool, kMcdErrorTypeFatal, \
    "Out of memory for McdModelPairs in McdSpace. Resizing not implemented. "
    "Please use a larger number of pairs in McdAxisSortCreate."),
ERR(kMcdErrorNum_Mem_SpaceModelPool, kMcdErrorTypeFatal, \
    "Out of memory for McdModels in McdSpace. Resizing not implemented. "
    "Please use a larger number of models in McdAxisSortCreate."),

/* WARNINGS: */
/* Out of (pool), memory */
ERR(kMcdErrorNum_Mem_PairData, kMcdErrorTypeWarning, \
    "Ran out of memory for PairData objects."),
ERR(kMcdErrorNum_contactBufferOverrun, kMcdErrorTypeWarning, \
    "Internal error: Ran out of contact in pool, "),
ERR(kMcdErrorNum_TriangleArrayOverrun, kMcdErrorTypeWarning, \
    "No memory for adding more triangles - McdTriangleMeshAddTriangle(),"),
ERR(kMcdErrorNum_DegenerateTriangle, kMcdErrorTypeInfo, \
    "McdTriangleMeshAddTriangle(): degenerate triangle ignored! "),
ERR(kMcdErrorNum_TriangleMeshNoTriangles, kMcdErrorTypeWarning, \
    "TriangleMesh has no triangles, cannot build triangle mesh."),
ERR(kMcdErrorNum_TriangleMeshNotBuilt, kMcdErrorTypeFatal, \
    "TriangleMesh is not built - the function McdTriangleMeshBuild(mesh), shoud be called."),
ERR(kMcdErrorNum_NoIntersectionFn, kMcdErrorTypeWarning, \
    "No intersection function available for this pair combination. Pair will be ignored. This could also be due to an invalid Request for the given pair: "),

ERR(McdErrorNum_InvalidRequest, kMcdErrorTypeWarning, \
    "Invalid Request "),
ERR(McdErrorNum_RegisterInteractionUnknownGeom, kMcdErrorTypeWarning, \
    "Attempt to register an interaction involving an unregistered geometry type. Interaction ignored. Pair combination was "),

ERR(McdErrorNum_FindInteractionUnknownGeom, kMcdErrorTypeWarning, \
    "Attempt to find an interaction involving an unregistered geometry type. Empty interaction will be used for debug version, but release version will produce undefined behaviour."),

ERR(McdErrorNum_SafeTimeUnimplementedCase, kMcdErrorTypeWarning, \
    "Unimplemented case in McdSafeTime in "),
ERR(McdErrorNum_RegisterInteractionWithInvalidGeometryType,
    kMcdErrorTypeWarning, \
    "Attempt to register an interaction involving an invalid geometry type. Failure to register geometry type before registering interactions?"),
ERR(kMcdErrorNum_SpaceNotChanging, kMcdErrorTypeWarning, \
    "Attempt to use a state-modification function while McdSpaceIsChanging()"
    " is false. Call McdSpaceBeginChanges() first. Function name: "),
ERR(kMcdErrorNum_SpaceIsChanging, kMcdErrorTypeWarning, \
    "Attempt to use a state-query function while McdSpaceIsChanging()"
    " is true. Call McdSpaceEndChanges() first. Function name: "),

ERR(kMcdErrorNum_DestroyModelInSpace, kMcdErrorTypeWarning, \
    "McdModelDestroy: model being destroyed while still in an McdSpace."),

ERR(kMcdErrorNum_boxBoxInvalid, kMcdErrorTypeWarning, \
    "McdBoxBox: internal error due to invalid transform."),

ERR(kMcdErrorNum_InertiaTensorInvalid, kMcdErrorTypeWarning, \
    "McdGeometryGetMassProperties: Inertia tensor matrix is non-symmetric."),

ERR(kMcdErrorNum_McdFrameworkNULL, kMcdErrorTypeWarning, \
    "McdTerm: McdMcdFramework already terminated."),

/* This has to be last: due MAXErrDefined*/
ERR(kMcdMAXErrDefined, kMcdErrorTypeFatal, "No error")
