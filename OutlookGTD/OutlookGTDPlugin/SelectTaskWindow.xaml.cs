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
using System.ComponentModel;

namespace OutlookGTDPlugin
{
    /// <summary>
    /// Interaction logic for SelectTaskWindow.xaml
    /// </summary>
    public partial class SelectTaskWindow : Window
    {
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;

        public SelectTaskWindow()
        {
            InitializeComponent();
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (DataContext as SelectTaskViewModel).SearchFilter = _textBox.Text;
            var dv = CollectionViewSource.GetDefaultView((DataContext as SelectTaskViewModel).TaskDisplayItems);
            dv.MoveCurrentTo((DataContext as SelectTaskViewModel).FirstVisible());
        }

        private void _clearButton_Click(object sender, RoutedEventArgs e)
        {
            _textBox.Text = string.Empty;
        }

        private void _sortClicked(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = sender as GridViewColumnHeader;
            String field = column.Tag as String;

            if (_CurSortCol != null)
            {
              AdornerLayer.GetAdornerLayer(_CurSortCol).Remove(_CurAdorner);
              _taskListView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (_CurSortCol == column && _CurAdorner.Direction == newDir)
              newDir = ListSortDirection.Descending;

            _CurSortCol = column;
            _CurAdorner = new SortAdorner(_CurSortCol, newDir);
            AdornerLayer.GetAdornerLayer(_CurSortCol).Add(_CurAdorner);
            _taskListView.Items.SortDescriptions.Add(new SortDescription(field, newDir));
        }

        public class SortAdorner : Adorner
        {
            private readonly static Geometry _AscGeometry =
                Geometry.Parse("M 0,0 L 10,0 L 5,5 Z");
            private readonly static Geometry _DescGeometry =
                Geometry.Parse("M 0,5 L 10,5 L 5,0 Z");

            public ListSortDirection Direction { get; private set; }

            public SortAdorner(UIElement element, ListSortDirection dir)
                : base(element)
            { Direction = dir; }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (AdornedElement.RenderSize.Width < 20)
                    return;

                drawingContext.PushTransform(
                    new TranslateTransform(
                      AdornedElement.RenderSize.Width - 15,
                      (AdornedElement.RenderSize.Height - 5) / 2));

                drawingContext.DrawGeometry(Brushes.Black, null,
                    Direction == ListSortDirection.Ascending ?
                      _AscGeometry : _DescGeometry);

                drawingContext.Pop();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _textBox.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                (DataContext as SelectTaskViewModel).SelectNext();
            }
            else if (e.Key == Key.Up)
            {
                (DataContext as SelectTaskViewModel).SelectPrev();
            }
            else if (e.Key == Key.Escape)
            {
                _textBox.Text = string.Empty;
            }
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
