using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Netstatplus
{
	class Program
	{
		static void Main(string[] args)
		{
			var proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "netstat",
					Arguments = "-ano",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};
			proc.Start();
			var ConList = new List<NetstatEntry>();
			while (!proc.StandardOutput.EndOfStream)
			{
				string line = proc.StandardOutput.ReadLine();
				if (line is null || !(line.Contains("TCP") | line.Contains("UDP")))
					continue;
				ConList.Add(new NetstatEntry(line));
			}

			var lastNSE = ConList.First();
			var pads = ConList.GetConnectionPads();

			foreach (var con in ConList.OrderBy(o => o.PID)
				.ThenBy(o => o.LocalPort)
				.ThenBy(o => o.ForeignPort))
			{
				var output = (con.SharePID(lastNSE) ? "" : "\n")
							 + con.GetNameAndState(lastNSE, pads);
				Console.WriteLine(output);
				lastNSE = con;
			}
		}
	}
}
