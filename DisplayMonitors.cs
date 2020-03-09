using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

namespace DisplayMonitorsWonderwareLib
{
    [StructLayout(LayoutKind.Sequential)]
    struct Rect : IComparable
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override string ToString()
        {
            return $"{nameof(left)}: {left}, {nameof(top)}: {top}, {nameof(right)}: {right}, {nameof(bottom)}: {bottom}";
        }

        public int CompareTo(object obj)
        {
            Rect r = (Rect) obj;

            if (this.CompareValue() < r.CompareValue())
                return -1;
            else if (this.CompareValue() == r.CompareValue())
                return 0;
            else
                return 1;
        }

        private int CompareValue()
        {
            return left + top;
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
        enum ReadStatus
        {
            Success = 0,
            Failure = 1
        }
        private List<Rect> monitorRectsList;

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
            
            monitorRectsList.Add(new Rect(){top = mi.monitor.top, left = mi.monitor.left, bottom = mi.monitor.bottom, right = mi.monitor.right});
            return true;
        }

        private ReadStatus ReadMonitors()
        {
            monitorRectsList = new List<Rect>();
            MonitorEnumDelegate med = new MonitorEnumDelegate(MonitorEnum);
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, med, IntPtr.Zero);

            return monitorRectsList.Count > 0 ? ReadStatus.Success : ReadStatus.Failure;
        }

        public int[] MonitorsCoordinates()
        {
            int[] cordsArray = new int[0];
            if (ReadMonitors() != ReadStatus.Success) return cordsArray;

            int cordsArraySize = monitorRectsList.Count * 2;
            cordsArray = new int[cordsArraySize];
            for (int i = 0; i < monitorRectsList.Count; i++)
            {
                cordsArray[i * 2] = monitorRectsList[i].left;
                cordsArray[i * 2 + 1] = monitorRectsList[i].top;
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

        public int[] MonitorsCoordinatesTopLeftFirst()
        { 
            int[] cordsArray = MonitorsCoordinates();
            if (ReadMonitors() != ReadStatus.Success) return cordsArray;

            monitorRectsList.Sort();
            for (int i = 0; i < monitorRectsList.Count; i++)
            {
                cordsArray[i * 2] = monitorRectsList[i].left;
                cordsArray[i * 2 + 1] = monitorRectsList[i].top;
            }
            return cordsArray;
        }
    }
}
