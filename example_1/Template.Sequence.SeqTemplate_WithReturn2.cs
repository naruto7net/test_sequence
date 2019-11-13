﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaftechLibraries.Sequences;

namespace example_1
{
    public class Template : SequenceNoExecuteMethodAbstract
    {
        #region Fields
        string param1_;
        int param2_;
        string return_;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SeqTemplate"/> class.
        /// Customize your constructor.
        /// </summary>
        /// <param name="name">The name.</param>
        public Template(string name) : base(name)
        {
        }
        #endregion

        #region Private methods
        /// <summary>
        /// After sequence clean up.
        /// </summary>
        protected override void afterSequence()
        {
            //Clean up code here...
        }

        /// <summary>
        /// Before sequence preparation/clean up
        /// </summary>
        protected override void beforeSequence()
        {
            //Preparation/clean up code here...
        }

        /// <summary>
        /// Boiler plate codes to implement your own sequences
        /// This method will be called by ExecuteAsync method do while spin loop.
        /// Different operations can be done by changing switch expression value.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SequenceIsInvalidException"></exception>
        protected override async Task executeSequence()
        {
            //Template
            switch (seq)
            {
                case "Start.":
                    //Change "seq" value (switch expression) which determine the next case (switch section) to run in the next spin loop.
                    seq = "Do work.";

                    //If change "macroSeq" instead of "seq" value. This will mark the next case  (switch section) as stepable when IsMacroStepping() is true 
                    //macroSeq = "Do work.";
                    break;

                case "Do work.":
                    //Remember to set your return value.
                    return_ = "Hellow World!";

                    //Call this method to complete sequence.
                    completeSequence();
                    break;

                default: //When unknown case is being called due to sequence programming bug.
                    throw new SequenceIsInvalidException(seq);
            }

            await Task.FromResult(0);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Boiler plate codes to implement your own ExecuteAsync method with return value.
        /// Call this awaitable method to execute sequences.
        /// </summary>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <returns></returns>
        public async Task<string> ExecuteAsync(string param1, int param2)
        {
            try
            {
                checkExecuteAllow();
                resetTimer();
                beforeSequenceResetBaseVariables();
                beforeSequence();
                globalBusyState.IsBusy = true;
                localBusyState.IsBusy = true;

                //Validate parameters.
                #region Param validation
                param1_ = param1;
                param2_ = param2;
                #endregion

                #region Sequences
                do
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                    #region Looping...
                    #region Step by step handling
                    if (macroStepByStep == false && microStepByStep == false)
                    {
                        if (macroSeq != string.Empty)
                        {
                            seq = macroSeq;
                            macroSeq = string.Empty;
                        }
                    }
                    else
                    {
                        if (nextMacroStep == true)
                        {
                            nextMacroStep = false;
                            if (macroSeq != string.Empty)
                            {
                                seq = macroSeq;
                                macroSeq = string.Empty;
                            }
                        }
                    }

                    if (microStepByStep == true)
                    {
                        if (nextMicroStep == true)
                        {
                            nextMicroStep = false;
                            lastMicroSeq = seq;
                        }
                    }

                    #endregion

                    #region Time out handling

                    if (pause == true ||
                        (microStepByStep == true && seq != lastMicroSeq) ||
                        (macroStepByStep == true && macroSeq != string.Empty))
                    {
                        tOut.Pause();
                    }
                    else if (alarmSet == true)
                    {
                        tOut.Stop();
                    }
                    else
                    {
                        tOut.Start();
                    }

                    #endregion

                    if (pause == false && alarmSet == false &&
                        (microStepByStep == false || (microStepByStep == true && seq == lastMicroSeq)) &&
                        (macroStepByStep == false || (macroStepByStep == true && macroSeq == string.Empty)))
                    {
                        #region Pre-switch handling (Do not remove)

                        if (seq != lastSeq)
                        {
                            broadcastSequence(seq);
                            lastSeq = seq;
                        }

                        #endregion

                        await executeSequence();
                    }

                    #endregion

                    if (seqcomplete == false)
                    {
                        await Task.Delay(SequenceLoopDelay_ms);
                    }
                } while (seqcomplete == false);
                if (seqcomplete == true && IsInitializeSequence == true)
                {
                    initializeState.IsInitialized = true;
                }
                #endregion

                CycleTime = timer.Elapsed.Duration();
                broadcastLog("Sequence cycle time " + CycleTime.TotalSeconds.ToString() + "s");
                globalBusyState.IsBusy = false;
                localBusyState.IsBusy = false;
                afterSequence();

                //Return code here.
                return return_;
            }
            catch
            {
                CycleTime = timer.Elapsed.Duration();
                broadcastLog("Sequence cycle time " + CycleTime.TotalSeconds.ToString() + "s");
                globalBusyState.IsBusy = false;
                localBusyState.IsBusy = false;
                afterSequence();
                Cancel();
                throw;
            }
        }
        #endregion
    }
}