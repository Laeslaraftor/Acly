using Acly.Performing;
using Acly.Requests;
using Acly.Tasks;
using Acly.Test;
using Acly;
using Acly.Numbers;
using System.Diagnostics;
using System.Text;
using System.Net.Sockets;
using Acly.Commands;
using Acly.JsonData;
using Acly.Player;
using System.Net;
using Acly.Serialize;

Api.BaseUrl = "https://api.acly.ru";
Api.BaseFileExtension = "php";
//Api.BaseParameters = () => "auth_key=awd";

Console.WriteLine("Начато");

//IAsyncTask Load = Loading.Start<TestLoading>();
//Load.Completed += () =>
//{
//	Console.WriteLine("Выполнено");
//};
//Load.ProgressUpdated += Value =>
//{
//	Console.WriteLine("Прогресс: " + Value);
//};
//Load.Failed += Info =>
//{
//	Console.WriteLine(Info.ToString());
//};

//AsyncValueAnimation anim = new();
//Stopwatch timer = null;

//anim.SetDuration(TimeSpan.FromSeconds(1)).SetFromTo(0, 100).SetTickEvent((a, v) =>
//{
//	Log.Message("Анимация: " + v);
//});
//anim.Ended += async (a, m) =>
//{
//	timer.Stop();
//	Log.Message("Время выполнения: " + timer.Elapsed.TotalSeconds);

//	await Task.Delay(TimeSpan.FromSeconds(0.5));

//	a.Start();
//	timer = Stopwatch.StartNew();
//};

//anim.Start();
//timer = Stopwatch.StartNew();

//string BaseFiles = "https://user-files.acly.ru/files/";
//string BaseOut = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Test\";

//string FileUrl = "https://api.acly.ru/openbeat/game/get_resources.php";
//string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\test.php";

//void OnDownloadCompleted(/*byte[] Result*/)
//{
//	Console.WriteLine("Загрузка завершена: " /*+ Result.Length*/);
//}
//void OnDownloadProgress(float Percent)
//{
//	Console.WriteLine("Загрузка: " + Math.Round(Percent * 100, 2) + "%");
//}
//void OnDownloadError(IAsyncTaskError Error)
//{
//	Console.WriteLine(Error);
//}
//DownloadFileInfo File(string In, string Out)
//{
//	return new(BaseFiles + In, BaseOut + Out);
//}

//while (true)
//{
//	using IAsyncTask Downloading = Ajax.DownloadAsync(new List<DownloadFileInfo>
//	{
//		File("27ec9e4ea84ef6e2272fe5707dc1dff7_-Disconnected_-.mp3", "disconnected.mp3"),
//		File("50c18cf9683b37cd4e2e4c2c5f38d269AclyApp.apk", "50c18cf9683b37cd4e2e4c2c5f38d269AclyApp.apk"),
//		File("5bbea854be5aa3b7468b2642edc2f669Resourse-Pack.zip", "5bbea854be5aa3b7468b2642edc2f669Resourse-Pack.zip"),
//		File("a39b869c60540b958f7cdeb4ea0a4bbcNew-World252.rar", "a39b869c60540b958f7cdeb4ea0a4bbcNew-World252.rar"),
//		File("ab815e51dba9ef6c2de92c2822462957Fall-Out-Boy-Immortals-(Official-Music-Video)-(From-_-Big-Hero-6_-)[1].mp3", "immortals.mp3"),
//		File("dc0950e3c70ddb333254d5453db48069images.zip", "images.zip")
//	});
//	Downloading.Completed += OnDownloadCompleted;
//	Downloading.ProgressUpdated += OnDownloadProgress;
//	Downloading.Failed += OnDownloadError;
//	Console.ReadLine();
//}

//ApiResponse test = await Ajax.GetJson<ApiResponse>("https://api.acly.ru/openbeat/auth/game_auth.php?auth_key=20e3ddfbcd40cd7cc5d09f58fa3f8780&os=19c19fbd145dc61536ac7268e3af5dec&processor=89050b8ba659eb5c0e519992a155affa&userName=1f97037455429593f76d36bf9bd07810");
//Console.WriteLine(test.ToString());

Log.Enabled = false;

TimeSpan Timeout = TimeSpan.FromSeconds(5);
ISendable Sendable = null;
IDisposable Disposable = null;
IServer CurrentServer = null;

async void StartClient()
{
	Console.WriteLine("Подключение...");
	IClient Srv = null;
	try
	{
		Srv = await Client.Connect(IPAddress.Parse("192.168.0.100"), 11000, Timeout);
	}
	catch (Exception Error)
	{
		Console.WriteLine(Error);
		return;
	}
	
	Sendable = Srv;
	Disposable = Srv;
	Console.WriteLine("Подключено\n");

	Srv.Received += OnDataReceived;
	Srv.Disconnected += OnServerDisconnected;

	void OnServerDisconnected(string Reason)
	{
		Srv = null;
		Sendable = null;
		Console.WriteLine(Reason);
	}
	void OnDataReceived(byte[] Data)
	{
		string Value = Encoding.UTF8.GetString(Data);
		Console.WriteLine(/*"Получено сообщение: " + */Value);
	}
}
async void StartServer()
{
	CommandServer CmdSrv = new("localhost", 11000);
	return;
	Console.WriteLine("Подключение...");
	IServer Srv = Server.Create("localhost", 11000);
	Sendable = Srv;
	Disposable = Srv;
	CurrentServer = Srv;
	Console.WriteLine("Подключено\n");

	Srv.Connected += OnNewConnection;
	Srv.Received += OnDataReceived;
	Srv.Disconnected += OnServerDisconnected;

	void OnServerDisconnected(Socket Socket, string Reason)
	{
		Console.WriteLine("Отключение: {0}. (Подключений: {1})", Reason, Srv.TotalConnections);
	}
	async void OnDataReceived(Socket Socket, byte[] Data)
	{
		string Value = Encoding.UTF8.GetString(Data);
		Console.WriteLine("Запрос: " + Value);

		try
		{
			string Request = await Ajax.Get(Value);
			byte[] RequestData = Encoding.UTF8.GetBytes(Request);

			await Socket.SendAsync(RequestData);
		}
		catch (Exception Error)
		{
			Console.WriteLine("Ошибка: " + Error.Message);
		}
	}
	void OnNewConnection(Socket Socket)
	{
		Console.WriteLine("Новое подключение! (Всего: {0})", Srv.TotalConnections);
	}
}

Console.WriteLine("a) клиент \n b) сервер");
string Answ = Console.ReadLine();
Console.Clear();

if (Answ == "a")
{
	StartClient();
}
else
{
	StartServer();
}

while (Sendable != null)
{
	string Input = Console.ReadLine();
	byte[] TextData = Encoding.UTF8.GetBytes(Input);

	Sendable?.Send(TextData, 0, TextData.Length);

	continue;

	if (Command.TryParse(Input, out Command? Result))
	{
		Console.WriteLine(Result.ToString());
	}

	if (Input == "end")
	{
		Disposable?.Dispose();
		break;
	}
	if (Input.Length > 4 && Input.Substring(0, 4) == "/msg" && Sendable != null)
	{
		string[] Split = Input.Split(' ');
		byte[] Data = Encoding.UTF8.GetBytes(Split[1]);

		Sendable?.Send(Data);
	}
	else if (Input.Length > 5 && Input.Substring(0, 6) == "/limit" && CurrentServer != null)
	{
		string[] Split = Input.Split(' ');
		int.TryParse(Split[1], out int Limit);

		CurrentServer.MaximumConnections = Limit;
		Console.WriteLine("Установлен лимит подключений: " + Limit);
	}
}

//ApiResponse Result = await Ajax.GetJson<ApiResponse>("https://api.acly.ru/auth/get_gameAccount.php?auth_key=e589fc2f0249d24378ae3242ade790d9");
//Console.WriteLine(Result.ToString());
Console.ReadLine();