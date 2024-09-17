#region Usings

using System;
using System.Diagnostics;
using System.Threading.Tasks;

#endregion

namespace Utilities {

    public class Computer {

        #region Methods

        // Get Process Utilization
        public static async Task<double> GetProcessUtilization() {
			var startTime = DateTime.UtcNow;
			var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
			await Task.Delay(500);
			var endTime = DateTime.UtcNow;
			var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
			var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
			var totalMsPassed = (endTime - startTime).TotalMilliseconds;
			var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
			return cpuUsageTotal * 100;
		}

        #endregion

    }

}
