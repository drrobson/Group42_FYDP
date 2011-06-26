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
