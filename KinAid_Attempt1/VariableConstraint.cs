using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    /// <summary>
    /// A variable constraint defines a particular movement being performed as part of an exercise
    /// </summary>
    public class VariableConstraint : IConstraint
    {
        string name; // name of the constraint
        public TimeSpan timeout // amount of time in total that the user can take to complete this constraint
        {
            get;
            private set;
        }
        LimbOrientationOld startingOrientation; //initial orientation
        LimbOrientationOld endingOrientation; // objective to reach to meet the constraint

        public VariableConstraint(string name, TimeSpan timeout, LimbOrientationOld startingOrientation, LimbOrientationOld endingOrientation)
        {
            this.name = name;
            this.timeout = timeout;
            this.startingOrientation = startingOrientation;
            this.endingOrientation = endingOrientation;
        }

        public SharedContent.Progression verify(SkeletonData currData, SkeletonData newData)
        {
            //LimbOrientation currOrientation = new LimbOrientation(currData.Joints[endingOrientation.pivotID], currData.Joints[endingOrientation.movableID]);
            LimbOrientationOld newOrientation = new LimbOrientationOld(newData.Joints[endingOrientation.pivotID], newData.Joints[endingOrientation.movableID]);

            if (LimbOrientationOld.areOrientationsEqual(endingOrientation, newOrientation))
            {
                Console.WriteLine("Found equal in verify");
                return SharedContent.Progression.Completed;
            }

            return LimbOrientationOld.checkLimbProgression(startingOrientation, newOrientation, endingOrientation);
        }
    }
}
