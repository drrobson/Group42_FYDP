using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinAid_Attempt1
{
    public static class ScreenManager
    {
        private static IScreenHost Host;

        public static IScreenHost GetHost()
        {
            return Host;
        }

        public static void SetHost(IScreenHost host)
        {
            Host = host;
        }

        public static void SetScreen(IScreen screen)
        {
            Host.setScreen(screen);
        }
    }
}
