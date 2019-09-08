﻿using ConsoleMultiplexer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMultiplexer.Controls
{
	public class TextBlock : Control
	{
		private string _text = "";
		public string Text
		{
			get => _text;
			set => Setter
				.Set(ref _text, value)
				.Then(Resize);
		}

		private Color? _color;
		public Color? Color
		{
			get => _color;
			set => Setter
				.Set(ref _color, value)
				.Then(Redraw);
		}

		public override Character this[Position position]
		{
			get
			{
				if (Text == null) return Character.Empty;
				if (position.X >= Text.Length) return Character.Empty;
				if (position.Y >= 1) return Character.Empty;
				return new Character(Text[position.X], foreground: Color);
			}
		}

		protected override void Resize()
		{
			Redraw(new Size(Text.Length, 1));
		}
	}
}
