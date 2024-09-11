using System;
using System.Windows;
using System.Windows.Controls;

namespace MVA_poe.Controls
{
    /// <summary>
    /// Interaction logic for MyTextBox.xaml
    /// </summary>
    public partial class MyTextBox : UserControl
    {
        public MyTextBox()
        {
            InitializeComponent();
        }

        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static readonly DependencyProperty HintProperty =
        DependencyProperty.Register("Hint", typeof(string), typeof(MyTextBox));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty =
        DependencyProperty.Register("Caption", typeof(string), typeof(MyTextBox));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = textBox.Text;
        }

        public static readonly DependencyProperty TextProperty =
                   DependencyProperty.Register("Text", typeof(string), typeof(MyTextBox), new PropertyMetadata(string.Empty));

      
    }
}
