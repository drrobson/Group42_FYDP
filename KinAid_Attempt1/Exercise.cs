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
        public PoseConstraint initialConstraint // the constraints that define the user's initial pose
        {
            get;
            private set;
        }
        public GlobalConstraint[] globalConstraints // the constraints that should hold throughout the entire exercise
        {
            get;
            private set;
        }

        public SharedContent.Progression progression // the progression of the overall exercise
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

            progression = SharedContent.Progression.NotStarted;
            currentState = new ExerciseState(variableConstraints);
        }

        public void startExercise()
        {
            currentState.timeStarted = DateTime.Now;
        }

        public SharedContent.Progression updateExercise(SkeletonData data)
        {
            if (progression == SharedContent.Progression.NotStarted && initialConstraint.verify(data) != SharedContent.Progression.Completed)
            { // check the initial pose constraints for this exercise
                return SharedContent.Progression.NotStarted;
            }
            progression = SharedContent.Progression.Start;
            if (initialConstraint.verify(data) == SharedContent.Progression.Completed)
            { // check to see if the user has started moving
                return SharedContent.Progression.Start;
            }

            foreach (GlobalConstraint constraint in globalConstraints)
            { // check all of the global constraints
                if (constraint.verify(data) != SharedContent.Progression.Completed)
                {
                    return SharedContent.Progression.Failed;
                }
            }

            progression = currentState.updateState(data);
            if (progression == SharedContent.Progression.Completed)
            { // check if a particular state of the exercise was completed
                progression = nextState();
            }
            return progression;
        }

        public SharedContent.Progression nextState()
        {
            return 0;
        }
    }
}
