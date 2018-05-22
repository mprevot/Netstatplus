using System;
using System.Collections.Generic;
using System.Text;

namespace Netstatplus
{
    public static class Extensions
    {
	    public static State ToState(this string s)
	    {
		    if (s == "LISTENING")
			    return State.Listening;
		    if (s == "ESTABLISHED")
			    return State.Established;
		    if (s == "CLOSE_WAIT")
			    return State.CloseWait;
		    if (s == "TIME_WAIT")
			    return State.TimeWait;
			return State.NA;
		}

	    public static Protocol ToProtocol(this string s)
	    {
			return  s == "TCP" ? Protocol.TCP : Protocol.UDP;
		}

	    public static ConPads GetConnectionPads(this List<NetstatEntry> connections)
	    {
			return new ConPads(connections);
	    }
	}
}
