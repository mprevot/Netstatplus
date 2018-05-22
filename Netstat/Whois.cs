using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netstatplus
{
    public class Whois
    {
	    private string _ip;
	    private Process _whoisProcess;
	    private Task _getOrgTask;

	    public string IP
	    {
		    get => _ip;
		    set
		    {
			    _ip = value;
			    _whoisProcess = new Process
			    {
				    StartInfo = new ProcessStartInfo
				    {
					    FileName = "bash",
					    Arguments = $"-c \"whois {_ip}\"",
					    UseShellExecute = false,
					    RedirectStandardOutput = true,
					    CreateNoWindow = true
				    }
			    };

			}
	    }
	    public string OrgName { get; set; }
		public string Organization { get; set; }
	    public string Country { get; set; }
		public string Domain { get; set; }

	    public Whois(string ip)
	    {
		    IP = ip;
		    GetOrg();
	    }

	    protected void GetOrg()
	    {
		    switch (IP)
		    {
			    case null:
			    case "0.0.0.0":
			    case "127.0.0.1":
			    case "[::]":
			    case "*":
				    return;
		    }

		    _getOrgTask = new Task(() =>
			{
				_whoisProcess.Start();
				var whoisResult = new List<string>();
				while (!_whoisProcess.StandardOutput.EndOfStream)
				{
					string line = _whoisProcess.StandardOutput.ReadLine();
					if (line is null) continue;
					if (line.StartsWith("Organization"))
						Organization = line.Split(":")[1].Trim();
					if (line.StartsWith("OrgName"))
						OrgName = line.Split(":")[1].Trim();
					if (line.StartsWith("Country"))
						Country = line.Split(":")[1].Trim();
				}
			});
			_getOrgTask.Start();
		}

	    public override string ToString()
	    {
		    _getOrgTask?.Wait(200);
		    var elements = new List<object> {OrgName, Country};
		    var result = string.Join(" ", elements.Where(o => !(o is null || o.Equals(string.Empty))).Select(o => o));
			return result;
	    }
    }
}
