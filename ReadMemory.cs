using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics; // for Process
using System.Runtime.InteropServices; // for PInvoke calls
using System.Windows.Forms;
using ProcessPrivileges;

namespace Penguin2
{
    //ReadProcessMemory:
    //pHandle, our IntPtr to the process we opened.
    //Address, where we start to read Out of The Memory
    //buffer, This defines where he should save The Values he read out of Memory
    //4, this is 4, because an Integer has normally 4 Bytes... You can check that using MessageBox.Show(sizeof(int).ToString());
    //IntPtr.Zero, This would be where we wanna save how much he did read... I do never check that or save HowManyByte I've read... Just don't care about that.

    //WriteProcessMemory:
    //pHandle, Same as above.
    //Address, Same as Above
    //buffer, What value we wanna write there.
    //buffer.Lenght, how Many Bytes he needs to write.
    //IntPtr.Zero, same as above.

    class ReadMemory
    {
        enum TOKEN_INFORMATION_CLASS
        {
            /// <summary>
            /// The buffer receives a TOKEN_USER structure that contains the user account of the token.
            /// </summary>
            TokenUser = 1,

            /// <summary>
            /// The buffer receives a TOKEN_GROUPS structure that contains the group accounts associated with the token.
            /// </summary>
            TokenGroups,

            /// <summary>
            /// The buffer receives a TOKEN_PRIVILEGES structure that contains the privileges of the token.
            /// </summary>
            TokenPrivileges,

            /// <summary>
            /// The buffer receives a TOKEN_OWNER structure that contains the default owner security identifier (SID) for newly created objects.
            /// </summary>
            TokenOwner,

            /// <summary>
            /// The buffer receives a TOKEN_PRIMARY_GROUP structure that contains the default primary group SID for newly created objects.
            /// </summary>
            TokenPrimaryGroup,

            /// <summary>
            /// The buffer receives a TOKEN_DEFAULT_DACL structure that contains the default DACL for newly created objects.
            /// </summary>
            TokenDefaultDacl,

            /// <summary>
            /// The buffer receives a TOKEN_SOURCE structure that contains the source of the token. TOKEN_QUERY_SOURCE access is needed to retrieve this information.
            /// </summary>
            TokenSource,

            /// <summary>
            /// The buffer receives a TOKEN_TYPE value that indicates whether the token is a primary or impersonation token.
            /// </summary>
            TokenType,

            /// <summary>
            /// The buffer receives a SECURITY_IMPERSONATION_LEVEL value that indicates the impersonation level of the token. If the access token is not an impersonation token, the function fails.
            /// </summary>
            TokenImpersonationLevel,

            /// <summary>
            /// The buffer receives a TOKEN_STATISTICS structure that contains various token statistics.
            /// </summary>
            TokenStatistics,

            /// <summary>
            /// The buffer receives a TOKEN_GROUPS structure that contains the list of restricting SIDs in a restricted token.
            /// </summary>
            TokenRestrictedSids,

            /// <summary>
            /// The buffer receives a DWORD value that indicates the Terminal Services session identifier that is associated with the token.
            /// </summary>
            TokenSessionId,

            /// <summary>
            /// The buffer receives a TOKEN_GROUPS_AND_PRIVILEGES structure that contains the user SID, the group accounts, the restricted SIDs, and the authentication ID associated with the token.
            /// </summary>
            TokenGroupsAndPrivileges,

            /// <summary>
            /// Reserved.
            /// </summary>
            TokenSessionReference,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if the token includes the SANDBOX_INERT flag.
            /// </summary>
            TokenSandBoxInert,

            /// <summary>
            /// Reserved.
            /// </summary>
            TokenAuditPolicy,

            /// <summary>
            /// The buffer receives a TOKEN_ORIGIN value.
            /// </summary>
            TokenOrigin,

            /// <summary>
            /// The buffer receives a TOKEN_ELEVATION_TYPE value that specifies the elevation level of the token.
            /// </summary>
            TokenElevationType,

            /// <summary>
            /// The buffer receives a TOKEN_LINKED_TOKEN structure that contains a handle to another token that is linked to this token.
            /// </summary>
            TokenLinkedToken,

            /// <summary>
            /// The buffer receives a TOKEN_ELEVATION structure that specifies whether the token is elevated.
            /// </summary>
            TokenElevation,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if the token has ever been filtered.
            /// </summary>
            TokenHasRestrictions,

            /// <summary>
            /// The buffer receives a TOKEN_ACCESS_INFORMATION structure that specifies security information contained in the token.
            /// </summary>
            TokenAccessInformation,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if virtualization is allowed for the token.
            /// </summary>
            TokenVirtualizationAllowed,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if virtualization is enabled for the token.
            /// </summary>
            TokenVirtualizationEnabled,

            /// <summary>
            /// The buffer receives a TOKEN_MANDATORY_LABEL structure that specifies the token's integrity level.
            /// </summary>
            TokenIntegrityLevel,

            /// <summary>
            /// The buffer receives a DWORD value that is nonzero if the token has the UIAccess flag set.
            /// </summary>
            TokenUIAccess,

            /// <summary>
            /// The buffer receives a TOKEN_MANDATORY_POLICY structure that specifies the token's mandatory integrity policy.
            /// </summary>
            TokenMandatoryPolicy,

            /// <summary>
            /// The buffer receives the token's logon security identifier (SID).
            /// </summary>
            TokenLogonSid,

            /// <summary>
            /// The maximum value for this enumeration
            /// </summary>
            MaxTokenInfoClass
        }
        // *** DLL imports

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImportAttribute("User32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle,
            UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern Boolean SetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass,
            ref UInt32 TokenInformation, UInt32 TokenInformationLength);

        //proccID ==> Id of the Process Aion.bin
        //pHandle ==> When we open the process over OpenProcess we save the open Handle into this variable.
        //base_adress ==> Adress of the moduleEntry of game.dll  

        // ************************************************************************************************
        // ................................................................................................

        public static int proccID;
        public static IntPtr pHandle;
        public static int base_adress = 0x400000; // = 0x010f0000;

        // ************************************************************************************************
        // ................................................................................................

        public static void OpenProcess(String gameProcessName, int index)
        {
            Process hCurrentProcess = Process.GetCurrentProcess();

            String processName = gameProcessName;

            //Lets get every process with the name "Wow-64.exe":
            Process[] procs = Process.GetProcessesByName(processName);

            //foreach (Process proc in procs)
            //{
            //    MessageBox.Show(proc.ProcessName);
            //}

            //IF the array procs is 0, we know that the game isn't started.
            if (procs.Length == 0)
            {
                proccID = 0;
            }
            else
            {
                /*
                 *  All = 0x001F0FFF,
                    Terminate = 0x00000001,
                    CreateThread = 0x00000002,
                    VMOperation = 0x00000008,
                    VMRead = 0x00000010,
                    VMWrite = 0x00000020,
                    DupHandle = 0x00000040,
                    SetInformation = 0x00000200,
                    QueryInformation = 0x00000400,
                    Synchronize = 0x00100000
                 */

                //OpenProcess opens the process so we can Read out of it.
                //0x1F0FFF means full Access, Writing, Reading, etc.
                proccID = procs[index].Id;

                //MessageBox.Show("Procc ID: " + proccID);

                try
                {
                    pHandle = OpenProcess(0x001F0FFF, false, proccID);
                    //MessageBox.Show("Process Opened: " + pHandle);
                    if (pHandle == (IntPtr)0)
                        MessageBox.Show("Open Process failed");
                }
                catch
                {
                    MessageBox.Show("Failed to open process");
                }

                //MessageBox.Show("ProccID: " + proccID.ToString());
                //MessageBox.Show("pHandle: " + pHandle.ToString());

                //Now we get the base_address of the module Game.dll because we need this later for Player information (Game.dll+Offsets)
                //ProcessModuleCollection modules = procs[0].Modules;

                // work around
                //base_adress = modules[0].BaseAddress.ToInt32();


                //foreach (ProcessModule module in modules)
                //{
                //    if (module.ModuleName == processName)
                //    {
                //        base_adress = module.BaseAddress.ToInt32();
                //    }
                //}

            }

        }

        public static void setForeground()
        {
            Process p = Process.GetProcessById(proccID);
            SetForegroundWindow(p.MainWindowHandle);
        }
        public static int readInt(long Address)
        {
            byte[] buffer = new byte[sizeof(int)];
            ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)4, IntPtr.Zero);
            return BitConverter.ToInt32(buffer, 0);
        }
        public static uint readUInt(long Address)
        {
            byte[] buffer = new byte[sizeof(int)];
            ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)4, IntPtr.Zero);
            return (uint)BitConverter.ToUInt32(buffer, 0);
        }
        public static float readFloat(long Address)
        {
            byte[] buffer = new byte[sizeof(float)];
            ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)4, IntPtr.Zero);
            return BitConverter.ToSingle(buffer, 0);
        }
        public static string ReadUnicodeString(long Address)
        {
            byte[] buffer = new byte[50];

            ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)50, IntPtr.Zero);

            string ret = Encoding.Unicode.GetString(buffer);

            if (ret.IndexOf('\0') != -1)
                ret = ret.Remove(ret.IndexOf('\0'));
            return ret;
        }
        public static byte readByte(long Address)
        {
            byte[] buffer = new byte[1];
            ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)1, IntPtr.Zero);
            return buffer[0];
        }

        public static byte[] readPlayerStack(long Address)
        {
            byte[] buffer = new byte[175];
            ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)175, IntPtr.Zero);
            return buffer;
        }

        public static string ReadAsciiString(long Address, int size)
        {
            byte[] buffer = new byte[size];

            ReadProcessMemory(pHandle, (UIntPtr)Address, buffer, (UIntPtr)size, IntPtr.Zero);

            string ret = Encoding.ASCII.GetString(buffer);

            if (ret.IndexOf('\0') != -1)
                ret = ret.Remove(ret.IndexOf('\0'));
            return ret;
        }
    }
}
