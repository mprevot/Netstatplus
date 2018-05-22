using System.Collections.Generic;
using System.Linq;

namespace Netstatplus
{
	public enum State
	{
		Listening,
		CloseWait,
		TimeWait,
		Established,
		NA
	}
	public enum Protocol
	{
		TCP,
		UDP
	}

	public struct ConPads
	{
		public int processPad;
		public int pidPad;
		public int statePad;
		public int laPad;
		public int faPad;
		public int whoisPad;

		public ConPads(List<NetstatEntry> connectionList)
		{
			processPad = connectionList.Max(o => o.ProcessName.Length) + 1;
			pidPad = connectionList.Max(o => o.PID.ToString().Length) + 1;
			statePad = connectionList.Max(o => o.State.ToString().Length) + 1;
			laPad = connectionList.Max(o => o.LocalAddress.ToString().Length) + 1;
			faPad = connectionList.Max(o => o.ForeignAddress.ToString().Length) + 1;
			whoisPad = connectionList.Max(o => o.Whois.ToString().Length) + 1;
		}
	}
}