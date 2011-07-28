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
    /// Interaction logic for ExerciseSelector.xaml
    /// </summary>
    public partial class ExerciseSelector : UserControl, IScreen//, IKinectFrames (when I add the Kinect control feature)
    {
        public UIElement element
        {
            get
            {
                return this;
            }
        }

        public ExerciseSelector()
        {
            InitializeComponent();

            Exercise[] exercises = ExerciseFactory.GetExercises();
            for (int i = 0; i < exercises.Length; i++)
            {
#if AUDIOUI
                SharedContent.Sr.registerSpeechCommand(exercises[i].name, selectedExercise);
                Uri source = new Uri("Images/Voice.bmp", UriKind.Relative);
                LabelAndImage labelImage = new LabelAndImage(new BitmapImage(source),
                    SharedContent.GetCommandString(exercises[i].name), System.Windows.HorizontalAlignment.Left, 
                    (double)Application.Current.Resources["BigButtonHeight"], (double)Application.Current.Resources["BigButtonWidth"],
                    (double)Application.Current.Resources["BigButtonFont"]);
                buttonPanel.Children.Add(labelImage);
#elif BUTTONUI
                Button button = new Button();
                button.Content = SharedContent.GetCommandString(exercises[i].name);
                button.Height = (double)Application.Current.Resources["BigButtonHeight"];
                button.Width = (double)Application.Current.Resources["BigButtonWidth"];
                button.FontSize = (double)Application.Current.Resources["BigButtonFont"];
                button.Click += selectedExercise;
                button.Tag = i;
                buttonPanel.Children.Add(button);
#endif
            }
        }

#if AUDIOUI
        private void selectedExercise(string exercise)
        {
            int exerciseIndex = 0;
            Exercise[] exercises = ExerciseFactory.GetExercises();
            for (int i = 0; i < exercises.Length; i++)
            {
                if (SharedContent.GetCommandString(exercises[i].name).Equals(exercise))
                {
                    exerciseIndex = i;
                    break;
                }
            }

            AudioMessageBox amb = new AudioMessageBox("Do you wish to calibrate the exercise?", MessageBoxButton.OK);
            amb.Owner = (Window)ScreenManager.GetHost();
            bool? dialogResult = amb.ShowDialog();

            if (dialogResult.HasValue && dialogResult == true)
            {
                SharedContent.IsCalibrated = true;
                ScreenManager.SetScreen(new CalibratingView(ExerciseFactory.GetExercises()[exerciseIndex]));
            }
            else if (dialogResult.HasValue && dialogResult == false)
            {
                SharedContent.IsCalibrated = false;
                ScreenManager.SetScreen(new ExercisePreview(ExerciseFactory.GetExercises()[exerciseIndex]));
            }
        }
#elif BUTTONUI
        private void selectedExercise(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            MessageBoxResult doCalibration = 
                MessageBox.Show("Do you wish to calibrate the exercise?", "KinAid", MessageBoxButton.YesNo);
            bool? dialogResult = (doCalibration == MessageBoxResult.Yes ? true : false);

            if (dialogResult.HasValue && dialogResult == true)
            {
                SharedContent.IsCalibrated = true;
                ScreenManager.SetScreen(new CalibratingView(ExerciseFactory.GetExercises()[(int)button.Tag]));
            }
            else if (dialogResult.HasValue && dialogResult == false)
            {
                SharedContent.IsCalibrated = false;
                ScreenManager.SetScreen(new ExercisePreview(ExerciseFactory.GetExercises()[(int)button.Tag]));
            }
        }
#endif

        private void selectedAddExercise(object sender, RoutedEventArgs e)
        {
#if AUDIOUI
            AudioMessageBox amb = new AudioMessageBox("This feature has not yet been implemented", MessageBoxButton.OK);
            amb.Owner = (Window)ScreenManager.GetHost();
            amb.ShowDialog();
#elif BUTTONUI
            MessageBoxResult notImplemented =
                MessageBox.Show("This feature has not yet been implemented", "KinAid", MessageBoxButton.OK);
#endif
        }

    }
}
