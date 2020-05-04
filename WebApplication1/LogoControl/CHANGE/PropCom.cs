using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WebApplication1
{
    
    public class PropComm
    {
        public const string NA = "_N/A_";

        public Sharp7.S7Client Client;        
        public AutoSync AutoSync = new AutoSync();
        bool firstSync = true;
        int poolCnt = 1;
        
        public PropComm(Sharp7.S7Client client)
        {
            Client = client;    
        }

        public void SyncVars()
        {
            FirstSync();

            AutoSync.VarsListPool[1].ForEach(plcVar => plcVar.SyncWithPLC());

            //
            if (IsItsTurn(2))
            {
                AutoSync.VarsListPool[2].ForEach(plcVar => plcVar.SyncWithPLC());
            }

            //
            if (IsItsTurn(3))
            {
                AutoSync.VarsListPool[3].ForEach(plcVar => plcVar.SyncWithPLC());
            }

            //
            if (IsItsTurn(4))
            {
                AutoSync.VarsListPool[4].ForEach(plcVar => plcVar.SyncWithPLC());
            }

            //
            if (IsItsTurn(5))
            {
                AutoSync.VarsListPool[5].ForEach(plcVar => plcVar.SyncWithPLC());
            }

            PoolCnt();
        }

        void FirstSync()
        {
            if (!firstSync)
            {
                return;         
            }

            // code only on first sync
            foreach (var item in AutoSync.VarsList)
            {                
                AutoSync.VarsListPool[item.SyncEvery_X_Time].Add(item); // adds variables to different pools of syncing. if variable must be synced every 2nd loop then for is added to list 2; 
            }

                firstSync = false;
        }

        void PoolCnt()
        {
            if (poolCnt >= 60) // common denominator of 1,2,3,4,5 is 60
            {
                poolCnt = 1;
            }
            else
            {
                poolCnt++;
            }
        }

        bool IsItsTurn(int turn)
        {
            if (turn == 2) // every 2 loops
            {
                if (poolCnt % 2 == 0)
                {
                    return true;
                }                
            }

            if (turn == 3)// every 3 loops
            {
                if (poolCnt % 3 == 0)
                {
                    return true;
                }
            }

            if (turn == 4)// every 4 loops
            {
                if (poolCnt % 4 == 0)
                {
                    return true;
                }
            }

            if (turn == 5)// every 5 loops
            {
                if (poolCnt % 5 == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class AutoSync
    {
        public List<PlcVars.PlcType> VarsList { get; private set; }
        public List<PlcVars.PlcType>[] VarsListPool = new List<PlcVars.PlcType>[6]; // different pools for syncing every other loop
        

        public AutoSync()
        {
            VarsList = new List<PlcVars.PlcType>();

            for (int i = 1; i < 5; i++)
            {
                VarsListPool[i] = new List<PlcVars.PlcType>();
            }
            
        }
        public void Add(PlcVars.PlcType var)
        {
            VarsList.Add(var);
        }
    }
        
}


