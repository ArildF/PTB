using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for TaskControl.xaml
	/// </summary>
	public partial class TaskControl : UserControl
	{
		public TaskControl()
		{
			//  MUST do this BEFORE InitializeComponent()
			if (DesignerProperties.GetIsInDesignMode(this))
			{
				if (AppDomain.CurrentDomain.BaseDirectory.Contains("Blend 4"))
				{
					// load styles resources
					foreach (var resource in new[] { "../../Resources/Styles.xaml", "../../Resources/Art/Check.xaml" })
					{
						ResourceDictionary rd = new ResourceDictionary();
						rd.Source = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, resource),
						                    UriKind.Absolute);
						Resources.MergedDictionaries.Add(rd);
					}

					// load any other resources this control needs such as Converters
					//Resources.Add("booleanNOTConverter", new BooleanNOTConverter());
				}
			}
			InitializeComponent();
		}
	}
}
