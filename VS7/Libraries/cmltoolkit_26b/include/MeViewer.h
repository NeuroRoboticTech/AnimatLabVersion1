#ifndef _MEVIEWER_H
#define _MEVIEWER_H
/* -*- mode: C; -*- */

/*
   Copyright (c) 2000-2004 CMLabs Simulations Inc.
   
   http://www.cm-labs.com

   $Name:  $
   
   Date: $Date: 2005/05/03 17:36:51 $
   
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

/* #define MOVIE */

#include "MeViewerTypes.h"
#include "MeMessage.h"
#include "MeMath.h"
#include "MeSimpleFile.h"
#include "MeProfile.h"
#include "MeCommandLine.h"
#include <windows.h>

#ifdef __cplusplus
extern "C"
{
#endif
void              MEAPI RParseText(RRender *rc, char *text_in, char *text_out,
                                   int outbuffersize);

int               MEAPI RBmpLoad(RRender* rc, char *filename, RImage *p_image,
                                 int bRequireBGR);
int               MEAPI RBmpLoadWithTransparency(RRender* rc, char *filename, 
                                 RImage *p_image, int bRequireBGR, int transparencyR, 
                                 int transparencyG, int transparencyB);

/* RRender */
RRender *         MEAPI RRenderContextCreate(MeCommandLineOptions* options, MeCommandLineOptions* overrideoptions, int eat);
RRender *         MEAPI RRenderContextCreateWithParent(MeCommandLineOptions* options, MeCommandLineOptions* overrideoptions, int eat, HWND hWndParent);
void              MEAPI RRenderSetUpAxisAndHandedness(RRender *rc, int upAxisZ, int righthanded);
int               MEAPI RRenderContextDestroy( RRender *rc );
long							MEAPI RRenderWndProc(HWND win, UINT msg, WPARAM wparam, LPARAM lparam);
void              MEAPI RRenderRun(RRender* rc, RMainLoopCallBack func, void *userdata);
void							MEAPI RRenderRunInit(RRender* rc,  RMainLoopCallBack func, void *userdata);
BOOL							MEAPI RRenderStep(RRender* rc);
void							MEAPI RRenderRunCleanup(RRender* rc);

void              MEAPI RRenderQuit( RRender *rc );
void              MEAPI RRenderSetPause( RRender *rc, int p);
void              MEAPI RRenderUpdateProjectionMatrices( RRender *rc );
void              MEAPI RRenderUpdateGraphicMatrices( RRender *rc );

int               MEAPI RRenderTextureCreate(RRender *rc, const char *filename);
int               MEAPI RRenderTextureCreateFromRGBA(RRender *rc, const char *filename, 
                                                     unsigned char *rgbaBuffer, int width, int height);

void              MEAPI RRenderToggleUserHelp( RRender *rc );
void              MEAPI RRenderCreateUserHelp( RRender *rc, char *help[],
                                              int arraySize );
void							MEAPI RRenderToggleFrameLock( RRender *rc );
void							MEAPI RRenderSetFrameLockType( RRender *rc, unsigned int iVal );
void							MEAPI RRenderSetFrameLockRate(RRender *rc, unsigned int iRate);
void							MEAPI RRenderSetDisplayFps(RRender *rc, int bVal);
void							MEAPI RRenderSetAlphaBlend3D(RRender *rc, int bVal);

void              MEAPI RRenderSkydomeCreate(RRender *const rc, const char * name,
                                             int tileU, int tileV);

void MEAPI RRenderSetAppName(RRender* rc, const char* appname);

/* RGraphic */
RGraphic *        MEAPI RGraphicCreate(RRender *rc, char *filename,
                                       AcmeReal xScale,
                                       AcmeReal yScale,
                                       AcmeReal zScale,
                                       const float color[4],
                                       MeMatrix4Ptr matrix,
                                       MeBool is2D, MeBool bKeepAspectRatio);

RGraphic *        MEAPI RGraphicCreateEmpty( int numVertices );
RGraphic *        MEAPI RGraphicProceduralCreate(RRender *rc, RUserDrawFn userFn, void* userData, 
                                                 const float color[4], MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicLoadMEG( char *filename );

RGraphic *        MEAPI RGraphicLoadASE(RRender *rc, char* filename,
                                        AcmeReal xScale,
                                        AcmeReal yScale,
                                        AcmeReal zScale,
                                        const float color[4],
                                        MeMatrix4Ptr matrix);

int               MEAPI RGraphicSave( RGraphic *rg, char *filename );
int								MEAPI RGraphicDumpVertices( RGraphic *rg, char *filename );

void              MEAPI RGraphicDestroy(RGraphic *rg);
void              MEAPI RGraphicDelete(RRender *rc, RGraphic *rg, int is2D);

AcmeReal          MEAPI RGetRenderingTime(RRender* rc);
AcmeReal          MEAPI RGetIdleTime(RRender* rc);

/*must call after object is created (sorts by alpha) */
void              MEAPI RGraphicAddToList(RRender *rc, RGraphic *rg, int is2D);
void              MEAPI RGraphicRemoveFromList(RRender *rc, RGraphic *rg,
                                               int is2D);
void              MEAPI RGraphicAddToByTypeList( RRender *rc, RGraphic *gr, RGraphicType_enum type );
void              MEAPI RGraphicRemoveFromByTypeList( RRender *rc, RGraphic *gr );
int               MEAPI RGraphicSetTexture(RRender *rc, RGraphic *rg,
                                           const char *filename);
int               MEAPI RGraphicSetTextureFromRGBA(RRender *rc, RGraphic *rg, unsigned char *rgbaBuffer, int width, int height);

void              MEAPI RGraphicSetTextureTransparencyColor(RRender *rc, 
                                           int textureID, int r, int g, int b);

/* called by RGraphicLoad */
void              MEAPI RGraphicFillObjectBuffer( char *filename );
void              MEAPI RGraphicNormalize( RGraphic *rg, int bKeepAspectRatio);
void              MEAPI RGraphicScale(RGraphic *rg, AcmeReal xScale,
                                      AcmeReal yScale, AcmeReal zScale);

/* Primitives creation */
RGraphic *        MEAPI RGraphic2DRectangleCreate(RRender *rc,
                                                  AcmeReal orig_x,
                                                  AcmeReal orig_y,
                                                  AcmeReal width,
                                                  AcmeReal height,
                                                  const float color[4]);

RGraphic *        MEAPI RGraphicBoxCreate(RRender *rc, AcmeReal width,
                                          AcmeReal height, AcmeReal depth,
                                          const float color[4],
                                          MeMatrix4Ptr matrix);

RGraphic *				MEAPI RGraphicConeCreate_New(RRender *rc, AcmeReal lower_radius,
																					 AcmeReal upper_radius, AcmeReal height,
																					 const float color[4], MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicConeCreate(RRender *rc, AcmeReal radius,
                                           AcmeReal upper_height,
                                           AcmeReal lower_height,
                                           const float color[4],
                                           MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicCylinderCreate(RRender *rc, AcmeReal radius,
                                               AcmeReal height,
                                               const float color[4],
                                               MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicCapsuleCreate(RRender *rc, AcmeReal radius,
                                               AcmeReal length,
                                               const float color[4],
                                               MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicGroundPlaneCreate(RRender *rc,
                                                  AcmeReal side_length,
                                                  int triangles_per_side,
                                                  const float color[4],
                                                  AcmeReal y_position );

RGraphic *        MEAPI RGraphicLineCreate(RRender *rc, AcmeReal origin[3],
                                           AcmeReal end[3],
                                           const float color[4],
                                           MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicSphereCreate(RRender *rc,
                                             AcmeReal radius,
                                             const float color[4],
                                             MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicSquareCreate(RRender *rc,
                                             AcmeReal side,
                                             const float color[4],
                                             MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicTextCreate(RRender *rc,  char *text,
                                           AcmeReal orig_x, AcmeReal orig_y,
                                           const float color[4]);

RGraphic *        MEAPI RGraphicTorusCreate(RRender *rc, AcmeReal innerRadius,
                                            AcmeReal outerRadius,
                                            const float color[4],
                                            MeMatrix4Ptr matrix);

RGraphic *        MEAPI RGraphicFrustumCreate(RRender *rc,
                                              AcmeReal bottomRadius,
                                              AcmeReal topRadius,
                                              AcmeReal bottom,
                                              AcmeReal top, int sides,
                                              const float color[4],
                                              MeMatrix4Ptr matrix);
RGraphic *        MEAPI RGraphicXYRectangleCreate(RRender *rc, AcmeReal sideX, 
                                              AcmeReal sideY, const float color[4], 
                                              MeMatrix4Ptr matrix);
RGraphic *        MEAPI RGraphicBillboardCreate(RRender *rc,AcmeReal width, AcmeReal height, 
                                              const float color[4], MeMatrix4Ptr matrix);
void              MEAPI RBillBoardUpdate(RRender *rc, RGraphic *billboard);
void              MEAPI RBillboardUpdateAll(RRender *rc);



/* Utilities functions for manipulating RGraphics */
int               MEAPI RGraphicLineMoveEnds(RGraphic *lineG,
                                             AcmeReal *origin, AcmeReal *end);
void              MEAPI RGraphicSetTransformPtr(RGraphic *g,
                                                MeMatrix4Ptr matrix);
void              MEAPI RGraphicSetColor(RGraphic *g, const float color[4]);
void              MEAPI RGraphicGetColor(RGraphic *g, float color[4]);
void              MEAPI RConvertTriStripToTriList(RGraphic* rg,
                                                  RObjectVertex* strips,
                                                  int* stripSize,
                                                  int* stripStart,
                                                  int numStrips);
void              MEAPI RGraphicSetWireframe(RGraphic *g, MeBool b);

/* Lights */

void MEAPI RLightSwitchOn(RRender* rc, RRenderLight light);
void MEAPI RLightSwitchOff(RRender* rc, RRenderLight light);

/* Camera functions */
void              MEAPI RCameraGetLookAt(RRender *rc, AcmeVector3 camlookat);
void              MEAPI RCameraGetPosition(RRender *rc, AcmeVector3 pos );
AcmeReal          MEAPI RCameraGetDistance(RRender *rc);
AcmeReal          MEAPI RCameraGetElevation(RRender *rc);
AcmeReal          MEAPI RCameraGetRotation(RRender *rc);

void              MEAPI RCameraGetUp(RRender *rc, AcmeVector3 up);
void              MEAPI RCameraGetMatrix( RRender *rc, MeMatrix4Ptr m );


void              MEAPI RCameraSetElevation(RRender* rc, const MeReal elevation);
void              MEAPI RCameraSetOffset(RRender* rc, const MeReal offset);
void              MEAPI RCameraSetLookAt( RRender *rc, const AcmeVector3 lookAt);
void              MEAPI RCameraSetLookAtAndPosition(RRender *rc,
                                                    const MeVector3 lookAt,
                                                    const MeVector3 position);
void              MEAPI RCameraSetView( RRender *rc, AcmeReal dist,
                                       AcmeReal theta, AcmeReal phi );
void              MEAPI RCameraSetMatrix( RRender *rc, MeMatrix4Ptr m );
void              MEAPI RCameraSetFov( RRender *rc, AcmeReal fov );

void              MEAPI RCameraPanX( RRender *rc, AcmeReal dist );
void              MEAPI RCameraPanY( RRender *rc, AcmeReal dist );
void              MEAPI RCameraPanZ( RRender *rc, AcmeReal dist );
void              MEAPI RCameraRotateAngle( RRender *rc, AcmeReal d_theta );
void              MEAPI RCameraRotateElevation( RRender *rc, AcmeReal d_phi );
void              MEAPI RCameraZoom( RRender *rc, AcmeReal dist );
void              MEAPI RCameraUpdate(RRender *rc);
void							MEAPI RCameraTrackingObject(RRender *rc, MeBool bVal);
void              MEAPI RRenderFindClickDir(RRender *rc, int x, int y, MeVector3 normClickDir);

void							MEAPI RRenderSetWireframe(RRender *rc, MeBool b);

/* Callbacks */
void              MEAPI RRenderSetActionNCallBackWithText(RRender *rc, int N,
                                                  RButtonPressCallBack func,
                                                  void *userdata, char *text);
void              MEAPI RRenderSetActionNKey(RRender* rc, const unsigned int N,
                                             const char key);
void              MEAPI RRenderSetDown2CallBackWithText(RRender *rc,
                                                RButtonPressCallBack func,
                                                void *userdata, char *text );
void              MEAPI RRenderSetDownCallBackWithText(RRender *rc,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );
void              MEAPI RRenderSetJoystickCallBackWithText(RRender *rc,
                                                   RJoystickCallBack func,
                                                   void *userdata, char *text );
void              MEAPI RRenderSetLeft2CallBackWithText(RRender *rc,
                                                RButtonPressCallBack func,
                                                void *userdata, char *text );
void              MEAPI RRenderSetLeftCallBackWithText(RRender *rc,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );
void              MEAPI RRenderSetMouseCallBackWithText(RRender *rc,
                                                RMouseCallBack func,
                                                void *userdata, char *text );
void              MEAPI RRenderSetRight2CallBackWithText(RRender *rc,
                                                 RButtonPressCallBack func,
                                                 void *userdata, char *text );
void              MEAPI RRenderSetRightCallBackWithText(RRender *rc,
                                                RButtonPressCallBack func,
                                                void *userdata, char *text );
void              MEAPI RRenderSetUp2CallBackWithText(RRender *rc,
                                              RButtonPressCallBack func,
                                              void *userdata, char *text );
void              MEAPI RRenderSetUpCallBackWithText(RRender *rc,
                                             RButtonPressCallBack func,
                                             void *userdata, char *text );

/*
void              MEAPI RRenderSetHomeCallBackWithText(RRender *rc,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );
void              MEAPI RRenderSetEndCallBackWithText(RRender *rc,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );
void              MEAPI RRenderSetPageUpCallBackWithText(RRender *rc,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );
void              MEAPI RRenderSetPageDownCallBackWithText(RRender *rc,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );
void              MEAPI RRenderSetInsertCallBackWithText(RRender *rc,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );
void              MEAPI RRenderSetDeleteCallBackWithText(RRender *rc,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );
*/

void              MEAPI RRenderSetKeyCallBackWithText(RRender *rc, int key,
                                               RButtonPressCallBack func,
                                               void *userdata, char *text );



/* For backwards compatability */
#define RRenderSetUpCallBack(rc,f,u)       RRenderSetUpCallBackWithText(rc,f,u,0)
#define RRenderSetDownCallBack(rc,f,u)     RRenderSetDownCallBackWithText(rc,f,u,0)
#define RRenderSetLeftCallBack(rc,f,u)     RRenderSetLeftCallBackWithText(rc,f,u,0)
#define RRenderSetRightCallBack(rc,f,u)    RRenderSetRightCallBackWithText(rc,f,u,0)
#define RRenderSetUp2CallBack(rc,f,u)      RRenderSetUp2CallBackWithText(rc,f,u,0)
#define RRenderSetDown2CallBack(rc,f,u)    RRenderSetDown2CallBackWithText(rc,f,u,0)
#define RRenderSetLeft2CallBack(rc,f,u)    RRenderSetLeft2CallBackWithText(rc,f,u,0)
#define RRenderSetRight2CallBack(rc,f,u)   RRenderSetRight2CallBackWithText(rc,f,u,0)
#define RRenderSetMouseCallBack(rc,f,u)    RRenderSetMouseCallBackWithText(rc,f,u,0)
#define RRenderSetJoystickCallBack(rc,f,u) RRenderSetJoystickCallBackWithText(rc,f,u,0)
#define RRenderSetActionNCallBack(rc,n, f,u)  RRenderSetActionNCallBackWithText(rc, n,f,u,0)
#define RRenderSetKeyCallBack(rc,k, f,u) RRenderSetKeyCallBackWithText(rc,k, f,u,0)
int MEAPI RExecuteKeyCallback(RRender *rc, int key); /* internal */
int MEAPI RGetLastKeyModifiers(RRender *rc);
int MEAPI RGetLastKey(RRender *rc);

/* Performance measurement and UI */
RPerformanceBar * MEAPI RPerformanceBarCreate(RRender *rc);
void              MEAPI RPerformanceBarUpdate(RRender *rc, AcmeReal coltime,
                                              AcmeReal dyntime,
                                              AcmeReal rentime,
                                              AcmeReal idletime);

void              MEAPI RRenderDisplayFps(RRender *rc, AcmeReal fps);
AcmeReal					MEAPI RRenderGetFps(RRender *rc);

void MEAPI RRenderDisplayTime( RRender *rc );
void MEAPI RRenderSetDisplayTime( RRender *rc, int bDisplay, long lStartSlice, float fltStepIncrement, float fltPlaybackSpeed);
void MEAPI RRenderSetPlaybackRate(RRender *rc, float fltPlaybackSpeed);

/* Particle Systems */
RParticleSystem * MEAPI RParticleSystemNew(int numParticles,
                                           MeVector3 *positions,
                                           int tex_id, const float color[4],
                                           AcmeReal size ); /* internal */

RParticleSystem * MEAPI RParticleSystemCreate(RRender *rc, int numParticles,
                                              MeVector3 *positions,
                                              char *tex_filename,
                                              const float color[4],
                                              AcmeReal tri_size );

void              MEAPI RParticleSystemDestroy(RParticleSystem *ps);
void              MEAPI RParticleSystemSetParticleCount(RParticleSystem *ps, int numParticles);
void              MEAPI RParticleSystemAddToList(RRender *rc,
                                                 RParticleSystem *ps);
void              MEAPI RParticleSystemRemoveFromList(RRender *rc,
                                                      RParticleSystem *ps );
void              MEAPI RParticleSystemSetColorAndSizeCB(RParticleSystem *ps, 
                                                         RParticleSystemGetParticleColorAndSizeCB cb, 
                                                         void* particleGraphicInfo);

/* MeViewer Menu System */
RMenu*            MEAPI RMenuCreate(RRender* rc, const char* name);
void              MEAPI RMenuDestroy(RMenu* rm);
void              MEAPI RRenderSetDefaultMenu(RRender *rc, RMenu* menu);
void              MEAPI RMenuDisplay(RMenu* rm);

void              MEAPI RMenuAddToggleEntry(RMenu* rm, const char * name,
                                            RMenuToggleCallback func,
                                            MeBool defaultValue);
void              MEAPI RMenuAddValueEntry(RMenu* rm, const char * name,
                                           RMenuValueCallback func,
                                           MeReal hi, MeReal lo,
                                           MeReal increment,
                                           MeReal defaultValue);
void              MEAPI RMenuAddSubmenuEntry(RMenu* rm, const char * name,
                                             RMenu* submenu);
/* MeViewer Menu with index */
void              MEAPI RMenuAddToggleEntryWithIndex(RMenu* rm, const char * name,
						    RMenuToggleCallbackWithIndex func,
						    MeBool defaultValue,
						    int index);
void              MEAPI RMenuAddValueEntryWithIndex(RMenu* rm, const char * name,
						    RMenuValueCallbackWithIndex func,
						    MeReal hi, MeReal lo,
						    MeReal increment,
						    MeReal defaultValue,
						    int index); 

/* Internal */

void              MEAPI RDisplayBanner(void);
void              MEAPI RDisplayCommandLineHelp(void);
void              MEAPI ROGLSetFrameLock(int flag);

#ifdef __cplusplus
}
#endif

#endif
