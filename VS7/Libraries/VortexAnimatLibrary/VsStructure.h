// VsOrganism.h: interface for the VsOrganism class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_VSSTRUCTURE_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_)
#define AFX_VSSTRUCTURE_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

namespace VortexAnimatLibrary
{
	namespace Environment
	{
		class VORTEX_PORT VsStructure : public Structure   
		{
		public:
			VsStructure();
			virtual ~VsStructure();

			virtual void Initialize(Simulator *lpSim);
		};

	}			// Environment
}				//VortexAnimatLibrary

#endif // !defined(AFX_VSSTRUCTURE_H__8E0C0060_8F52_4E17_BF36_B05EFE795684__INCLUDED_)
