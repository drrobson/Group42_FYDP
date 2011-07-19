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

        public ExerciseSelector()
        {
            InitializeComponent();

            //SharedContent.Sr.registerSpeechCommand(

            Exercise[] exercises = ExerciseFactory.GetExercises();
            for (int i = 0; i < exercises.Length; i++)
            {
                Button button = new Button();
                button.Content = String.Format("Exercise {0}", i);
                button.Height = 150;
                button.Width = 280;
                button.Click += selectedExercise;
                button.Tag = i;
                buttonPanel.Children.Add(button);
            }
        }

        public UIElement element
        {
            get
            {
                return this;
            }
        }

        private void selectedExercise(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
#if BUTTONUI
            MessageBoxResult doCalibration = 
                MessageBox.Show("Do you wish to calibrate the exercise?", "KinAid", MessageBoxButton.YesNo);
            bool? dialogResult = (doCalibration == MessageBoxResult.Yes ? true : false);
#elif AUDIOUI
            AudioMessageBox amb = new AudioMessageBox("");
            amb.Owner = (Window) ScreenManager.GetHost();
            bool? dialogResult = amb.ShowDialog();
#endif
            if (dialogResult.HasValue && dialogResult == true)
            {
                ScreenManager.SetScreen(new CalibratingView(null));
            }
            else if (dialogResult.HasValue && dialogResult == false)
            {
                ScreenManager.SetScreen(new ExercisePreview());
            }
        }

        private void selectedAddExercise(object sender, RoutedEventArgs e)
        {
            /*
             * To Implement
             */
        }

    }
}
