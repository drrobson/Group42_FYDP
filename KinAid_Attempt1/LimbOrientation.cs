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
        public SharedContent.BodyPartID limbID;

        public Vector3D upperLimbInclination;
        public Vector3D calibratedUpperLimbInclination;
        public Vector3D lowerLimbInclination;
        public Vector3D calibratedLowerLimbInclination;
        public double bendInLimb;
        public double calibratedBendInLimb;
        
        /*
        public static Vector3D[] IdealNeutralUpperLimbOrientations = new Vector3D[] { new Vector3D(0, -1, 0), new Vector3D(0, -1, 0), new Vector3D(0, -1, 0), new Vector3D(0, -1, 0) };
        public static double[] IdealNeutralBendInLimbs = new double[] { 0, 0, 0, 0 };
        public static Vector3D[] ActualNeutralUpperLimbOrientations = new Vector3D[4];
        public static double[] ActualNeutralBendInLimbs = new double[4];
         * */

        public LimbOrientation(SharedContent.BodyPartID limbID, SkeletonData bodyPartData)
        {
            this.limbID = limbID;
            this.upperLimbInclination = LimbOrientation.CalculateUpperLimbInclination(limbID, bodyPartData);
            this.upperLimbInclination.Normalize();
            this.lowerLimbInclination = LimbOrientation.CalculateLowerLimbInclination(limbID, bodyPartData);
            this.lowerLimbInclination.Normalize();
            this.bendInLimb = LimbOrientation.CalculateBendInLimb(limbID, bodyPartData);

            this.calibratedUpperLimbInclination = this.upperLimbInclination;
            this.calibratedLowerLimbInclination = this.lowerLimbInclination;
            this.calibratedBendInLimb = this.bendInLimb;
        }

        public LimbOrientation(SharedContent.BodyPartID limbID, Vector3D upperLimbInclination, Vector3D lowerLimbInclination)
        {
            this.limbID = limbID;
            this.upperLimbInclination = upperLimbInclination;
            this.lowerLimbInclination = lowerLimbInclination;
            this.bendInLimb = Vector3D.AngleBetween(upperLimbInclination, lowerLimbInclination);

            this.calibratedUpperLimbInclination = this.upperLimbInclination;
            this.calibratedLowerLimbInclination = this.lowerLimbInclination;
            this.calibratedBendInLimb = this.bendInLimb;
        }

        public static double CalculateBendInLimb(SharedContent.BodyPartID limbID, SkeletonData bodyPartData)
        {
            Vector[] limbJoints = LimbOrientation.GetLimbJoints(limbID, bodyPartData);

            Vector3D upperLimbIncl = LimbOrientation.CalculateUpperLimbInclination(limbID, bodyPartData);
            Vector3D lowerLimbIncl = new Vector3D(limbJoints[2].X - limbJoints[1].X, limbJoints[2].Y - limbJoints[1].Y, limbJoints[2].Z - limbJoints[1].Z);
            return Vector3D.AngleBetween(lowerLimbIncl, upperLimbIncl);
        }

        public static Vector3D CalculateLowerLimbInclination(SharedContent.BodyPartID limbID, SkeletonData bodyPartData)
        {
            Vector[] limbJoints = LimbOrientation.GetLimbJoints(limbID, bodyPartData);

            return new Vector3D(limbJoints[2].X - limbJoints[1].X, limbJoints[2].Y - limbJoints[1].Y, limbJoints[2].Z - limbJoints[1].Z);
        }

        public static Vector3D CalculateUpperLimbInclination(SharedContent.BodyPartID limbID, SkeletonData bodyPartData)
        {
            Vector[] limbJoints = LimbOrientation.GetLimbJoints(limbID, bodyPartData);

            return new Vector3D(limbJoints[1].X - limbJoints[0].X, limbJoints[1].Y - limbJoints[0].Y, limbJoints[1].Z - limbJoints[0].Z);
        }

        public override void CalibrateOrientation(SkeletonData bodyPartData)
        {
            this.calibratedBendInLimb = LimbOrientation.CalculateBendInLimb(this.limbID, bodyPartData);
            this.calibratedLowerLimbInclination = LimbOrientation.CalculateLowerLimbInclination(this.limbID, bodyPartData);
            this.calibratedUpperLimbInclination = LimbOrientation.CalculateUpperLimbInclination(this.limbID, bodyPartData);
        }

        /// <summary>
        /// Returns an array of the joints comprising the limb, from upper joint to lower joint
        /// </summary>
        /// <param name="limbID"></param>
        /// <param name="bodyPartData"></param>
        /// <returns></returns>
        public static Vector[] GetLimbJoints(SharedContent.BodyPartID limbID, SkeletonData bodyPartData)
        {
            Vector[] limbJoints = new Vector[3];
            switch (limbID)
            {
                case SharedContent.BodyPartID.RightArm:
                    limbJoints[0] = bodyPartData.Joints[JointID.ShoulderRight].Position;
                    limbJoints[1] = bodyPartData.Joints[JointID.ElbowRight].Position;
                    limbJoints[2] = bodyPartData.Joints[JointID.HandRight].Position;
                    break;
                case SharedContent.BodyPartID.LeftArm:
                    limbJoints[0] = bodyPartData.Joints[JointID.ShoulderLeft].Position;
                    limbJoints[1] = bodyPartData.Joints[JointID.ElbowLeft].Position;
                    limbJoints[2] = bodyPartData.Joints[JointID.HandLeft].Position;
                    break;
                case SharedContent.BodyPartID.RightLeg:
                    limbJoints[0] = bodyPartData.Joints[JointID.HipRight].Position;
                    limbJoints[1] = bodyPartData.Joints[JointID.KneeRight].Position;
                    limbJoints[2] = bodyPartData.Joints[JointID.FootRight].Position;
                    break;
                case SharedContent.BodyPartID.LeftLeg:
                    limbJoints[0] = bodyPartData.Joints[JointID.HipLeft].Position;
                    limbJoints[1] = bodyPartData.Joints[JointID.KneeLeft].Position;
                    limbJoints[2] = bodyPartData.Joints[JointID.FootLeft].Position;
                    break;
            }

            return limbJoints;
        }

        public override bool IsBodyPartInOrientation(SkeletonData bodyPartData)
        {
            Vector3D currentUpperLimbInclination = LimbOrientation.CalculateUpperLimbInclination(this.limbID, bodyPartData);

            if (Vector3D.AngleBetween(currentUpperLimbInclination, this.calibratedUpperLimbInclination) > SharedContent.AllowableDeviationInDegrees)
            {
                return false;
            }

            Vector3D currentLowerLimbInclination = LimbOrientation.CalculateLowerLimbInclination(this.limbID, bodyPartData);

            if (Vector3D.AngleBetween(currentLowerLimbInclination, this.calibratedLowerLimbInclination) > SharedContent.AllowableDeviationInDegrees)
            {
                return false;
            }

            /*
            double currentBendInLimb = LimbOrientation.CalculateBendInLimb(this.limbID, bodyPartData);

            if (Math.Abs(currentBendInLimb - this.calibratedBendInLimb) > SharedContent.AllowableDeviationInDegrees)
            {
                return false;
            }
             * */

            return true;
        }

        public override bool IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation)
        {
            throw new NotImplementedException();
        }
    }
}
