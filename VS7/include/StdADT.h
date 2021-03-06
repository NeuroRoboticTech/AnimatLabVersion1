#ifndef __STD_ADT_DLL_H__
#define __STD_ADT_DLL_H__

class CStdSerialize;

template <class T>
// the linked list class
class CStdPtrArray : public vector<T*>
{

public:
	virtual ~CStdPtrArray() {clear();};

	virtual void Add(T *lpVal)
	{push_back(lpVal);};
	
	virtual void clear()
	{		
		int iCount = size();
		for(int i=0; i<iCount; i++)
		{
			T *lpVal = at(i);
			if(lpVal) 
				delete lpVal;

			at(i) = NULL;
		}

		vector<T*>::clear();
	};

	virtual void RemoveAll() {clear();};

	virtual void RemoveAt(int iPos)
	{
		int iSize = size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		erase(begin()+iPos);
	};

	virtual void RemoveAt(vector<T*>::iterator oPos)
	{erase(oPos);}

	virtual void erase(vector<T*>::iterator oPos)
	{
		if(oPos!=end())
		{
			if(*oPos) 
				delete *oPos;
			*oPos = NULL;
		}

		vector<T*>::erase(oPos);
	}

	virtual void erase(vector<T*>::iterator oBegin, vector<T*>::iterator oEnd)
	{
		vector<T*>::iterator oPos;
		for(oPos=oBegin; oPos!=oEnd; ++oPos)
		{
			if(*oPos) 
				delete *oPos;
			*oPos = NULL;
		}

		vector<T*>::erase(oBegin, oEnd);
	}


	virtual void SetSize(long lSize)
	{resize(lSize);};

	virtual void InsertAt(int iPos, T *lpVal)
	{
		int iSize = size();

		if( (iPos < 0) || (iPos > iSize) )
			return;
		else if(iPos == iSize)
			Add(lpVal);
		else
			insert((begin()+iPos), lpVal);
	};


	//These are for backward compatibility
	virtual int GetSize() {return size();}; 
	virtual void Clear() {clear();};

};


template<class T>
void CopyPtrArray(CStdPtrArray<T> &oSource, CStdPtrArray<T> &oDest)
{
	int iCount, iIndex;
	T *lpNewObj=NULL, *lpOldObj;

	oDest.RemoveAll();

	iCount = oSource.GetSize();
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpOldObj = oSource[iIndex];
		lpNewObj = dynamic_cast<T *>(lpOldObj->Clone());
		
		if(lpNewObj)
			oDest.Add(lpNewObj);
	}
}


template <class T>
// the linked list class
class CStdArray : public vector<T>
{

public:
	virtual ~CStdArray() {clear();};

	virtual void Add(T lpVal)
	{push_back(lpVal);};
	
	virtual void RemoveAll() {clear();};

	virtual void RemoveAt(int iPos)
	{
		int iSize = size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		erase(begin()+iPos);
	};

	virtual void RemoveAt(vector<T>::iterator oPos)
	{erase(oPos);}

	virtual void SetSize(long lSize)
	{resize(lSize);};

	virtual void InsertAt(int iPos, T lpVal)
	{
		int iSize = size();

		if( (iPos < 0) || (iPos > iSize) )
			return;
		else if(iPos == iSize)
			Add(lpVal);
		else
			insert((begin()+iPos), lpVal);
	};

	//These are for backward compatibility
	virtual int GetSize() {return size();}; 
	virtual void Clear() {clear();};

};


template <class T>
// the linked list class
class CStdPtrDeque : public deque<T*>
{

public:
	virtual ~CStdPtrDeque() {clear();};

	virtual void AddFront(T *lpVal)
	{push_front(lpVal);};

	virtual void AddBack(T *lpVal)
	{push_back(lpVal);};

	virtual void Add(T *lpVal)
	{push_back(lpVal);};
	
	virtual void Push(T lpVal)
	{push_front(lpVal);};
	
	virtual T Pop()
	{
		T lpVal = front();
		pop_front();
		return lpVal;
	};
	
	virtual void clear()
	{		
		int iCount = size();
		for(int i=0; i<iCount; i++)
		{
			if(at(i)) delete at(i);
			at(i) = NULL;
		}

		deque<T*>::clear();
	};

	virtual void RemoveAll() {clear();};

	virtual void RemoveAt(int iPos)
	{
		int iSize = size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		erase(begin()+iPos);
	};

	virtual void RemoveAt(deque<T*>::iterator oPos)
	{erase(oPos);}

	virtual void erase(deque<T*>::iterator oPos)
	{
		if(oPos!=end())
		{
			if(*oPos) 
				delete *oPos;
			*oPos = NULL;
		}

		deque<T*>::erase(oPos);
	}

	virtual void erase(deque<T*>::iterator oBegin, deque<T*>::iterator oEnd)
	{
		deque<T*>::iterator oPos;
		for(oPos=oBegin; oPos!=oEnd; ++oPos)
		{
			if(*oPos) 
				delete *oPos;
			*oPos = NULL;
		}

		deque<T*>::erase(oBegin, oEnd);
	}


	virtual void InsertAt(int iPos, T *lpVal)
	{
		int iSize = size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		insert((begin()+iPos), lpVal);
	};


	//These are for backward compatibility
	virtual int GetSize() {return size();}; 
	virtual void Clear() {clear();};

};



template <class T>
// the linked list class
class CStdDeque : public deque<T>
{

public:
	virtual ~CStdDeque() {clear();};

	virtual void AddFront(T lpVal)
	{push_front(lpVal);};

	virtual void AddBack(T lpVal)
	{push_back(lpVal);};

	virtual void Add(T lpVal)
	{push_back(lpVal);};
	
	virtual void Push(T lpVal)
	{push_front(lpVal);};
	
	virtual T Pop()
	{
		T lpVal = front();
		pop_front();
		return lpVal;
	};

	virtual void RemoveAll() {clear();};

	virtual void RemoveAt(int iPos)
	{
		int iSize = size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		erase(begin()+iPos);
	};

	virtual void RemoveAt(deque<T>::iterator oPos)
	{erase(oPos);}


	virtual void InsertAt(int iPos, T lpVal)
	{
		int iSize = size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		insert((begin()+iPos), lpVal);
	};


	//These are for backward compatibility
	virtual int GetSize() {return size();}; 
	virtual void Clear() {clear();};

};


template <class T>
// the Stack class
class CStdPtrStack : public stack<T*>
{
public:
	virtual ~CStdPtrStack() {clear();};

	virtual void RemoveAll() {clear();};
	virtual void Clear() {clear();};
	virtual void clear()
	{	
		T *lpNode;

		int iCount = size();
		for(int i=0; i<iCount; i++)
		{
			lpNode = Pop();
			if(lpNode) delete lpNode;
		}
	};

	virtual void Push(T *lpElement)
	{push(lpElement);};

	virtual T *Pop()
	{
		T *lpElement = NULL;

		if( size() > 0 )
			{
				lpElement = top();
				pop();
				return lpElement;
			}
			else
				return NULL;
	};

	virtual T *Top() {return Examine();}

	virtual int IsEmpty() {return !size();};
	virtual int GetSize() {return size();};
	virtual T *Examine()
	{
	   	if( size() )
				return top();
			else
				return NULL;
	};
};


template <class T>
// the Stack class
class CStdStack : public stack<T>
{
public:
	virtual ~CStdStack() {clear();};

	virtual void RemoveAll() {clear();};
	virtual void Clear() {clear();};
	virtual void clear()
	{	
		int iCount = size();
		for(int i=0; i<iCount; i++)
			pop();
	};

	virtual void Push(T lpElement)
	{push(lpElement);};

	virtual T Pop()
	{
		T oElement;

		if( size() > 0 )
			{
				oElement = top();
				pop();
				return oElement;
			}
			else
				return NULL;
	};

	virtual T Top() {return Examine();}

	virtual int IsEmpty() {return !size();};
	virtual int GetSize() {return size();};
	virtual T Examine()
	{return Top();};
};



template <class Key, class T>
// the linked list class
class CStdPtrMap : public map<Key, T*>
{

public:
	virtual ~CStdPtrMap() {clear();};

	virtual void Add(Key oKey, T *lpVal)
	{
		map<Key, T*>::iterator oPos = find(oKey);
		if(oPos!=end())
			THROW_ERROR(Std_Err_lDuplicateKeyInMap, Std_Err_strDuplicateKeyInMap); 

		insert(make_pair(oKey, lpVal));
	};

	virtual void RemoveAll() {clear();};

	virtual void Remove(const Key &oKey)
	{
		map<Key, T*>::iterator oPos = find(oKey);
		if(oPos==end())
			THROW_ERROR(Std_Err_lKeyNotFoundInMap, Std_Err_strKeyNotFoundInMap); 

		erase(oKey);
	};

	//These are for backward compatibility
	virtual int GetSize() {return size();}; 
	virtual void Clear() {clear();};

	virtual void clear()
	{	
		//Lets go through and delete all the non null pointers.
		map<Key, T*>::iterator oPos;
		for(oPos=begin(); oPos!=end(); ++oPos)
		{
			if(oPos->second) delete oPos->second;
			oPos->second = NULL;
		}
		
		map<Key, T*>::clear();
	};

	virtual size_type erase(const Key &oKey)
	{	
		map<Key, T*>::iterator oPos = find(oKey);
		if(oPos!=end() && oPos->second)
		{
			if(oPos->second) delete oPos->second;
			oPos->second = NULL;
		}
		return map<Key, T*>::erase(oKey);
	}

/*
	virtual void erase(map<Key, T*>::iterator oPos)
	{
		if(oPos!=end() && oPos->second)
		{
			if(oPos->second) delete oPos->second;
			oPos->second = NULL;
		}

		map<Key, T*>::erase(oPos);
	}

	virtual void erase(map<Key, T*>::iterator oBegin, map<Key, T*>::iterator oEnd)
	{
		map<Key, T*>::iterator oPos;
		for(oPos=oBegin; oPos!=oEnd; ++oPos)
		{
			if(oPos->second) delete oPos->second;
			oPos->second = NULL;
		}

		map<Key, T*>::erase(oBegin, oEnd);
	}
	*/
};


template<class Key, class T>
void CopyPtrMap(CStdPtrMap<Key, T> &oSource, CStdPtrMap<Key, T> &oDest)
{
	T *lpNewObj=NULL;

	oDest.RemoveAll();

	map<Key, T*>::iterator oPos;
	for(oPos=oSource.begin(); oPos!=oSource.end(); ++oPos)
	{
		if(oPos->second) 
			lpNewObj = dynamic_cast<T *>(oPos->second->Clone());
		else
			lpNewObj = NULL;

		oDest.Add(oPos->first, lpNewObj);
	}
}


template <class Key, class T>
// the linked list class
class CStdMap : public map<Key, T>
{

public:
	virtual ~CStdMap() {clear();};

	virtual void Add(Key oKey, T oVal)
	{
		map<Key, T>::iterator oPos = find(oKey);
		if(oPos!=end())
			THROW_ERROR(Std_Err_lDuplicateKeyInMap, Std_Err_strDuplicateKeyInMap); 

		insert(make_pair(oKey, oVal));
	};

	virtual void RemoveAll() {clear();};

	virtual void RemoveAt(iterator it)
	{erase(it);};

	virtual void Remove(const Key &oKey)
	{
		map<Key, T>::iterator oPos = find(oKey);
		if(oPos==end())
			THROW_ERROR(Std_Err_lKeyNotFoundInMap, Std_Err_strKeyNotFoundInMap); 

		erase(oKey);
	};

	//These are for backward compatibility
	virtual int GetSize() {return size();}; 
	virtual void Clear() {clear();};
};


template <class T>
class CStdPoint
{
public:
	T x;
	T y;
	T z;

	CStdPoint()
	{
		x = 0;
		y = 0;
		z = 0;
	};

	CStdPoint(T valx, T valy, T valz)
	{
		x = valx;
		y = valy;
		z = valz;
	};

	void Set(T X, T Y, T Z)
	{
		x = X;
		y = Y;
		z = Z;
	};

	bool operator==(const CStdPoint<T> &oPoint)
	{
		if( (x == oPoint.x) && (y == oPoint.y) && (z == oPoint.z) )
			return true;
		return false;
	};

	bool operator!=(const CStdPoint<T> &oPoint)
	{
		if( (x == oPoint.x) && (y == oPoint.y) && (z == oPoint.z) )
			return false;
		return true;
	};

	void operator=(const CStdPoint<T> &oPoint)
	{
		x=oPoint.x;
		y=oPoint.y;
		z=oPoint.z;
	};

	void operator+=(const CStdPoint<T> &oPoint)
	{
		x+=oPoint.x;
		y+=oPoint.y;
		z+=oPoint.z;
	};

	void operator-=(const CStdPoint<T> &oPoint)
	{
		x-=oPoint.x;
		y-=oPoint.y;
		z-=oPoint.z;
	};

	CStdPoint<T> operator+(const CStdPoint<T> &oPoint)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x + oPoint.x;
		oNewPoint.y = y + oPoint.y;
		oNewPoint.z = z + oPoint.z;
		return oNewPoint;
	};

	CStdPoint<T> operator-(const CStdPoint<T> &oPoint)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x - oPoint.x;
		oNewPoint.y = y - oPoint.y;
		oNewPoint.z = z - oPoint.z;
		return oNewPoint;
	};

	void operator+=(const float fltVal)
	{
		x+=fltVal;
		y+=fltVal;
		z+=fltVal;
	};

	void operator-=(const float fltVal)
	{
		x-=fltVal;
		y-=fltVal;
		z-=fltVal;
	};

	void operator*=(const float fltVal)
	{
		x*=fltVal;
		y*=fltVal;
		z*=fltVal;
	};

	void operator/=(const float fltVal)
	{
		if(fltVal)
		{
			x/=fltVal;
			y/=fltVal;
			z/=fltVal;
		}
		else
			THROW_ERROR(Std_Err_lDivByZero, Std_Err_strDivByZero);
	};

	CStdPoint<T> operator+(const float fltVal)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x + fltVal;
		oNewPoint.y = y + fltVal;
		oNewPoint.z = z + fltVal;
		return oNewPoint;
	};

	CStdPoint<T> operator-(const float fltVal)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x - fltVal;
		oNewPoint.y = y - fltVal;
		oNewPoint.z = z - fltVal;
		return oNewPoint;
	};

	CStdPoint<T> operator*(const float fltVal)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x * fltVal;
		oNewPoint.y = y * fltVal;
		oNewPoint.z = z * fltVal;
		return oNewPoint;
	};

	CStdPoint<T> operator/(const float fltVal)
	{
		if(!fltVal)
			THROW_ERROR(Std_Err_lDivByZero, Std_Err_strDivByZero);

		CStdPoint<T> oNewPoint;

		oNewPoint.x = x / fltVal;
		oNewPoint.y = y / fltVal;
		oNewPoint.z = z / fltVal;
		return oNewPoint;
	};

	double Magnitude()
	{return sqrt( (x*x) + (y*y) + (z*z) );};

	void Normalize()
	{
		double dblMag = Magnitude();

		if(dblMag > 0)
		{
			x/=dblMag;
			y/=dblMag;
			z/=dblMag;
		}
		else
		{
			x=1;
			y=0;
			z=0;
		}
	};

	T operator[](const int iIndex)
	{
		switch(iIndex)
		{
		case 0:
			return x;
		case 1:
			return y;
		case 2:
			return z;
		default:
			THROW_PARAM_ERROR(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex, "Index", iIndex);
		}
		return 0;
	};
};

template<class T>
void TracePointArray(CStdPtrArray< CStdPoint<T> > &aryPoints, const char* Description=NULL)
{
	int iCount, iIndex;
	CStdPoint<T> *lpPoint=NULL;

	std::ostringstream oss;

	if(Description)
	{
		oss << Description;
		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);
	}

	iCount = aryPoints.GetSize();
	char strText[200];
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPoint = aryPoints[iIndex];
		if(lpPoint)
		{
			sprintf(strText, "(%f, %f, %f)", lpPoint->x, lpPoint->y, lpPoint->z);
			Std_TraceMsg(StdLogDebug, strText, "", -1, STD_TRACE_TO_FILE, false);
		}
	}
}

template<class T>
void TracePointArray(CStdArray< CStdPoint<T> > &aryPoints, const char* Description=NULL)
{
	int iCount, iIndex;
	CStdPoint<T> oPoint;

	std::ostringstream oss;

	if(Description)
	{
		oss << Description;
		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);
	}

	iCount = aryPoints.GetSize();
	char strText[200];
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		oPoint = aryPoints[iIndex];
		sprintf(strText, "(%f, %f, %f)", oPoint.x, oPoint.y, oPoint.z);
		Std_TraceMsg(StdLogDebug, strText, "", -1, STD_TRACE_TO_FILE, false);
	}
}

#define CStdIPoint CStdPoint<int>
#define CStdLPoint CStdPoint<long>
#define CStdFPoint CStdPoint<float>
#define CStdDPoint CStdPoint<double>

#define IntVector vector<int>
#define LongVector vector<long>
#define FloatVector vector<float>
#define DoubleVector vector<float>


#ifdef STD_TRACING_ON
	template<typename T>
	void TraceSTL_Obj(const char* SourceFileName, int SourceLineNum, const char* ObjectName, T t, int Index = -1, bool bShowSource = true)
	{
		std::ostringstream oss;
		
		oss << ObjectName;
		if (Index > -1) oss << "[" << Index << "]";
		oss << " = " << t;

		if(bShowSource)
			Std_TraceMsg(StdLogDebug, oss.str(), SourceFileName, SourceLineNum, STD_TRACE_TO_FILE, false);
		else
			Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);
	}

	template<typename T>
	void TraceSTL_Container(const char* SourceFileName, int SourceLineNum, const char* ObjectName, T &t, const char* Description=NULL)
	{
		std::ostringstream oss;

		oss << SourceFileName << "(" << SourceLineNum << ")" << "\r\n";
		if(Description)
		 oss << Description;

		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);

		int Idx = 0;
		for (T::const_iterator i = t.begin();i != t.end();++i, ++Idx)
		{
				std::string szObjectName = ObjectName;
				TraceSTL_Obj("", -1, ObjectName, *i, Idx, false);
		}

		Std_TraceMsg(StdLogDebug,"\r\n", "", -1, STD_TRACE_TO_FILE, false);
	}

	template<typename T>
	void TraceSTL_AsscContainer(const char* SourceFileName, int SourceLineNum, const char* ObjectName, T &t, const char* Description=NULL)
	{
		std::ostringstream oss;

		oss << SourceFileName << "(" << SourceLineNum << ")" << "\r\n";
		if(Description)
		 oss << Description;

		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);

		int Idx = 0;
		for (T::const_iterator i = t.begin();i != t.end();++i, ++Idx)
		{
				std::string szObjectName = ObjectName;
				TraceSTL_Obj("", -1, ObjectName, *i, Idx, false);
		}

		Std_TraceMsg(StdLogDebug,"\r\n", "", -1, STD_TRACE_TO_FILE, false);
	}

	template<typename T>
	void TraceSTL_Array(const char* SourceFileName, int SourceLineNum, const char* ObjectName, T *aryVals, int iCount, const char* Description=NULL)
	{
		std::ostringstream oss;

		oss << SourceFileName << "(" << SourceLineNum << ")" << "\r\n";
		if(Description)
		 oss << Description;

		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);

		for(int Idx=0; Idx<iCount; Idx++)
		{
				std::string szObjectName = ObjectName;
				TraceSTL_Obj("", -1, ObjectName, aryVals[Idx], Idx, false);
		}

		Std_TraceMsg(StdLogDebug,"\r\n", "", -1, STD_TRACE_TO_FILE, false);
	}


	#define TRACE_STL_OBJ_NS(t)										TraceSTL_Obj(__FILE__, __LINE__, #t, t, -1, false)
	#define TRACE_STL_OBJ(t)											TraceSTL_Obj(__FILE__, __LINE__, #t, t)
	#define TRACE_STL_CONTAINER(t)								TraceSTL_Container(__FILE__, __LINE__, #t, t)
	#define TRACE_STL_CONTAINER_DESC(t, n)				TraceSTL_Container(__FILE__, __LINE__, #t, t, n)
	#define TRACE_STL_ASSC_CONTAINER(t)						TraceSTL_AsscContainer(__FILE__, __LINE__, #t, t)
	#define TRACE_STL_ASSC_CONTAINER_DESC(t, n)		TraceSTL_AsscContainer(__FILE__, __LINE__, #t, t, n)
	#define TRACE_STL_ARRAY(t, c)									TraceSTL_Array(__FILE__, __LINE__, #t, t, c)
	#define TRACE_STL_ARRAY_DESC(t, c, n)					TraceSTL_Array(__FILE__, __LINE__, #t, t, c, n)
#else //STD_TRACING_ON
	#define TRACE_STL_OBJ_NS(t)
	#define TRACE_STL_OBJ(t)             
	#define TRACE_STL_CONTAINER(t)             
	#define TRACE_STL_CONTAINER_DESC(t, n)             
	#define TRACE_STL_ASSC_CONTAINER(t)
	#define TRACE_STL_ASSC_CONTAINER_DESC(t)
	#define TRACE_STL_ARRAY(t, c)
	#define TRACE_STL_ARRAY_DESC(t, c, n)
#endif //STD_TRACING_ON

#define SAVE_TEXT_STL_OBJ(t, oss)												SaveToTextSTL_Obj(#t, t, oss)
#define SAVE_TEXT_STL_CONTAINER(t, oss)									SaveToTextSTL_Container(#t, t, oss)
#define SAVE_TEXT_STL_CONTAINER_DESC(t, n, oss)					SaveToTextSTL_Container(#t, t, oss, n)
#define SAVE_TEXT_STL_ASSC_CONTAINER(t, oss)						SaveToTextSTL_AsscContainer(#t, t, oss)
#define SAVE_TEXT_STL_ASSC_CONTAINER_DESC(t, n, oss)		SaveToTextSTL_AsscContainer(#t, t, oss, n)

template<typename T>
void SaveToTextSTL_Obj(const char* ObjectName, T t, std::ostringstream &oss, int Index = -1)
{oss <<  t << "\r\n";}

template<typename T>
void SaveToTextSTL_Container(const char* ObjectName, T &t, std::ostringstream &oss, const char* Description=NULL)
{
	if(Description)
	 oss << Description << "\r\n";

	int Idx = 0;
	for (T::const_iterator i = t.begin();i != t.end();++i, ++Idx)
	{
			std::string szObjectName = ObjectName;
			SaveToTextSTL_Obj(ObjectName, *i, oss, Idx);
	}

	oss << "\r\n";
}

template<typename T>
void SaveToTextSTL_AsscContainer(const char* ObjectName, T &t, std::ostringstream &oss, const char* Description=NULL)
{
	if(Description)
	 oss << Description << "\r\n";

	int Idx = 0;
	for (T::const_iterator i = t.begin();i != t.end();++i, ++Idx)
	{
			std::string szObjectName = ObjectName;
			SaveToTextSTL_Obj(ObjectName, i->first, oss, Idx);
	}

	oss << "\r\n";
}



#endif // __STD_ADT_DLL_H__

