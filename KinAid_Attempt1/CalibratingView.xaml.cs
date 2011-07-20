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
using System.Windows.Media.Media3D;
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
        int secondsLeft;
        bool capturing;
        int currentPoseBeingCalibrated;

        Exercise ex;

        public UIElement element
        {
            get
            {
                return this;
            }
        }

        public CalibratingView(Exercise ex)
        {
            InitializeComponent();

            this.ex = ex;

            secondsLeft = SharedContent.CalibrationSeconds * ex.getPosesToBeCalibrated().Count();
            capturing = false;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timerSecondPassed;
            timer.Start();

            currentPoseBeingCalibrated = 0;
            drawPose();
        }

        private void timerSecondPassed(object source, EventArgs e)
        {
            if (capturing) // wait one second for the capture to take effect
            {
                capturing = false;
                return;
            }

            secondsLeft--;
            secondsLabel.Content = String.Format("{0}", secondsLeft);
            if (secondsLeft % SharedContent.CalibrationSeconds == 0)
            {
                capturing = true;
                SharedContent.Nui.SkeletonFrameReady += nuiSkeletonFrameReady;
                if (secondsLeft == 0)
                {
                    timer.Stop();
                }
            }
        }

        public void nuiSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SharedContent.Nui.SkeletonFrameReady -= nuiSkeletonFrameReady;

            ex.getPosesToBeCalibrated()[currentPoseBeingCalibrated].CalibratePose(e.SkeletonFrame.Skeletons[0]);
            currentPoseBeingCalibrated++;
            drawPose();

            if (secondsLeft == 0)
            {
                ScreenManager.SetScreen(new ExerciseSelector());
            }
        }

        private void drawPose()
        {
            // need to figure out a simple way to draw this (maybe draw images for now?)
        }
    }
}
