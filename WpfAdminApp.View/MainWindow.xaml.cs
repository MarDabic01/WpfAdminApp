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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAdminApp.ViewModel;

namespace WpfAdminApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(Mediator.Instance);
            this.DataContext = mainWindowViewModel;
        }

        NewEditWindowViewModel newEditWindowViewModel;
        NewEditWindow newEditWindow;

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            newEditWindow = new NewEditWindow();
            newEditWindowViewModel = new NewEditWindowViewModel(Mediator.Instance);
            newEditWindow.DataContext = newEditWindowViewModel;
            newEditWindow.ShowDialog();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;

            newEditWindow = new NewEditWindow();
            newEditWindowViewModel = new NewEditWindowViewModel(viewModel.CurrentPerson.Clone(), Mediator.Instance);
            newEditWindowViewModel.Done += NewEditWindowViewModel_Done;

            newEditWindow.DataContext = newEditWindowViewModel;
            newEditWindow.ShowDialog();
        }

        private void NewEditWindowViewModel_Done(object sender, NewEditWindowViewModel.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);

            if (e.Success)
            {
                newEditWindow.Close();
            }
        }
    }
}
