using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinAid_Attempt1
{
    public static class ScreenManager
    {
        private static IScreenHost Host;

        public static void setHost(IScreenHost host)
        {
            Host = host;
        }

        public static void setScreen(IScreen screen)
        {
            Host.setScreen(screen);
        }
    }
}
