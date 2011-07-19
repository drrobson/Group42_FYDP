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

        public override UserPerformanceAnalysisInfo IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation)
        {
            LimbOrientation initialLimbOrientation = (LimbOrientation)initialOrientation;
            LimbOrientation finalLimbOrientation = (LimbOrientation)finalOrientation;

            UserPerformanceAnalysisInfo upperLimbInclinationInfo = BodyPartOrientation.IsVectorOnPathInPlane(this.calibratedUpperLimbInclination, initialLimbOrientation.calibratedUpperLimbInclination,
                finalLimbOrientation.calibratedUpperLimbInclination);
            if (upperLimbInclinationInfo.failed) return upperLimbInclinationInfo;

            UserPerformanceAnalysisInfo lowerLimbInclinationInfo = BodyPartOrientation.IsVectorOnPathInPlane(this.calibratedLowerLimbInclination, initialLimbOrientation.calibratedLowerLimbInclination,
                finalLimbOrientation.calibratedLowerLimbInclination);
            if (lowerLimbInclinationInfo.failed) return lowerLimbInclinationInfo;

            double currentChangeInBendInLimb = Math.Abs(this.calibratedBendInLimb - initialLimbOrientation.calibratedBendInLimb);
            double totalChangeInBendInLimb = Math.Abs(finalLimbOrientation.calibratedBendInLimb - initialLimbOrientation.calibratedBendInLimb);

            if (finalLimbOrientation.calibratedBendInLimb < initialLimbOrientation.calibratedBendInLimb)
            {
                if (this.calibratedBendInLimb > initialLimbOrientation.calibratedBendInLimb + SharedContent.AllowableDeviationInDegrees ||
                    this.calibratedBendInLimb < finalLimbOrientation.calibratedBendInLimb - SharedContent.AllowableDeviationInDegrees)
                {
                    return new UserPerformanceAnalysisInfo(true, String.Format("The bend in the {0} is not in the range defined in the exercise step", Enum.GetName(typeof(SharedContent.BodyPartID),this.limbID)));
                }
                if (this.calibratedBendInLimb > initialLimbOrientation.calibratedBendInLimb) currentChangeInBendInLimb = 0;
            }
            else
            {
                if (this.calibratedBendInLimb < initialLimbOrientation.calibratedBendInLimb - SharedContent.AllowableDeviationInDegrees ||
                    this.calibratedBendInLimb > finalLimbOrientation.calibratedBendInLimb + SharedContent.AllowableDeviationInDegrees)
                {
                    return new UserPerformanceAnalysisInfo(true, String.Format("The bend in the {0} is not in the range defined in the exercise step", Enum.GetName(typeof(SharedContent.BodyPartID), this.limbID)));
                }
                if (this.calibratedBendInLimb < initialLimbOrientation.calibratedBendInLimb) currentChangeInBendInLimb = 0;
            }

            UserPerformanceAnalysisInfo bendInLimbInfo = new UserPerformanceAnalysisInfo();
            bendInLimbInfo.failed = false;

            if (totalChangeInBendInLimb < SharedContent.AllowableDeviationInDegrees)
            {
                bendInLimbInfo.negligableAction = true;
            }
            else
            {
                bendInLimbInfo.percentComplete = (int)(100 * (currentChangeInBendInLimb / totalChangeInBendInLimb));
            }

            int maxChange = int.MinValue, minChange = int.MaxValue, percentSum = 0, numPercents = 0;
            LimbComponentID maxChangeLimbComponent = LimbComponentID.UpperLimb, minChangeLimbComponent = LimbComponentID.UpperLimb;
            if (!bendInLimbInfo.negligableAction)
            {
                percentSum += bendInLimbInfo.percentComplete;
                numPercents++;
                if (bendInLimbInfo.percentComplete > maxChange)
                {
                    maxChange = bendInLimbInfo.percentComplete;
                    maxChangeLimbComponent = LimbComponentID.BendInLimb;
                }
                if (bendInLimbInfo.percentComplete < minChange)
                {
                    minChange = bendInLimbInfo.percentComplete;
                    minChangeLimbComponent = LimbComponentID.BendInLimb;
                }
            }
            if (!lowerLimbInclinationInfo.negligableAction)
            {
                percentSum += lowerLimbInclinationInfo.percentComplete;
                numPercents++;
                if (lowerLimbInclinationInfo.percentComplete > maxChange)
                {
                    maxChange = lowerLimbInclinationInfo.percentComplete;
                    maxChangeLimbComponent = LimbComponentID.LowerLimb;
                }
                if (lowerLimbInclinationInfo.percentComplete < minChange)
                {
                    minChange = lowerLimbInclinationInfo.percentComplete;
                    minChangeLimbComponent = LimbComponentID.LowerLimb;
                }
            }
            if (!upperLimbInclinationInfo.negligableAction)
            {
                percentSum += upperLimbInclinationInfo.percentComplete;
                numPercents++;
                if (upperLimbInclinationInfo.percentComplete > maxChange)
                {
                    maxChange = upperLimbInclinationInfo.percentComplete;
                    maxChangeLimbComponent = LimbComponentID.UpperLimb;
                }
                if (upperLimbInclinationInfo.percentComplete < minChange)
                {
                    minChange = upperLimbInclinationInfo.percentComplete;
                    minChangeLimbComponent = LimbComponentID.UpperLimb;
                }
            }

            if (numPercents == 0)
            {
                return new UserPerformanceAnalysisInfo(true);
            }
            else
            {
                if (maxChange - minChange > SharedContent.AllowableDeviationInPercent)
                {
                    return new UserPerformanceAnalysisInfo(true, String.Format("The difference in the percentage completion of the movement for the {0} and {1} of the {2} exceeded the maximum allowable deviation of {3}",
                        Enum.GetName(typeof(LimbComponentID), maxChangeLimbComponent), Enum.GetName(typeof(LimbComponentID), minChangeLimbComponent),
                        Enum.GetName(typeof(SharedContent.BodyPartID), this.limbID), SharedContent.AllowableDeviationInPercent));
                }
                else
                {
                    return new UserPerformanceAnalysisInfo(percentSum / numPercents);
                }
            }
        }

        private enum LimbComponentID
        {
            UpperLimb = 0,
            LowerLimb = 1,
            BendInLimb = 2
        }
    }
}
