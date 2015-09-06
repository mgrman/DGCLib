using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DGCLib_WinForms.Utilities
{
    public static class NativeMethods
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        [DllImport("kernel32.dll")]
        private static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern Boolean FreeConsole();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, UIntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        private static extern int GetUpdateRect(IntPtr hwnd, ref RECT rect, bool erase);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr handle, ref RECT rect);

        [DllImport("User32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);

        [DllImport("User32.dll")]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("user32.dll")]
        private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        private static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        internal enum Messages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_RBUTTONDOWN = 0x0204,
            WM_MBUTTONDOWN = 0x0207,
            WM_LBUTTONUP = 0x0202,
            WM_RBUTTONUP = 0x0205,
            WM_MBUTTONUP = 0x0208,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDBLCLK = 0x0209,

            WM_MOUSEWHEEL = 0x020A,
            WM_PAINT = 0x000F,
            WM_PRINTCLIENT = 0x0318,
            WM_ERASEBKGND = 0x0014,
            WM_HSCROLL = 0x114,
            WM_VSCROLL = 0x115,
            WM_SETREDRAW = 11,
            WM_CHANGEUISTATE = 0x0127,
            WM_MOUSEMOVE = 0x0200,
            WM_USER = 0x400,
            EM_GETSCROLLPOS = WM_USER + 221
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public int fErase;
            public RECT rcPaint;
            public int fRestore;
            public int fIncUpdate;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int Reserved4;
            public int Reserved5;
            public int Reserved6;
            public int Reserved7;
            public int Reserved8;
        }

        public static void AddConsole()
        {
            AllocConsole();
        }

        public static void RemoveConsole()
        {
            FreeConsole();
        }

        public static void SuspendDrawing(this Control parent)
        {
            SendMessage(parent.Handle, (uint)Messages.WM_SETREDRAW, new UIntPtr(0), new IntPtr(0));
        }

        public static void ResumeDrawing(this Control parent)
        {
            SendMessage(parent.Handle, (uint)Messages.WM_SETREDRAW, new UIntPtr(1), new IntPtr(0));
            parent.Refresh();
        }

        internal static bool QueryPerformanceFrequency_w(out long lpFrequency)
        {
            return QueryPerformanceFrequency(out lpFrequency);
        }

        internal static bool QueryPerformanceCounter_w(out long lpPerformanceCount)
        {
            return QueryPerformanceCounter(out lpPerformanceCount);
        }
    }
}