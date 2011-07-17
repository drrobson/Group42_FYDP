using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public interface IKinectFrames
    {
        //void nuiDepthFrameReady(object sender, ImageFrameReadyEventArgs e);
        //void nuiSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e);
        void nuiColorFrameReady(object sender, ImageFrameReadyEventArgs e);
    }
}
