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
        bool _ZigzagInProcess = false;
        bool _ZigzagzRobomInProcess = false;
        bool _RobX1InProcess = false;
        bool _RobY1InProcess = false;
        bool _RobX2InProcess = false;
        bool _RobY2InProcess = false;
        bool _KrozenjeInProcess = false;
        bool _EnKrogInProcess = false;
        bool permissionToRunLocal = false;

        public bool ZigzagInProcess 
        { get { return _ZigzagInProcess; } set { _ZigzagInProcess = value; } }
        public bool ZigzagzRobomInProcess 
        { get { return _ZigzagzRobomInProcess; } set { _ZigzagzRobomInProcess = value; } }
        public bool RobX1InProcess 
        { get { return _RobX1InProcess; } set { _RobX1InProcess = value; } }
        public bool RobY1InProcess 
        { get { return _RobY1InProcess; } set { _RobY1InProcess = value; } }
        public bool RobX2InProcess 
        { get { return _RobX2InProcess; } set { _RobX2InProcess = value; } }
        public bool RobY2InProcess 
        { get { return _RobY2InProcess; } set { _RobY2InProcess = value; } }
        public bool KrozenjeInProcess
        { get { return _KrozenjeInProcess; } set { _KrozenjeInProcess = value; } }
        public bool EnKrogInProcess 
        { get { return _EnKrogInProcess; } set { _EnKrogInProcess = value; } }

        Patern Current;
        int MaxWaitTime_s = 60;
        int X_Steps = XmlController.GetXStep();
        int Y_Steps = XmlController.GetYStep();

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

        public bool StartOneCircle()
        {
            if (permisionToRun())
            {
                doOneCircle();
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
            }

        }

        public void StopPatern()
        {
            stopMoving();
            
            if (Current != null)
            {
                Current.Stop();
            }

             ZigzagInProcess = false;
             ZigzagzRobomInProcess = false;
             RobX1InProcess = false;
             RobY1InProcess = false;
             RobX2InProcess = false;
             RobY2InProcess = false;
             KrozenjeInProcess = false;
             EnKrogInProcess = false;
        }


        // advanced helper methods
        void doZigZag()
        {
            StopPatern();
            Action Action = new Action(() => ZigZagPatern_method());
            Patern Patern = new Patern("ZigZag", Action);
            Current = Patern;

            ZigzagInProcess = true;

        }

        void doZigZagzRobom()
        {
            StopPatern();
            Action Action = new Action(() => ZigZagzRobomn_method());
            Patern Patern = new Patern("ZigZagzRobom", Action);
            Current = Patern;

            ZigzagzRobomInProcess = true;

        }

        void doRobX1()
        {
            StopPatern();
            Action Action = new Action(() => RobX1_method());
            Patern Patern = new Patern("RobX1", Action);
            Current = Patern;

            RobX1InProcess = true;

        }

        void doRobX2()
        {
            StopPatern();
            Action Action = new Action(() => RobX2_method());
            Patern Patern = new Patern("RobX1", Action);
            Current = Patern;
    
            RobX2InProcess = true;

        }
        void doRobY1()
        {
            StopPatern();
            Action Action = new Action(() => RobY1_method());
            Patern Patern = new Patern("RobY1", Action);
            Current = Patern;

            RobY1InProcess = true;

        }

        void doRobY2()
        {
            StopPatern();
            Action Action = new Action(() => RobY2_method());
            Patern Patern = new Patern("RobY2", Action);
            Current = Patern;

            RobY2InProcess = true;

        }

        void doOneCircle()
        {
            StopPatern();
            Action Action = new Action(() => OneCircle_method());
            Patern Patern = new Patern("OneCircle", Action);
            Current = Patern;

            EnKrogInProcess = true;

        }

        void doCircles()
        {
            StopPatern();
            Action Action = new Action(() => Circling_method());
            Patern Patern = new Patern("Circling", Action);
            Current = Patern;
            
            KrozenjeInProcess = true;
          
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
            permissionToRunLocal = true;
            try
            {
                while (true)
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
                                        
                   
                    Thread.Sleep(150);
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
            }

        }

        // basic helper methods
        bool permisionToRun()
        {
            return prop1.PermissionToRun.Value_bool;
        }

        void goUpFor()
        {
            var impulses = Y_Steps;
            if (impulses < 1)
            {
                impulses = 1; // safety check value
            }

            var currentPosY = prop1.YImpulses.Value_short;
            var targetPosY = currentPosY + impulses;
            DateTime dtStart = DateTime.Now;
            DateTime dtStop = dtStart.AddSeconds(MaxWaitTime_s);
            while (true)
            {
                currentPosY = prop1.YImpulses.Value_short; // update current position

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
            var impulses = Y_Steps;
            if (impulses < 1)
            {
                impulses = 1; // safety check value
            }

            var currentPosY = prop1.YImpulses.Value_short;
            var targetPosY = currentPosY - impulses;
            DateTime dtStart = DateTime.Now;
            DateTime dtStop = dtStart.AddSeconds(MaxWaitTime_s);
            while (true)
            {
                currentPosY = prop1.YImpulses.Value_short; // update current position

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
            var impulses = X_Steps;
            if (impulses < 1)
            {
                impulses = 1; // safety check value
            }

            var currentPosX = prop1.XImpulses.Value_short;
            var targetPosX = currentPosX + impulses;
            DateTime dtStart = DateTime.Now;
            DateTime dtStop = dtStart.AddSeconds(MaxWaitTime_s);
            while (true)
            {
                currentPosX = prop1.XImpulses.Value_short; // update current position

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
            var impulses = X_Steps;
            if (impulses < 1)
            {
                impulses = 1; // safety check value
            }

            var currentPosX = prop1.XImpulses.Value_short;
            var targetPosX = currentPosX - impulses;
            DateTime dtStart = DateTime.Now;
            DateTime dtStop = dtStart.AddSeconds(MaxWaitTime_s);
            while (true)
            {
                currentPosX = prop1.XImpulses.Value_short; // update current position

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

        void updateSteps()
        {
            X_Steps = XmlController.GetXStep();
            Y_Steps = XmlController.GetYStep();
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

        public Patern(string PaternName, Action action)
        {
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