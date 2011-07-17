using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public static class SharedContent
    {
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

        public static double AllowableDeviation = 15;

        public static Runtime Nui;

        public enum LimbID
        {
            LeftHand,
            LeftForearm,
            LeftArm,
            RightHand,
            RightForearm,
            RightArm,
            Neck,
            MainBody,
            LeftThigh,
            LeftCalf,
            LeftFoot,
            RightThigh,
            RightCalf,
            RightFoot,
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

        public static Exercise[] GetExercises()
        {
            LimbOrientation[] limb1 = { new LimbOrientation(JointID.ShoulderLeft, JointID.ElbowLeft, 90, 180, 90) };
            PoseConstraint pc = new PoseConstraint(limb1);
            LimbOrientation limb2 = new LimbOrientation(JointID.ShoulderLeft, JointID.ElbowLeft, 180, 90, 90);
            GlobalConstraint[] gcs = { new GlobalConstraint(JointID.ShoulderLeft, JointID.ElbowLeft, JointID.WristLeft, 90, 15) };
            VariableConstraint[] vcs = { new VariableConstraint("TEST", new TimeSpan(0, 0, 10), limb2) };
            Exercise ex1 = new Exercise(null, pc, gcs, vcs);

            return new Exercise[] { ex1 };
        }
    }
}
