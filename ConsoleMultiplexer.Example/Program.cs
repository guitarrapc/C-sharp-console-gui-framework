﻿using ConsoleMultiplexer.Controls;
using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleMultiplexer.Example
{
	class TestContext : IDrawingContext
	{
		public Size MinSize => new Size(100, 30);
		public Size MaxSize => new Size(100, 30);

		Character[,] _memory = new Character[100, 30];

		private IControl _control;
		public IControl Control
		{
			get => _control;
			set
			{
				_control = value;
				_control.Context = this;
				Redraw(_control);
			}
		}

		public void Redraw(IControl control)
		{
			if (control != _control) return;

			Update(control, control.Size.AsRect());
		}

		public void Update(IControl control, in Rect rect)
		{
			if (control != _control) return;

			foreach (var position in rect)
			{
				var c = control[Position.At(position.X, position.Y)];

				if (c.Content != _memory[position.X, position.Y].Content)
				{
					_memory[position.X, position.Y] = c;

					var color = c.Foreground ?? Color.White;

					Console.SetCursorPosition(position.X, position.Y);
					Console.Write($"\x1b[38;2;{color.Red};{color.Green};{color.Blue}m{c.Content}");
				}
			}
		}

		public event SizeLimitsChangedHandler SizeLimitsChanged;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.SetWindowSize(1, 1);
			Console.SetBufferSize(200, 5);
			Console.SetWindowSize(200, 50);
			Console.CursorVisible = false;

			var testContext = new TestContext();

			var textBlock1 = new TextBlock
			{
				Text = "Heheszki1",
				Color = new Color(87, 200, 157)
			};

			var border1 = new Border
			{
				BorderPlacement = BorderPlacement.All,
				Content = textBlock1
			};

			var textBlock2 = new TextBlock
			{
				Text = "Heheszki2",
				Color = new Color(157, 42, 157)
			};

			var border2 = new Border
			{
				BorderPlacement = BorderPlacement.Left,
				Content = textBlock2,
				BorderColor = new Color(200, 0, 0)
			};

			var stackPanel1 = new VerticalStackPanel();
			stackPanel1.Add(border1);
			stackPanel1.Add(border2);

			var border3 = new Border
			{
				BorderPlacement = BorderPlacement.All,
				Content = stackPanel1,
				BorderColor = new Color(100, 100, 50)
			};

			//var textBlock2 = new TextBlock
			//{
			//	Text = "Heheszki2"
			//};

			//var stackPanel = new VerticalStackPanel();
			//stackPanel.Add(new Border { Content = new TextBlock { Text = "Counter" } });
			//stackPanel.Add(textBlock1);
			//stackPanel.Add(new Border { Content = new TextBlock { Text = "Stoper" } });
			//stackPanel.Add(textBlock2);

			//var border = new Border()
			//{
			//	BorderPlacement = BorderPlacement.Left | BorderPlacement.Top | BorderPlacement.Right,
			//	Content = stackPanel
			//};

			testContext.Control = border3;

			int frames = 0;
			var watch = new Stopwatch();
			watch.Start();

			for (int i = 0; ; i++)
			{
				textBlock1.Text = $"{i}";

				if (watch.ElapsedMilliseconds > 1000)
				{
					textBlock2.Text = $"{frames}";
					Thread.Sleep(1000);
					watch.Restart();
					frames = 0;
					border3.BorderPlacement ^= BorderPlacement.Left;
				}

				frames++;
			}
		}
	}
}
