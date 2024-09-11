using System.Windows;
using System.Windows.Controls;

namespace MVA_poe.Controls
{
    public partial class MyPasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(MyPasswordBox), new PropertyMetadata(string.Empty));

        public string PHint
        {
            get { return (string)GetValue(PHintProperty); }
            set { SetValue(PHintProperty, value); }
        }

        public static readonly DependencyProperty PHintProperty =
        DependencyProperty.Register("PHint", typeof(string), typeof(MyPasswordBox));

        public string PCaption
        {
            get { return (string)GetValue(PCaptionProperty); }
            set { SetValue(PCaptionProperty, value); }
        }

        public static readonly DependencyProperty PCaptionProperty =
        DependencyProperty.Register("PCaption", typeof(string), typeof(MyPasswordBox));


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public MyPasswordBox()
        {
            InitializeComponent();
            pBox.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = pBox.Password;
        }
    }
}
