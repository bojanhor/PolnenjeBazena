using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApplication1
{
    public class Kontrola
    {
        Prop1 prop1;
        public short selectedBazen;

        public Patern Current;
        
        int MaxWaitTime_s = 60;

        public Kontrola()
        {            
            Misc.SmartThread MovementAutoResolver = new Misc.SmartThread(() => MovementResolver_method());
            MovementAutoResolver.Start("MovementAutoResolver", true);
        }

     
        public bool StartZigZag()
        {
           
            if (permisionToRun())
            {
                doZigZag();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StartZigZagzRobom()
        {
          
            if (permisionToRun())
            {
                doZigZagzRobom();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StartRobX1()
        {
            
            if (permisionToRun())
            {
                doRobX1();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StartRobX2()
        {
            
            if (permisionToRun())
            {
                doRobX2();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StartRobY1()
        {
            
            if (permisionToRun())
            {
                doRobY1();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StartRobY2()
        {
            
            if (permisionToRun())
            {
                doRobY2();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StartRobSKrozenjemX1()
        {
            

            if (permisionToRun())
            {
                doKrozniRobX1();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool StartRobSKrozenjemX2()
        {

            if (permisionToRun())
            {
                doKrozniRobX2();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool StartRobSKrozenjemY1()
        {
           

            if (permisionToRun())
            {
                doKrozniRobY1();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool StartRobSKrozenjemY2()
        {
          

            if (permisionToRun())
            {
                doKrozniRobY2();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StartCircling()
        {
            if (permisionToRun())
            {
                doCircles();
                return true;
            }
            else
            {
                return false;
            }
        }

       
        // Patern Methods
        void ZigZagPatern_method()
        {

            InitZigZag();

            var buff = ZigZagCrossFromX1Y1();

            while (true)
            {
                buff = DecideZigZagPatern(buff);
                Thread.Sleep(50); // safety
            }

        }

        void ZigZagzRobomn_method()
        {
            InitZigZag();

            var buff = ZigZagCrossFromX1Y1();

            while (true)
            {
                buff = DecideZigZagPatern(buff);
                buff = DecideRobPaternForZigZag(buff);
                buff = DecideZigZagPatern(buff);
                Thread.Sleep(50); // safety
            }

        }

        public void StopPatern()
        {
            stopMoving();
            
            if (Current != null)
            {
                Current.Stop();
            }                        
        }


        // advanced helper methods
        void doZigZag()
        {
            var paternName = "ZigZag";
            
            Action Action = new Action(() => ZigZagPatern_method());
            Patern Patern = new Patern(paternName, Action, this);

        }

        void doZigZagzRobom()
        {
            var paternName = "ZigZagzRobom";
            
            Action Action = new Action(() => ZigZagzRobomn_method());
            Patern Patern = new Patern(paternName, Action, this);

        }

        void doRobX1()
        {
            var paternName = "RobX1";
            StopPatern();
            Action Action = new Action(() => RobX1_method());
            Patern Patern = new Patern(paternName, Action, this);
        }

        void doRobX2()
        {
            var paternName = "RobX2";
            
            Action Action = new Action(() => RobX2_method());
            Patern Patern = new Patern(paternName, Action, this);
        }
        void doRobY1()
        {
            var paternName = "RobY1";
            
            Action Action = new Action(() => RobY1_method());
            Patern Patern = new Patern(paternName, Action, this);
        }

        void doRobY2()
        {
            var paternName = "RobY2";
            
            Action Action = new Action(() => RobY2_method());
            Patern Patern = new Patern(paternName, Action, this);
        }

        void doKrozniRobX1()
        {
            var paternName = "KrozniRobX1";

            Action Action = new Action(() => KrozniRobX1_method());
            Patern Patern = new Patern(paternName, Action, this);
        }
        void doKrozniRobX2()
        {
            var paternName = "KrozniRobX2";

            Action Action = new Action(() => KrozniRobX2_method());
            Patern Patern = new Patern(paternName, Action, this);
        }
        void doKrozniRobY1()
        {
            var paternName = "KrozniRobY1";

            Action Action = new Action(() => KrozniRobY1_method());
            Patern Patern = new Patern(paternName, Action, this);
        }
        void doKrozniRobY2()
        {
            var paternName = "KrozniRobY2";

            Action Action = new Action(() => KrozniRobY2_method());
            Patern Patern = new Patern(paternName, Action, this);
        }

        void doOneCircle()
        {
            var paternName = "OneCircle";
            
            Action Action = new Action(() => OneCircle_method());
            Patern Patern = new Patern(paternName, Action, this);

        }

        void doCircles()
        {
            var paternName = "Circling";
            
            Action Action = new Action(() => Circling_method());
            Patern Patern = new Patern(paternName, Action, this);
        }


        PaternStartPos DecideZigZagPatern(PaternStartPos position)
        {
            if (position == PaternStartPos.X1Y1)
            {
                return ZigZagCrossFromX1Y1();
            }
            else if (position == PaternStartPos.X1Y2)
            {
                return ZigZagCrossFromX1Y2();
            }
            else if (position == PaternStartPos.X2Y1)
            {
                return ZigZagCrossFromX2Y1();
            }
            else if (position == PaternStartPos.X2Y2)
            {
                return ZigZagCrossFromX2Y2();
            }
            else
            {
                throw new Exception("Internal error:  DecideZigZagPatern() method does not support wanted Patern.");
            }

        }

        PaternStartPos DecideRobPaternForZigZag(PaternStartPos position)
        {
            if (position == PaternStartPos.X1Y1)
            {
                return RobFromX1Y1_ForZigZag();
            }
            else if (position == PaternStartPos.X1Y2)
            {
                return RobFromX1Y2_ForZigZag();
            }
            else if (position == PaternStartPos.X2Y1)
            {
                return RobFromX2Y1_ForZigZag();
            }
            else if (position == PaternStartPos.X2Y2)
            {
                return RobFromX2Y2_ForZigZag();
            }
            else
            {
                throw new Exception("Internal error:  DecideZigZagPatern() method does not support wanted Patern.");
            }

        }

        void MovementResolver_method()
        {
            while (!Helper.LogoControllerInitialized)
            {
                Thread.Sleep(100);                
            }

            prop1 = Val.logocontroler.Prop1;
            int cnt = 0;
            try
            {
                
                while(true)
                {
                    if (isUp())
                    {
                        stopUp();
                    }

                    if (isDwn())
                    {
                        stopDwn();
                    }
                    if (isLft())
                    {
                        stopLft();
                    }

                    if (isRght())
                    {
                        stopRght();
                    }

                    if (cnt >= 5)
                    {
                        if (Val.guiController.PageDefault_ != null)
                        {
                            Val.guiController.PageDefault_.setHWLimits();
                            cnt = 0;
                        }
                    }

                    if (prop1.Ustavljeno.Value_bool)
                    {

                    }


                    cnt++;
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Movement Auto resolver has chrashed. " + ex.Message);
            }

        }


        // helpers
        void InitZigZag()
        {
            goDwn_NoWait();
            goLft_NoWait();
            waitForDwn();
            waitForLft();
        }

        PaternStartPos ZigZagCrossFromX1Y1()
        {
            while (true)
            {

                if (isUp())
                {
                    goRght();
                    return PaternStartPos.X2Y2;
                }
                else
                {
                    goRght();
                    goUpFor();
                }

                if (isUp())
                {
                    goLft();
                    return PaternStartPos.X1Y2;
                }
                else
                {
                    goLft();
                    goUpFor();
                }
                Thread.Sleep(50); // safety
            }
        }
        PaternStartPos ZigZagCrossFromX2Y1()
        {
            while (true)
            {

                if (isUp())
                {
                    goLft();
                    return PaternStartPos.X1Y2;
                }
                else
                {
                    goLft();
                    goUpFor();
                }

                if (isUp())
                {
                    goRght();
                    return PaternStartPos.X2Y2;
                }
                else
                {
                    goRght();
                    goUpFor();
                }
                Thread.Sleep(50); // safety
            }

        }

        PaternStartPos ZigZagCrossFromX1Y2()
        {
            while (true)
            {

                if (isRght())
                {
                    goDwn();
                    return PaternStartPos.X2Y1;
                }
                else
                {
                    goDwn();
                    goRghtFor();
                }

                if (isRght())
                {
                    goUp();
                    return PaternStartPos.X2Y2;
                }
                else
                {
                    goUp();
                    goRghtFor();
                }
                Thread.Sleep(50); // safety
            }

        }
        PaternStartPos ZigZagCrossFromX2Y2()
        {
            while (true)
            {
                if (isDwn())
                {
                    goLft();
                    return PaternStartPos.X1Y1;
                }
                else
                {
                    goLft();
                    goDwnFor();
                }

                if (isDwn())
                {
                    goRght();
                    return PaternStartPos.X2Y1;
                }
                else
                {
                    goRght();
                    goDwnFor();
                }
                Thread.Sleep(50); // safety
            }

        }

        PaternStartPos RobFromX1Y1_ForZigZag()
        {
            goDwn_NoWait();
            goLft_NoWait();

            waitForDwn();
            waitForLft();

            goRght();
            goLft();
            goRght();

            return PaternStartPos.X2Y1;
        }
        PaternStartPos RobFromX2Y1_ForZigZag()
        {
            goDwn_NoWait();
            goRght_NoWait();

            waitForDwn();
            waitForRght();

            goUp();
            goDwn();
            goUp();

            return PaternStartPos.X2Y2;
        }
        PaternStartPos RobFromX1Y2_ForZigZag()
        {
            goUp_NoWait();
            goLft_NoWait();

            waitForUp();
            waitForLft();

            goDwn();
            goUp();
            goDwn();

            return PaternStartPos.X1Y1;
        }
        PaternStartPos RobFromX2Y2_ForZigZag()
        {
            goUp_NoWait();
            goRght_NoWait();

            waitForUp();
            waitForRght();

            goLft();
            goRght();
            goLft();

            return PaternStartPos.X1Y2;
        }

        void RobY1_method()
        {
            goDwn_NoWait();
            goLft_NoWait();

            waitForDwn();
            waitForLft();

            while (true)
            {
                goRght();
                goLft();
                Thread.Sleep(50); // safety
            }

        }
        void RobX2_method()
        {
            goDwn_NoWait();
            goRght_NoWait();

            waitForDwn();
            waitForRght();

            while (true)
            {
                goUp();
                goDwn();
                Thread.Sleep(50); // safety
            }


        }
        void RobX1_method()
        {
            goUp_NoWait();
            goLft_NoWait();

            waitForUp();
            waitForLft();

            while (true)
            {
                goDwn();
                goUp();
                Thread.Sleep(50); // safety
            }


        }
        void RobY2_method()
        {
            goUp_NoWait();
            goRght_NoWait();

            waitForUp();
            waitForRght();

            while (true)
            {
                goLft();
                goRght();
                Thread.Sleep(50); // safety
            }


        }

        void KrozniRobY1_method()
        {
            goDwn_NoWait();
            goLft_NoWait();

            waitForDwn();
            waitForLft();

            while (true)
            {
                goRght();
                goUpFor();
                goLft();
                goDwn();
                Thread.Sleep(50); // safety
            }

        }
        void KrozniRobX2_method()
        {
            goDwn_NoWait();
            goRght_NoWait();

            waitForDwn();
            waitForRght();

            while (true)
            {
                goUp();
                goLftFor();
                goDwn();
                goRght();
                Thread.Sleep(50); // safety
            }


        }
        void KrozniRobX1_method()
        {
            goUp_NoWait();
            goLft_NoWait();

            waitForUp();
            waitForLft();

            while (true)
            {
                goDwn();
                goRghtFor();
                goUp();
                goLft();
                Thread.Sleep(50); // safety
            }


        }
        void KrozniRobY2_method()
        {
            goUp_NoWait();
            goRght_NoWait();

            waitForUp();
            waitForRght();

            while (true)
            {
                goLft();
                goDwnFor();
                goRght();
                goUp();
                Thread.Sleep(50); // safety
            }


        }

        void OneCircle_method()
        {
            goDwn_NoWait();
            goLft_NoWait();

            waitForDwn();
            waitForLft();

            goRght();
            goUp();
            goLft();
            goDwn();
        }

        void Circling_method()
        {
            goDwn_NoWait();
            goLft_NoWait();

            waitForDwn();
            waitForLft();

            while (true)
            {
                goRght();
                goUp();
                goLft();
                goDwn();
                Thread.Sleep(50); // safety
            }

        }

        // basic helper methods
        bool permisionToRun()
        {
            return prop1.PermissionToRun.Value_bool;
        }

        void goUpFor()
        {
            var currentPosY = prop1.YPos.Value_short;
            var targetPosY = currentPosY + XmlController.GetYStep();
            DateTime dtStart = DateTime.Now;
            DateTime dtStop = dtStart.AddSeconds(MaxWaitTime_s);
            goUp_NoWait();

            while (true)
            {
                currentPosY = prop1.YPos.Value_short; // update current position
                if (currentPosY >= targetPosY)
                {
                    stopUp();
                    return; // position is reached
                }

                if (isUp())
                {
                    stopUp();
                    return;
                }

                if (DateTime.Now >= dtStop)
                {
                    stopUp();
                    return; // timeout err
                }

                Thread.Sleep(XmlController.GetReadWriteCycle(1) / 2);
            }

        }

        void goDwnFor()
        {
            var currentPosY = prop1.YPos.Value_short;
            var targetPosY = currentPosY - XmlController.GetYStep();
            DateTime dtStart = DateTime.Now;
            DateTime dtStop = dtStart.AddSeconds(MaxWaitTime_s);
            goDwn_NoWait();

            while (true)
            {
                currentPosY = prop1.YPos.Value_short; // update current position
                if (currentPosY <= targetPosY)
                {
                    stopDwn();
                    return; // position is reached
                }

                if (isDwn())
                {
                    stopDwn();
                    return;
                }


                if (DateTime.Now >= dtStop)
                {
                    stopDwn();
                    return; // timeout err
                }

                Thread.Sleep(XmlController.GetReadWriteCycle(1) / 2);
            }

        }

        void goRghtFor()
        {

            var currentPosX = prop1.XPos.Value_short;
            var targetPosX = currentPosX + XmlController.GetXStep();
            DateTime dtStart = DateTime.Now;
            DateTime dtStop = dtStart.AddSeconds(MaxWaitTime_s);
            goRght_NoWait();

            while (true)
            {
                currentPosX = prop1.XPos.Value_short; // update current position
                if (currentPosX >= targetPosX)
                {
                    stopRght();
                    return; // position is reached
                }

                if (isRght())
                {
                    stopRght();
                    return;
                }

                if (DateTime.Now >= dtStop)
                {
                    stopRght();
                    return; // timeout err
                }

                Thread.Sleep(XmlController.GetReadWriteCycle(1) / 2);
            }

        }

        void goLftFor()
        {
            var currentPosX = prop1.XPos.Value_short;
            var targetPosX = currentPosX - XmlController.GetXStep();
            DateTime dtStart = DateTime.Now;
            DateTime dtStop = dtStart.AddSeconds(MaxWaitTime_s);
            goLft_NoWait();

            while (true)
            {
                currentPosX = prop1.XPos.Value_short; // update current position
                if (currentPosX <= targetPosX)
                {
                    stopLft();
                    return; // position is reached
                }

                if (isLft())
                {
                    stopLft();
                    return;
                }

                if (DateTime.Now >= dtStop)
                {
                    stopLft();
                    return; // timeout err
                }

                Thread.Sleep(XmlController.GetReadWriteCycle(1) / 2);
            }

        }

        void waitForUp()
        {
            for (int i = 0; i < MaxWaitTime_s; i++)
            {
                if (isUp()) { stopUp(); break; }
                Thread.Sleep(999);
            }
        }
        void waitForDwn()
        {
            for (int i = 0; i < MaxWaitTime_s; i++)
            {
                if (isDwn()) { stopDwn(); break; }
                Thread.Sleep(999);
            }
        }
        void waitForLft()
        {
            for (int i = 0; i < MaxWaitTime_s; i++)
            {
                if (isLft()) { stopLft(); break; }
                Thread.Sleep(999);
            }
        }
        void waitForRght()
        {
            for (int i = 0; i < MaxWaitTime_s; i++)
            {
                if (isRght()) { stopRght(); break; }
                Thread.Sleep(999);
            }
        }

        void goUp()
        {
            if (permisionToRun())
            {
                if (!isUp())
                {
                    prop1.AutoDirY2.Value_bool = true;
                    prop1.AutoDirY1.Value_bool = false;
                    waitForUp();
                }
            }
        }

        void goDwn()
        {
            if (permisionToRun())
            {
                if (!isDwn())
                {
                    prop1.AutoDirY1.Value_bool = true;
                    prop1.AutoDirY2.Value_bool = false;
                    waitForDwn();
                }
            }
        }

        void goLft()
        {
            if (permisionToRun())
            {
                if (!isLft())
                {
                    prop1.AutoDirX1.Value_bool = true;
                    prop1.AutoDirX2.Value_bool = false;
                    waitForLft();
                }
            }
        }

        void goRght()
        {
            if (permisionToRun())
            {
                if (!isRght())
                {
                    prop1.AutoDirX2.Value_bool = true;
                    prop1.AutoDirX1.Value_bool = false;
                    waitForRght();
                }
            }
        }

        void goUp_NoWait()
        {
            if (permisionToRun())
            {
                if (!isUp())
                {
                    prop1.AutoDirY2.Value_bool = true;
                    prop1.AutoDirY1.Value_bool = false;
                }
            }
        }

        void goDwn_NoWait()
        {
            if (permisionToRun())
            {
                if (!isDwn())
                {
                    prop1.AutoDirY1.Value_bool = true;
                    prop1.AutoDirY2.Value_bool = false;
                }
            }
        }

        void goLft_NoWait()
        {
            if (permisionToRun())
            {
                if (!isLft())
                {
                    prop1.AutoDirX1.Value_bool = true;
                    prop1.AutoDirX2.Value_bool = false;
                }
            }
        }

        void goRght_NoWait()
        {
            if (permisionToRun())
            {
                if (!isRght())
                {
                    prop1.AutoDirX2.Value_bool = true;
                    prop1.AutoDirX1.Value_bool = false;
                }
            }
        }

        void stopUp()
        {
            prop1.AutoDirY2.Value_bool = false;
        }
        void stopDwn()
        {
            prop1.AutoDirY1.Value_bool = false;
        }
        void stopLft()
        {
            prop1.AutoDirX1.Value_bool = false;
        }
        void stopRght()
        {
            prop1.AutoDirX2.Value_bool = false;
        }

        public void stopMoving()
        {
            prop1.Halt.SendPulse();
            stopUp(); stopDwn(); stopLft(); stopRght();
        }

        bool isUp()
        {
            return prop1.ReadKSY2.Value_bool;
        }
        bool isDwn()
        {
            return prop1.ReadKSY1.Value_bool;
        }
        bool isLft()
        {
            return prop1.ReadKSX1.Value_bool;
        }
        bool isRght()
        {
            return prop1.ReadKSX2.Value_bool;
        }

    }

    enum PaternStartPos
    {
        X1Y1, X2Y1, X1Y2, X2Y2
    }

    public class Patern
    {
        Misc.SmartThread paternThread;
        ThreadStart ts;
        public string Name = "";

        public Patern(string PaternName, Action action, Kontrola kontrola)
        {
            Name = PaternName;
            kontrola.StopPatern();

            if (kontrola.Current != null && kontrola.Current.Name == Name)
            {
                Val.logocontroler.Prop1.Halt.SendPulse();
                kontrola.Current = null;
                return;
            }
            Val.logocontroler.Prop1.Start.SendPulse();

            kontrola.Current = this;

            ts = new ThreadStart(action);
            paternThread = new Misc.SmartThread(ts);
            paternThread.Start(PaternName, ApartmentState.MTA, true);
            
        }

        public void Stop()
        {            
            paternThread.ForceAbort();
        }


    }
}