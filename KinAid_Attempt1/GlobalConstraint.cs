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
        SharedContent.LimbID constraintLimb1; // One of the limbs to which the constraint applies
        SharedContent.LimbID constraintLimb2; // One of the limbs to which the constraint applies
        double constraintAngle; // The allowable angle between both limbs in the pair
        double allowableDeviation; // The allowable deviation for the angle between two limbs in the pair

        /// <summary>
        /// Verifies that the orientation of the limb(s) defined in this instance occur in the SkeletonData that is passed in
        /// </summary>
        /// <param name="currData"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        public SharedContent.Progression verify(SkeletonData currData, SkeletonData newData = null)
        {
            return 0;
        }
    }
}
