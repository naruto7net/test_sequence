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
           
            template = new Template("Process",
                () =>
                {
                    Console.WriteLine("Home motor");
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Point A");
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Point B");
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Cylinder Up");
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Cylinder Down");
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Vacuum On");
                    return Task.FromResult(0);
                },
                () =>
                {
                    Console.WriteLine("Vacuum Off");
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
