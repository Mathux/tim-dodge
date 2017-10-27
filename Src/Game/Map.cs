﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace tim_dodge
{
	public class Map
	{
		public Map(Texture2D Background)
		{
			this.Background = Background;
			gMap = new GraphicalMap();
			pMap = new PhysicalMap();
		}

		public GraphicalMap gMap;
		public PhysicalMap pMap;

		public Texture2D Background;

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Background, Vector2.Zero, Color.White);
			gMap.Draw(spriteBatch);
		}

		public enum Ground
		{
			LeftGround = 0,
			MiddleGround = 1,
			RightGround = 2,
			LeftDurt = 3,
			MiddleDurt = 4,
			RightDurt = 5,
			RightEGround = 6,
			BottomRightDurt = 7,
			BottomDurt = 8,
			BottomLeftDurt = 9,
			LeftEGround = 10,
			BottomLeft2Durt = 11,
			LeftPlatform = 12,
			MiddlePlatform = 13,
			RightPlatform = 14,
			BottomRight2Durt = 15
		}


	}
}
