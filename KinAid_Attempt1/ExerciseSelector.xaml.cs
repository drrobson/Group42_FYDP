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
            ScreenManager.setScreen(new ExercisePreview());
        }

        private void selectedCalibrate(object sender, RoutedEventArgs e)
        {

        }

        private void selectedAddExercise(object sender, RoutedEventArgs e)
        {

        }

    }
}
