using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace example_1
{
    class Process 
    {
        Template template;
        

        public Process()
        {

            #region TESTING

            //Galil.IGalil g = new Galil.Galil();
            //g.address = "192.168.1.2";

            WaftechLibraries.Motion.GalilMotion.ICommandSimulator sim = new WaftechLibraries.Motion.GalilMotion.CommandSimulator(1);
            Galil.IGalil g2 = new WaftechLibraries.Motion.GalilMotion.GalilSimulator(sim);

            WaftechLibraries.IO.WaftechGalil.WafGalilIO ioCard = new WaftechLibraries.IO.WaftechGalil.WafGalilIO(g2);

            WaftechLibraries.IO.DummyIOController io = new WaftechLibraries.IO.DummyIOController();


            WaftechLibraries.Motion.AbstractAxisParameter param = new WaftechLibraries.Motion.AxisParameter();
            param.PulseToRealWorldUnitMultiplier = 1;
            param.EncoderToReferencePositionMultiplier = 1;
            WaftechLibraries.Motion.AxisAbstract a = new WaftechLibraries.Motion.GalilMotion.GalilAxis("X", g2, () => param, () => io.DigitalOutputSet(0, string.Empty), () => io.DigitalInputStatus(0, string.Empty), () => io.DigitalInputStatus(0, string.Empty));


            // WaftechLibraries.Sequences.InitializeStatus initStatus = new WaftechLibraries.Sequences.InitializeStatus();
            //template.EnableInitializeCheck = true;
            //template.InjectInitStateObject(initStatus);
            //template.EnableInitializeCheck = true;

            #endregion

            template = new Template("Process",
                 async () =>
                {
                    Console.WriteLine("Home motor");
                    await a.HomeAsync();
                    //return Task.FromResult(0);
                },
                async () =>
                {
                    Console.WriteLine("Point A");
                    //await a.MoveAbsoluteAsync(0, 50000, 100000, 100000, 0, 0,0, true, null, null);
                    await a.MoveAbsoluteAsync(0, 50000, 100000, 100000, 1, 1, 1, true, () => { }, () => { });
                    //return Task.FromResult(0);

                },
                async () =>
                {
                    Console.WriteLine("Point B");
                    await a.MoveAbsoluteAsync(108000, 50000, 100000, 100000,1, 1, 1, true, null, null);
                    //return Task.FromResult(0);
                },   
                () =>
                {
                    Console.WriteLine("Cylinder Up");
                     ioCard.DigitalOutputClear(1, string.Empty);
                    return Task.FromResult(0);

                },
                () =>
                {
                    Console.WriteLine("Cylinder Down");
                     ioCard.DigitalOutputSet(1, string.Empty);
                    return Task.FromResult(0);
                },
               () =>
                {
                    Console.WriteLine("Vacuum On");
                     ioCard.DigitalOutputClear(2, string.Empty);
                    return Task.FromResult(0);
                },
                 () =>
                {
                    Console.WriteLine("Vacuum Off");
                     ioCard.DigitalOutputSet(2, string.Empty);
                    return Task.FromResult(0);
                });

            //WaftechLibraries.Log.DefaultLogger logger = new WaftechLibraries.Log.DefaultLogger();
            //seqUp.GetSetLogger = logger;
           

            template.GetSetLogger.LogBroadcast += GetSetLogger_LogBroadcast;
            template.GetSetAlarmHandler.AlarmBroadcast += GetSetAlarmHandler_AlarmBroadcast;
        }

        private void GetSetAlarmHandler_AlarmBroadcast(object sender, WaftechLibraries.Alarm.AlarmBroadcastEventArgs e)
        {
            ////throw new NotImplementedException();
        }

        private void GetSetLogger_LogBroadcast(object sender, WaftechLibraries.Log.LogBroadcastEventArgs e)
        {
            Console.WriteLine(" Name=" + e.BroadcasterName + " DateTime=" + e.MessageDateTime + " Message=" + e.Message);
        }

        public Task MoveAndToggleAsync(int cycle)
        {
            return template.ExecuteAsync(cycle);
        }

    }
}
