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

        public ExercisePreview()
        {
            InitializeComponent();
        }

        private void Element_MediaOpened(object source, EventArgs e)
        {

        }

        private void Element_MediaEnded(object source, EventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
