using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ff14bot.AClasses;
using ff14bot.Helpers;

namespace RBrestartButton
{
	public class RBrestartButton : BotPlugin
	{
		public override string Name => "RBrestartButton";
		public override string Author => "Zimble";
		public override Version Version => new Version(1, 0, 0, 0);
		public override bool WantButton => false;

		public override void OnEnabled() { }

		public override void OnDisabled() { }

		public override void OnInitialize()
		{
			AddMainMenuButton("Restart RB", (sender, e) => RestartRebornBuddy());
		}

		public void RestartRebornBuddy()
		{
			Logging.Write(Colors.Wheat, $"[{Name}] - Restart button clicked");
			Process RBprocess = Process.GetCurrentProcess();
			Process.Start(@"Plugins\RBrestartButton\WaitForClose.bat", $"{RBprocess.Id} {ff14bot.Core.Memory.Process.Id}");
			RBprocess.CloseMainWindow();
		}

		private static void AddMainMenuButton(string label, RoutedEventHandler onClick)
		{
			if (!Application.Current.MainWindow.CheckAccess())
			{
				Logging.WriteDiagnostic($"Failed to add button \"{label}\": Current thread {Thread.CurrentThread.ManagedThreadId} \"{Thread.CurrentThread.Name}\" cannot access MainWindow dispatcher.");

				return;
			}

			Application.Current.MainWindow.Dispatcher.Invoke(() =>
			{
				Window mainWindow = Application.Current.MainWindow;
				ComboBox mainMenu = mainWindow.FindName("BotBox") as ComboBox;
				Grid buttonGrid = mainMenu.Parent as Grid;

				Button newButton = new Button
				{
					HorizontalAlignment = HorizontalAlignment.Right,
					VerticalAlignment = VerticalAlignment.Top,
					Width = 129,
					Height = 18,
					Margin = new Thickness(0, 250, 10, 0),

					IsEnabled = true,
					Visibility = Visibility.Visible,

					Name = $"button{buttonGrid.Children.Count + 1}",
					Content = label,
				};

				newButton.Click += onClick;

				buttonGrid.Children.Add(newButton);
			});
		}
	}
}
