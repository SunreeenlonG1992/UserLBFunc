using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.PrivateImplementationDetails;

namespace StartTime
{

    /// <summary>
    /// 该函数现阶段实现的功能是:START信号为1 开始计时，启动计时后START信号可以复位，当START信号再次触发为1，计时清零，重新开始计数。
    /// 目标实现的功能是START信号启动计时，STOP信号停止计时，输出记录的时长。计时开始后START信号可以复位，再次触发将清零重新计时。
    /// </summary>
    [TxPlcLogicBehaviorFunctionAttribute("StartTime")]
    [TxPlcLogicBehaviorFunctionDescription("A simulation time start monitor-start time monitor since START singal until STOP signal\r\n(BOOL:START,BOOL:STOP)=>(REAL:OUT)")]
    public class StartTime : ITxPlcLogicBehaviorFunction
    {
        private bool former_start=false;
        private bool former_stop=false;
        private double Time = 0.0;
        private double intervalTime = 0.0;
        private TxPlcValue RetxPlcValue = new TxPlcValue();

        private ArrayList m_typesArray = new ArrayList();
        private ArrayList m_namesArray = new ArrayList();
        private TxPlcSignalDataType m_returnValueType;
        public StartTime()
        {
            m_namesArray.Add("START[BOOL]");
            m_typesArray.Add(TxPlcSignalDataType.Bool);
            m_namesArray.Add("STOP[BOOL]");
            m_typesArray.Add(TxPlcSignalDataType.Bool);
            m_returnValueType = TxPlcSignalDataType.Real;
        }
        public TxPlcValue Evaluate(ArrayList parameters)
        {
            TxSimulationPlayer simulationPlayer = TxApplication.ActiveDocument.SimulationPlayer;

            TxPlcValue txplcValue = (TxPlcValue)parameters[0];
            TxPlcValue txplcValue2 = (TxPlcValue)parameters[1];
            
            bool flag_start = txplcValue.BooleanValue && !this.former_start;
            bool flag_stop = (!this.former_stop) && txplcValue2.BooleanValue;
            if (flag_start)
            {
                this.Time = simulationPlayer.CurrentTime;
               
            }
          
            //this.intervalTime = simulationPlayer.CurrentTime - this.Time;
            this.former_start = txplcValue.BooleanValue;
            if (flag_stop)
            {
                this.intervalTime= simulationPlayer.CurrentTime - this.Time;
            }
          
                this.intervalTime = simulationPlayer.CurrentTime - this.Time;
           
            this.RetxPlcValue.RealValue = (float)(this.intervalTime);
            this.former_stop = txplcValue2.BooleanValue;
            return RetxPlcValue;
        }
       

        public ArrayList ParametersNames()
        {
            return m_namesArray;
        }

        public ArrayList ParametersTypes()
        {
            return m_typesArray;
        }

        public TxPlcSignalDataType ReturnValueType()
        {
            return m_returnValueType;
        }
    }
}
       
