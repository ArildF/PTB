using System;
using System.Windows;
using System.Windows.Threading;

namespace Rogue.Ptb.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private Bootstrapper _bootstrapper;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			//HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

			_bootstrapper = new Bootstrapper();

			DispatcherUnhandledException += OnDispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

			_bootstrapper.Bootstrap(e.Args);

			var shellView = _bootstrapper.CreateShell();

			shellView.Window.Show();
		}

		private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			HandleException((Exception)e.ExceptionObject);
		}

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			HandleException(e.Exception);
		}

		private void HandleException(Exception exception)
		{
			Action<Exception> handler = _bootstrapper != null ? _bootstrapper.TryResolveExceptionHandler() : null;
			handler = handler ??
			          (ex => MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error));

			handler(exception);
		}
	}
}
