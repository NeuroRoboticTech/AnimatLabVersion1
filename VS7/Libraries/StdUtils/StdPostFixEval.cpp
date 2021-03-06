// StdPostFixEval.cpp: implementation of the CStdPostFixEval class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CStdPostFixEval::CStdPostFixEval()
{
	m_dblSolution = 0.0;
}

CStdPostFixEval::~CStdPostFixEval()
{

}


double CStdPostFixEval::Solution()
{return m_dblSolution;}

string CStdPostFixEval::Equation()
{return m_strEquation;}

void CStdPostFixEval::Equation(string strVal)
{
	SavePostFixInArray(strVal);
	m_strEquation = strVal;
}


void CStdPostFixEval::AddVariable(string strVarName)
{
	CStdVariable *lpVar = FindVariable(strVarName);
	if(lpVar) 
		THROW_TEXT_ERROR(Std_Err_lVariableExists, Std_Err_strVariableExists, strVarName);

	lpVar = new CStdVariable;
	lpVar->m_strVariable = strVarName;
	m_aryVariables.Add(lpVar);
}

void CStdPostFixEval::SetVariable(string strVarName, double dblVal)
{
	CStdVariable *lpVar = FindVariable(strVarName);
	if(!lpVar) 
		THROW_TEXT_ERROR(Std_Err_lVariableNotExists, Std_Err_strVariableNotExists, strVarName);
	
	lpVar->m_dblValue = dblVal;
}

CStdVariable *CStdPostFixEval::FindVariable(string strVarName)
{
	int iSize = m_aryVariables.GetSize(), i;
	CStdVariable *lpVar=NULL;

	for(i=0; i<iSize; i++)
	{
		lpVar = m_aryVariables[i];
		if(lpVar->m_strVariable == strVarName) return lpVar;
	}

	return NULL;
}


void CStdPostFixEval::FillInVariables(CStdArray<string> &aryPostFix)
{
	int iSize=aryPostFix.GetSize(), i;
	string strTemp;
	CStdVariable *lpVar=NULL;

	for(i=0; i<iSize; i++)
	{
		lpVar = FindVariable(aryPostFix[i]);
		if(lpVar)
		{
			strTemp = Std_Format("%f", lpVar->m_dblValue);
			aryPostFix[i] = strTemp;
		}
	}
}


double CStdPostFixEval::Solve()
{
	CStdArray<string> aryPostFix;
	double dblVal, dblLeft, dblRight;
	long lSize, i;
	string strTemp;

	aryPostFix = m_aryPostFix;
	FillInVariables(aryPostFix);

	lSize = aryPostFix.GetSize();
	for(i=0; i<lSize; i++)
	{
		strTemp = aryPostFix[i];
		if(strTemp == "^")
		{
			GetParams(dblLeft, dblRight, 2);
			dblVal = pow(dblLeft, dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "*")
		{
			GetParams(dblLeft, dblRight, 2);
			dblVal = dblLeft * dblRight;
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "/")
		{
			GetParams(dblLeft, dblRight, 2);
			if(!dblRight) THROW_ERROR(Std_Err_lDivByZero, Std_Err_strDivByZero);
			dblVal = dblLeft / dblRight;
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "%")
		{
			GetParams(dblLeft, dblRight, 2);
			if(!dblRight) THROW_ERROR(Std_Err_lDivByZero, Std_Err_strDivByZero);
			dblVal = ((long) dblLeft) % ((long) dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "+")
		{
			GetParams(dblLeft, dblRight, 2);
			dblVal = dblLeft + dblRight;
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "-")
		{
			GetParams(dblLeft, dblRight, 2);
			dblVal = dblLeft - dblRight;
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "cos")
		{
			GetParams(dblLeft, dblRight, 1);
			dblVal = cos(dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "sin")
		{
			GetParams(dblLeft, dblRight, 1);
			dblVal = sin(dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "tan")
		{
			GetParams(dblLeft, dblRight, 1);
			dblVal = tan(dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "acos")
		{
			GetParams(dblLeft, dblRight, 1);
			dblVal = acos(dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "asin")
		{
			GetParams(dblLeft, dblRight, 1);
			dblVal = asin(dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "atan")
		{
			GetParams(dblLeft, dblRight, 1);
			dblVal = atan(dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "sqrt")
		{
			GetParams(dblLeft, dblRight, 1);
			if(!dblRight) THROW_ERROR(Std_Err_lSqrtNegNumber, Std_Err_strSqrtNegNumber);
			dblVal = sqrt(dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "exp")
		{
			GetParams(dblLeft, dblRight, 1);
			dblVal = exp(dblRight);
			m_aryStack.Push(dblVal);
		}
		else if(strTemp == "rnd")
		{
			dblVal = Std_DRand(0, 1);
			m_aryStack.Push(dblVal);
		}
		else
		{
			//If it is else then it must be a number
			dblVal = atof(strTemp.c_str());
			m_aryStack.Push(dblVal);
		}
	}

	//If there is more than one entry in the stack then something is wrong
	if(m_aryStack.GetSize() != 1) THROW_ERROR(Std_Err_lToManyParamsLeft, Std_Err_strToManyParamsLeft);
	dblVal = m_aryStack.Pop();

	return dblVal;
}



void CStdPostFixEval::SavePostFixInArray(string &strEqu)
{
	long lSize = strEqu.length();
	int i;
	string strTemp;
	
	m_aryPostFix.RemoveAll();

	for(i=0; i<lSize; i++)
	{
		if(strEqu[i] == ',')
		{
			m_aryPostFix.Add(strTemp);
			strTemp = "";
		}
		else
			strTemp+=strEqu[i];
	
	}

	if(strTemp.length() > 0) 
		m_aryPostFix.Add(strTemp);

}



void CStdPostFixEval::GetParams(double &dblLeft, double &dblRight, int iNumParams)
{
	dblLeft = 0.0;
	dblRight = 0.0;

	if(m_aryStack.GetSize() < iNumParams) 
		THROW_PARAM_ERROR(Std_Err_lInvalidNumParams, Std_Err_strInvalidNumParams, "NumParams", iNumParams);

	dblRight = m_aryStack.Pop();

	if(iNumParams>1) 
		dblLeft = m_aryStack.Pop();
	else
		dblLeft = 0;

}

