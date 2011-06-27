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
        LimbOrientation startingOrientation; //initial orientation
        LimbOrientation endingOrientation; // objective to reach to meet the constraint

        public VariableConstraint(string name, TimeSpan timeout, LimbOrientation startingOrientation, LimbOrientation endingOrientation)
        {
            this.name = name;
            this.timeout = timeout;
            this.startingOrientation = startingOrientation;
            this.endingOrientation = endingOrientation;
        }

        public SharedContent.Progression verify(SkeletonData currData, SkeletonData newData)
        {
            //LimbOrientation currOrientation = new LimbOrientation(currData.Joints[endingOrientation.pivotID], currData.Joints[endingOrientation.movableID]);
            LimbOrientation newOrientation = new LimbOrientation(newData.Joints[endingOrientation.pivotID], newData.Joints[endingOrientation.movableID]);

            if (LimbOrientation.areOrientationsEqual(endingOrientation, newOrientation))
            {
                Console.WriteLine("Found equal in verify");
                return SharedContent.Progression.Completed;
            }

            return LimbOrientation.checkLimbProgression(startingOrientation, newOrientation, endingOrientation);
        }
    }
}
