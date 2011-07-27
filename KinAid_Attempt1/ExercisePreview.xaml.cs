using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinAid_Attempt1
{
    /// <summary>
    /// Interaction logic for ExercisePreview.xaml
    /// </summary>
    public partial class ExercisePreview : UserControl, IScreen
    {
        public UIElement element
        {
            get
            {
                return this;
            }
        }

        Exercise ex;

        public ExercisePreview(Exercise ex)
        {
            InitializeComponent();

            this.ex = ex;

#if AUDIOUI
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Play, selectedResponse);
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Pause, selectedResponse);
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Stop, selectedResponse);
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Continue, selectedResponse);
#endif
        }

        private void Element_MediaOpened(object source, EventArgs e)
        {
            // no use for this (yet ?)
        }

        private void Element_MediaEnded(object source, EventArgs e)
        {
            exercisePreview.Stop();
        }

#if AUDIOUI
        private void selectedResponse(string response)
        {
            if (response.Equals(SharedContent.GetCommandString(SharedContent.Commands.Play)))
            {
                exercisePreview.Play();
            }
            else if (response.Equals(SharedContent.GetCommandString(SharedContent.Commands.Pause)))
            {
                exercisePreview.Pause();
            }
            else if (response.Equals(SharedContent.GetCommandString(SharedContent.Commands.Stop)))
            {
                exercisePreview.Stop();
            }
            else if (response.Equals(SharedContent.GetCommandString(SharedContent.Commands.Continue)))
            {
                SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Play);
                SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Pause);
                SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Stop);
                SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Continue);

                ScreenManager.SetScreen(new ExerciseView(ex));
            }
        }
#elif BUTTONUI
        private void selectedPlay(object sender, RoutedEventArgs e)
        {
            exercisePreview.Play();
        }

        private void selectedPause(object sender, RoutedEventArgs e)
        {
            exercisePreview.Pause();
        }

        private void selectedStop(object sender, RoutedEventArgs e)
        {
            exercisePreview.Stop();
        }

        private void selectedContinue(object sender, RoutedEventArgs e)
        {
            ScreenManager.SetScreen(new ExerciseView(ex));
        }
#endif
    }
}
