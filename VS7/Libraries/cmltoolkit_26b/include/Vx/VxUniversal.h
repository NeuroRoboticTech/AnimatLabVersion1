#if !defined(VX_UNIVERSAL_H)
#define VX_UNIVERSAL_H

/* -*- mode: C; -*- */

/*
   Copyright (c) 2000-2004 CMLabs Simulations Inc.
   
   http://www.cm-labs.com

   $Name:  $
   
   Date: $Date: 2004/12/03 01:21:31 $
   
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

// VxUniversal.h: interface for the VxUniversal class.
//
//////////////////////////////////////////////////////////////////////

#include "Vx/VxConstraint.h"

namespace Vx
{

/**
 *  Universal joint.
 *
 */
class VX_DLLSPEC VxUniversal: public VxConstraint
{

public:
    VxUniversal();
    VxUniversal(VxPart* part1, VxPart* part2, VxReal3 pos, VxReal3 axis1, VxReal3 axis2);
    virtual ~VxUniversal();

    static int getClassType();
    virtual const char* getClassName() const { return "VxUniversal"; }

    void setAttachmentPosition(VxReal3 v);
    void setAttachmentAxes(VxReal3 ax1, VxReal3 ax2);

protected:

private:
    void _reset();
    void init();
};

} // namespace Vx

#endif // VX_UNIVERSAL_H