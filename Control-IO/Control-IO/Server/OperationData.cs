using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace udptest.Operation
{
    class OperationData
    {
        #region Mouse Event Simulation
        private const int LEFTDOWN = 0x02;
        private const int LEFTUP = 0x04;
        private const int RIGHTDOWN = 0x08;
        private const int RIGHTUP = 0x10;
        private const int WHEEL_DOWN = 0x20;
        private const int WHEEL_UP = 0x40;
        private const int WHEEL_SCROLL = 0x800;
        private const int KEY_DOWN_EVENT = 0x01;
        private const int KEY_UP_EVENT = 0x02;
        private const int KEY_CTRL = 0xA2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void MouseLeftDown()
        {
            mouse_event(2, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        }

        public static void MouseLeftUp()
        {
            mouse_event(4, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        }

        public static void MouseRightDown()
        {
            mouse_event(8, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        }

        public static void MouseRightUp()
        {
            mouse_event(16, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        }

        public static void MousewheelDown()
        {
            mouse_event(32, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        }

        public static void MousewheelUp()
        {
            mouse_event(64, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        }

        public static void MouseWheelClick()
        {
            MousewheelDown();
            MousewheelUp();
        }

        public static void MousewheelScroll(int scrollValue)
        {
            mouse_event(2048, Cursor.Position.X, Cursor.Position.Y, scrollValue, 0);
        }
        public static void MousewheelZoom(int scrollValue)
        {
            keybd_event(KEY_CTRL, 0x45, KEY_DOWN_EVENT | 0, 0);
            mouse_event(WHEEL_SCROLL, Cursor.Position.X, Cursor.Position.Y, scrollValue, 0);
            keybd_event(KEY_CTRL, 0x45, KEY_DOWN_EVENT | KEY_UP_EVENT, 0);
        }
        #endregion

        #region Volume Control
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        public static void VolumeDown()
        {
            keybd_event(174, 0, 0u, 0);
        }

        public static void VolumeUP()
        {
            keybd_event(175, 0, 0u, 0);
        }
        #endregion
    }
}
