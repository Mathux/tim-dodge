﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tim_dodge
{
	public class Renderer
	{
		RenderTarget2D renderTarget;
		readonly GraphicsDeviceManager graphics;
	 
		public Renderer(GraphicsDeviceManager graphics)
		{
			this.graphics = graphics;

			renderTarget = new RenderTarget2D(
				graphics.GraphicsDevice,
				graphics.PreferredBackBufferWidth,
				graphics.PreferredBackBufferHeight);
		}

		float angle = 0;
		float pas = -0.007f;
		const float max = (float) Math.PI / 16;	

		public void Draw(SpriteBatch spriteBatch, GameManager game)
		{
			angle += pas;
			if (angle > max)
				pas = -pas;
			//angle -= (float) (2 * Math.PI);
			if (angle < -max)
				pas = -pas;
			
			Rend(spriteBatch, game);
			
			graphics.GraphicsDevice.Clear(Color.Black);

			float w = ((Texture2D)renderTarget).Width;
			float h = ((Texture2D)renderTarget).Height;
			float diag = (float)Math.Sqrt(w * w + h * h);

			Vector2 middle = new Vector2(w / 2.0f, h / 2.0f);

			float proportion = h / w;
			//float angle = (float)(3*Math.PI/4);
			//angle = (float)Math.Atan2(w, h) + 0.4f;
			//angle = 0.4f;
			float scale = (float) Math.Abs((h / (Math.Cos(Math.Atan2(w,h)-angle)*diag)));
			scale = Math.Min(scale,(float)Math.Abs((h / (Math.Cos(Math.Atan2(w, h) - angle + 2 * Math.Atan2(h, w)) * diag))));
			//scale = 0.7f;

			try
			{
				if (!game.GameRunning || !game.rotation)
				{
					angle = 0;
					scale = 1;
				}
			}
			catch { angle = 0; scale = 1;}

			spriteBatch.Begin();
			spriteBatch.Draw((Texture2D)renderTarget, middle, null, Color.White,
			                 angle, middle,
			                 scale, SpriteEffects.None, 0); // (float)(Math.PI * 2)
			spriteBatch.End();

		}

		public void Rend(SpriteBatch spriteBatch, GameManager game)
		{
			// Set the device to the render target
			graphics.GraphicsDevice.SetRenderTarget(renderTarget);

			graphics.GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();
			game.Draw(spriteBatch);
			spriteBatch.End();

			// Reset the device to the back buffer
			graphics.GraphicsDevice.SetRenderTarget(null);
		}
	}
}
