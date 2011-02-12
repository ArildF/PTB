using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace Rogue.Ptb.UI.Views
{
	public class Dialog : UserControl
	{
		private object _returnValue;
		private readonly Subject<bool> _closeResult;

		public Dialog()
		{
			Loaded += OnLoaded;


			_closeResult = new Subject<bool>();
		}

		public IObservable<bool> CloseResult
		{
			get { return _closeResult; }
		}


		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
		}

		public object ReturnValue
		{
			get { return _returnValue; }
			set
			{
				_returnValue = value;
				_closeResult.OnNext(_returnValue != null);
			}
		}

		public ICommand OkCommand { get; protected set; }
	}
}
