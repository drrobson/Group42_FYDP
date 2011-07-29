using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        bool hasCaptured;
        int currentPoseBeingCalibrated;

        Exercise ex;

        SoundPlayer secondPassedSound;
        SoundPlayer snapshotSound;

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

            secondPassedSound = new SoundPlayer("Sounds/Countdown.wav");
            secondPassedSound.LoadAsync();
            snapshotSound = new SoundPlayer("Sounds/Camera.wav");
            snapshotSound.LoadAsync();
            
            secondsLeft = SharedContent.CalibrationSeconds * ex.getPosesToBeCalibrated().Count();
            secondsLabel.Content = String.Format("{0}", SharedContent.CalibrationSeconds);
            capturing = false;
            hasCaptured = false;

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
                if (hasCaptured)
                {
                    hasCaptured = false;
                    capturing = false;
                }
                return;
            }
            
            secondPassedSound.Play();
            secondsLeft--;
            secondsLabel.Content = String.Format("{0}", secondsLeft % SharedContent.CalibrationSeconds);
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

            snapshotSound.Play();

            foreach (SkeletonData sk in e.SkeletonFrame.Skeletons)
            {
                if (sk.TrackingState == SkeletonTrackingState.Tracked)
                {
                    ex.getPosesToBeCalibrated()[currentPoseBeingCalibrated].CalibratePose(sk);
                }
            }
            currentPoseBeingCalibrated++;
            drawPose();

            secondsLabel.Content = String.Format("{0}", SharedContent.CalibrationSeconds);
            hasCaptured = true;

            if (secondsLeft == 0)
            {
                ScreenManager.SetScreen(new ExerciseView(ex));
            }
        }

        private void drawPose()
        {
            // need to figure out a simple way to draw this (maybe draw images for now?)
        }
    }
}
