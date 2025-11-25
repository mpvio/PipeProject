using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PipeProject.Views
{
    public partial class PipeView : UserControl
    {
        public PipeView()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // regex to allow numbers and decimal points only
            Regex regex = new Regex(@"^[0-9]*[.]?[0-9]*$");

            // check if new text is valid
            TextBox textBox = sender as TextBox;
            string proposedText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            e.Handled = !regex.IsMatch(proposedText);
        }
    }
}