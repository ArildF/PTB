using System;
using System.Linq;
using System.Windows.Controls;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Views;
using StructureMap;

namespace Rogue.Ptb.UI
{
	public class DialogDisplayer : IDialogDisplayer
	{
		private readonly Func<Dialog, DialogHost> _dialogCreator;
		private readonly IContainer _container;

		public DialogDisplayer(Func<Dialog, DialogHost> dialogCreator, IContainer container)
		{
			_dialogCreator = dialogCreator;
			_container = container;
		}

		public TDialogReturnValue ShowDialogFor<TDialogReturnValue>(DialogArgsBase args) 
			where TDialogReturnValue : DialogReturnValueBase
		{
			var baseType = typeof (DialogReturnValueBase<,>);

			var returnValueType = typeof (TDialogReturnValue).TraverseBy(type => type.BaseType)
				.Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == baseType).FirstOrDefault();

			if (returnValueType == null)
			{
				throw new InvalidOperationException("Cannot show dialog for " + typeof (TDialogReturnValue));
			}

			var genArgs = returnValueType.GetGenericArguments();


			var dialogType = genArgs[0];

			if (!typeof(Dialog).IsAssignableFrom(dialogType))
			{
				throw new InvalidOperationException("Cannot use " + dialogType + " for dialog");
			}

			var dialog = (Dialog)_container.With(args).GetInstance(dialogType);
			var host = _dialogCreator(dialog);

			host.ShowDialog();


			return (TDialogReturnValue) dialog.ReturnValue;
		}
	}
}
