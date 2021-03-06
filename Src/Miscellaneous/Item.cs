﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tim_dodge
{
	/// <summary>
	/// Manager items : text, texture, position
	/// </summary>
	public class Item
	{
		public Rectangle source;
		public Color Color;
		public float Opacity; // between 0.0f and 1.0f : indice of transparence

		protected String text;
		protected SpriteFont fontItem;
		protected SpriteFont DefaultFont;
		protected Color DefaultColor;
		protected Texture2D Image;
		protected Vector2 position;
		protected Vector2 origin;
		protected Vector2 size;

		public String Text
		{
			get { return text; }
			set 
			{
				text = value;
				Size = fontItem.MeasureString(Text);
			}
		}

		public Vector2 Position 
		{
			get { return position; }
			set
			{
				position = value;
				source.X = (int)(position.X + origin.X);
				source.Y = (int)(position.Y + origin.Y);
			}
		}

		public Vector2 Origin
		{
			get { return origin; }
			set
			{
				origin = value;
				source.X = (int)(position.X + origin.X);
				source.Y = (int)(position.Y + origin.Y);
			}
		}

		public Vector2 Size
		{
			get { return size; }
			set
			{
				size = value;
				source.Width = (int)size.X;
				source.Height = (int)size.Y;
			}
		}

		public Item(String Text, SpriteFont fontItem, Color Color)
		{
			this.text = Text;
			this.fontItem = DefaultFont = fontItem;
			this.Color = DefaultColor = Color;
			Size = fontItem.MeasureString(Text);
			DefaultConstruct();
		}

		public Item(Texture2D Image)
		{
			this.Image = Image;
			Size = new Vector2(TimGame.GAME_WIDTH, TimGame.GAME_HEIGHT); // Default value
			Color = Color.White;
			DefaultConstruct();
		}

		private void DefaultConstruct()
		{
			Opacity = 1.0f;
			position = Vector2.Zero;
			origin = Vector2.Zero;
			source = new Rectangle((int)(position.X + origin.X),
			                       (int)(position.Y + origin.Y),
			                       (int)(size.X),
			                       (int)(size.Y));
		}

		public void unsetColor()
		{
			Color = DefaultColor;
		}

		public void unsetFont()
		{
			fontItem = DefaultFont;
			Size = fontItem.MeasureString(Text);
			source = new Rectangle((int)(position.X + origin.X),
					   (int)(position.Y + origin.Y),
					   (int)(size.X),
					   (int)(size.Y));

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (text != null && fontItem != null)
				spriteBatch.DrawString(fontItem, text, Position + Origin, Color * Opacity);

			else if (Image != null)
				spriteBatch.Draw(Image, source, Color * Opacity);
		}
	}
}
