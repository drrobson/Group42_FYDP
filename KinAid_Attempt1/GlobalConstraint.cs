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
        SharedContent.Axis axis;
        double constraintAngle; // The allowable angle between both limbs in the pair
        double allowableDeviation; // The allowable deviation for the angle between two limbs in the pair

        public GlobalConstraint(JointID firstJoint, JointID secondJoint, JointID thirdJoint, 
            SharedContent.Axis axis, double constraintAngle, double allowableDeviation)
        {
            this.firstJoint = firstJoint;
            this.secondJoint = secondJoint;
            this.thirdJoint = thirdJoint;
            this.axis = axis;
            this.constraintAngle = constraintAngle;
            this.allowableDeviation = allowableDeviation;
        }

        public SharedContent.Progression verify(SkeletonData currData, SkeletonData newData = null)
        {
            LimbOrientation limb1 = new LimbOrientation(currData.Joints[firstJoint], currData.Joints[secondJoint]);
            LimbOrientation limb2 = new LimbOrientation(currData.Joints[secondJoint], currData.Joints[thirdJoint]);
            if (Math.Abs(LimbOrientation.angleBetweenLimbs(limb1, limb2) - constraintAngle) < allowableDeviation)
            {
                return SharedContent.Progression.Completed;
            }
            return SharedContent.Progression.Failed;
        }
    }
}
