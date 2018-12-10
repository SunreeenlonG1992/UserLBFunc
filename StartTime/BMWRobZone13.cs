using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace StartTime
{
    [TxPlcLogicBehaviorFunctionAttribute("RobZoneLU")]
    [TxPlcLogicBehaviorFunctionDescription("The Robot inteference zone function")]
    public class BMWRobZone13 : ITxPlcLogicBehaviorFunction
    {
        private ArrayList m_typesArray=new ArrayList();
        private ArrayList m_namesArray= new ArrayList();
        private TxPlcSignalDataType m_returnValueType;

        public BMWRobZone13()
        {
            m_typesArray.Add(TxPlcSignalDataType.Byte);
            m_typesArray.Add(TxPlcSignalDataType.Int);
            m_typesArray.Add(TxPlcSignalDataType.Bool);
            m_typesArray.Add(TxPlcSignalDataType.Bool);
            m_namesArray.Add("EntryByte");
            m_namesArray.Add("Rob_No");
            m_namesArray.Add("ZoneReq");
            m_namesArray.Add("ZoneRls");
            m_returnValueType = TxPlcSignalDataType.Int;
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
        public static Int32 OldinInt = -1;
        public static Int32[] AllocatedZones = new Int32[256];
        public static Int32 zone;
        public TxPlcValue Evaluate(ArrayList parameters)
        {
            

            TxPlcValue valEntrybyte = (TxPlcValue)parameters[0];
            Byte inGoCollZone = (Byte)valEntrybyte.ByteValue;
            TxPlcValue valRobNo = (TxPlcValue)parameters[1];
            Int32 inRob_No = (Int32)valRobNo.IntValue;
            TxPlcValue valZonereq = (TxPlcValue)parameters[2];
            Boolean inZoneReq = (Boolean)valZonereq.BooleanValue;
            TxPlcValue valZoneRls = (TxPlcValue)parameters[3];
            Boolean inZoneRls = (Boolean)valZoneRls.BooleanValue;

            TxPlcValue OutByte = new TxPlcValue();
            Int16 outCollZoneFree = 0;
            Int16 outCollZoneReleased = 0;
            Int16 giCollZoneTemp = 0;

            if (((!(inZoneReq || inZoneRls)) && (inGoCollZone == 0)) || (inGoCollZone > 255))
            {
                OutByte.IntValue = 0;
                
            }
            else
            {
                if (inZoneReq)
                {
                    if ((AllocatedZones[inGoCollZone] == inRob_No) || (AllocatedZones[inGoCollZone] == 0))
                    {
                        AllocatedZones[inGoCollZone] = inRob_No;
                        giCollZoneTemp = inGoCollZone;
                        outCollZoneFree = 256;

                    }
                }
                if (inZoneRls)
                {
                    if ((AllocatedZones[inGoCollZone] == inRob_No))
                    {
                        AllocatedZones[inGoCollZone] = 0;
                        giCollZoneTemp = inGoCollZone;
                    }
                    else
                    {
                        outCollZoneReleased = 512;
                    }

                }
                if (inGoCollZone == 255)
                {
                    for (int zone = 1; zone < 255; zone++)
                    {
                        if (AllocatedZones[zone] == inRob_No)
                        {
                            AllocatedZones[zone] = 0;
                        }
                    }
                    giCollZoneTemp = inGoCollZone;
                }

                OutByte.IntValue = (short)(outCollZoneReleased + outCollZoneFree + giCollZoneTemp);



            }




            return OutByte;

        }

    }
}
