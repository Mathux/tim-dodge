﻿using System;
using System.Text;
using System.Xml;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace tim_dodge
{
	public class Sprite
	{
		private XmlDocument doc = new XmlDocument();

		public Sprite()
		{
			LoadXml(); 
			nowState = State.Stay;
			nowFrame = 0;
			time = 0;
			Effect = SpriteEffects.None;
		}

		public SpriteEffects Effect
		{
			get;
			protected set;
		}

		public enum State {
			Stay = 0,
			Walk = 1
		}

		public const float FrameTime = 0.02f;
		public const int NbState = 2;

		public float time;

		public State state
		{
			get;
			private set;
		}

		private RectSprite[][] rect;

		private State nowState;

		private int nowFrame;

		public void NextFrame()
		{
			nowFrame += 1;
			if (nowFrame == NumberOfSprite())
			{
				nowFrame = 0;
			}
		}

		public void ChangeState(State state)
		{
			if (state != nowState && (int)state < NbState)
			{
				nowState = state;
				nowFrame = 0;
			}	
		}

		protected Controller.Direction direction;

		public void ChangeDirection(Controller.Direction new_dir)
		{
			if (new_dir != direction)
			{
				direction = new_dir;
				if (new_dir == Controller.Direction.RIGHT)
					Effect = SpriteEffects.None;
				if (new_dir == Controller.Direction.LEFT)
					Effect = SpriteEffects.FlipHorizontally;
			}
		}

		public void UpdateFrame(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			while (time > FrameTime)
			{
				NextFrame();
				time -= FrameTime;
			}
		}

		public int NumberOfSprite()
		{
			return rect[(int)nowState].Length;
		}

		public Rectangle RectOfSprite()
		{
			return (rect[(int)nowState][nowFrame]).source;
		}

		public void LoadXml()
		{
			var res = GetType().Module.Assembly.GetManifestResourceStream("tim_dodge.Content.character.TimXml.xml");
			var stream = new System.IO.StreamReader(res); 

			string docs = stream.ReadToEnd();
			doc.LoadXml(docs);
			XmlElement all = doc.DocumentElement;

			if (all != null)
			{
				rect = new RectSprite[all.ChildNodes.Count][];

				int i = 0;
				foreach (XmlNode child in all.ChildNodes)
				{
					rect[i] = new RectSprite[child.ChildNodes.Count];
					int j = 0;
					foreach (XmlNode ligne in child.ChildNodes)
					{
						rect[i][j] = new RectSprite(ligne.Attributes);
						j += 1;
					}
					i += 1;
				}
			}
		}
	}
}
