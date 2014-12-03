using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace Penguin2
{
    class WinHandleMethods
    {
     [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        IntPtr pFoundWindow;
        String gameWindowName;
        int processNumber = 0;

        public WinHandleMethods(String gameWindowName, int processNumber)
        {
            this.gameWindowName = gameWindowName;
            this.processNumber = processNumber;
        }

        public IntPtr getGameWindowHandle(int processNumber)
        {
            Process[] processes = Process.GetProcessesByName(gameWindowName);

            pFoundWindow = processes[processNumber].MainWindowHandle;

            return pFoundWindow;
        }
        public void setGameToFocusWindow()
        {
            SetForegroundWindow(getGameWindowHandle(processNumber));
        }
    }
}
