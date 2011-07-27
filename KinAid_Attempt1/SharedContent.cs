using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public static class SharedContent
    {
        /// <summary>
        /// Front-end shared content
        /// </summary>

        public const int CalibrationSeconds = 5;

        public static Dictionary<JointID, Brush> JointColors = new Dictionary<JointID, Brush>() { 
            {JointID.HipCenter, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {JointID.Spine, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {JointID.ShoulderCenter, new SolidColorBrush(Color.FromRgb(168, 230, 29))},
            {JointID.Head, new SolidColorBrush(Color.FromRgb(200, 0,   0))},
            {JointID.ShoulderLeft, new SolidColorBrush(Color.FromRgb(79,  84,  33))},
            {JointID.ElbowLeft, new SolidColorBrush(Color.FromRgb(84,  33,  42))},
            {JointID.WristLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {JointID.HandLeft, new SolidColorBrush(Color.FromRgb(215,  86, 0))},
            {JointID.ShoulderRight, new SolidColorBrush(Color.FromRgb(33,  79,  84))},
            {JointID.ElbowRight, new SolidColorBrush(Color.FromRgb(33,  33,  84))},
            {JointID.WristRight, new SolidColorBrush(Color.FromRgb(77,  109, 243))},
            {JointID.HandRight, new SolidColorBrush(Color.FromRgb(37,   69, 243))},
            {JointID.HipLeft, new SolidColorBrush(Color.FromRgb(77,  109, 243))},
            {JointID.KneeLeft, new SolidColorBrush(Color.FromRgb(69,  33,  84))},
            {JointID.AnkleLeft, new SolidColorBrush(Color.FromRgb(229, 170, 122))},
            {JointID.FootLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {JointID.HipRight, new SolidColorBrush(Color.FromRgb(181, 165, 213))},
            {JointID.KneeRight, new SolidColorBrush(Color.FromRgb(71, 222,  76))},
            {JointID.AnkleRight, new SolidColorBrush(Color.FromRgb(245, 228, 156))},
            {JointID.FootRight, new SolidColorBrush(Color.FromRgb(77,  109, 243))}
        };

        public static Runtime Nui;
        public static SpeechRecognizer Sr;

        public const string RecognizerId = "SR_MS_en-US_Kinect_10.0";
        public static string[] CommandStrings = new string[] {
            // General UI Commands
            "calibrate",
            "exercise",
            "play",
            "pause",
            "stop",
            "continue",
            "retry",
            "back",
            "yes",
            "no",
            // Exercise Commands
            "Right Shoulder Abduction",
            "Right Bicep Curl",
        };
        public enum Commands
        {
            // General UI Commands
            Calibrate,
            Exercise,
            Play,
            Pause,
            Stop,
            Continue,
            Retry,
            Back,
            Yes,
            No,
            // Exercise Commands
            RightShoulderAbduction,
            RightBicepCurl,
        }
        public static string GetCommandString(Commands command)
        {
            return CommandStrings[(int)command];
        }

        /// <summary>
        /// Back-end shared content
        /// </summary>

        public static bool IsCalibrated = false;

        private static double AllowableDeviationInDegrees = 8;
        private static double AllowableDeviationInDegreesNoCal = 30;
        public static double GetAllowableDeviationInDegrees()
        {
            return IsCalibrated ? AllowableDeviationInDegrees : AllowableDeviationInDegreesNoCal;
        }
        private static double AllowableDeviationInPercent = 15;
        private static double AllowableDeviationInPercentNoCal = 30;
        public static double GetAllowableDeviationInPercent()
        {
            return IsCalibrated ? AllowableDeviationInPercent : AllowableDeviationInPercentNoCal;
        }

        public static Pose NeutralPose = new Pose(new Dictionary<BodyPartID, BodyPartOrientation>()
        {
            {SharedContent.BodyPartID.Head, new HeadOrientation(new Vector3D(0,1,-0.2))},
            {SharedContent.BodyPartID.Torso, new TorsoOrientation(0, new Vector3D(0,1,-0.1))},
            {SharedContent.BodyPartID.RightArm, new LimbOrientation(BodyPartID.RightArm, new Vector3D(0,-1,0), new Vector3D(0,-1,0))},
            {SharedContent.BodyPartID.LeftArm, new LimbOrientation(BodyPartID.LeftArm, new Vector3D(0,-1,0), new Vector3D(0,-1,0))},
            {SharedContent.BodyPartID.RightLeg, new LimbOrientation(BodyPartID.RightLeg, new Vector3D(0,-1,0), new Vector3D(0,-1,0))},
            {SharedContent.BodyPartID.LeftLeg, new LimbOrientation(BodyPartID.LeftLeg, new Vector3D(0,-1,0), new Vector3D(0,-1,0))}
        });

        public enum BodyPartID
        {
            RightArm = 0,
            LeftArm = 1,
            RightLeg = 2,
            LeftLeg = 3,
            Torso = 4,
            Head = 5
        }

        public enum ExerciseType
        {
            MoveAndHold,
            MoveAndBack,
            MoveOnly,
            HoldOnly,
        }

        public enum Progression
        {
            NotStarted = -2,
            Failed = -1,
            Started = 0,
            Completed = 100,
        }
    }
}
