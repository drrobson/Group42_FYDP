﻿using System;
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

        public ExerciseFeedback()
        {
            InitializeComponent();
        }

        private void selectedRetry(object sender, RoutedEventArgs e)
        {
            ScreenManager.setScreen(new ExercisePreview());
        }

        private void selectedBack(object sender, RoutedEventArgs e)
        {
            ScreenManager.setScreen(new ExerciseSelector());
        }
    }
}
