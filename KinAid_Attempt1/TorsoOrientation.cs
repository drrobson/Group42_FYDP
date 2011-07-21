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
        public double rotation;
        public double calibratedRotation;
        public Vector3D inclination;
        public Vector3D calibratedInclination;

        public TorsoOrientation(SkeletonData bodyPartData)
        {
            this.rotation = TorsoOrientation.CalculateTorsoRotation(bodyPartData);
            this.inclination = TorsoOrientation.CalculateTorsoInclination(bodyPartData);
            this.inclination.Normalize();

            this.calibratedRotation = this.rotation;
            this.calibratedInclination = this.inclination;
        }

        public TorsoOrientation(double torsoRotation, Vector3D torsoInclination)
        {
            this.rotation = torsoRotation;
            this.inclination = torsoInclination;
            this.inclination.Normalize();

            this.calibratedRotation = this.rotation;
            this.calibratedInclination = this.inclination;
        }

        public static Vector3D CalculateTorsoInclination(SkeletonData bodyPartData)
        {
            Vector hipCenterPosition = bodyPartData.Joints[JointID.HipCenter].Position;
            Vector shoulderCenterPosition = bodyPartData.Joints[JointID.ShoulderCenter].Position;

            return new Vector3D(shoulderCenterPosition.X - hipCenterPosition.X, shoulderCenterPosition.Y - hipCenterPosition.Y,
                shoulderCenterPosition.Z - hipCenterPosition.Z);
        }

        public static double CalculateTorsoRotation(SkeletonData bodyPartData)
        {
            //We examine angular difference between the vector of center hip to right hip and center shoulder to right shoulder to determine the rotation of the torso
            Vector hipCenterPosition = bodyPartData.Joints[JointID.HipCenter].Position;
            Vector hipRightPosition = bodyPartData.Joints[JointID.HipRight].Position;
            Vector3D hipVector = new Vector3D(hipRightPosition.X - hipCenterPosition.X, 0, hipRightPosition.Z - hipCenterPosition.Z);

            Vector shoulderCenterPosition = bodyPartData.Joints[JointID.ShoulderCenter].Position;
            Vector shoulderRightPosition = bodyPartData.Joints[JointID.ShoulderRight].Position;
            Vector3D shoulderVector = new Vector3D(shoulderRightPosition.X - shoulderCenterPosition.X, 0, shoulderRightPosition.Z - shoulderCenterPosition.Z);

            double degreesOfRotation = Vector3D.AngleBetween(shoulderVector, hipVector);

            if (shoulderRightPosition.Z < hipRightPosition.Z)
            {
                degreesOfRotation = -degreesOfRotation;
            }
            return degreesOfRotation;
        }

        public override void CalibrateOrientation(SkeletonData bodyPartData)
        {
            this.calibratedInclination = TorsoOrientation.CalculateTorsoInclination(bodyPartData);
            this.calibratedRotation = TorsoOrientation.CalculateTorsoRotation(bodyPartData);
        }

        public override bool IsBodyPartInOrientation(SkeletonData bodyPartData)
        {
            double currentRotation = TorsoOrientation.CalculateTorsoRotation(bodyPartData);

            if (Math.Abs(currentRotation - this.calibratedRotation) > SharedContent.GetAllowableDeviationInDegrees())
            {
                return false;
            }

            Vector3D currentInclination = TorsoOrientation.CalculateTorsoInclination(bodyPartData);

            if (Vector3D.AngleBetween(currentInclination, this.calibratedInclination) > SharedContent.GetAllowableDeviationInDegrees())
            {
                return false;
            }

            return true;
        }

        public override UserPerformanceAnalysisInfo IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation)
        {
            TorsoOrientation initialTorsoOrientation = (TorsoOrientation)initialOrientation;
            TorsoOrientation finalTorsoOrientation = (TorsoOrientation)finalOrientation;

            //Compute inclination information
            UserPerformanceAnalysisInfo inclinationInfo = BodyPartOrientation.IsVectorOnPathInPlane(this.calibratedInclination, initialTorsoOrientation.calibratedInclination, finalTorsoOrientation.calibratedInclination);
            if (inclinationInfo.failed) return inclinationInfo;
            if (Vector3D.AngleBetween(initialTorsoOrientation.inclination, finalTorsoOrientation.inclination) <= SharedContent.GetAllowableDeviationInDegrees())
            {
                //The actual exercise has a negligable change in the torso inclination
                inclinationInfo.negligableAction = true;
            }

            //Compute rotation information
            if (this.calibratedRotation < Math.Min(initialTorsoOrientation.calibratedRotation, finalTorsoOrientation.calibratedRotation) - SharedContent.GetAllowableDeviationInDegrees() ||
                this.calibratedRotation > Math.Max(initialTorsoOrientation.calibratedRotation, finalTorsoOrientation.calibratedRotation) + SharedContent.GetAllowableDeviationInDegrees())
            {
                //The current rotation is not within the range specified by the initial and final orientations
                return new UserPerformanceAnalysisInfo(true, "User's torso rotation is not in the range defined by the exercise step");
            }

            UserPerformanceAnalysisInfo rotationInfo = new UserPerformanceAnalysisInfo(false);
            if (Math.Abs(finalTorsoOrientation.rotation - initialTorsoOrientation.rotation) <= SharedContent.GetAllowableDeviationInDegrees())
            {
                //The actual exercise has a negligable change in the torso rotation
                rotationInfo.negligableAction = true;
            }
            double currentRotationTraveled = Math.Abs(this.calibratedRotation - initialTorsoOrientation.calibratedRotation);
            double totalRotationToTravel = Math.Abs(finalTorsoOrientation.calibratedRotation - initialTorsoOrientation.calibratedRotation);
            rotationInfo.percentComplete = (int)(100 * (currentRotationTraveled / totalRotationToTravel));

            if (rotationInfo.negligableAction && inclinationInfo.negligableAction)
            {
                return new UserPerformanceAnalysisInfo(true);
            }
            else if (inclinationInfo.negligableAction)
            {
                return rotationInfo;
            }
            else if (rotationInfo.negligableAction)
            {
                return inclinationInfo;
            }
            else
            {
                if (Math.Abs(inclinationInfo.percentComplete - rotationInfo.percentComplete) > SharedContent.GetAllowableDeviationInPercent())
                {
                    return new UserPerformanceAnalysisInfo(true, String.Format("The difference in the percentage completion of the change in torso inclination and change in torso rotation exceeded the maximum allowable deviation of {0}",
                        SharedContent.GetAllowableDeviationInPercent()));
                }

                //Return the average percent complete
                return new UserPerformanceAnalysisInfo((int)((inclinationInfo.percentComplete + rotationInfo.percentComplete) / 2.0));
            }
        }
    }
}
