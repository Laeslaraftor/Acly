using Acly.Numbers;
using System.Diagnostics;
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

namespace Acly.Test2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			anim.SetDuration(TimeSpan.FromSeconds(1)).SetFromTo(0, 100).SetTickEvent((a, v) =>
			{
				//Log.Message("Анимация: " + v);

				Dispatcher.Invoke(() =>
				{
					Counter.Text = v.ToString();
				});
			});
			anim.Ended += async (a, m) =>
			{
				timer.Stop();
				//Log.Message("Время выполнения: " + timer.Elapsed.TotalSeconds);

				Dispatcher.Invoke(() =>
				{
					Counter.Text = "Время выполнения: " + timer.Elapsed.TotalSeconds;
				});

				await Task.Delay(TimeSpan.FromSeconds(1));

				a.Start();
				timer = Stopwatch.StartNew();
			};
		}

		private readonly AsyncValueAnimation anim = new();
		private Stopwatch timer = null;

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			anim.Start();
			timer = Stopwatch.StartNew();
		}
	}
}