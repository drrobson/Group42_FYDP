using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;

namespace KinAid_Attempt1
{
    public class SpeechRecognizer
    {
        public delegate void SpeechCommandReceived();

        SpeechCommandReceived[] commandDelegates;
        
        KinectAudioSource audioSource;
        SpeechRecognitionEngine sre;
        RecognizerInfo ri;

        public SpeechRecognizer()
        {
            this.commandDelegates = new SpeechCommandReceived[SharedContent.CommandStrings.Length];
            for (int i = 0; i < commandDelegates.Length; i++)
            {
                commandDelegates[i] = null;
            }

            ri = SpeechRecognitionEngine.InstalledRecognizers().Where(r => r.Id == SharedContent.RecognizerId).FirstOrDefault();
            if (ri == null)
            {
                MessageBox.Show(
                    "Could not find speech recognizer: {0}. Please refer to the application requirements.", SharedContent.RecognizerId);
                Environment.Exit(-1);
            }
        }

        public void initialize()
        {
            Choices choices = new Choices();
            foreach (string command in SharedContent.CommandStrings)
            {
                choices.Add(command);
            }

            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ri.Culture;
            gb.Append(choices);

            Grammar g = new Grammar(gb);

            sre = new SpeechRecognitionEngine(ri.Id);
            sre.LoadGrammar(g);
            sre.SpeechRecognized += wireSpeechToCommand;

            Thread speechRecoThread = new Thread(startSpeechRecognitionThread);
            speechRecoThread.Start();
        }

        private void startSpeechRecognitionThread()
        {
            audioSource = new KinectAudioSource();
            audioSource.FeatureMode = true;
            audioSource.AutomaticGainControl = false;
            audioSource.SystemMode = SystemMode.OptibeamArrayOnly;
            audioSource.MicArrayMode = MicArrayMode.MicArrayAdaptiveBeam;
            Stream s = audioSource.Start();
            sre.SetInputToAudioStream(s, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void stopListeningCommands()
        {
            sre.RecognizeAsyncCancel();
            sre.RecognizeAsyncStop();
            audioSource.Dispose();
        }

        public void registerSpeechCommand(SharedContent.Commands searchedCommand, SpeechCommandReceived commandDelegate)
        {
            string searchedCommandString = SharedContent.GetCommandString(searchedCommand);
            for (int i = 0; i < commandDelegates.Length; i++)
            {
                if (SharedContent.CommandStrings[i] == searchedCommandString)
                {
                    commandDelegates[i] = commandDelegate;
                }
            }
        }

        public void unregisterSpeechCommand(SharedContent.Commands searchedCommand)
        {
            string searchedCommandString = SharedContent.GetCommandString(searchedCommand);
            for (int i = 0; i < commandDelegates.Length; i++)
            {
                if (SharedContent.CommandStrings[i] == searchedCommandString)
                {
                    commandDelegates[i] = null;
                }
            }
        }

        public void wireSpeechToCommand(object source, SpeechRecognizedEventArgs e)
        {
            for (int i = 0; i < SharedContent.CommandStrings.Length; i++)
            {
                if (SharedContent.CommandStrings[i] == e.Result.Text && commandDelegates[i] != null)
                {
                    commandDelegates[i]();
                }
            }
        }
    }
}
