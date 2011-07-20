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

            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Play, selectedPlay);
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Pause, selectedPause);
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Stop, selectedStop);
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Continue, selectedContinue);
        }

        private void Element_MediaOpened(object source, EventArgs e)
        {
            // no use for this (yet ?)
        }

        private void Element_MediaEnded(object source, EventArgs e)
        {
            exercisePreview.Stop();
        }

        private void selectedPlay(object sender, RoutedEventArgs e)
        {
            selectedPlay();
        }

        private void selectedPlay()
        {
            exercisePreview.Play();
        }

        private void selectedPause(object sender, RoutedEventArgs e)
        {
            selectedPause();
        }

        private void selectedPause()
        {
            exercisePreview.Pause();
        }

        private void selectedStop(object sender, RoutedEventArgs e)
        {
            selectedStop();
        }

        private void selectedStop()
        {
            exercisePreview.Stop();
        }

        private void selectedContinue(object sender, RoutedEventArgs e)
        {
            selectedContinue();
        }

        private void selectedContinue()
        {
            ScreenManager.SetScreen(new ExerciseView(null));
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Play);
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Pause);
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Stop);
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Continue);
        }
    }
}
