using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Grabacr07.KanColleViewer.Views.Contents
{
	/// <summary>
	/// Fleets.xaml の相互作用ロジック
	/// </summary>
	public partial class Fleets : UserControl
	{
		public Fleets()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ScrollViewer scrollViewer = sender as ScrollViewer;

			if (Keyboard.Modifiers != ModifierKeys.Shift)
				return;

			if (e.Delta > 0)
			{
				scrollViewer.LineLeft();
			}
			else
			{
				scrollViewer.LineRight();
			}

			e.Handled = true;
		}
	}
}
