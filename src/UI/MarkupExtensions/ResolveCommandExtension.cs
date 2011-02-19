using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Commands;

namespace Rogue.Ptb.UI.MarkupExtensions
{
	[MarkupExtensionReturnType(typeof(ICommand))]
	public class ResolveCommandExtension : MarkupExtension
	{
		private readonly string _name;

		public ResolveCommandExtension()
		{
			
		}

		public ResolveCommandExtension(string name)
		{
			_name = name;
		}

		public ResolveCommandExtension(CommandName commandName)
		{
			_name = commandName.Name;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var pvt = (IProvideValueTarget) serviceProvider.GetService(typeof (IProvideValueTarget));
			var fwe = pvt.TargetObject as FrameworkElement;
			if (fwe == null)
			{
				return this;
			}

			var property = pvt.TargetProperty as DependencyProperty;
			if (property == null)
			{
				return null;
			}

			return new ProxyCommand(fwe, _name);
		}

		public class ProxyCommand : ICommand
		{
			private readonly FrameworkElement _frameworkElement;
			private readonly string _name;

			public ProxyCommand(FrameworkElement frameworkElement, string name)
			{
				_frameworkElement = frameworkElement;
				_name = name;

				_frameworkElement.DataContextChanged += FrameworkElementOnDataContextChanged;
			}

			private void FrameworkElementOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
			{
				if (CanExecuteChanged != null)
				{
					CanExecuteChanged(this, EventArgs.Empty);
				}
			}

			public void Execute(object parameter)
			{
				Command.DoIfNotNull(c => c.Execute(parameter));
			}

			protected ICommand Command
			{
				get
				{
					var dc = _frameworkElement.GetValue(FrameworkElement.DataContextProperty);

					if (dc == null)
					{
						return null;
					}

					var resolver = dc as ICommandResolver;
					if (resolver == null)
					{
						return null;
					}

					var name = _name ?? (string)_frameworkElement.GetValue(FrameworkElement.NameProperty);

					return resolver.Resolve(CommandName.Create(name));
				}
			}

			public bool CanExecute(object parameter)
			{
				return Command.IfNotNull(c => c.CanExecute(parameter));
			}

			public event EventHandler CanExecuteChanged;
		}
	}
}
