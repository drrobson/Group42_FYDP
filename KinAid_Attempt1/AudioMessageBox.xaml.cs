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
using System.Windows.Shapes;

namespace KinAid_Attempt1
{
    /// <summary>
    /// Interaction logic for AudioMessageBox.xaml
    /// </summary>
    public partial class AudioMessageBox : Window
    {
        MessageBoxButton type;

        public AudioMessageBox(string message, MessageBoxButton type)
        {
            InitializeComponent();

            this.message.Content = message;

            this.type = type;
            switch (type)
            {
                case MessageBoxButton.OK:
                    SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Okay, selectedResponse);
                    break;
                case MessageBoxButton.YesNo:
                    SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Yes, selectedResponse);
                    SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.No, selectedResponse);
                    break;
            }
        }

        public void selectedResponse(string response)
        {
            switch (type)
            {
                case MessageBoxButton.OK:
                    SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Okay);
                    break;
                case MessageBoxButton.YesNo:
                    SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Yes);
                    SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.No);
                    break;
            }

            if (response.Equals(SharedContent.GetCommandString(SharedContent.Commands.Yes)))
            {
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (!DialogResult.HasValue)
            {
                DialogResult = false;
            }

            switch (type)
            {
                case MessageBoxButton.OK:
                    SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Okay);
                    break;
                case MessageBoxButton.YesNo:
                    SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Yes);
                    SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.No);
                    break;
            }
        }
    }
}
