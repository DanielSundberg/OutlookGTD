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
using System.Globalization;

namespace OutlookGTDPlugin
{
    /// <summary>
    /// Interaction logic for SelectTaskWindow.xaml
    /// </summary>
    public partial class SelectTaskWindow : Window
    {
        public SelectTaskWindow()
        {
            InitializeComponent();
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (DataContext as SelectTaskViewModel).SearchFilter = _textBox.Text;
            //treeView.InvalidateVisual();
        }

    }
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

}
