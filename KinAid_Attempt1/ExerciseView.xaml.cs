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
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    /// <summary>
    /// Interaction logic for ExerciseView.xaml
    /// </summary>
    public partial class ExerciseView : UserControl, IScreen, IKinectFrames
    {
        int totalFrames = 0;
        int lastFrames = 0;
        DateTime lastTime = DateTime.MaxValue;

        public UIElement element
        {
            get
            {
                return this;
            }
        }

        public static Pose tPose = new Pose(new Dictionary<SharedContent.BodyPartID, BodyPartOrientation>()
            {
                {SharedContent.BodyPartID.RightArm, new LimbOrientation(SharedContent.BodyPartID.RightArm, new Vector3D(1,0,0), new Vector3D(1,0,0))},
                {SharedContent.BodyPartID.LeftArm, new LimbOrientation(SharedContent.BodyPartID.LeftArm, new Vector3D(-1,0,0), new Vector3D(-1,0,0))},
                {SharedContent.BodyPartID.RightLeg, new LimbOrientation(SharedContent.BodyPartID.RightLeg, new Vector3D(0,-1,0), new Vector3D(0,-1,0))},
                {SharedContent.BodyPartID.LeftLeg, new LimbOrientation(SharedContent.BodyPartID.LeftLeg, new Vector3D(0,-1,0), new Vector3D(0,-1,0))}
            });

        // We want to control how depth data gets converted into false-color data
        // for more intuitive visualization, so we keep 32-bit color frame buffer versions of
        // these, to be updated whenever we receive and process a 16-bit frame.
        /*const int RED_IDX = 2;
        const int GREEN_IDX = 1;
        const int BLUE_IDX = 0;
        byte[] depthFrame32 = new byte[320 * 240 * 4];*/

        public ExerciseView()
        {
            InitializeComponent();

            lastTime = DateTime.Now;

            //SharedContent.Nui.DepthFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nuiDepthFrameReady);
            SharedContent.Nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nuiSkeletonFrameReady);
            SharedContent.Nui.VideoFrameReady += new EventHandler<ImageFrameReadyEventArgs>(nuiColorFrameReady);
        }

        /// We aren't going to use the depth data for now
        // Converts a 16-bit grayscale depth frame which includes player indexes into a 32-bit frame
        // that displays different players in different colors
        /*byte[] convertDepthFrame(byte[] depthFrame16)
        {
            for (int i16 = 0, i32 = 0; i16 < depthFrame16.Length && i32 < depthFrame32.Length; i16 += 2, i32 += 4)
            {
                int player = depthFrame16[i16] & 0x07;
                int realDepth = (depthFrame16[i16 + 1] << 5) | (depthFrame16[i16] >> 3);
                // transform 13-bit depth information into an 8-bit intensity appropriate
                // for display (we disregard information in most significant bit)
                byte intensity = (byte)(255 - (255 * realDepth / 0x0fff));

                depthFrame32[i32 + RED_IDX] = 0;
                depthFrame32[i32 + GREEN_IDX] = 0;
                depthFrame32[i32 + BLUE_IDX] = 0;

                // choose different display colors based on player
                switch (player)
                {
                    case 0:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity / 2);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity / 2);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity / 2);
                        break;
                    case 1:
                        depthFrame32[i32 + RED_IDX] = intensity;
                        break;
                    case 2:
                        depthFrame32[i32 + GREEN_IDX] = intensity;
                        break;
                    case 3:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity / 4);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity);
                        break;
                    case 4:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity / 4);
                        break;
                    case 5:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity / 4);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity);
                        break;
                    case 6:
                        depthFrame32[i32 + RED_IDX] = (byte)(intensity / 2);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(intensity / 2);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(intensity);
                        break;
                    case 7:
                        depthFrame32[i32 + RED_IDX] = (byte)(255 - intensity);
                        depthFrame32[i32 + GREEN_IDX] = (byte)(255 - intensity);
                        depthFrame32[i32 + BLUE_IDX] = (byte)(255 - intensity);
                        break;
                }
            }
            return depthFrame32;
        }

        
        public void nuiDepthFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            PlanarImage Image = e.ImageFrame.Image;
            byte[] convertedDepthFrame = convertDepthFrame(Image.Bits);

            depth.Source = BitmapSource.Create(
                Image.Width, Image.Height, 96, 96, PixelFormats.Bgr32, null, convertedDepthFrame, Image.Width * 4);
        }*/

        /// Skeleton data is for debug use only
        /*private Point getDisplayPosition(Joint joint)
        {
            float depthX, depthY;
            nui.SkeletonEngine.SkeletonToDepthImage(joint.Position, out depthX, out depthY);
            depthX = Math.Max(0, Math.Min(depthX * 320, 320));  //convert to 320, 240 space
            depthY = Math.Max(0, Math.Min(depthY * 240, 240));  //convert to 320, 240 space
            int colorX, colorY;
            ImageViewArea iv = new ImageViewArea();
            // only ImageResolution.Resolution640x480 is supported at this point
            nui.NuiCamera.GetColorPixelCoordinatesFromDepthPixel(ImageResolution.Resolution640x480, iv, (int)depthX, (int)depthY, (short)0, out colorX, out colorY);

            // map back to skeleton.Width & skeleton.Height
            return new Point((int)(skeleton.Width * colorX / 640.0), (int)(skeleton.Height * colorY / 480));
        }

        Polyline getBodySegment(Microsoft.Research.Kinect.Nui.JointsCollection joints, Brush brush, params JointID[] ids)
        {
            PointCollection points = new PointCollection(ids.Length);
            for (int i = 0; i < ids.Length; ++i)
            {
                points.Add(getDisplayPosition(joints[ids[i]]));
            }

            Polyline polyline = new Polyline();
            polyline.Points = points;
            polyline.Stroke = brush;
            polyline.StrokeThickness = 5;
            return polyline;
        }
        */
        
        public void nuiSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame skeletonFrame = e.SkeletonFrame;
            //int iSkeleton = 0;
            //Brush[] brushes = new Brush[6];
            //brushes[0] = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            //brushes[1] = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            //brushes[2] = new SolidColorBrush(Color.FromRgb(64, 255, 255));
            //brushes[3] = new SolidColorBrush(Color.FromRgb(255, 255, 64));
            //brushes[4] = new SolidColorBrush(Color.FromRgb(255, 64, 255));
            //brushes[5] = new SolidColorBrush(Color.FromRgb(128, 128, 255));

            //skeleton.Children.Clear();
            foreach (SkeletonData data in skeletonFrame.Skeletons)
            {
                if (SkeletonTrackingState.Tracked == data.TrackingState)
                {
                    if (SharedContent.NeutralPose.IsInPose(data))
                    {
                        Console.Write("In neutral pose!");
                    }
                    if (tPose.IsInPose(data))
                    {
                        Console.Write("In T pose!");
                    }
                    //// Draw bones
                    //Brush brush = brushes[iSkeleton % brushes.Length];
                    //skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.HipCenter, JointID.Spine, JointID.ShoulderCenter, JointID.Head));
                    //skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.ShoulderCenter, JointID.ShoulderLeft, JointID.ElbowLeft, JointID.WristLeft, JointID.HandLeft));
                    //skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.ShoulderCenter, JointID.ShoulderRight, JointID.ElbowRight, JointID.WristRight, JointID.HandRight));
                    //skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.HipCenter, JointID.HipLeft, JointID.KneeLeft, JointID.AnkleLeft, JointID.FootLeft));
                    //skeleton.Children.Add(getBodySegment(data.Joints, brush, JointID.HipCenter, JointID.HipRight, JointID.KneeRight, JointID.AnkleRight, JointID.FootRight));

                    //// Draw joints
                    //foreach (Joint joint in data.Joints)
                    //{
                    //    Point jointPos = getDisplayPosition(joint);
                    //    Line jointLine = new Line();
                    //    jointLine.X1 = jointPos.X - 3;
                    //    jointLine.X2 = jointLine.X1 + 6;
                    //    jointLine.Y1 = jointLine.Y2 = jointPos.Y;
                    //    jointLine.Stroke = SharedContent.JointColors[joint.ID];
                    //    jointLine.StrokeThickness = 6;
                    //    skeleton.Children.Add(jointLine);
                    //}

                    //if (startExercise)
                    //{
                    //    ex1.updateExercise(data);
                    //    exerciseCorrect.Text = ex1.printState();
                    //}
                }
                //iSkeleton++;
            } // for each skeleton
        }

        public void nuiColorFrameReady(object sender, ImageFrameReadyEventArgs e)
        {
            // 32-bit per pixel, RGBA image
            PlanarImage Image = e.ImageFrame.Image;
            video.Source = BitmapSource.Create(
                Image.Width, Image.Height, 96, 96, PixelFormats.Bgr32, null, Image.Bits, Image.Width * Image.BytesPerPixel);
            ++totalFrames;

            DateTime cur = DateTime.Now;
            if (cur.Subtract(lastTime) > TimeSpan.FromSeconds(1))
            {
                int frameDiff = totalFrames - lastFrames;
                lastFrames = totalFrames;
                lastTime = cur;
                fpsLabel.Content = frameDiff.ToString() + " fps";
            }
        }
    }
}
