using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.IO;

using Microsoft.Research.Kinect.Nui;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;

namespace KinAid_Attempt1
{
    public delegate void SpeechCommandReceived();

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
        public const string RecognizerId = "SR_MS_en-US_Kinect_10.0";
        public static RecognizerInfo Ri = SpeechRecognitionEngine.InstalledRecognizers().Where(r => r.Id == RecognizerId).FirstOrDefault();

        public enum LimbID
        {
            RightArm = 0,
            LeftArm = 1,
            RightLeg = 2,
            LeftLeg = 3
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
            LimbOrientationOld[] limb1 = { new LimbOrientationOld(JointID.ShoulderLeft, JointID.ElbowLeft, 90, 180, 90) };
            PoseConstraint pc = new PoseConstraint(limb1);
            LimbOrientationOld limb2 = new LimbOrientationOld(JointID.ShoulderLeft, JointID.ElbowLeft, 180, 90, 90);
            GlobalConstraint[] gcs = { new GlobalConstraint(JointID.ShoulderLeft, JointID.ElbowLeft, JointID.WristLeft, 90, 15) };
            VariableConstraint[] vcs = { new VariableConstraint("TEST", new TimeSpan(0, 0, 10), limb1[0], limb2) };
            Exercise ex1 = new Exercise(null, pc, gcs, vcs);

            return new Exercise[] { ex1 };
        }

        private static SpeechCommandReceived[] CommandDelegates;
        private static string[] Commands;
        private static KinectAudioSource AudioSource;
        private static SpeechRecognitionEngine Sre;

        public static void RegisterSpeechCommands(string[] commands, SpeechCommandReceived[] commandDelegates)
        {
            CommandDelegates = commandDelegates;
            Commands = commands;

            Choices choices = new Choices();
            foreach (string command in commands)
            {
                choices.Add(command);
            }

            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = SharedContent.Ri.Culture;
            gb.Append(choices);

            Grammar g = new Grammar(gb);

            Sre = new SpeechRecognitionEngine(SharedContent.Ri.Id);
            Sre.LoadGrammar(g);
            Sre.SpeechRecognized += WireSpeechToCommand;

            Thread speechRecoThread = new Thread(StartSpeechRecognitionThread);
            speechRecoThread.Start();
        }

        private static void StartSpeechRecognitionThread()
        {
            AudioSource = new KinectAudioSource();
            AudioSource.FeatureMode = true;
            AudioSource.AutomaticGainControl = false;
            AudioSource.SystemMode = SystemMode.OptibeamArrayOnly;
            AudioSource.MicArrayMode = MicArrayMode.MicArrayAdaptiveBeam;
            Stream s = AudioSource.Start();
            Sre.SetInputToAudioStream(s, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            Sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        public static void StopListeningCommands()
        {
            Sre.RecognizeAsyncCancel();
            Sre.RecognizeAsyncStop();
        }

        public static void WireSpeechToCommand(object source, SpeechRecognizedEventArgs e)
        {
            for (int i = 0; i < Commands.Length; i++)
            {
                if (Commands[i] == e.Result.Text)
                {
                    CommandDelegates[i]();
                }
            }
        }
    }
}
