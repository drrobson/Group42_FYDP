﻿using System;
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
        public IConstraint[] variableConstraints // the constraints for each limb that define the motion in the exercise
        {
            get;
            private set;
        }

        SkeletonData currentData; // current orientation of each limb
        SharedContent.Progression totalProgression; // percentage of progression in this particular state
        public DateTime timeStarted // the time at  which the user started the exercise to make sure he isn't going over the timeout period
        {
            get;
            set;
        }

        public ExerciseState(IConstraint[] constraints)
        {
            this.variableConstraints = constraints;
        }

        public ExerciseState(IConstraint constraint)
        {
            this.variableConstraints = new IConstraint[1];
            variableConstraints[0] = constraint;
        }

        /// <summary>
        /// This method updates the particular state of the exercise, by checking the SkeletonData against the constraint
        /// </summary>
        /// <param name="data">The data with which the state should be updated</param>
        /// <returns>A SharedContent.Progression value indicating the status of the exercise</returns>
        public SharedContent.Progression updateState(SkeletonData newData)
        {
            foreach (IConstraint constraint in variableConstraints)
            {
                SharedContent.Progression tempProgression = constraint.verify(currentData, newData);
                totalProgression = SharedContent.Progression.Completed;
                if (tempProgression < totalProgression)
                { // the progress of the constraint furthest from completion 
                    totalProgression = tempProgression;
                }
                if (DateTime.Now - timeStarted > ((VariableConstraint)constraint).timeout)
                { // if any constraint times out, then the exercise is not completed
                    return SharedContent.Progression.Failed;
                }
            }
            currentData = newData;

            return totalProgression;
        }
    }
}
