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
        public ExerciseStep[] exerciseSteps;

        public ExerciseStatus exerciseStatus
        {
            get;
            private set;
        }
        public int currentStepIndex
        {
            get;
            private set;
        }
        public SharedContent.Commands name
        {
            get;
            private set;
        }
        public string description;
        public string statusMessage
        {
            get;
            private set;
        }

        public Exercise(SharedContent.Commands name, string description, Pose[] exercisePoses, ExerciseStep[] exerciseSteps)
        {
            this.currentStepIndex = 0;
            this.name = name;
            this.description = description;
            this.exercisePoses = exercisePoses;
            this.exerciseSteps = exerciseSteps;

            this.exerciseStatus = ExerciseStatus.NotStarted;
            this.statusMessage = "Waiting for user to assume the starting pose of the first exercise step";
        }

        public Pose[] getPosesToBeCalibrated()
        {
            return this.exercisePoses;
        } 

        public ExerciseStatusInfo PerformExercise(SkeletonData userData)
        {
            string statusMessage = "Status message not set in Exercise";
            ExerciseStepStatusInfo exerciseStepInfo = this.exerciseSteps[currentStepIndex].PerformStep(userData);

            switch (this.exerciseStatus)
            {
                case ExerciseStatus.NotStarted:
                    if (exerciseStepInfo.exerciseStepStatus == ExerciseStepStatus.NotInInitialPose)
                    {
                        statusMessage = "Waiting for user to assume the starting pose of the first exercise step";
                    }
                    else if (exerciseStepInfo.exerciseStepStatus == ExerciseStepStatus.ReadyToStart)
                    {
                        this.exerciseStatus = ExerciseStatus.InProgress;
                        statusMessage = exerciseStepInfo.statusMessage;
                    }
                    break;
                case ExerciseStatus.InProgress:
                    if (exerciseStepInfo.exerciseStepStatus == ExerciseStepStatus.ReadyToStart)
                    {
                        statusMessage = exerciseStepInfo.statusMessage;
                    }
                    else if (exerciseStepInfo.exerciseStepStatus == ExerciseStepStatus.InProgress)
                    {
                        statusMessage = exerciseStepInfo.statusMessage;
                    }
                    else if (exerciseStepInfo.exerciseStepStatus == ExerciseStepStatus.Failed)
                    {
                        this.exerciseStatus = ExerciseStatus.Failed;
                        statusMessage = exerciseStepInfo.statusMessage;
                    }
                    else if (exerciseStepInfo.exerciseStepStatus == ExerciseStepStatus.Complete)
                    {
                        currentStepIndex++;

                        if (currentStepIndex == exerciseSteps.Length)
                        {
                            this.exerciseStatus = ExerciseStatus.Complete;
                            statusMessage = "Exercise complete!";
                        }
                    }
                    else
                    {
                        statusMessage = exerciseStepInfo.statusMessage;
                    }
                    break;
            }

            return new ExerciseStatusInfo(this.exerciseStatus, statusMessage);
        }

        public void Reset()
        {
            this.currentStepIndex = 0;
            this.exerciseStatus = ExerciseStatus.NotStarted;
            foreach (ExerciseStep step in exerciseSteps)
            {
                //step.reset();
            }
        }
    }

    public enum ExerciseStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Complete = 2,
        Failed = 3
    }

    public struct ExerciseStatusInfo
    {
        public ExerciseStatusInfo(ExerciseStatus exStatus, string statusMsg) { this.exerciseStatus = exStatus; this.statusMessage = statusMsg; }
        public ExerciseStatus exerciseStatus;
        public string statusMessage;
    }
}
