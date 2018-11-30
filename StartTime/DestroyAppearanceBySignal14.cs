using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.PrivateImplementationDetails;

namespace DestroyAppearanceBySignal
{
    [TxPlcLogicBehaviorFunction("DestroyAppearanceBySignal"), TxPlcLogicBehaviorFunctionCategory("MINO LB Function"),
    TxPlcLogicBehaviorFunctionDescription("When the Trigger signal is true,Destroys all appearances in Range of the LB's Self Frame\r\n (INT:RANGE,BOOL:Trigger Signal) => (BOOL:OUT)")]


    public class DestroyAppearanceBySignal : ITxPlcLogicBehaviorInvokingResourceFunction, ITxPlcLogicBehaviorFunctionBase
    {
        private ArrayList m_typesArray = new ArrayList();
        private ArrayList m_namesArray = new ArrayList();
        private TxPlcSignalDataType m_returnValueType;
        private TxObjectList m_partAppearances = new TxObjectList();
        public DestroyAppearanceBySignal()
        {
            this.m_typesArray.Add(6);
            this.m_typesArray.Add(0);
            this.m_namesArray.Add("RANGE[INT]");
            this.m_namesArray.Add("Triger[Bool]");
            this.m_returnValueType = 0;
        }
        private void RegisterToEvenets()
        {
            TxObjectList allDescendants = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(ITxPartAppearance)));
            this.m_partAppearances = allDescendants;
        }
        public ArrayList ParametersTypes()
        {
            return this.m_typesArray;
        }
        public ArrayList ParametersNames()
        {
            return this.m_namesArray;
        }
        public TxPlcSignalDataType ReturnValueType()
        {
            return this.m_returnValueType;
        }
        public TxPlcValue Evaluate(ArrayList parameters, ITxObject invokingResource)
        {
            bool booleanValue = ((TxPlcValue)parameters[1]).BooleanValue;
            float realValue = ((TxPlcValue)parameters[0]).RealValue;
            bool flag = true;
            TxPlcValue result = new TxPlcValue();
            bool flag2 = booleanValue;
            if (flag2)
            {
                this.RegisterToEvenets();
                ITxLocatableObject txLocatableObject = invokingResource as ITxLocatableObject;
                bool flag3 = txLocatableObject != null && this.m_partAppearances != null;
                if (flag3)
                {
                    TxTransformation absoluteLocation = txLocatableObject.AbsoluteLocation;
                    for (int i = 0; i < this.m_partAppearances.Count; i++)
                    {
                        ITxObject txObject = this.m_partAppearances[i];
                        bool flag4 = txObject is ITxLocatableObject;
                        if (flag4)
                        {
                            TxVector translation = (absoluteLocation - (txObject as ITxLocatableObject).AbsoluteLocation).Translation;
                            double num = this.Lenght(translation);
                            bool flag5 = num <= (double)realValue;
                            if (flag5)
                            {
                                this.m_partAppearances.Remove(txObject);
                                txObject.Delete();
                                flag = false;
                                break;
                            }
                        }
                    }
                    bool flag6 = flag;
                    if (flag6)
                    {
                        Thread.Sleep(500);
                    }
                }
            }
            return result;
        }
        private double Lenght(TxVector translation)
        {
            return Math.Sqrt(translation.X * translation.X + translation.Y * translation.Y + translation.Z * translation.Z);
        }
    }
}
