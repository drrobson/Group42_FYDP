﻿using System;
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

        public static bool areOrientationsEqual(LimbOrientation limbOrientation1, Joint limb2pivot, Joint limb2movable)
        {
            return areOrientationsEqual(limbOrientation1, new LimbOrientation(limb2pivot, limb2movable));
        }

        public static SharedContent.Progression checkLimbProgression(LimbOrientation curLimbOrientation, 
            LimbOrientation newLimbOrientation,
            LimbOrientation endLimbOrientation)
        {
            double prevXDelta = endLimbOrientation.xAngle - curLimbOrientation.xAngle;
            double progXDelta = endLimbOrientation.xAngle - newLimbOrientation.xAngle;
            double prevYDelta = endLimbOrientation.yAngle - curLimbOrientation.yAngle;
            double progYDelta = endLimbOrientation.yAngle - newLimbOrientation.yAngle;
            double prevZDelta = endLimbOrientation.zAngle - curLimbOrientation.zAngle;
            double progZDelta = endLimbOrientation.zAngle - newLimbOrientation.zAngle;

            if (progXDelta > progYDelta && progXDelta > progZDelta && prevXDelta > progXDelta)
            { // Rotating mostly around the x axis
                return (SharedContent.Progression) (newLimbOrientation.xAngle / endLimbOrientation.xAngle * 
                    (double) SharedContent.Progression.Completed);
            }

            if (progYDelta > progZDelta && prevYDelta > progYDelta)
            { // Rotating mostly around the y axis
                return (SharedContent.Progression) (newLimbOrientation.yAngle / endLimbOrientation.yAngle * 
                    (double)SharedContent.Progression.Completed);
            }

            if (prevZDelta > progZDelta)
            { // Rotating mostly around the z axis
                return (SharedContent.Progression) (newLimbOrientation.zAngle / endLimbOrientation.zAngle * 
                    (double)SharedContent.Progression.Completed);
            }

            return SharedContent.Progression.Failed;
        }
    }
}
