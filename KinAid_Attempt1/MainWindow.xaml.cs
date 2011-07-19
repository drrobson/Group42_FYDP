using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Research.Kinect.Nui;
using Microsoft.Research.Kinect.Audio;

namespace KinAid_Attempt1
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IScreenHost
    {
        public IScreen currentScreen
        {
            get;
            private set;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void setScreen(IScreen screen)
        {
            this.currentScreen = screen;
            this.LayoutRoot.Children.Clear();
            this.LayoutRoot.Children.Add(screen.element);
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            ScreenManager.SetHost(this);

            SharedContent.Nui = new Runtime();
            SharedContent.Sr = new SpeechRecognizer();
            SharedContent.Sr.initialize();

            try
            {
                SharedContent.Nui.Initialize(
                    RuntimeOptions.UseDepthAndPlayerIndex |
                    RuntimeOptions.UseSkeletalTracking |
                    RuntimeOptions.UseColor);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                Environment.Exit(-1);
            }

            try
            {
                SharedContent.Nui.VideoStream.Open(ImageStreamType.Video, 2, ImageResolution.Resolution640x480, ImageType.Color);
                SharedContent.Nui.DepthStream.Open(ImageStreamType.Depth, 2, ImageResolution.Resolution320x240, ImageType.DepthAndPlayerIndex);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Failed to open stream. Please make sure to specify a supported image type and resolution.");
                Environment.Exit(-1);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SharedContent.Nui.Uninitialize();
            Environment.Exit(0);
        }
    }

}
