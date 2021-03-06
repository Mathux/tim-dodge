﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tim_dodge
{
	/// <summary>
	/// Manager texts like score of level.
	/// </summary>
	public class Stat : Item
	{
		public int value { get; private set; }
		public String Title;

		public Stat(SpriteFont fontStat, Color Color, String Title, int value) : base(Title + value, fontStat, Color)
		{
			this.Title = Title;
			this.value = value;
		}

		public void incr(int i)
		{
			value += i;
		}

		public void decr(int i)
		{
			value -= i;
			if (value < 0)
				value = 0;
		}

		public void set(int i)
		{
			value = i;
			if (value < 0)
				value = 0;
		}

		public new void Draw(SpriteBatch spriteBatch)
		{
			Text = Title + value;
			base.Draw(spriteBatch);
		}

	}
}
