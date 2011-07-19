﻿using System;
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

        /*
        public static double IdealNeutralTorsoRotation = 0;
        public static Vector3D IdealNeutralTorsoInclination = new Vector3D(0, 1, 0);
        public static double ActualNeutralTorsoRotation;        //Looking at the top of the user's head, rotation is positive in the counter-clockwise direction
        public static Vector3D ActualNeutralTorsoInclination;   //Torso orientation relative to the hips
         * */

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

            if (Math.Abs(currentRotation - this.calibratedRotation) > SharedContent.AllowableDeviationInDegrees)
            {
                return false;
            }

            Vector3D currentInclination = TorsoOrientation.CalculateTorsoInclination(bodyPartData);

            if (Vector3D.AngleBetween(currentInclination, this.calibratedInclination) > SharedContent.AllowableDeviationInDegrees)
            {
                return false;
            }

            return true;
        }

        public override int IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation)
        {
            TorsoOrientation initialTorsoOrientation = (TorsoOrientation)initialOrientation;
            TorsoOrientation finalTorsoOrientation = (TorsoOrientation)finalOrientation;

            int percentInclinationTraveled = BodyPartOrientation.IsVectorOnPathInPlane(this.calibratedInclination, initialTorsoOrientation.calibratedInclination, finalTorsoOrientation.calibratedInclination);
            if (percentInclinationTraveled == -1) return -1;

            double currentRotationTraveled = Math.Abs(this.calibratedRotation - initialTorsoOrientation.calibratedRotation);
            double totalRotationToTravel = Math.Abs(finalTorsoOrientation.calibratedRotation - initialTorsoOrientation.calibratedRotation);
            if (totalRotationToTravel <= SharedContent.AllowableDeviationInDegrees)
            {
                if (percentInclinationTraveled == -2) return -2;
                else return percentInclinationTraveled;
            }

            if (finalTorsoOrientation.calibratedRotation < initialTorsoOrientation.calibratedRotation)
            {
                if (this.calibratedRotation > initialTorsoOrientation.calibratedRotation + SharedContent.AllowableDeviationInDegrees ||
                    this.calibratedRotation < initialTorsoOrientation.calibratedRotation - SharedContent.AllowableDeviationInDegrees)
                {
                    return -1;
                }
                if (this.calibratedRotation > initialTorsoOrientation.calibratedRotation) currentRotationTraveled = 0;
            }
            else
            {
                if (this.calibratedRotation < initialTorsoOrientation.calibratedRotation - SharedContent.AllowableDeviationInDegrees ||
                    this.calibratedRotation > finalTorsoOrientation.calibratedRotation + SharedContent.AllowableDeviationInDegrees)
                {
                    return -1;
                }
                if (this.calibratedRotation < initialTorsoOrientation.calibratedRotation) currentRotationTraveled = 0;
            }

            int percentRotationTraveled;
            if (totalRotationToTravel == 0)
            {
                percentRotationTraveled = percentInclinationTraveled;
            }
            else
            {
                percentRotationTraveled = (int)(100 * (currentRotationTraveled / totalRotationToTravel));
            }

            if (Math.Abs(percentInclinationTraveled - percentRotationTraveled) > SharedContent.AllowableDeviationInPercent)
            {
                return -1;
            }

            //Return the average percent complete
            return (int)((percentInclinationTraveled + percentRotationTraveled) / 2.0);
        }
    }
}
