using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class Exercise
    {
        GlobalConstraint[] globalConstraints;
        ExerciseState currentState;
        PoseConstraint initialConstraint;
        VariableConstraint[] variableConstraints;
        JointID[] limbs;

        public int nextState()
        {
            return 0;
        }
    }
}
