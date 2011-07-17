using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class LimbOrientation : BodyPartOrientation
    {
        public SharedContent.LimbID limbID;
        public Vector3D upperLimbOrientation;
        public double bendInLimb;

        //Initialize neutral limb orientations to ideal neutral pose (arms at sides, feet below hips)
        public static Vector3D[] NeutralUpperLimbOrientations = new Vector3D[] { new Vector3D(0, 180, 0), new Vector3D(0, 180, 0), new Vector3D(0, 180, 0), new Vector3D(0, 180, 0) };
        public static double[] NeutralBendInLimbs = new double[] { 0, 0, 0, 0 };

        public LimbOrientation(SharedContent.LimbID limbID)
        {
            this.limbID = limbID;
        }

        public void CalibrateNeutral(SkeletonData neutralOrientationData)
        {
            Vector upperJointPosition, middleJointPosition, lowerJointPosition;
            
            switch (limbID)
            {
                case SharedContent.LimbID.RightArm:
                    upperJointPosition = neutralOrientationData.Joints[JointID.ShoulderRight].Position;
                    middleJointPosition = neutralOrientationData.Joints[JointID.ElbowRight].Position;
                    lowerJointPosition = neutralOrientationData.Joints[JointID.HandRight].Position;
                    break;
                case SharedContent.LimbID.LeftArm:
                    upperJointPosition = neutralOrientationData.Joints[JointID.ShoulderLeft].Position;
                    middleJointPosition = neutralOrientationData.Joints[JointID.ElbowLeft].Position;
                    lowerJointPosition = neutralOrientationData.Joints[JointID.HandLeft].Position;
                    break;
                case SharedContent.LimbID.RightLeg:
                    upperJointPosition = neutralOrientationData.Joints[JointID.HipRight].Position;
                    middleJointPosition = neutralOrientationData.Joints[JointID.KneeRight].Position;
                    lowerJointPosition = neutralOrientationData.Joints[JointID.FootRight].Position;
                    break;
                case SharedContent.LimbID.LeftLeg:
                    upperJointPosition = neutralOrientationData.Joints[JointID.HipLeft].Position;
                    middleJointPosition = neutralOrientationData.Joints[JointID.KneeLeft].Position;
                    lowerJointPosition = neutralOrientationData.Joints[JointID.FootLeft].Position;
                    break;
            }

            NeutralUpperLimbOrientations[(int)limbID] = new Vector3D(middleJointPosition.X - upperJointPosition.X, middleJointPosition.Y - upperJointPosition.Y, middleJointPosition.Z - upperJointPosition.Z);

            Vector3D lowerLimbOrientation = new Vector3D(lowerJointPosition.X - middleJointPosition.X, lowerJointPosition.Y - middleJointPosition.Y, lowerJointPosition.Z - middleJointPosition.Z);
            NeutralBendInLimbs[(int)limbID] = Vector3D.AngleBetween(lowerLimbOrientation, NeutralUpperLimbOrientations[(int)limbID]);
        }

        public static LimbOrientation GetNeutralOrientation(SharedContent.LimbID limbID)
        {
            LimbOrientation neutralOrientation = new LimbOrientation(limbID);
            neutralOrientation.upperLimbOrientation = LimbOrientation.NeutralUpperLimbOrientations[(int)limbID];
            neutralOrientation.bendInLimb = LimbOrientation.NeutralBendInLimbs[(int)limbID];

            return neutralOrientation;
        }
    }
}
