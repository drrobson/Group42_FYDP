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
    /// Interaction logic for LabelAndImage.xaml
    /// </summary>
    public partial class LabelAndImage : UserControl
    {
        public ImageSource ImageSource
        {
            get { return theImage.Source; }
            set { theImage.Source = value; }
        }

        public string LabelContent
        {
            get { return (string)theLabel.Content; }
            set { theLabel.Content = value; }
        }

        public HorizontalAlignment ControlHorizontalAlignment
        {
            get { return thePanel.HorizontalAlignment; }
            set { thePanel.HorizontalAlignment = value; }
        }

        public double PanelHeight
        {
            get { return thePanel.Height; }
            set { thePanel.Height = value; }
        }

        public double PanelWidth
        {
            get { return theLabel.Width; }
            set { theLabel.Width = value; }
        }

        public double LabelFont
        {
            get { return theLabel.FontSize; }
            set { theLabel.FontSize = value; }
        }

        public LabelAndImage()
        {
            InitializeComponent();
        }

        public LabelAndImage(ImageSource ImageSource, string LabelContent,
            HorizontalAlignment ControlHorizontalAlignment = HorizontalAlignment.Left,
            double PanelHeight = 0, double PanelWidth = 0, double LabelFont = 0)
        {
            InitializeComponent();

            this.ImageSource = ImageSource;
            this.LabelContent = LabelContent;
            this.ControlHorizontalAlignment = ControlHorizontalAlignment;
            
            if (PanelHeight == 0)
            {
                this.PanelHeight = (double)Application.Current.Resources["SmallButtonHeight"];
            }
            else
            {
                this.PanelHeight = PanelHeight;
            }
            if (PanelWidth == 0)
            {
                this.PanelWidth = (double)Application.Current.Resources["SmallButtonWidth"];
            }
            else
            {
                this.PanelWidth = PanelWidth;
            }
            if (LabelFont == 0)
            {
                this.LabelFont = (double)Application.Current.Resources["SmallButtonFont"];
            }
            else
            {
                this.LabelFont = LabelFont;
            }
        }
    }
}
