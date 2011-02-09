using System.Windows;

namespace Rogue.Ptb.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var bootstrapper = new Bootstrapper();

			bootstrapper.Bootstrap();

			var shellView = bootstrapper.CreateShell();

			shellView.Window.Show();
		}
	}
}
