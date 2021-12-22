using DataManipulationService.Models;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DataManipulationService.MOP
{

    [Serializable]
    public class TwitterApiServiceMonitor : OnMethodBoundaryAspect
    {
        //public override void OnResume(MethodExecutionArgs args)
        //{
        //    var temp = args.YieldValue;

        //    base.OnResume(args);
        //}

        //public override void OnYield(MethodExecutionArgs args)
        //{
        //    base.OnYield(args);
        //}

        [NonSerialized]
        Stopwatch stopwatch;
        public override void OnEntry(MethodExecutionArgs args)
        {
            stopwatch = Stopwatch.StartNew();

            var methodArgs = args.Arguments;
            if (args.Method.Name == "GetTrendingAsync")
            {
                if (methodArgs.Count != 1 && methodArgs.First() is string)
                {
                    int val = -1;

                    if (!(int.TryParse(methodArgs.First().ToString(), out val) && val > 0))
                        File.AppendAllText("mop.txt", "GetTrendingAsync wrong arguments \n");
                }
                else
                {
                    File.AppendAllText("mop.txt", "GetTrendingAsync called successfully \n");
                }
            }
            if (args.Method.Name == "GetAvailableTrendsAsync")
            {

                File.AppendAllText("mop.txt", "GetAvailableTrendsAsync called successfully \n");
            }
            if (args.Method.Name == "GetTweetsSample")
            {
                File.AppendAllText("mop.txt", "GetTweetsSample called successfully \n");
            }

            base.OnEntry(args);
        }
        public override void OnExit(MethodExecutionArgs args)
        {

            var result = args.ReturnValue;
            if (args.Method.Name == "GetAvailableTrendsAsync" || args.Method.Name == "GetTrendingAsync")
            {
                if (result is null || result.ToString().Contains("errors"))
                {
                    File.AppendAllText("mop.txt", $"{args.Method.Name} wrong response  \n");
                }
                else
                {
                    File.AppendAllText("mop.txt", $"{args.Method.Name} correct response  \n");
                }
            }
            if (args.Method.Name == "GetTweetsSample")
            {
                if (result is null || result.GetType() != typeof(List<Tweet>))
                {
                    File.AppendAllText("mop.txt", $"{args.Method.Name} wrong response  \n");
                }
                else
                {
                    File.AppendAllText("mop.txt", $"{args.Method.Name} correct response  \n");
                }
            }

            base.OnExit(args);
        }

    }
}
