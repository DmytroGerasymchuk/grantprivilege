using System;
using System.Reflection;
using System.Security.Principal;
using System.Collections.Generic;

namespace grantprivilege
{
    class Program
    {
        static Int32 Main(string[] args)
        {
            Console.WriteLine(ProgramTitleString + " " + CopyrightString);
            Console.WriteLine(FullVersionString);
            
            if (args.Length != 2)
            {
                ColorWriteLine(ConsoleColor.Red, "Invalid arguments.");

                Console.WriteLine();

                Console.Write("Usage: " + System.IO.Path.GetFileName(Assembly.GetEntryAssembly().Location) + " ");
                ColorWriteLine(ConsoleColor.White, "ACCOUNT-NAME RIGHT-OR-PRIVILEGE-NAME-OR-SHORTCUT");

                Console.WriteLine();

                ColorWrite(ConsoleColor.White, "ACCOUNT-NAME");
                Console.WriteLine(" --> name of target Windows account, for example DOMAIN\\USER");
                
                ColorWrite(ConsoleColor.White, "RIGHT-OR-PRIVILEGE-NAME-OR-SHORTCUT");
                Console.WriteLine(" --> string constant which defines which right or privilege to grant, see the following lists:");

                Console.WriteLine();
                ColorWriteLine(ConsoleColor.White, "Shortcuts");
                Console.WriteLine("sql --> SeLockMemoryPrivilege plus SeManageVolumePrivilege");

                Console.WriteLine();
                ColorWriteLine(ConsoleColor.White, "Account Rights Constants");
                Console.WriteLine("SeBatchLogonRight");
                Console.WriteLine("SeDenyBatchLogonRight");
                Console.WriteLine("SeDenyInteractiveLogonRight");
                Console.WriteLine("SeDenyNetworkLogonRight");
                Console.WriteLine("SeDenyRemoteInteractiveLogonRight");
                Console.WriteLine("SeDenyServiceLogonRight");
                Console.WriteLine("SeInteractiveLogonRight");
                Console.WriteLine("SeNetworkLogonRight");
                Console.WriteLine("SeRemoteInteractiveLogonRight");
                Console.WriteLine("SeServiceLogonRight");

                Console.WriteLine();
                ColorWriteLine(ConsoleColor.White, "Privilege Constants");
                Console.WriteLine("SeAssignPrimaryTokenPrivilege --> Replace a process-level token");
                Console.WriteLine("SeAuditPrivilege --> Generate security audits");
                Console.WriteLine("SeBackupPrivilege --> Back up files and directories");
                Console.WriteLine("SeChangeNotifyPrivilege --> Bypass traverse checking");
                Console.WriteLine("SeCreateGlobalPrivilege --> Create global objects");
                Console.WriteLine("SeCreatePagefilePrivilege --> Create a pagefile");
                Console.WriteLine("SeCreatePermanentPrivilege --> Create permanent shared objects");
                Console.WriteLine("SeCreateSymbolicLinkPrivilege --> Create symbolic links");
                Console.WriteLine("SeCreateTokenPrivilege --> Create a token object");
                Console.WriteLine("SeDebugPrivilege --> Debug programs");
                Console.WriteLine("SeEnableDelegationPrivilege --> Enable computer and user accounts to be trusted for delegation");
                Console.WriteLine("SeImpersonatePrivilege --> Impersonate a client after authentication");
                Console.WriteLine("SeIncreaseBasePriorityPrivilege --> Increase scheduling priority");
                Console.WriteLine("SeIncreaseQuotaPrivilege --> Adjust memory quotas for a process");
                Console.WriteLine("SeIncreaseWorkingSetPrivilege --> Increase a process working set");
                Console.WriteLine("SeLoadDriverPrivilege --> Load and unload device drivers");
                Console.WriteLine("SeLockMemoryPrivilege --> Lock pages in memory");
                Console.WriteLine("SeMachineAccountPrivilege --> Add workstations to domain");
                Console.WriteLine("SeManageVolumePrivilege --> Manage the files on a volume");
                Console.WriteLine("SeProfileSingleProcessPrivilege --> Profile single process");
                Console.WriteLine("SeRelabelPrivilege --> Modify an object label");
                Console.WriteLine("SeRemoteShutdownPrivilege --> Force shutdown from a remote system");
                Console.WriteLine("SeRestorePrivilege --> Restore files and directories");
                Console.WriteLine("SeSecurityPrivilege --> Manage auditing and security log");
                Console.WriteLine("SeShutdownPrivilege --> Shut down the system");
                Console.WriteLine("SeSyncAgentPrivilege --> Synchronize directory service data");
                Console.WriteLine("SeSystemEnvironmentPrivilege --> Modify firmware environment values");
                Console.WriteLine("SeSystemProfilePrivilege --> Profile system performance");
                Console.WriteLine("SeSystemtimePrivilege --> Change the system time");
                Console.WriteLine("SeTakeOwnershipPrivilege --> Take ownership of files or other objects");
                Console.WriteLine("SeTcbPrivilege --> Act as part of the operating system");
                Console.WriteLine("SeTimeZonePrivilege --> Change the time zone");
                Console.WriteLine("SeTrustedCredManAccessPrivilege --> Access Credential Manager as a trusted caller");
                Console.WriteLine("SeUndockPrivilege --> Remove computer from docking station");
                Console.WriteLine("SeUnsolicitedInputPrivilege --> Required to read unsolicited input from a terminal device");

                return 1;
            }

            List<String> Rights = new List<String>();

            if (args[1].CompareTo("sql") == 0)
            {
                Rights.Add("SeLockMemoryPrivilege");
                Rights.Add("SeManageVolumePrivilege");
            }
            else
                Rights.Add(args[1]);

            long Result;

            foreach (String Right in Rights)
            {
                ColorWrite(ConsoleColor.Yellow, "Granting ");
                ColorWrite(ConsoleColor.White, Right);
                ColorWrite(ConsoleColor.Yellow, " to ");
                ColorWrite(ConsoleColor.White, args[0]);
                ColorWriteLine(ConsoleColor.Yellow, "...");

                Result = LsaUtility.AddRight(args[0], Right);

                if (Result != 0)
                {
                    Console.WriteLine("System error code: {0}.", Result);
                    return 666;
                }
            }

            return 0;
        }

        private static void ColorWriteLine(ConsoleColor C, String Message)
        {
            ColorWrite(C, Message);
            Console.WriteLine();
        }

        private static void ColorWrite(ConsoleColor C, String Message)
        {
            Console.ForegroundColor = C;
            Console.Write(Message);
            Console.ResetColor();
        }

        private static String ProgramTitleString
        {
            get
            {
                Assembly A = Assembly.GetEntryAssembly();
                Object[] Attributes = A.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (Attributes.Length > 0)
                {
                    AssemblyTitleAttribute TitleAttribute = (AssemblyTitleAttribute)Attributes[0];
                    if (TitleAttribute.Title.Length > 0) return TitleAttribute.Title;
                }
                return A.GetName().Name;
            }
        }

        public static string CopyrightString
        {
            get
            {
                Assembly A = Assembly.GetEntryAssembly();
                Object[] Attributes = A.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return Attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)Attributes[0]).Copyright;
            }
        }

        private static String FullVersionString
        {
            get
            {
                Assembly A = Assembly.GetEntryAssembly();
                AssemblyName AN = A.GetName();
                Version AV = AN.Version;
                return
                    "Version " + AV.Major.ToString() +
                    "." + AV.Minor.ToString() +
                    " Build " + AV.Build.ToString() +
                    " Revision " + AV.Revision.ToString();
            }
        }

    }
}
