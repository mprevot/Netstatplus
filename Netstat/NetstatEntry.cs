using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Netstat
{
	public class NetstatEntry
	{
		private string _foreignAddress = "";
		private string _foreignIp;
		public Protocol Protocol { get; set; }
		public string LocalAddress { get; set; } = "";
		public string LocalPort
		{
			get
			{
				var bits = LocalAddress.Split(":");
				return bits[bits.Length - 1];
			}
		}

		public string ForeignAddress
		{
			get => _foreignAddress;
			set
			{
				_foreignAddress = value;
				var idx = _foreignAddress.LastIndexOf(":", StringComparison.Ordinal);
				ForeignIP = _foreignAddress.Substring(0, idx);
			}
		}

		public string ForeignIP
		{
			get => _foreignIp;
			set
			{
				_foreignIp = value;
				Whois = new Whois(_foreignIp);
			}
		}

		public string ForeignOrg { get; set; } = "";
		public string ForeignPort
		{
			get
			{
				var bits = ForeignAddress.Split(":");
				return bits[bits.Length - 1];
			}
		}

		public Whois Whois { get; set; }
		public State State { get; set; } = State.NA;
		public int PID { get; set; } = -1;
		public string ProcessName { get; set; } = "";
		public bool ShareLocalPort(NetstatEntry entry) => entry.LocalPort.Equals(LocalPort);
		public bool ShareForeignPort(NetstatEntry entry) => entry.ForeignPort.Equals(ForeignPort);
		public bool SharePID(NetstatEntry entry) => entry.PID.Equals(PID);
		public bool AutoConnects(NetstatEntry entry) => SharePID(entry) && entry.LocalPort.Equals(ForeignPort) && entry.ForeignPort.Equals(LocalPort);

		public NetstatEntry(string entry)
		{
			var entries = Regex.Split(entry, @"\s+")
				.Where(o => o.Length >= 1).Select(o => o).ToList();
			if (entries.Count == 0 | entries.Count == 2 | entries.Count == 7)
				return;
			if (entries.Count == 5)
			{
				Protocol = entries[0].ToProtocol();
				LocalAddress = entries[1];
				ForeignAddress = entries[2];
				State = entries[3].ToState();
				PID = int.Parse(entries[4]);
				ProcessName = Process.GetProcessById(PID).ProcessName;
				return;
			}
			if (entries.Count == 4)
			{
				Protocol = entries[0].ToProtocol();
				LocalAddress = entries[1];
				ForeignAddress = entries[2];
				PID = int.Parse(entries[3]);
				ProcessName = Process.GetProcessById(PID).ProcessName;
				return;
			}

			Console.WriteLine($"unknown case: {entries.Count}\n{entry}\n{string.Join("][",entries)}");
		}

		public string GetNameAndState()
		{
			return $"{ProcessName.PadRight(25)} {State.ToString().PadRight(15)} {PID.ToString().PadRight(7)} {LocalAddress.PadRight(20)} {ForeignAddress.PadRight(20)}";
		}

		public string GetNameAndState(NetstatEntry nse, ConPads pads)
		{
			var output = "";
			if (SharePID(nse))
			{
				output += $"{"".PadRight(pads.processPad)} {"".PadRight(pads.pidPad)} {State.ToString().PadRight(pads.statePad)} {LocalAddress.PadRight(pads.laPad)} {ForeignAddress.PadRight(pads.faPad)} ";
				output += AutoConnects(nse) ? "[localhost selfconnect]" : $"{Whois.ToString().PadRight(pads.whoisPad)}";
				return output;
			}
			return $"{ProcessName.PadRight(pads.processPad)} {PID.ToString().PadRight(pads.pidPad)} {State.ToString().PadRight(pads.statePad)} {LocalAddress.PadRight(pads.laPad)} {ForeignAddress.PadRight(pads.faPad)} {Whois.ToString().PadRight(pads.whoisPad)}";
		}
	}
}