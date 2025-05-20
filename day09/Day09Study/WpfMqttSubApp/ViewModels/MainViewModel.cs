using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using MQTTnet;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Threading;
using WpfMqttSubApp.Models;

namespace WpfMqttSubApp.ViewModels
{

    public partial class MainViewModel : ObservableObject, IDisposable
    {
        private IMqttClient mqttClient;
        private readonly IDialogCoordinator dialogCoordinator;
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private int lineCounter = 1; // TODO : 텍스트가 너무 많아져서 느려지면 초기화시 사용

        private string connString = string.Empty;
        private MySqlConnection connection;

        private string _brokerHost;
        private string _databaseHost;
        private string _logText;

        // 속성 BrokerHost, DatabaseHost
        // 메서드 ConnectBrokerCommand, ConnectDatabaseCommand
        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            this.dialogCoordinator = dialogCoordinator;

            BrokerHost = "210.119.12.60";
            DatabaseHost = "210.119.12.60";

            connection = new MySqlConnection(); // 예외처리용

            // RichTextBox 테스트용
            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += (sender, e) =>
            //{
            //    // RichTextBox 추가 내용
            //    LogText += $"Log [{DateTime.Now:HH:mm:ss}] - {counter++}\n";
            //    Debug.WriteLine($"Log [{DateTime.Now:HH:mm:ss}] - {counter++}");
            //};
            //timer.Start();
        }

        public string LogText
        {
            get => _logText;
            set => SetProperty(ref _logText, value);
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
       

        private async Task ConnectMqttBroker()
        {
            // MQTT 클라이언트 생성
            var mqttFactory = new MqttClientFactory();
            mqttClient = mqttFactory.CreateMqttClient();

            // MQTT 클라이언트 접속 설정
            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(BrokerHost)
                .WithCleanSession(true)
                .Build();
            
            // MQTT 접속 후 이벤트 처리
            mqttClient.ConnectedAsync += async e =>
            {
                LogText += "MQTT 브로커 접속 성공!\n";
                // 연결 이후 구독(Subscribe)
                await mqttClient.SubscribeAsync("smarthome/60/topic");
            };
            // MQTT 구독 메시지 로그 출력
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = e.ApplicationMessage.ConvertPayloadToString(); // byte 데이터를 UTF-8 문자열로 변환
                
                // json으로 변경
                var data = JsonConvert.DeserializeObject<FakeInfo>(payload);
                Debug.WriteLine($"{data.Count} / {data.Sensing_Dt} / {data.Light} / {data.Humid} / {data.Human}");

                SaveSensingData(data);

                LogText += $"LINENUMBER : {lineCounter++}\n";
                LogText += $"{payload}\n";

                return Task.CompletedTask;
            };

            await mqttClient.ConnectAsync(mqttClientOptions); // MQTT 서버에 접속 
        }
        private async Task SaveSensingData(FakeInfo data)
        {
            string query = @"INSERT INTO fakedatas(sensing_dt, pub_id, count, temp, humid, light, human) 
                             VALUES(@sensing_dt, @pub_id, @count, @temp, @humid, @light, @human)";


            try
            {
                if (connection.State == System.Data.ConnectionState.Open) // db가 열려 있을 경우 진행
                {
                    using var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@sensing_dt", data.Sensing_Dt);
                    cmd.Parameters.AddWithValue("@pub_id", data.Pub_Id);
                    cmd.Parameters.AddWithValue("@count", data.Count);
                    cmd.Parameters.AddWithValue("@temp", data.Temp);
                    cmd.Parameters.AddWithValue("@humid", data.Humid);
                    cmd.Parameters.AddWithValue("@light", data.Light);
                    cmd.Parameters.AddWithValue("@human", data.Human);

                    await cmd.ExecuteNonQueryAsync(); // 이전까지는 cmd.ExecuteNonQueryAsync() 사용 // => 비동기 처리
                }

            }
            catch (Exception)
            {

                // TODO : 아무 예외처리 안해도 됨
            }

        }
        private async Task ConnectDatabaseServer()
        {
            try
            {
                connection = new MySqlConnection(connString);
                connection.Open();
                LogText += $"{DatabaseHost} DB 접속 접속 성공! {connection.State}\n";

            }
            catch (Exception ex)
            {
                LogText += $"{DatabaseHost} DB 접속 t;fvp 성공! : {ex.Message}\n";
            }
        }

        [RelayCommand]
        public async Task ConnectBroker()
        {
            if (string.IsNullOrEmpty(BrokerHost))
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "브로커 연결", "브로커 호스트를 입력하세요");
                return;
            }
            // MQTT 브로커에 접속해서 데이터를 가져오기
            await ConnectMqttBroker();
        }

        [RelayCommand]
        public async Task ConnectDatabase()
        {
            if (string.IsNullOrEmpty(DatabaseHost))
            {
                await this.dialogCoordinator.ShowMessageAsync(this, "DB 연결", "DB 호스트를 입력하세요");
                return;
            }
            
            connString = $"Server={DatabaseHost};Database=smarthome;Uid=root;Pwd=12345;Charset=utf8";

            await ConnectDatabaseServer();
        }

        public void Dispose()
        {
            // 리소스 해제를 명시적으로 처리하는 기능 추가
            connection?.Close(); // DB 접속 해제
        }
    }
}
