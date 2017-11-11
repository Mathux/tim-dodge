﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace tim_dodge
{
	/// <summary>
	/// Bonus which fall from the sky and add bonus score to the player.
	/// </summary>
	public class Coin : NonPlayerObject
	{
		public Coin(Texture t, Sprite s, Vector2 p) : base(t, s, p)
		{
			Mass = 7;
			Bonus = 250;
		}

		protected void autoDestruct(GameTime gameTime)
		{
			if (Damaged)
			{
				wait_before_die -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (wait_before_die <= 0f)
					Dead = true;
			}
		}

		protected override void ApplyCollision(Vector2 imp, PhysicalObject obj, GameTime gt)
		{
			base.ApplyCollision(imp, obj, gt);
			Damaged = true;
			wait_before_die = 0f;
		}

		public override void UpdatePosition(List<PhysicalObject> objects, Map map, GameTime gameTime)
		{
			base.UpdatePosition(objects, map, gameTime);
			autoDestruct(gameTime);
		}
		float wait_before_die = 1.0f;

		public override void TouchPlayer()
		{
			Load.sounds.playSound(Sound.SoundName.coin);
		}
	}
}
