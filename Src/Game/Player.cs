﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace tim_dodge
{
	public class Player : PhysicalObject
	{

		public Heart Life;
		//public Stat Score;


		public bool IsDead()
		{
			return (Life.value == 0);
		}

		public Player(Vector2 pos, Heart Life, GameInstance gi)
			: base(Load.TimTexture, new Sprite("Content.character.TimXml.xml"), pos)
		{
			JumpImpulsion = new Vector2(0f, -180f);//-250f);//-180f);
			JumpMore = new Vector2(0, -150);
			DashForceLeft = new Vector2(-1500f, 0f);
			DashForceRight = -DashForceLeft;
			this.map = gi.Level.Current.map;
			Mass = 50;
			gameInst = gi;
			Sprite.ChangeDirection(Controller.Direction.RIGHT);

			this.Life = Life;

			//this.Score = Score;
			min_time_between_squat = Sprite.GetFrameTimeOfState((int)State.Squat) * 8;
		}

		public enum State
		{
			Stay = 0,
			Walk = 1,
			Jump = 2,
			Squat = 3,
			JumpH = 4,
			Tie = 5,
			Dead = 6
		}

		protected SoundEffect jump;
		protected Map map;

		protected Vector2 JumpImpulsion;
		protected Vector2 JumpMore;

		protected Vector2 DashForceLeft;
		protected Vector2 DashForceRight;

		protected GameInstance gameInst;

		protected float elapsed_since_last_jump = 0;
		const float min_time_between_jump = 0.25f;
		float min_time_between_squat;
 		protected float elapsed_since_last_squat = 0;
 		protected bool squatMode = false;

		public bool CanJump()
		{
			return map.pMap.nearTheGround(this) && elapsed_since_last_jump >= min_time_between_jump;
		}

		public bool CanSquat()
 		{
 			return elapsed_since_last_squat >= min_time_between_squat;
 		}

		public void Move(KeyboardState state, GameTime gameTime)
		{
			List<Controller.Direction> directions = Controller.GetDirections(state);

			elapsed_since_last_jump += (float)gameTime.ElapsedGameTime.TotalSeconds;
			elapsed_since_last_squat += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (gameInst.Level.Current.Beginning)
			{
				if (Sprite.NowState() != (int)State.Tie)
					ChangeSpriteState((int)State.Tie);
			}

			else 
			{
				if (CanSquat())
					squatMode = false;

				if (directions.Exists(el => el == Controller.Direction.TOP))
				{
					if (CanJump())
					{
						Load.sounds.playSound(Sound.SoundName.jump);
						elapsed_since_last_jump = 0;
						map.pMap.magnetizeToGround(this);
						ApplyNewImpulsion(JumpImpulsion);
					}
					else
					{
						if (Velocity.Y < 0) // if we are in the first state of jumping 
							ApplyNewForce(JumpMore);
					}
				}

				if (directions.Exists(el => el == Controller.Direction.LEFT))
				{
					Sprite.ChangeDirection(Controller.Direction.LEFT);
					if (velocity.X >= -3)
						ApplyNewForce(DashForceLeft);
				}

				if (directions.Exists(el => el == Controller.Direction.RIGHT))
				{
					Sprite.ChangeDirection(Controller.Direction.RIGHT);
					if (velocity.X <= 3)
						ApplyNewForce(DashForceRight);
				}

				if (directions.Exists(el => el == Controller.Direction.BOTTOM))
				{
					if (CanSquat())
					{
						ChangeSpriteState((int)State.Squat);
						elapsed_since_last_squat = 0;
						squatMode = true;
					}
				}

				if (!squatMode)
				{
					if (!map.pMap.nearTheGround(this))
					{
						if (Math.Abs(Velocity.X) > 2)
							ChangeSpriteState((int)State.Jump);
						else
							ChangeSpriteState((int)State.JumpH);

					}

					else
					{
						if (Math.Abs(Velocity.X) > 0.7)
							ChangeSpriteState((int)State.Walk);
						else
							ChangeSpriteState((int)State.Stay);
					}
				}
			}
		}

		const double time_invicibility = 0.5f;
		const double time_bonus = 0.5f;

		protected double last_damage_time = 0f;
		protected double last_bonus_time = 0f;

		protected override void ApplyCollision(Vector2 imp, int id, GameTime gt)
		{
			base.ApplyCollision(imp, id, gt);
			// Apply damage if necessary

			List<NonPlayerObject> es = gameInst.Level.Current.falling.FallingList.FindAll(en => en.ID == id);
			es.AddRange(gameInst.Level.Current.walking.EnemiesList.FindAll(en => en.ID == id));

			if (es.Count > 0)
			{
				foreach (NonPlayerObject e in es.FindAll((NonPlayerObject obj) => obj.Damage == 0))
				{
					Life.incr(e.Life);
					gameInst.scoreTim.incr(e.Bonus);
					e.TouchPlayer();
				}

				if (es.Exists(e => e.Bonus > 0 || e.Life > 0))
				{
					color = Color.LightBlue;
					last_bonus_time = gt.TotalGameTime.TotalSeconds;
				}

				if (gt.TotalGameTime.TotalSeconds - last_damage_time >= time_invicibility)
				{
					// take all the player wich give damage
					foreach (NonPlayerObject e in es.FindAll((NonPlayerObject obj) => obj.Damage>0))
					{
						Life.decr(e.Damage);
						e.TouchPlayer();
					}
					if (es.Exists(e => e.Damage > 0))
					{
						color = Color.IndianRed;
						last_damage_time = gt.TotalGameTime.TotalSeconds;
					}
				}

			}

		}
		public override void UpdatePosition(List<PhysicalObject> objects, Map map, GameTime gameTime)
		{
			base.UpdatePosition(objects, map, gameTime);
			if (gameTime.TotalGameTime.TotalSeconds - last_damage_time >= time_invicibility && 
			    gameTime.TotalGameTime.TotalSeconds - last_bonus_time >= time_bonus)
				color = Color.White;
		}
	}
}
