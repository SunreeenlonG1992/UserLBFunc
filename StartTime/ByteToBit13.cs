using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.PrivateImplementationDetails;

namespace UserDefinedFunction
{
    [TxPlcLogicBehaviorFunctionAttribute("ByteToBitCS")]
    [TxPlcLogicBehaviorFunctionDescription("Transfor the byte value to boolen")]
    public class ByteToBit13 : ITxPlcLogicBehaviorFunction
    {
        private ArrayList m_typesArray = new ArrayList();
        private ArrayList m_namesArray = new ArrayList();
        private TxPlcSignalDataType m_returnValueType;
        //声明
        public ByteToBit13()
        {
            m_typesArray.Add(TxPlcSignalDataType.Int);
            m_typesArray.Add(TxPlcSignalDataType.Byte);
            m_namesArray.Add("bit");
            m_namesArray.Add("Byte_InputSignal");
            m_returnValueType = TxPlcSignalDataType.Bool;
        }
        //定义在逻辑块中该函数的变量类型
        public ArrayList ParametersTypes()
        {
            return m_typesArray;
        }
        public ArrayList ParametersNames()
        {
            return m_namesArray;
        }
        public TxPlcSignalDataType ReturnValueType()
        {
            return m_returnValueType;
        }
        //函数逻辑 Evaluates the function

        public TxPlcValue Evaluate(ArrayList parameters)
        {
            TxPlcValue plcVal1 = (TxPlcValue)parameters[0];
            TxPlcValue plcVal2 = (TxPlcValue)parameters[1];
            TxPlcValue newVal = new TxPlcValue();
            int bitVal1 = plcVal1.IntValue;
            byte byteVal2 = (byte)plcVal2.ByteValue;
           


            string str = Convert.ToString(byteVal2, 2);
            if (bitVal1 <= str.Length)
            {
                string str2 = str.Substring((str.Length - bitVal1), 1);
                //newVal.BooleanValue = bool.Parse(str.Substring((str.Length - bitVal1), 1));
                //newVal.BooleanValue = Convert.ToBoolean(str.Substring(1, 1));
                if (str2 == "1")
                {
                    newVal.BooleanValue = true;
                }
                else
                {
                    newVal.BooleanValue = false;
                }
              
                //newVal.BooleanValue = Convert.ToBoolean(str.Substring(1, 1));
            }
            
            else
            {
                newVal.BooleanValue = false;
            }



            return newVal;
        }
    }
}

