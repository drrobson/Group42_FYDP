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
    /// Interaction logic for ExerciseFeedback.xaml
    /// </summary>
    public partial class ExerciseFeedback : UserControl, IScreen
    {
        public UIElement element
        {
            get
            {
                return this;
            }
        }

        Exercise ex;

        public ExerciseFeedback(Exercise ex)
        {
            InitializeComponent();

            this.ex = ex;
            ExerciseStep[] steps = ex.exerciseSteps;
            foreach (ExerciseStep step in steps)
            {
                Uri source;
                switch (step.stepStatus)
                {
                    case ExerciseStepStatus.Complete:
                        source = new Uri(@"/KinAid_Attempt1;CheckboxPass.bmp", UriKind.Relative);
                        break;
                    case ExerciseStepStatus.Failed:
                        source = new Uri(@"/KinAid_Attempt1;CheckboxFail.bmp", UriKind.Relative);
                        break;
                    default:
                        source = new Uri(@"/KinAid_Attempt1;Checkbox.bmp", UriKind.Relative);
                        break;
                }
                Image statusImage = new Image();
                statusImage.Source = new BitmapImage(source);
                Label statusLabel = new Label();
                statusLabel.Width = 300;
                statusLabel.Content = step.stepName;
                stepPanel.Children.Add(statusImage);
                stepPanel.Children.Add(statusLabel);
            }
        }

        private void selectedRetry(object sender, RoutedEventArgs e)
        {
            ex.Reset();
            ScreenManager.SetScreen(new ExerciseView(ex));
        }

        private void selectedBack(object sender, RoutedEventArgs e)
        {
            ScreenManager.SetScreen(new ExerciseSelector());
        }

        // recalibrate action

        // see informationn action
    }
}
