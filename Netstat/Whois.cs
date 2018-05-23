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
		public List<string> Organization { get; set; } = new List<string>();
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
				bool gotDescr = false;
				bool gotCountry = false;
				var headers = new List<string> { "organization", "orgname", "org-name", "descr" };
				while (!_whoisProcess.StandardOutput.EndOfStream)
				{
					var line = _whoisProcess.StandardOutput.ReadLine();
					if (line is null) continue;
					var header = line.Split(":")[0].ToLower();
					if (headers.Where(o => header.StartsWith(o)).Select(o => o).Any())
					{
						if (header.StartsWith("descr"))
						{
							if (gotDescr) continue;
							gotDescr = true;
						}
						Organization.Add(line.Split(":")[1].Trim());
						continue;
					}
					if (header.StartsWith("country"))
					{
						if (gotCountry) continue;
						gotCountry = true;
						Country = line.Split(":")[1].Trim();
					}
				}
			});
			_getOrgTask.Start();
		}

	    public override string ToString()
	    {
		    _getOrgTask?.Wait(200);
		    var finalOrg = "";
			if (Organization.Count > 0)
				finalOrg = Organization.OrderBy(o => o.Contains("(")).First();
			var elements = new List<object> { finalOrg, Country};
		    var result = string.Join(" ", elements.Where(o => !(o is null || o.Equals(string.Empty))).Select(o => o));
			return result;
	    }
    }
}
