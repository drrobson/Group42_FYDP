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
        public string stepName
        {
            get;
            private set;
        }

        private DateTime startTime;

        public ExerciseStep(Pose initialPose, Pose finalPose, TimeSpan expectedDuration, ExerciseStepType stepType, string stepName)
        {
            this.initialPose = initialPose;
            this.finalPose = finalPose;
            this.expectedDuration = expectedDuration;
            this.stepType = stepType;
            this.stepName = stepName;

            stepStatus = ExerciseStepStatus.NotInInitialPose;
        }

        public ExerciseStepStatusInfo PerformStep(SkeletonData userData)
        {
            string statusMessage = "Status message not set";
            switch (this.stepStatus)
            {
                case ExerciseStepStatus.NotInInitialPose:
                    if (initialPose.IsInPose(userData))
                    {
                        this.stepStatus = ExerciseStepStatus.ReadyToStart;
                        statusMessage = "Waiting for user to start performing the exercise step";
                    }
                    else
                    {
                        statusMessage = "Waiting for user to assume the starting pose of the exercise step";
                    }
                    break;
                case ExerciseStepStatus.ReadyToStart:
                    if (this.stepType == ExerciseStepType.Hold || !initialPose.IsInPose(userData))
                    {
                        //User leaving the initial pose equates to starting the exercise
                        startTime = DateTime.Now;
                        this.stepStatus = ExerciseStepStatus.InProgress;
                        statusMessage = "Detected the user starting to perform the exercise step";
                    }
                    else
                    {
                        statusMessage = "Waiting for user to start performing the exercise step";
                    }
                    break;
                case ExerciseStepStatus.InProgress:
                    statusMessage = "Analyzing user's performance of the exercise step...";
                    bool userInFinalPose = finalPose.IsInPose(userData);
                    if (this.stepType == ExerciseStepType.Hold)
                    {
                        if (!userInFinalPose)
                        {
                            this.stepStatus = ExerciseStepStatus.Failed;
                            statusMessage = "User left the pose that they are supposed to be holding";
                        }
                        else
                        {
                            //If the pose has been held for a sufficiently long time...
                            if (DateTime.Now >= this.GetExpectedCompletionTime() - new TimeSpan(0, 0, 0, 0, (int)(this.expectedDuration.TotalMilliseconds * (SharedContent.GetAllowableDeviationInPercent() / 100.0))))
                            {
                                //We check whether it has not been held for too long
                                if (DateTime.Now <= this.GetExpectedCompletionTime() + new TimeSpan(0, 0, 0, 0, (int)(this.expectedDuration.TotalMilliseconds * (SharedContent.GetAllowableDeviationInPercent() / 100.0))))
                                {
                                    this.stepStatus = ExerciseStepStatus.Complete;
                                    statusMessage = "Completed the hold of the pose for the appropriate length of time";
                                }
                                else
                                {
                                    //Held for too long
                                    this.stepStatus = ExerciseStepStatus.Failed;
                                    statusMessage = "User held the pose for too long";
                                }
                            }
                        }
                    }
                    else if (userInFinalPose)
                    {
                        this.stepStatus = ExerciseStepStatus.Complete;
                        statusMessage = "User completed the exercise step";
                    }
                    else
                    {
                        UserPerformanceAnalysisInfo performanceInfo = this.IsUserPerformingStepCorrectly(userData);
                        if (performanceInfo.failed)
                        {
                            this.stepStatus = ExerciseStepStatus.Failed;
                            statusMessage = performanceInfo.failureMessage;
                        }
                        else
                        {
                            statusMessage += String.Format("User is {0} % complete the exercise step", performanceInfo.percentComplete);
                        }
                    }
                    break;
            }

            return new ExerciseStepStatusInfo(this.stepStatus, statusMessage);
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
        public UserPerformanceAnalysisInfo IsUserPerformingStepCorrectly(SkeletonData userData)
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

    public struct UserPerformanceAnalysisInfo
    {
        public UserPerformanceAnalysisInfo(bool failed, string failureMsg) { this.failed = failed; this.failureMessage = failureMsg; this.negligableAction = false; this.percentComplete = -1; }
        public UserPerformanceAnalysisInfo(int percentComplete) { this.failed = false; this.failureMessage = ""; this.negligableAction = false; this.percentComplete = percentComplete; }
        public UserPerformanceAnalysisInfo(bool negligableAction) { this.failed = false; this.failureMessage = ""; this.negligableAction = negligableAction; this.percentComplete = -1; }
        public bool failed;
        public bool negligableAction;
        public string failureMessage;
        public int percentComplete;
    }

    public struct ExerciseStepStatusInfo
    {
        public ExerciseStepStatusInfo(ExerciseStepStatus exStepStatus, string statusMsg) { this.exerciseStepStatus = exStepStatus; this.statusMessage = statusMsg; }
        public ExerciseStepStatus exerciseStepStatus;
        public string statusMessage;
    }

    public enum ExerciseStepType
    {
        Hold = 0,
        Movement = 1
    }
}
