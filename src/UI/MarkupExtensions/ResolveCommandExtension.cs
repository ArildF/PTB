using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Rogue.Ptb.UI.Commands;

namespace Rogue.Ptb.UI.MarkupExtensions
{
	[MarkupExtensionReturnType(typeof(ICommand))]
	public class ResolveCommandExtension : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var pvt = (IProvideValueTarget) serviceProvider.GetService(typeof (IProvideValueTarget));
			var fwe = pvt.TargetObject as FrameworkElement;
			if (fwe == null)
			{
				return null;
			}

			var property = pvt.TargetProperty as DependencyProperty;
			if (property == null)
			{
				return null;
			}

			fwe.DataContextChanged += (sender, args) => OnDataContextChanged(fwe, property);
			return null;
		}

		private static void OnDataContextChanged(FrameworkElement fwe, DependencyProperty property)
		{
			ICommand command = GetCommand(fwe);

			fwe.SetValue(property, command);
		}

		private static ICommand GetCommand(FrameworkElement fwe)
		{
			var dc = fwe.GetValue(FrameworkElement.DataContextProperty);

			if (dc == null)
			{
				return null;
			}

			var resolver = dc as ICommandResolver;
			if (resolver == null)
			{
				throw new Exception("DataContext must be an ICommandResolver");
			}

			var name = (string) fwe.GetValue(FrameworkElement.NameProperty);
			return resolver.Resolve(CommandName.Create(name));
		}
	}
}
