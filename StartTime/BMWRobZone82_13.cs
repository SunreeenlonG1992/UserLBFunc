using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace StartTime
{
    [TxPlcLogicBehaviorFunctionAttribute("RobZone")]
    [TxPlcLogicBehaviorFunctionDescription("The Robot inteference zone function")]
    public class BMWRobZone82_13 : ITxPlcLogicBehaviorFunction
    {
        private ArrayList m_typesArray;
        private ArrayList m_namesArray;
        private TxPlcSignalDataType m_returnValueType;
        private int zone;
        private StaticLocalInitFlag OldinIntInit;
        private StaticLocalInitFlag AllocatedZonesInit;
        private int OldinInt;
        private int[] AllocatedZones;

        public BMWRobZone82_13()
        {
            this.m_typesArray = new ArrayList();
            this.m_namesArray = new ArrayList();
            this.OldinIntInit = new StaticLocalInitFlag();
            this.AllocatedZonesInit = new StaticLocalInitFlag();
            this.m_typesArray.Add(TxPlcSignalDataType.Byte);
            this.m_typesArray.Add(TxPlcSignalDataType.Int);
            this.m_typesArray.Add(TxPlcSignalDataType.Bool);
            this.m_typesArray.Add(TxPlcSignalDataType.Bool);
            this.m_namesArray.Add("EntryByte");
            this.m_namesArray.Add("Rob_No");
            this.m_namesArray.Add("ZoneReq");
            this.m_namesArray.Add("ZoneRls");
            this.m_returnValueType = TxPlcSignalDataType.Int;
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

        public TxPlcValue Evaluate(ArrayList parameters)
        {
            TxPlcValue valEntrybyte = (TxPlcValue)parameters[0];
            checked
            {
                byte inGoCollZone = (byte)valEntrybyte.ByteValue;
                TxPlcValue valRobNo = (TxPlcValue)parameters[1];
                int inRob_No = (int)valRobNo.IntValue;
                TxPlcValue valZonereq = (TxPlcValue)parameters[2];
                bool inZoneReq = valZonereq.BooleanValue;
                TxPlcValue valZoneRls = (TxPlcValue)parameters[3];
                bool inZoneRls = valZoneRls.BooleanValue;
                TxPlcValue OutByte = new TxPlcValue();
                int outCollZoneFree = 0;
                int outCollZoneReleased = 0;
                int giCollZoneTemp = 0;
                bool flag = false;
                bool flag2;
                try
                {
                    Monitor.Enter(this.OldinIntInit, ref flag);
                    flag2 = (this.OldinIntInit.State == 0);
                    if (flag2)
                    {
                        this.OldinIntInit.State = 2;
                        this.OldinInt = -1;
                    }
                    else
                    {
                        flag2 = (this.OldinIntInit.State == 2);
                        if (flag2)
                        {
                            throw new IncompleteInitialization();
                        }
                    }
                }
                finally
                {
                    this.OldinIntInit.State = 1;
                    flag2 = flag;
                    if (flag2)
                    {
                        Monitor.Exit(this.OldinIntInit);
                    }
                }
                bool flag3 = false;
                try
                {
                    Monitor.Enter(this.AllocatedZonesInit, ref flag3);
                    flag2 = (this.AllocatedZonesInit.State == 0);
                    if (flag2)
                    {
                        this.AllocatedZonesInit.State = 2;
                        this.AllocatedZones = new int[256];
                    }
                    else
                    {
                        flag2 = (this.AllocatedZonesInit.State == 2);
                        if (flag2)
                        {
                            throw new IncompleteInitialization();
                        }
                    }
                }
                finally
                {
                    this.AllocatedZonesInit.State = 1;
                    flag2 = flag3;
                    if (flag2)
                    {
                        Monitor.Exit(this.AllocatedZonesInit);
                    }
                }
                
                if (((!(inZoneReq | inZoneRls) & inGoCollZone == 0) | inGoCollZone > 255))
                {
                    OutByte.IntValue = 0;
                   
                }
                else
                {
                    
                    if (inZoneReq)
                    {
                        
                        if ((this.AllocatedZones[(int)inGoCollZone] == inRob_No | this.AllocatedZones[(int)inGoCollZone] == 0))
                        {
                            this.AllocatedZones[(int)inGoCollZone] = inRob_No;
                            giCollZoneTemp = (int)inGoCollZone;
                            outCollZoneFree = 256;
                        }
                    }
                    
                    if (inZoneRls)
                    {
                        
                        if ((this.AllocatedZones[(int)inGoCollZone] == inRob_No))
                        {
                            this.AllocatedZones[(int)inGoCollZone] = 0;
                            giCollZoneTemp = (int)inGoCollZone;
                        }
                        outCollZoneReleased = 512;
                    }
                   
                    if (inGoCollZone == 255)
                    {
                        this.zone = 1;
                        int arg_25C_0;
                        int num4;
                        do
                        {
                            //flag2 = (this.AllocatedZones[this.zone] == inRob_No);
                            if ((this.AllocatedZones[this.zone] == inRob_No))
                            {
                                this.AllocatedZones[this.zone] = 0;
                            }
                            this.zone++;
                            arg_25C_0 = this.zone;
                            num4 = 254;
                        }
                        while (arg_25C_0 <= num4);
                        giCollZoneTemp = (int)inGoCollZone;
                    }
                    OutByte.IntValue = ((short)(outCollZoneReleased + outCollZoneFree + giCollZoneTemp));
                    
                }
                return OutByte;
            }
        }
    }
}
