using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.PrivateImplementationDetails;

namespace MINOLBFunction
{
    //[TxPlcLogicBehaviorFunction("DisplayBySignal"), TxPlcLogicBehaviorFunctionCategory("MINO LB Function"), 
    //TxPlcLogicBehaviorFunctionDescription("Singal is true,the Component will display! When signal is false,the Component will blank!")]
    [TxPlcLogicBehaviorFunctionAttribute("DisplayBySignal")]
    [TxPlcLogicBehaviorFunctionDescription("Singal is true,the Component will display! When signal is false,the Component will blank!")]

    public class DisplayBySignal13 : ITxPlcLogicBehaviorInvokingResourceFunction, ITxPlcLogicBehaviorFunctionBase
    {
        private ArrayList m_typesArray = new ArrayList();
        private ArrayList m_namesArray = new ArrayList();
        private TxPlcSignalDataType m_returnValueType;
        private List<ITxDevice> m_diaplayDevice = new List<ITxDevice>();



        public DisplayBySignal13()
        {
            m_typesArray.Add(TxPlcSignalDataType.Bool);
            //m_typesArray.Add(TxPlcSignalDataType.Int);
            m_namesArray.Add("Signal[BOOL]");
            //m_namesArray.Add("DisplayDeviceRANG[Real]");
            m_returnValueType = TxPlcSignalDataType.Bool;

        }

        public TxPlcValue Evaluate(ArrayList parameters, ITxObject invokingResource)
        {
            

            TxPlcValue SignalplcVal = parameters[0] as TxPlcValue;
            //TxPlcValue DecSigplcVal = parameters[1] as TxPlcValue;
            TxPlcValue result = new TxPlcValue();
            //Double fThreshould = DecSigplcVal.RealValue;
            ITxLocatableObject m_res = invokingResource as ITxLocatableObject;

                if (m_res != null)
                {
               
                ITxDisplayableObject m_displayCom = m_res as ITxDisplayableObject;

                if (m_displayCom != null)
                {

                    if (SignalplcVal.BooleanValue is true)
                    {
                        m_displayCom.Display();
                        result.BooleanValue = true;
                    }

                    else
                    {
                        m_displayCom.Blank();
                        result.BooleanValue = false;
                    }
                }

                                //}
                            //}
                        //}
                    //}
                }


            


            return result;




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
