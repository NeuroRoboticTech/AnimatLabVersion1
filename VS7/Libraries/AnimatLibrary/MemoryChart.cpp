// MemoryChart.cpp: implementation of the MemoryChart class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "Gain.h"
#include "Node.h"
#include "Joint.h"
#include "ReceptiveField.h"
#include "ContactSensor.h"
#include "RigidBody.h"
#include "IODataEntry.h"
#include "Microcontroller.h"
#include "Structure.h"
#include "NeuralModule.h"
#include "Adapter.h"
#include "NervousSystem.h"
#include "Organism.h"
#include "ActivatedItem.h"
#include "ActivatedItemMgr.h"
#include "DataColumn.h"
#include "DataChart.h"
#include "MemoryChart.h"
#include "DataChartMgr.h"
#include "ExternalStimuliMgr.h"
#include "KeyFrame.h"
#include "SimulationRecorder.h"
#include "OdorType.h"
#include "Odor.h"
#include "Simulator.h"

namespace AnimatLibrary
{
	namespace Charting
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

MemoryChart::MemoryChart()
{
}

MemoryChart::~MemoryChart()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of MemoryChart\r\n", "", -1, FALSE, TRUE);}
}

BOOL MemoryChart::Lock()
{
	if(m_oRowCountLock.TryEnter())
		return TRUE;
	else
		return FALSE;
}

void MemoryChart::Unlock()
{m_oRowCountLock.Leave();}

void MemoryChart::Initialize(Simulator *lpSim)
{
	DataChart::Initialize(lpSim);

	//The time window this memory chart will use for data collection is determined by
	//the starttime/endtime. This is translated to startslice/endslice in ActivatedItem::Initialize
	//and then datachart::Initialize creates a buffer of the correct size.
}

void MemoryChart::StepSimulation(Simulator *lpSim)
{
	if(!(lpSim->TimeSlice()%m_iCollectInterval))
	{
		if(m_oRowCountLock.TryEnter())
		{
			if(m_lCurrentRow == m_lRowCount)
			{
				//If we have gotten to the point where the current row is equal to the row count
				//then we are about to overflow our buffer. We need to dump all of the data and
				//start over with a new time window.
				m_lCurrentRow = 0;
			}

			DataChart::StepSimulation(lpSim);

			m_oRowCountLock.Leave();
		}
		else
		{
			int iVal = 5;
		}
	}
}

void MemoryChart::Load(Simulator *lpSim, CStdXml &oXml)
{
	DataChart::Load(lpSim, oXml);

	oXml.IntoElem();  //Into MemoryChart Element

	oXml.OutOfElem(); //OutOf MemoryChart Element
}

	}			//Charting
}				//AnimatLibrary

