using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class LimbOrientation
    {
        private Vector3D pivotToMovable;
        private JointID pivotID, movableID;
        public double xAngle, yAngle, zAngle;

        public LimbOrientation(Joint pivot, Joint movable)
        {
            this.pivotToMovable = new Vector3D(movable.Position.X - pivot.Position.X,
                movable.Position.Y - pivot.Position.Y, movable.Position.Z - pivot.Position.Z);
            this.pivotID = pivot.ID;
            this.movableID = movable.ID;

            xAngle = Vector3D.AngleBetween(new Vector3D(1, 0, 0), pivotToMovable);
            yAngle = Vector3D.AngleBetween(new Vector3D(0, 1, 0), pivotToMovable);
            zAngle = Vector3D.AngleBetween(new Vector3D(0, 0, 1), pivotToMovable);
        }

        public LimbOrientation(JointID pivotID, JointID movableID, double xAngle, double yAngle, double zAngle)
        {
            this.pivotID = pivotID;
            this.movableID = movableID;

            this.xAngle = xAngle;
            this.yAngle = yAngle;
            this.zAngle = zAngle;
        }

        public static double angleBetweenLimbs(LimbOrientation limb1, LimbOrientation limb2)
        {
            return Vector3D.AngleBetween(limb1.pivotToMovable, limb2.pivotToMovable);
        }

        public static bool areOrientationsEqual(LimbOrientation limbOrientation1, LimbOrientation limbOrientation2)
        {
            if (
                ((limbOrientation1.xAngle + (limbOrientation1.xAngle * SharedContent.AllowableDeviation)) > limbOrientation2.xAngle &&
                 (limbOrientation1.xAngle - (limbOrientation1.xAngle * SharedContent.AllowableDeviation)) < limbOrientation2.xAngle)
                &&
                ((limbOrientation1.yAngle + (limbOrientation1.yAngle * SharedContent.AllowableDeviation)) > limbOrientation2.yAngle &&
                 (limbOrientation1.yAngle - (limbOrientation1.yAngle * SharedContent.AllowableDeviation)) < limbOrientation2.yAngle)
                &&
                ((limbOrientation1.zAngle + (limbOrientation1.zAngle * SharedContent.AllowableDeviation)) > limbOrientation2.zAngle &&
                 (limbOrientation1.zAngle - (limbOrientation1.zAngle * SharedContent.AllowableDeviation)) < limbOrientation2.zAngle)
                )
            {
                return true;
            }
            return false;
        }

        public static bool checkLimbProgression(LimbOrientation previousLimbOrientation, LimbOrientation progressingLimbOrientation,
            LimbOrientation futureLimbOrientation)
        {
            if (
                ((previousLimbOrientation.xAngle - (previousLimbOrientation.xAngle * SharedContent.AllowableDeviation)) < progressingLimbOrientation.xAngle &&
                 (futureLimbOrientation.xAngle + (futureLimbOrientation.xAngle * SharedContent.AllowableDeviation)) > progressingLimbOrientation.xAngle)
                &&
                ((previousLimbOrientation.yAngle - (previousLimbOrientation.yAngle * SharedContent.AllowableDeviation)) < progressingLimbOrientation.yAngle &&
                 (futureLimbOrientation.yAngle + (futureLimbOrientation.yAngle * SharedContent.AllowableDeviation)) > progressingLimbOrientation.yAngle)
                &&
                ((previousLimbOrientation.zAngle - (previousLimbOrientation.zAngle * SharedContent.AllowableDeviation)) < progressingLimbOrientation.zAngle &&
                 (futureLimbOrientation.zAngle + (futureLimbOrientation.zAngle * SharedContent.AllowableDeviation)) > progressingLimbOrientation.zAngle)
                )
            {
                return true;
            }
            return false;
        }
    }
}
