using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class TorsoOrientation : BodyPartOrientation
    {
        public double torsoRotation;
        public Vector3D torsoInclination;

        public static double NeutralTorsoRotation = 0;        //Looking at the top of the user's head, rotation is positive in the counter-clockwise direction
        public static Vector3D NeutralTorsoInclination = new Vector3D(0, 1, 0);   //Torso orientation relative to the hips

        public void CalibrateNeutral(SkeletonData neutralOrientationData)
        {
            //Arbitrarily decide to examine rotation of right shoulder to determine rotation value (assume left shoulder rotates in equal magnitude and opposite direction)
            //We examine angular difference between the vector of center hip to right hip and center shoulder to right shoulder to determine the rotation of the torso
            Vector hipCenterPosition = neutralOrientationData.Joints[JointID.HipCenter].Position;
            Vector hipRightPosition = neutralOrientationData.Joints[JointID.HipRight].Position;
            Vector3D hipVector = new Vector3D(hipRightPosition.X - hipCenterPosition.X, 0, hipRightPosition.Z - hipCenterPosition.Z);

            Vector shoulderCenterPosition = neutralOrientationData.Joints[JointID.ShoulderCenter].Position;
            Vector shoulderRightPosition = neutralOrientationData.Joints[JointID.ShoulderRight].Position;
            Vector3D shoulderVector = new Vector3D(shoulderRightPosition.X - shoulderCenterPosition.X, 0, shoulderRightPosition.Z - shoulderCenterPosition.Z);

            TorsoOrientation.NeutralTorsoRotation = Vector3D.AngleBetween(shoulderVector, hipVector);

            TorsoOrientation.NeutralTorsoInclination = new Vector3D(shoulderCenterPosition.X - hipCenterPosition.X, shoulderCenterPosition.Y - hipCenterPosition.Y,
                shoulderCenterPosition.Z - hipCenterPosition.Z);
        }

        public static TorsoOrientation GetNeutralOrientation()
        {
            TorsoOrientation neutralOrientation = new TorsoOrientation();
            neutralOrientation.torsoRotation = TorsoOrientation.NeutralTorsoRotation;
            neutralOrientation.torsoInclination = TorsoOrientation.NeutralTorsoInclination;

            return neutralOrientation;
        }
    }
}
