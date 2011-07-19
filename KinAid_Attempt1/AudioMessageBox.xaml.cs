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

            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.Yes, selectedYes);
            SharedContent.Sr.registerSpeechCommand(SharedContent.Commands.No, selectedNo);
        }

        public void selectedYes()
        {
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Yes);
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.No);

            DialogResult = true;
            this.Close();
        }

        public void selectedNo()
        {
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.Yes);
            SharedContent.Sr.unregisterSpeechCommand(SharedContent.Commands.No);

            DialogResult = false;
            this.Close();
        }
    }
}
