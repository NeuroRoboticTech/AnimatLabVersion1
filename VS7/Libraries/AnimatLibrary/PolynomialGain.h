// PolynomialGain.h: interface for the PolynomialGain class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_POLYGAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
#define AFX_POLYGAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif 

/*! \brief 
   xxxx.

   \remarks
   xxxx
		 
   \sa
	 xxx
	 
	 \ingroup AnimatLibrary
*/

namespace AnimatLibrary
{
	namespace Gains
	{

			class ANIMAT_PORT PolynomialGain : public Gain 
			{
			protected:
				float m_fltA;
				float m_fltB;
				float m_fltC;
				float m_fltD;

			public:
				PolynomialGain();
				virtual ~PolynomialGain();

				virtual float CalculateGain(float fltInput);
				virtual void Load(CStdXml &oXml);
				virtual void LoadFull(CStdXml &oXml);
			};

	}			//Gains
}				//AnimatLibrary

#endif // !defined(AFX_POLYGAIN_H__9FEE3153_B3B6_4064_B93B_35265C06E366__INCLUDED_)
