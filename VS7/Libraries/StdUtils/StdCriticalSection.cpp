/////////////////////////////////////////////////////////////////////////////
// CStdCriticalSection
//
// A ::TryEnterCriticalSection type thing that works on 9x
//
// Written by Olan Patrick Barnes (patrick@mfcfree.com)
// Copyright (c) 2001 Olan Patrick Barnes
//
// This code may be used in compiled form in any way you desire. This
// file may be redistributed by any means PROVIDING it is 
// not sold for profit without the authors written consent, and 
// providing that this notice and the authors name is included. 
//
// This file is provided "as is" with no expressed or implied warranty.
// The author accepts no liability if it causes any damage to you or your
// computer whatsoever.
//
// Description:
//
// ::TryEnterCriticalSection() is only available on NT platforms, and you
// may need to support 9x.  This is a custom critical section class that
// allows for "try-enter" logic.  It operates 100% in user mode (this
// class does not use expensive kernel objects), making use of the
// ::InterlockedExchange() and ::GetCurrentThreadId() API's.
//
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

//Copy constructor
// For internal use by CStdCriticalSection
// This locks access to the internal variables of
// an instance of CStdCriticalSection from other threads
CStdCriticalSection::InternalLocker::InternalLocker(LPLONG plBusy) :
   m_plBusy(NULL)
{
   while (::InterlockedExchange(plBusy, 1) != 0)
   {
      Sleep(0);
   }
   m_plBusy = plBusy;
}

//Destructor
// For internal use by CStdCriticalSection
// This unlocks the lock the constructor of this
// class gained.
CStdCriticalSection::InternalLocker::~InternalLocker()
{
   ::InterlockedExchange(m_plBusy, 0); //lint !e534
   m_plBusy = NULL;
}

//Constructor
CStdCriticalSection::CStdCriticalSection() :
   m_lBusy(0),
   m_dwOwner(0),
   m_ulRefCnt(0)
{
}

//Destructor
// This releases the critical section lock if one was gained
// with a TryEnter call
CStdCriticalSection::~CStdCriticalSection()
{
   Leave(); //lint !e534
}

//TryEnter
// This locks the critical section for the current thread if
// no other thread already owns the critical section.  If the
// current thread already owns the critical section and this
// is reentry, the current thread is allowed to pass
bool CStdCriticalSection::TryEnter()
{
   bool bRet(false);
   InternalLocker locker(&m_lBusy);
   if (m_dwOwner == 0)
   {
      //Nobody owns this cs, so the current will gain ownership
      //ATLASSERT(m_ulRefCnt == 0);
      m_dwOwner = ::GetCurrentThreadId();
      m_ulRefCnt = 1;
      bRet = true;
   }
   else if (m_dwOwner == ::GetCurrentThreadId())
   {
      //The current thread already owns this cs
      //ATLASSERT(m_ulRefCnt > 0);
      m_ulRefCnt++;
      bRet = true;
   }

   //If we return false, some other thread already owns this cs, so
   // we will not increment the recursive ownership count (m_ulRefCnt)
   return bRet;
}

//Leave
// This unlocks the critical section for the current thread if
// the current thread already owns the critical section and it only
// has one "lock" on the critical section.  If the lock count (the
// number of times the same thread has it locked) is greater than one,
// then the count is simply decremented.
bool CStdCriticalSection::Leave()
{
   InternalLocker locker(&m_lBusy);
   //If the current thread owns this cs
   if (m_dwOwner == ::GetCurrentThreadId())
   {
      //and if decrementing the recursive ownership count results in
      // a recursive ownership count of zero, then the current thread
      // should no longer own this cs
      //ATLASSERT(m_ulRefCnt > 0);
      if (--m_ulRefCnt == 0)
      {
         //By setting m_dwOwner to zero, we're stating that no thread owns
         // this cs
         m_dwOwner = 0;
      }
      return true;
   }
   return false;
}
