// StdVariant.h: interface for the CStdVariant class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STDVARIANT_H__F994980A_5C39_4141_948D_A2115FEDFC53__INCLUDED_)
#define AFX_STDVARIANT_H__F994980A_5C39_4141_948D_A2115FEDFC53__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


enum StdVariantType
{
	StdVtEmpty = -1,
	StdVtShort = 0,
	StdVtLong = 1,
	StdVtFloat = 2,
	StdVtDouble = 3,
	StdVtBool = 4,
	StdVtChar = 5,
	StdVtUChar = 6,
	StdVtUShort = 7,
	StdVtULong = 8,
	StdVtInt = 9,
	StdVtUInt = 10,
	StdVtString = 11
};

class STD_UTILS_PORT CStdVariant : public CStdSerialize  
{
protected:
	bool m_bCreatedVar;
	int m_iVariantType;

	short *m_lpShort;							// StdVtShort	2-byte signed int.
	long *m_lpLong;								// StdVtLong	4-byte signed int.
	float *m_lpFloat;							// StdVtFloat	4-byte real.
	double *m_lpDouble;						// StdVtDouble	8-byte real.
	bool *m_lpBool;								// StdVtBool	Boolean.
	char *m_lpChar;								// StdVtChar	Char.
	unsigned char *m_lpUChar;			// StdVtUChar	Unsigned char.
	unsigned short *m_lpUShort;		// StdVtUShort	2 byte unsigned int.
	unsigned long *m_lpULong;			// StdVtULong	4 byte unsigned int.
	int *m_lpInt;									// StdVtInt	2 byte signed int.
	unsigned int *m_lpUInt;				// StdVtUInt	2 byte unsigned int.
	string *m_lpString;						// StdVtString	string.

public:
	CStdVariant();
	virtual ~CStdVariant();

	void Reset();
	int VariantType();
	string VariantTypeName();

	short GetShort(bool bConvert = true);
	short *GetShortPtr();
	void SetValue(short Val);
	void SetPtr(short *lpVal);

	long GetLong(bool bConvert = true);
	long *GetLongPtr();
	void SetValue(long Val);
	void SetPtr(long *lpVal);

	float GetFloat(bool bConvert = true);
	float *GetFloatPtr();
	void SetValue(float Val);
	void SetPtr(float *lpVal);

	double GetDouble(bool bConvert = true);
	double *GetDoublePtr();
	void SetValue(double Val);
	void SetPtr(double *lpVal);

	bool GetBool(bool bConvert = true);
	bool *GetBoolPtr();
	void SetValue(bool Val);
	void SetPtr(bool *lpVal);

	char GetChar(bool bConvert = true);
	char *GetCharPtr();
	void SetValue(char Val);
	void SetPtr(char *lpVal);

	unsigned char GetUChar(bool bConvert = true);
	unsigned char *GetUCharPtr();
	void SetValue(unsigned char Val);
	void SetPtr(unsigned char *lpVal);

	unsigned short GetUShort(bool bConvert = true);
	unsigned short *GetUShortPtr();
	void SetValue(unsigned short Val);
	void SetPtr(unsigned short *lpVal);

	unsigned long GetULong(bool bConvert = true);
	unsigned long *GetULongPtr();
	void SetValue(unsigned long Val);
	void SetPtr(unsigned long *lpVal);

	int GetInt(bool bConvert = true);
	int *GetIntPtr();
	void SetValue(int Val);
	void SetPtr(int *lpVal);

	unsigned int GetUInt(bool bConvert = true);
	unsigned int *GetUIntPtr();
	void SetValue(unsigned int Val);
	void SetPtr(unsigned int *lpVal);

	string GetString(bool bConvert = true);
	string GetNumericString();
	string *GetStringPtr();
	void SetValue(string Val);
	void SetPtr(string *lpVal);

	void Copy(CStdVariant &oVar);
	void operator=(CStdVariant &oVar);

	//CStdSerialize overloads
	virtual CStdSerialize *Clone();
	virtual void Trace(ostream &oOs);
	virtual void Load(CStdXml &oXml);
	virtual void Save(CStdXml &oXml);
};

#endif // !defined(AFX_STDVARIANT_H__F994980A_5C39_4141_948D_A2115FEDFC53__INCLUDED_)
