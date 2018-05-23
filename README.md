# Netstatplus
Open source netstat with superpowers for security purposes.

- for Windows 10+ so far, for GNU systems
- c# dotnet core 2.1 implementation
- netstat + whois (GNU) based

## What it does

Netstatplus monitors all incoming and outgoing connections fo all Windows processes, including self connected processes.
It gives immediately **processes names** and **IPs' organization names** and **country**.

Netstat is the first component of an **open source antimalware** for Windows and GNU systems (based on GNU and dotnet core 2.1).

When your web browser is started, you already can see all the many connections involved in tracking.

You can see all the system processes being listening to connections, and take step to harden the security of your system.

## What it will do
- process static and dynamic analysis
- connection behavior analysis
- packet analysis

and in particular:
- fine grained analysis and chosen lock up of programs and processes 
- prevent ransomware attacks
- prevent rootkits installation or actions (eg., for security analysis)
- prevent or analyze meltdown, spectre, and hardware exploits

## How to use Netstatplus
### Raw program
Build the project, netstat.dll is produced.
Start the program with `dotnet netstat.dll` or use the native launcher (and have you program installed in `c:\Program Files\Netstatplus\`, and this path to `$PATH`).
### Self-contained publication
Use `dotnet publish --self-contained -r "win10-x64"` to create a self sufficient package (here a Windows 10 x64 version; pick another one here: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).
## Suggestions and contact
Provide suggestions as issues. For other topics, just write to me at mprevot@freebsd.org
