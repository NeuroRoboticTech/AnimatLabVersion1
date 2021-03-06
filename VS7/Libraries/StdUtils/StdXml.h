// StdXml.h: interface for the CStdXml class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_STDXML_H__FB208131_9BF4_4780_9326_20152582F9F0__INCLUDED_)
#define AFX_STDXML_H__FB208131_9BF4_4780_9326_20152582F9F0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class STD_UTILS_PORT CStdXml : public CMarkupSTL
{
protected:
	stack<string> m_aryTagStack;

	virtual void ClearTagStack();
	virtual string ValueErrorString(string strValue);

public:
	CStdXml();
	virtual ~CStdXml();

	virtual string Serialize();
	virtual void Deserialize(string &strXml);

	virtual bool IntoElem();
	virtual bool OutOfElem();
	virtual string FullTagPath(bool bAddChildName = true);

	virtual int NumberOfChildren();
	virtual BOOL FindElement(string strElementName, bool fThrowError = true);
	virtual BOOL FindChildByIndex(int iIndex, bool bThrowError = true);
	virtual BOOL FindChildElement(string strElementName, bool fThrowError = true);

	virtual bool IntoChildElement(string strElementName, bool bThrowError = true);

	virtual string GetChildString(string strElementName);
	virtual string GetChildString(string strElementName, string strDefault);
	virtual string GetChildString();
	virtual long GetChildLong(string strElementName);
	virtual long GetChildLong(string strElementName, long lDefault);
	virtual long GetChildLong();
	virtual int GetChildInt(string strElementName);
	virtual int GetChildInt(string strElementName, int iDefault);
	virtual int GetChildInt();
	virtual double GetChildDouble(string strElementName);
	virtual double GetChildDouble(string strElementName, double dblDefault);
	virtual double GetChildDouble();
	virtual float GetChildFloat(string strElementName);
	virtual float GetChildFloat(string strElementName, float fltDefault);
	virtual float GetChildFloat();
	virtual BOOL GetChildBool(string strElementName);
	virtual BOOL GetChildBool(string strElementName, bool bDefault);
	virtual BOOL GetChildBool();

	virtual void AddElement(string strElementName, string strData = "");
	//virtual bool RemoveElement(string strElementName, bool bThrowError = true);
	
	//Had to name the functions different here because bool was conflicting with int.
	virtual void AddChildElement(string strElementName);
	virtual void AddChildElement(string strElementName, string strVal);
	virtual void AddChildElement(string strElementName, char cVal);
	virtual void AddChildElement(string strElementName, unsigned char cVal);
	virtual void AddChildElement(string strElementName, long lVal);
	virtual void AddChildElement(string strElementName, int iVal);
	virtual void AddChildElement(string strElementName, double dblVal);
	virtual void AddChildElement(string strElementName, float fltVal);
	virtual void AddChildElement(string strElementName, bool bVal);

	virtual void AddChildCData(string strElementName, string strCData);

	virtual string GetAttribString(string strAttribName, bool bCanBeBlank = false, bool bThrowError = true, string strDefault = "");
	virtual long GetAttribLong(string strAttribName, bool bThrowError = true, long lDefault = 0);
	virtual int GetAttribInt(string strAttribName, bool bThrowError = true, int iDefault = 0);
	virtual double GetAttribDouble(string strAttribName, bool bThrowError = true, double dblDefault = 0);
	virtual float GetAttribFloat(string strAttribName, bool bThrowError = true, float fltDefault = 0);
	virtual BOOL GetAttribBool(string strAttribName, bool bThrowError = true, bool bDefault = false);

	virtual void SetAttrib(string strAttribName, string strVal);
	virtual void SetAttrib(string strAttribName, char cVal);
	virtual void SetAttrib(string strAttribName, unsigned char cVal);
	virtual void SetAttrib(string strAttribName, long lVal);
	virtual void SetAttrib(string strAttribName, int iVal);
	virtual void SetAttrib(string strAttribName, double dblVal);
	virtual void SetAttrib(string strAttribName, float fltVal);
	virtual void SetAttrib(string strAttribName, bool bVal);

	virtual string GetChildAttribString(string strAttribName, bool bCanBeBlank = false, bool bThrowError = true, string strDefault = "");
	virtual long GetChildAttribLong(string strAttribName, bool bThrowError = true, long lDefault = 0);
	virtual int GetChildAttribInt(string strAttribName, bool bThrowError = true, int iDefault = 0);
	virtual double GetChildAttribDouble(string strAttribName, bool bThrowError = true, double dblDefault = 0);
	virtual float GetChildAttribFloat(string strAttribName, bool bThrowError = true, float fltDefault = 0);
	virtual BOOL GetChildAttribBool(string strAttribName, bool bThrowError = true, bool bDefault = false);

	virtual void SetChildAttrib(string strAttribName, string strVal);
	virtual void SetChildAttrib(string strAttribName, char cVal);
	virtual void SetChildAttrib(string strAttribName, unsigned char cVal);
	virtual void SetChildAttrib(string strAttribName, long lVal);
	virtual void SetChildAttrib(string strAttribName, int iVal);
	virtual void SetChildAttrib(string strAttribName, double dblVal);
	virtual void SetChildAttrib(string strAttribName, float fltVal);
	virtual void SetChildAttrib(string strAttribName, bool bVal);

	virtual void AddChildDoc(string &strDoc);	
	virtual string GetChildDoc();
	virtual string GetParentTagName();

	virtual void Load(string strFilename);
	virtual void Save(string strFilename);
};

#endif // !defined(AFX_STDXML_H__FB208131_9BF4_4780_9326_20152582F9F0__INCLUDED_)
