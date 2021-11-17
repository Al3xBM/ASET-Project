using DataManipulationService.Services.TwitterApiService;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataManipulationService.MOP
{
    [Serializable]
    public class TwitterApiServiceMonitor : OnMethodBoundaryAspect
    {
        [NonSerialized]
        Stopwatch stopwatch;

        public override void OnEntry(MethodExecutionArgs args)
        {
            stopwatch = Stopwatch.StartNew();
            
            var methodArgs = args.Arguments;
            if(args.Method.Name == "GetTrendingAsync")
            {
                if(methodArgs.Count != 1 && methodArgs.First() is string)
                {
                    int val = -1;

                    if (!(int.TryParse(methodArgs.First().ToString(), out val) && val > 0))
                    File.WriteAllText("mop.txt", "GetTrendingAsync wrong call \n");
                }
                else
                {
                    File.WriteAllText("mop.txt", "GetTrendingAsync called successfully \n");
                }
            }

            base.OnEntry(args);
        }

    }
}
