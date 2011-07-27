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
        public AudioMessageBox(string message)
        {
            InitializeComponent();

            this.message.Content = message;

            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Yes, selectedResponse);
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.No, selectedResponse);
        }

        public void selectedResponse(string response)
        {
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Yes);
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.No);

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

            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Yes);
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.No);
        }
    }
}
