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

        public LimbOrientation(SharedContent.BodyPartID limbID, SkeletonData bodyPartData)
        {
            this.limbID = limbID;
            this.upperLimbInclination = LimbOrientation.CalculateUpperLimbInclination(limbID, bodyPartData);
            this.upperLimbInclination.Normalize();
            this.lowerLimbInclination = LimbOrientation.CalculateLowerLimbInclination(limbID, bodyPartData);
            this.lowerLimbInclination.Normalize();
            this.bendInLimb = Vector3D.AngleBetween(this.upperLimbInclination, this.lowerLimbInclination);

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
            this.calibratedLowerLimbInclination = LimbOrientation.CalculateLowerLimbInclination(this.limbID, bodyPartData);
            this.calibratedUpperLimbInclination = LimbOrientation.CalculateUpperLimbInclination(this.limbID, bodyPartData);
            this.calibratedBendInLimb = Vector3D.AngleBetween(this.calibratedUpperLimbInclination, this.calibratedLowerLimbInclination);
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

            double currentBendInLimb = Vector3D.AngleBetween(currentUpperLimbInclination, currentLowerLimbInclination);

            if (Math.Abs(currentBendInLimb - this.calibratedBendInLimb) > SharedContent.AllowableDeviationInDegrees)
            {
                return false;
            }

            return true;
        }

        public override int IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation)
        {
            LimbOrientation initialLimbOrientation = (LimbOrientation)initialOrientation;
            LimbOrientation finalLimbOrientation = (LimbOrientation)finalOrientation;

            int percentUpperLimbInclinationTraveled = BodyPartOrientation.IsVectorOnPathInPlane(this.calibratedUpperLimbInclination, initialLimbOrientation.calibratedUpperLimbInclination,
                finalLimbOrientation.calibratedUpperLimbInclination);
            if (percentUpperLimbInclinationTraveled == -1) return -1;

            int percentLowerLimbInclinationTraveled = BodyPartOrientation.IsVectorOnPathInPlane(this.calibratedLowerLimbInclination, initialLimbOrientation.calibratedLowerLimbInclination,
                finalLimbOrientation.calibratedLowerLimbInclination);
            if (percentLowerLimbInclinationTraveled == -1) return -1;

            double currentChangeInBendInLimb = Math.Abs(this.calibratedBendInLimb - initialLimbOrientation.calibratedBendInLimb);
            double totalChangeInBendInLimb = Math.Abs(finalLimbOrientation.calibratedBendInLimb - initialLimbOrientation.calibratedBendInLimb);

            if (finalLimbOrientation.calibratedBendInLimb < initialLimbOrientation.calibratedBendInLimb)
            {
                if (this.calibratedBendInLimb > initialLimbOrientation.calibratedBendInLimb + SharedContent.AllowableDeviationInDegrees ||
                    this.calibratedBendInLimb < finalLimbOrientation.calibratedBendInLimb - SharedContent.AllowableDeviationInDegrees)
                {
                    return -1;
                }
                if (this.calibratedBendInLimb > initialLimbOrientation.calibratedBendInLimb) currentChangeInBendInLimb = 0;
            }
            else
            {
                if (this.calibratedBendInLimb < initialLimbOrientation.calibratedBendInLimb - SharedContent.AllowableDeviationInDegrees ||
                    this.calibratedBendInLimb > finalLimbOrientation.calibratedBendInLimb + SharedContent.AllowableDeviationInDegrees)
                {
                    return -1;
                }
                if (this.calibratedBendInLimb < initialLimbOrientation.calibratedBendInLimb) currentChangeInBendInLimb = 0;
            }

            int percentChangeInBendInLimbTraveled;
            if (totalChangeInBendInLimb < SharedContent.AllowableDeviationInDegrees)
            {
                percentChangeInBendInLimbTraveled = -2;
            }
            else
            {
                percentChangeInBendInLimbTraveled = (int)(100 * (currentChangeInBendInLimb / totalChangeInBendInLimb));
            }

            List<int> nonNegligableChanges = new List<int>();
            if (percentChangeInBendInLimbTraveled != -2) nonNegligableChanges.Add(percentChangeInBendInLimbTraveled);
            if (percentLowerLimbInclinationTraveled != -2) nonNegligableChanges.Add(percentLowerLimbInclinationTraveled);
            if (percentUpperLimbInclinationTraveled != -2) nonNegligableChanges.Add(percentUpperLimbInclinationTraveled);

            if (nonNegligableChanges.Count == 0)
            {
                return -2;
            }
            else
            {
                if (nonNegligableChanges.Max() - nonNegligableChanges.Min() > SharedContent.AllowableDeviationInPercent)
                {
                    return -1;
                }
                else
                {
                    return (int)(nonNegligableChanges.Average());
                }
            }
        }
    }
}
