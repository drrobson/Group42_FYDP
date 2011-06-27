using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    /// <summary>
    /// This class defines a particular state in an exercise, and how close it is to completion.
    /// Exercises are defined in terms of variable constraints, so an exerciseState determines how close the user is to meeting a constraint.
    /// </summary>
    public class ExerciseState
    {
        public VariableConstraint[] variableConstraints // the constraints for each limb that define the motion in the exercise
        {
            get;
            private set;
        }

        SkeletonData currentData; // current orientation of each limb
        SharedContent.Progression totalProgression = SharedContent.Progression.Started; // percentage of progression in this particular state
        public DateTime timeStarted // the time at  which the user started the exercise to make sure he isn't going over the timeout period
        {
            get;
            set;
        }

        public ExerciseState(VariableConstraint[] constraints)
        {
            this.variableConstraints = constraints;
        }

        /// <summary>
        /// This method updates the particular state of the exercise, by checking the SkeletonData against the constraint
        /// </summary>
        /// <param name="data">The data with which the state should be updated</param>
        /// <returns>A SharedContent.Progression value indicating the status of the exercise</returns>
        public SharedContent.Progression updateState(SkeletonData newData)
        {
            double overallProgress = (double)SharedContent.Progression.Started;

            foreach (VariableConstraint constraint in variableConstraints)
            {
                //if (currentData == null) break;
                SharedContent.Progression currentProgress = constraint.verify(currentData, newData);
                if (currentProgress == SharedContent.Progression.NotStarted)
                {
                    continue;
                }

                overallProgress += (int)currentProgress;

                if (DateTime.Now - timeStarted > constraint.timeout)
                { // if any constraint times out, then the exercise is not completed
                    return SharedContent.Progression.Failed;
                }
            }
            currentData = newData;

            totalProgression = (SharedContent.Progression)(Math.Min((double)SharedContent.Progression.Completed, overallProgress / variableConstraints.Length));
            return totalProgression;
        }
    }
}
