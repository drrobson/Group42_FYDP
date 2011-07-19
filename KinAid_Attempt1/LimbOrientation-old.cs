using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class LimbOrientationOld
    {
        private Vector3D pivotToMovable;
        public JointID pivotID
        {
            get;
            private set;
        }
        public JointID movableID
        {
            get;
            private set;
        }
        public double xAngle
        {
            get;
            private set;
        }
        public double yAngle
        {
            get;
            private set;
        }
        public double zAngle
        {
            get;
            private set;
        }

        public LimbOrientationOld(Joint pivot, Joint movable)
        {
            this.pivotToMovable = new Vector3D(movable.Position.X - pivot.Position.X,
                movable.Position.Y - pivot.Position.Y, movable.Position.Z - pivot.Position.Z);
            this.pivotID = pivot.ID;
            this.movableID = movable.ID;

            xAngle = Vector3D.AngleBetween(new Vector3D(1, 0, 0), pivotToMovable);
            yAngle = Vector3D.AngleBetween(new Vector3D(0, 1, 0), pivotToMovable);
            zAngle = Vector3D.AngleBetween(new Vector3D(0, 0, 1), pivotToMovable);
        }

        public LimbOrientationOld(JointID pivotID, JointID movableID, double xAngle, double yAngle, double zAngle)
        {
            this.pivotID = pivotID;
            this.movableID = movableID;

            this.xAngle = xAngle;
            this.yAngle = yAngle;
            this.zAngle = zAngle;
        }

        public static double angleBetweenLimbs(LimbOrientationOld limb1, LimbOrientationOld limb2)
        {
            return Vector3D.AngleBetween(limb1.pivotToMovable, limb2.pivotToMovable);
        }

        public static bool areOrientationsEqual(LimbOrientationOld limbOrientation1, LimbOrientationOld limbOrientation2)
        {
            if (
                ((limbOrientation1.xAngle < limbOrientation2.xAngle && (limbOrientation1.xAngle + SharedContent.AllowableDeviationInDegrees) > limbOrientation2.xAngle) ||
                 (limbOrientation1.xAngle > limbOrientation2.xAngle && (limbOrientation1.xAngle - SharedContent.AllowableDeviationInDegrees) < limbOrientation2.xAngle))
                &&
                ((limbOrientation1.yAngle < limbOrientation2.yAngle && (limbOrientation1.yAngle + SharedContent.AllowableDeviationInDegrees) > limbOrientation2.yAngle) ||
                 (limbOrientation1.yAngle > limbOrientation2.yAngle && (limbOrientation1.yAngle - SharedContent.AllowableDeviationInDegrees) < limbOrientation2.yAngle))
                &&
                ((limbOrientation1.zAngle < limbOrientation2.zAngle && (limbOrientation1.zAngle + SharedContent.AllowableDeviationInDegrees) > limbOrientation2.zAngle) ||
                 (limbOrientation1.zAngle > limbOrientation2.zAngle && (limbOrientation1.zAngle - SharedContent.AllowableDeviationInDegrees) < limbOrientation2.zAngle))
                )
            {
                return true;
            }
            return false;
        }

        public static bool areOrientationsEqual(LimbOrientationOld limbOrientation1, Joint limb2pivot, Joint limb2movable)
        {
            return areOrientationsEqual(limbOrientation1, new LimbOrientationOld(limb2pivot, limb2movable));
        }

        public static SharedContent.Progression checkLimbProgression(LimbOrientationOld startLimbOrientation, 
            LimbOrientationOld curLimbOrientation,
            LimbOrientationOld endLimbOrientation)
        {
            /*
            double prevXAngle = endLimbOrientation.xAngle - curLimbOrientation.xAngle;
            double progXAngle = endLimbOrientation.xAngle - newLimbOrientation.xAngle;
            double prevYAngle = endLimbOrientation.yAngle - curLimbOrientation.yAngle;
            double progYAngle = endLimbOrientation.yAngle - newLimbOrientation.yAngle;
            double prevZAngle = endLimbOrientation.zAngle - curLimbOrientation.zAngle;
            double progZAngle = endLimbOrientation.zAngle - newLimbOrientation.zAngle;

            if (progXAngle > progYAngle && progXAngle > progZAngle && prevXAngle > progXAngle)
            { // Rotating mostly around the x axis
                return (SharedContent.Progression) (newLimbOrientation.xAngle / endLimbOrientation.xAngle * 
                    (double) SharedContent.Progression.Completed);
            }

            if (progYAngle > progZAngle && prevYAngle > progYAngle)
            { // Rotating mostly around the y axis
                return (SharedContent.Progression) (newLimbOrientation.yAngle / endLimbOrientation.yAngle * 
                    (double)SharedContent.Progression.Completed);
            }

            if (prevZAngle > progZAngle)
            { // Rotating mostly around the z axis
                return (SharedContent.Progression) (newLimbOrientation.zAngle / endLimbOrientation.zAngle * 
                    (double)SharedContent.Progression.Completed);
            }
             * */
            double totalRequiredDisplacement, totalRemainingDisplacement;

            totalRequiredDisplacement = Math.Abs(endLimbOrientation.xAngle - startLimbOrientation.xAngle) + Math.Abs(endLimbOrientation.yAngle - startLimbOrientation.yAngle)
                + Math.Abs(endLimbOrientation.zAngle - startLimbOrientation.zAngle);

            totalRemainingDisplacement = Math.Min(Math.Abs(endLimbOrientation.xAngle - curLimbOrientation.xAngle), Math.Abs(endLimbOrientation.xAngle - startLimbOrientation.xAngle))
                + Math.Min(Math.Abs(endLimbOrientation.yAngle - curLimbOrientation.yAngle), Math.Abs(endLimbOrientation.yAngle - startLimbOrientation.yAngle))
                + Math.Min(Math.Abs(endLimbOrientation.zAngle - curLimbOrientation.zAngle), Math.Abs(endLimbOrientation.zAngle - startLimbOrientation.zAngle));

            // For axes where the expected displacement is 0, if the actual displacement exceeds some global error threshold, we consider it an incorrectly performed exercise
            if (
                (endLimbOrientation.xAngle - startLimbOrientation.xAngle == 0 && Math.Abs(endLimbOrientation.xAngle - curLimbOrientation.xAngle) > SharedContent.AllowableDeviationInDegrees)
                || (endLimbOrientation.yAngle - startLimbOrientation.yAngle == 0 && Math.Abs(endLimbOrientation.yAngle - curLimbOrientation.yAngle) > SharedContent.AllowableDeviationInDegrees)
                || (endLimbOrientation.zAngle - startLimbOrientation.zAngle == 0 && Math.Abs(endLimbOrientation.zAngle - curLimbOrientation.zAngle) > SharedContent.AllowableDeviationInDegrees)
               )
            {
                Console.WriteLine("Performed exercise incorrectly! Attempt failed");
                return SharedContent.Progression.Failed;
            }

            return (SharedContent.Progression)((totalRemainingDisplacement / totalRequiredDisplacement) * (double)SharedContent.Progression.Completed);

            //return SharedContent.Progression.Failed;
        }
    }
}
