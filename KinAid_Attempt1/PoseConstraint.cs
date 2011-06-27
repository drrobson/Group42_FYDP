using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class PoseConstraint : IConstraint
    {
        LimbOrientation[] limbsToCheck;

        public PoseConstraint(LimbOrientation[] limbsToCheck)
        {
            this.limbsToCheck = limbsToCheck;
        }

        /// <summary>
        /// Verifies that the limbs in the SkeletonData passed in are oriented in the same way as the
        /// set of the LimbOrientations that comprise this pose
        /// </summary>
        /// <param name="currData"></param>
        /// <param name="newData"></param>
        /// <returns>Bool indicating whether the orientation of the limbs in the skeletal data are the same as this constraint's set of
        /// limb orientations</returns>
        public SharedContent.Progression verify(SkeletonData currData, SkeletonData newData = null)
        {
            foreach (LimbOrientation limb in limbsToCheck)
            {
                if (!LimbOrientation.areOrientationsEqual(limb, currData.Joints[limb.pivotID], currData.Joints[limb.movableID]))
                {
                    return SharedContent.Progression.Failed;
                }
            }

            return SharedContent.Progression.Completed;
        }
    }
}
