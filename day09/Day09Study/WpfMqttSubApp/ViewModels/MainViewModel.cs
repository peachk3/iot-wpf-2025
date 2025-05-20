using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;

namespace WpfMqttSubApp.ViewModels
{

    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogCoordinator dialogCoordinator;
        private string _brokerHost;
        private string _databaseHost;

        // 속성 BrokerHost, DatabaseHost
        // 메서드 ConnectBrokerCommand, ConnectDatabaseCommand
        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.dialogCoordinator = dialogCoordinator;

            BrokerHost = "210.119.21.60";
            DatabaseHost = "210.119.21.60";

        }

        public string BrokerHost
        {
            get => _brokerHost;
            set => SetProperty(ref _brokerHost, value);
        }
        public string DatabaseHost
        {
            get => _databaseHost;
            set => SetProperty(ref _databaseHost, value);
        }

        [RelayCommand]
        public async Task ConnectBroker()
        {
            await this.dialogCoordinator.ShowMessageAsync(this, "브로커 연결", "브로커 연결합니다!");
        }
        
        [RelayCommand]
        public async Task ConnectDatabase()
        {
            await this.dialogCoordinator.ShowMessageAsync(this, "DB 연결", "DB 연결합니다!");

        }
    }
}
