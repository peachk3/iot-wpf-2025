using System.Windows;
using System.Windows.Input;
using WpfSmartHomeApp.ViewModels;
using WpfSmartHomeApp.ViewModels;

namespace WpfSmartHomeApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 원래 없는 속성을 사용자가 추가하는 방법
        // 의존 속성(DependencyProperty)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 제목표시줄 X 버튼 누를때, Alt + F4 누를때 발생하는 이벤트
            e.Cancel = true; // 앱 종료를 막는 기능
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.LoadedCommand.Execute(null); // LoadedCommand -> ViewModel의 Onload
            }
        }
    }
}