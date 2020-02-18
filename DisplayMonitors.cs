using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace DisplayMonitorsWonderwareLib
{
    [StructLayout(LayoutKind.Sequential)]
    struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override string ToString()
        {
            return $"{nameof(left)}: {left}, {nameof(top)}: {top}, {nameof(right)}: {right}, {nameof(bottom)}: {bottom}";
        }
    }

    [ StructLayout( LayoutKind.Sequential ) ]
    struct MonitorInfo
    {
        public uint size;
        public Rect monitor;
        public Rect work;
        public uint flags;
    }

    public class DisplayMonitors
    {
        private List<int> monitorXcordsList;
        private List<int> monitorYcordsList;

        delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hmon, ref MonitorInfo mi);

        bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
        {
            MonitorInfo mi = new MonitorInfo();
            mi.size = (uint) Marshal.SizeOf(mi);
            bool success = GetMonitorInfo(hMonitor, ref mi);

            monitorXcordsList.Add(mi.monitor.left);
            monitorYcordsList.Add(mi.monitor.top);
            return true;
        }

        public int[] MonitorsCoordinates()
        {
            monitorXcordsList = new List<int>();
            monitorYcordsList = new List<int>();
            int[] cordsArray = new int[0];
            MonitorEnumDelegate med = new MonitorEnumDelegate(MonitorEnum);
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, med, IntPtr.Zero);
            if (monitorXcordsList.Count == monitorYcordsList.Count)
            {
                int cordsArraySize = monitorXcordsList.Count * 2;
                cordsArray = new int[cordsArraySize];
                for (int i = 0; i < monitorXcordsList.Count; i++)
                {
                    cordsArray[i * 2] = monitorXcordsList[i];
                    cordsArray[i * 2 + 1] = monitorYcordsList[i];
                }
            }
            return cordsArray;
        }

        public int[] MonitorsCoordinates0Based()
        { 
            int[] cordsArray = MonitorsCoordinates();
            if (cordsArray.Length <= 0) return cordsArray;

            int xMin = 0, yMin = 0;
            for (int i = 0; i < cordsArray.Length; i++)
            {
                if (i % 2 == 0)
                {
                    xMin = (cordsArray[i] < xMin) ? cordsArray[i] : xMin;
                }
                else
                {
                    yMin = (cordsArray[i] < yMin) ? cordsArray[i] : yMin;
                }
            }
            for (int i = 0; i < cordsArray.Length; i++)
            {
                if (i % 2 == 0)
                {
                    cordsArray[i] -= xMin;
                }
                else
                {
                    cordsArray[i] -= yMin;
                }
            }
            return cordsArray;
        }
    }
}
