﻿using System;
using Microsoft.Xna.Framework;

namespace tim_dodge
{
	public class Map
	{
		public Rectangle[] roofs
		{
			get;
			protected set;
		}
		public Rectangle[] grounds
		{
			get;
			protected set;
		}
		public Rectangle[] leftWalls
		{
			get;
			protected set;
		}
		public Rectangle[] rightWalls
		{
			get;
			protected set;
		}

		public Map()
		{
			// TODO: Different maps system
			roofs = new Rectangle[] { };
			leftWalls = new Rectangle[] { };
			rightWalls = new Rectangle[] { };
			grounds = new Rectangle[] { new Rectangle(0, 538, TimGame.WINDOW_WIDTH, 100) };
		}

		public void adjustPositionAndVelocity(PhysicalObject o)
		{
			Vector2 position = o.Position;
			Vector2 velocity = o.Velocity;
			foreach (Rectangle r in grounds)
			{
				if (Collision.rect_collision(new Rectangle(position.ToPoint(), o.Size), r) != null)
				{
					position.Y = r.Y - o.Size.Y;
					velocity.Y = Math.Min(velocity.Y, 0);
				}
			}
			foreach (Rectangle r in roofs)
			{
				if (Collision.rect_collision(new Rectangle(position.ToPoint(), o.Size), r) != null)
				{
					position.Y = r.Y + r.Size.Y;
					velocity.Y = Math.Max(velocity.Y, 0);
				}
			}
			foreach (Rectangle r in leftWalls)
			{
				if (Collision.rect_collision(new Rectangle(position.ToPoint(), o.Size), r) != null)
				{
					position.X = r.X + r.Size.X;
					velocity.X = Math.Max(velocity.X, 0);
				}
			}
			foreach (Rectangle r in rightWalls)
			{
				if (Collision.rect_collision(new Rectangle(position.ToPoint(), o.Size), r) != null)
				{
					position.X = r.X - o.Size.X;
					velocity.X = Math.Min(velocity.X, 0);
				}
			}
			o.Position = position;
			o.Velocity = velocity;
		}
	}
}