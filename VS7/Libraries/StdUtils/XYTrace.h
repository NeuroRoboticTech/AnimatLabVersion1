#ifndef XY_TRACE_H
#define XY_TRACE_H

#include <windows.h>
#include <tchar.h>
#include <stdio.h>

enum TraceLevel
{
	TraceNone = 0, // no trace
	TraceError = 10, // only trace error
	TraceInfo = 20, // some extra info
	TraceDebug = 30, // debugging info
	TraceDetail = 40 // detailed debugging info
};

extern void SetTraceFilePrefix(LPCTSTR strFilePrefix);
extern string GetTraceFilePrefix();
extern void SetTraceLevel(const int nLevel);
extern int GetTraceLevel();
//extern void Std_Log(const int nLevel, LPCTSTR strFormat, ...);

#endif // XY_TRACE_H
