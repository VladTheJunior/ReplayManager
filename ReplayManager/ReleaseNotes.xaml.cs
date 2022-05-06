using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReplayManager
{
    /// <summary>
    /// Логика взаимодействия для ReleaseNotes.xaml
    /// </summary>
    public partial class ReleaseNotes : Window
    {
        public ReleaseNotes()
        {
            InitializeComponent();
            var myCur = Application.GetResourceStream(new Uri("pack://application:,,,/resources/Cursor.cur")).Stream;
            Cursor = new Cursor(myCur);
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
