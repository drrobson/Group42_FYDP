using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class ExerciseStep
    {
        public Pose initialPose, finalPose;
        public TimeSpan expectedDuration;
        public ExerciseStepType stepType;
        public ExerciseStepStatus stepStatus;
        

        private DateTime startTime;

        public ExerciseStep(Pose initialPose, Pose finalPose, TimeSpan expectedDuration, ExerciseStepType stepType)
        {
            this.initialPose = initialPose;
            this.finalPose = finalPose;
            this.expectedDuration = expectedDuration;
            this.stepType = stepType;

            stepStatus = ExerciseStepStatus.NotInInitialPose;
        }

        public ExerciseStepStatus PerformStep(SkeletonData userData)
        {
            switch (this.stepStatus)
            {
                case ExerciseStepStatus.NotInInitialPose:
                    if (initialPose.IsInPose(userData))
                    {
                        this.stepStatus = ExerciseStepStatus.ReadyToStart;
                        return this.stepStatus;
                    }
                    break;
                case ExerciseStepStatus.ReadyToStart:
                    if (!initialPose.IsInPose(userData))
                    {
                        //User has left the initial pose -- equates to starting the exercise
                        startTime = DateTime.Now;
                        this.stepStatus = ExerciseStepStatus.InProgress;
                    }
                    break;
                case ExerciseStepStatus.InProgress:
                    if (finalPose.IsInPose(userData))
                    {
                        this.stepStatus = ExerciseStepStatus.Complete;
                    }
                    else
                    {
                        int percentComplete = this.IsUserPerformingStepCorrectly(userData);
                        if (percentComplete == -1)
                        {
                            this.stepStatus = ExerciseStepStatus.Failed;
                        }
                        Console.WriteLine("Exercise step is {0} percent complete", percentComplete);
                    }
                    break;
            }

            return this.stepStatus;
        }

        public DateTime GetExpectedCompletionTime()
        {
            if (this.stepStatus == ExerciseStepStatus.InProgress)
            {
                return this.startTime + this.expectedDuration;
            }
            else
            {
                return DateTime.MaxValue;
            }
        }

        /// <summary>
        /// Determines whether the user is performing the exercise correctly. Returns -1 if the user has failed to maintain proper form, else returns the approximate percentage
        /// of the exercise step that the user has completed
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public int IsUserPerformingStepCorrectly(SkeletonData userData)
        {
            Pose currentPose = new Pose(userData, initialPose.bodyPartsOfInterest);

            return currentPose.IsInBetweenPoses(initialPose, finalPose);
        }
    }

    public enum ExerciseStepStatus
    {
        NotInInitialPose = 0,
        ReadyToStart = 1,
        InProgress = 2,
        Complete = 3,
        Failed = 4
    }

    public enum ExerciseStepType
    {
        Hold = 0,
        Movement = 1
    }
}
