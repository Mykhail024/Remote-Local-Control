using Avalonia.Controls;
using System.IO;
using System.Text;

namespace RLCClient
{
    public class TextBoxOutputter : TextWriter
    {
        TextBox textBox = null;

        public TextBoxOutputter(TextBox output)
        {
            textBox = output;
        }
        public TextBoxOutputter() { }
        public void setTextBox(TextBox output)
        {
            textBox = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            textBox.Text = textBox.Text + value;
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
