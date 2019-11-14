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

        
           // WaftechLibraries.Sequences.InitializeStatus initStatus = new WaftechLibraries.Sequences.InitializeStatus();
            template.EnableInitializeCheck = true;
            //template.InjectInitStateObject(initStatus);
            //template.EnableInitializeCheck = true;

            template = new Template("Process",
                () =>
                {
                    Console.WriteLine("Home motor");
                   
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Point A");
                    //Console.WriteLine("STATUS : " + template.SequenceStatus.ToString());
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Point B");
                    //Console.WriteLine("STATUS : " + template.SequenceStatus.ToString());
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Cylinder Up");
                    //Console.WriteLine("STATUS : " + template.SequenceStatus.ToString());
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Cylinder Down");
                    //Console.WriteLine("STATUS : " + template.SequenceStatus.ToString());
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Vacuum On");
                    //Console.WriteLine("STATUS : " + template.SequenceStatus.ToString());
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Vacuum Off");
                    //Console.WriteLine("STATUS : " + template.SequenceStatus.ToString());
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
