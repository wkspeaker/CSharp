using System.ComponentModel;
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
using LangrisserTools.TimeCalculatorForGuild.ViewModels;

namespace LangrisserTools.TimeCalculatorForGuild
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            
            // 订阅窗口关闭事件，实现退出时自动保存
            this.Closing += MainWindow_Closing;
        }

        private void GenerateMsgButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.GenerateMsg();
            }
        }

        private void ToggleGuildTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.ToggleGuildType();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // 在窗口关闭时自动保存数据
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.SaveData();
            }
        }
    }
}