using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    /// <summary>
    /// This class defines a global constraint, which is a constraint that must hold throughout an exercise.
    /// </summary>
    public class GlobalConstraint : IConstraint
    {
        JointID firstJoint; // The pivot joint in the first limb
        JointID secondJoint; // The movable joint of the first limb/pivot joint of the second limb
        JointID thirdJoint; // The movable joint of the second limb
        double constraintAngle; // The allowable angle between both limbs in the pair
        double allowableDeviation; // The allowable deviation for the angle between two limbs in the pair

        public GlobalConstraint(JointID firstJoint, JointID secondJoint, JointID thirdJoint, 
            double constraintAngle, double allowableDeviation)
        {
            this.firstJoint = firstJoint;
            this.secondJoint = secondJoint;
            this.thirdJoint = thirdJoint;
            this.constraintAngle = constraintAngle;
            this.allowableDeviation = allowableDeviation;
        }

        public SharedContent.Progression verify(SkeletonData currData, SkeletonData newData = null)
        {
            LimbOrientationOld limb1 = new LimbOrientationOld(currData.Joints[firstJoint], currData.Joints[secondJoint]);
            LimbOrientationOld limb2 = new LimbOrientationOld(currData.Joints[secondJoint], currData.Joints[thirdJoint]);
            if (Math.Abs(LimbOrientationOld.angleBetweenLimbs(limb1, limb2) - constraintAngle) < allowableDeviation)
            {
                return SharedContent.Progression.Completed;
            }
            return SharedContent.Progression.Failed;
        }
    }
}
