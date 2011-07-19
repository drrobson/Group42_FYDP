using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    /// <summary>
    /// Interaction logic for CalibratingView.xaml
    /// </summary>
    public partial class CalibratingView : UserControl, IScreen//, IKinectFrames (when I add the Kinect control feature)
    {
        DispatcherTimer timer;
        int secondsPassed = 5;

        public UIElement element
        {
            get
            {
                return this;
            }
        }

        public CalibratingView()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timerSecondPassed;
            timer.Start();
        }

        private void timerSecondPassed(object source, EventArgs e)
        {
            secondsPassed--;
            secondsLabel.Content = String.Format("{0}", secondsPassed);
            if (secondsPassed == 0)
            {
                timer.Stop();
                SharedContent.Nui.SkeletonFrameReady += nuiSkeletonFrameReady;
            }
        }

        public void nuiSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SharedContent.Nui.SkeletonFrameReady -= nuiSkeletonFrameReady;

            /*
            SkeletonFrame skeletonFrame = e.SkeletonFrame;
            foreach (SkeletonData skeletonData in skeletonFrame.Skeletons)
            {
                if (skeletonData.TrackingState == SkeletonTrackingState.Tracked)
                {
                    ExerciseView.neutralPose.CalibratePose(skeletonData);
                    ExerciseView.tPose.CalibratePose(skeletonData);
                    break;
                }
            }
             * */

            ScreenManager.setScreen(new ExerciseSelector());
        }
    }
}
