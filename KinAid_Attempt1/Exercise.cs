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
        public Pose[] exercisePoses;
        //Array of exercise steps
        string name, description;

        public Exercise(Pose[] exercisePoses)
        {
            
        }

        /*
        /// <summary>
        /// Asserts the initial pose constraint; if the user's limb orientations as defined in the SkeletonData satisfy the initial
        /// pose constraint of the exercise then the exercise is considered to be started
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Bool indicating whether the exercise has been successfully started (i.e. the SkeletonData satisfies the initial pose constraint
        /// for the exercise</returns>
        public bool startExercise(SkeletonData data)
        {
            if (initialConstraint.verify(data) == SharedContent.Progression.Completed)
            {
                currentState.timeStarted = DateTime.Now;
                this.progression = SharedContent.Progression.Started;
                return true;
            }
            else
            {
                return false;
            }
        }
         * */

        public SharedContent.Progression updateExercise(SkeletonData data)
        {
            
            /*
            if (progression == SharedContent.Progression.NotStarted && !initialConstraint.verify(data))
            { // check the initial pose constraints for this exercise
                return SharedContent.Progression.NotStarted;
            }
            progression = SharedContent.Progression.Started;
            if (initialConstraint.verify(data))
            { // check to see if the user has started moving
                return SharedContent.Progression.Started;
            }
             * */

            /*
            foreach (GlobalConstraint constraint in globalConstraints)
            { // check all of the global constraints
                if (constraint.verify(data) == SharedContent.Progression.Failed)
                {
                    return SharedContent.Progression.Failed;
                }
            }

            progression = currentState.updateState(data);
            Console.WriteLine("Exercise is {0}% complete", progression);
            if (progression == SharedContent.Progression.Completed)
            { // check if a particular state of the exercise was completed
                progression = nextState();
            }
            return progression;
             * */
            return SharedContent.Progression.Failed;
        }

        public SharedContent.Progression nextState()
        {
            // We are still determining how to represent the multi-step aspect of exercises, for now we assume the exercise is complete
            return SharedContent.Progression.Completed;
        }

        public string printState()
        {
            /*
            switch (progression)
            {
                case SharedContent.Progression.NotStarted:
                    return "Looking for pose";
                case SharedContent.Progression.Started:
                    return "Start the exercise";
                case SharedContent.Progression.Completed:
                    return "Exercise Complete";
                case SharedContent.Progression.Failed:
                    return "Exercise Failed";
                default:
                    return String.Format("Exercise {0}% complete", progression);
            }
             * */
            return null;
        }
    }
}
