using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class HeadOrientation : BodyPartOrientation
    {
        public Vector3D inclination
        {
            get;
            set;
        }

        public static Vector3D NeutralInclination
        {
            get;
            private set;
        }

        public void CalibrateNeutral(SkeletonData neutralOrientationData)
        {
            Vector centerShoulderPosition = neutralOrientationData.Joints[JointID.ShoulderCenter].Position;
            Vector headPosition = neutralOrientationData.Joints[JointID.Head].Position;

            HeadOrientation.NeutralInclination = new Vector3D(headPosition.X - centerShoulderPosition.X, headPosition.Y - centerShoulderPosition.Y, headPosition.Z - centerShoulderPosition.Z);
        }

        public static HeadOrientation GetNeutralOrientation()
        {
            HeadOrientation neutralOrientation = new HeadOrientation();
            neutralOrientation.inclination = HeadOrientation.NeutralInclination;

            return neutralOrientation;
        }
    }
}
