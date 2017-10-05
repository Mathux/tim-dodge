﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tim_dodge
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class TimGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public const int WINDOW_WIDTH = 1280;
		public const int WINDOW_HEIGHT = 720;
		World world;
		Player player;

		public TimGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			world = new World();
			player = new Player(22, 80, 138, world); // 22 sprites (ici vers la droite) 

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			world.Texture = Content.Load<Texture2D>("background/winter");
			world.Position = new Vector2(0, 0);
			world.colorTab = new Color[world.Texture.Width * world.Texture.Height];
			world.Texture.GetData<Color>(world.colorTab);

			player.Texture = Content.Load<Texture2D>("character/Tim");
			player.Position = new Vector2(500, 538);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
#endif

			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
			player.UpdateFrame(gameTime);
			player.Move(Keyboard.GetState());

			base.Update(gameTime);

		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			world.Draw(spriteBatch);
			player.DrawAnimation(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}