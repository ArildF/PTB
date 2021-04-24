using System;
using System.Reflection;
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
				if (!(pvt.TargetObject is Freezable))
				{
					return this;
				}
				var freezable = (Freezable)pvt.TargetObject;
				return new FreezableProxyCommand(freezable, _name);
			}

			var property = pvt.TargetProperty as DependencyProperty;
			if (property == null)
			{
				return null;
			}

			return new ProxyCommand(fwe, _name);
		}

		public class FreezableProxyCommand : ProxyCommand
		{
			private readonly Freezable _freezable;

			public FreezableProxyCommand(Freezable freezable, string name) : base(null, name)
			{
				_freezable = freezable;

				var evt = _freezable.GetType().GetEvent("InheritanceContextChanged",
					BindingFlags.Instance | BindingFlags.NonPublic);
				var addMethod = evt.GetAddMethod(true);
				addMethod.Invoke(freezable, new[] {new EventHandler(OnInheritanceContextChanged)});

				SetFrameworkElementFromInheritanceContext();
			}

			private void OnInheritanceContextChanged(object sender, EventArgs e)
			{
				SetFrameworkElementFromInheritanceContext();
			}

			private void SetFrameworkElementFromInheritanceContext()
			{
				var fwe = _freezable.GetType()
					.GetProperty( "InheritanceContext", BindingFlags.Instance | BindingFlags.NonPublic) 
					.GetValue(_freezable, null) as FrameworkElement;

				FrameworkElement = fwe;
			}
		}

		public class ProxyCommand : ICommand
		{
			private FrameworkElement _frameworkElement;
			private readonly string _name;

			public ProxyCommand(FrameworkElement frameworkElement, string name)
			{
				_name = name;

				FrameworkElement = frameworkElement;
				CommandOnCanExecuteChanged(this, EventArgs.Empty);
			}

			private void FrameworkElementOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
			{
				if (Command != null)
				{
					Command.CanExecuteChanged -= CommandOnCanExecuteChanged;
				}

				if (CanExecuteChanged != null)
				{
					CanExecuteChanged(this, EventArgs.Empty);
				}

				if (Command != null)
				{
					Command.CanExecuteChanged += CommandOnCanExecuteChanged;
				}
			}

			private void CommandOnCanExecuteChanged(object? sender, EventArgs e)
			{
				CanExecuteChanged?.Invoke(this, e);
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

			protected FrameworkElement FrameworkElement
			{
				get {
					return _frameworkElement;
				}
				set {
					if (_frameworkElement != null)
					{
						_frameworkElement.DataContextChanged -= FrameworkElementOnDataContextChanged;
					}
					_frameworkElement = value;
					if (_frameworkElement != null)
					{
						_frameworkElement.DataContextChanged += FrameworkElementOnDataContextChanged;
					}
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
