using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    /// <summary>
    /// This class defines an exercise performed by the user in front of the Kinect.
    /// </summary>
    public class Exercise
    {
        public int[] limbs // the limbs involved in the exercise
        {
            get;
            private set;
        }
        public IConstraint initialConstraint // the constraints that define the user's initial pose
        {
            get;
            private set;
        }
        public IConstraint[] globalConstraints // the constraints that should hold throughout the entire exercise
        {
            get;
            private set;
        }

        public ExerciseState currentState // holds the state of the user's current position
        {
            get;
            set;
        }

        public Exercise(int[] limbs, 
            PoseConstraint initialConstraint, 
            GlobalConstraint[] globalConstraints, 
            VariableConstraint[] variableConstraints)
        {
            this.limbs = limbs;
            this.initialConstraint = initialConstraint;
            this.globalConstraints = globalConstraints;

            currentState = new ExerciseState(variableConstraints);
        }

        public void startExercise()
        {
            currentState.timeStarted = DateTime.Now;
        }

        public SharedContent.Progression updateExercise(SkeletonData data)
        {
            return 0;
        }

        public SharedContent.Progression nextState()
        {
            return 0;
        }
    }
}
