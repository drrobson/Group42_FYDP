using System.Windows;

namespace KinAid_Attempt1
{
    public interface IScreenHost
    {
        IScreen currentScreen
        {
            get;
        }

        void setScreen(IScreen screen);

        void exerciseSelected(int id);
    }
}
