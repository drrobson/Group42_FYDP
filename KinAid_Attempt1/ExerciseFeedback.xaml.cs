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
                        source = new Uri("Images/CheckboxPass.bmp", UriKind.Relative);
                        break;
                    case ExerciseStepStatus.Failed:
                        source = new Uri("Images/CheckboxFail.bmp", UriKind.Relative);                        
                        break;
                    default:
                        source = new Uri("Images/Checkbox.bmp", UriKind.Relative); 
                        break;
                }
                LabelAndImage lai = new LabelAndImage(new BitmapImage(source), step.stepName);
                lai.Width = 300;
                stepPanel.Children.Add(lai);
            }

            switch (ex.exerciseStatus)
            {
                case ExerciseStatus.Complete:
                    statusLabel.Content = "COMPLETE!";
                    break;
                case ExerciseStatus.Failed:
                    statusLabel.Content = "FAIL";
                    break;
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
