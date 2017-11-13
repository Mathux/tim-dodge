﻿using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;

namespace tim_tests
{
	[TestFixture, Apartment(ApartmentState.STA)]
	public class Tests
	{
		/* INIT */

		SimulatedGame g = null;

		public Tests() { SetUp(); }

		public void Dispose() { TearDown(); }

		[OneTimeSetUp]
		protected void SetUp()
		{
			if (g == null)
			{
				g = new SimulatedGame();
				g.RunOneFrame();
				Console.WriteLine("Simulated game initialized!");
			}
		}
		[OneTimeTearDown]
		protected void TearDown()
		{
			if (g != null)
			{
				g.Exit();
				g.Dispose();
				Console.WriteLine("Simulated game disposed!");
			}
		}

		/* UTILS */

		Texture2D loadTexture(string str)
		{
			var stream = typeof(MainClass).Module.Assembly.GetManifestResourceStream("tim_tests.Resources." + str);
			Texture2D t = g.textureFromStream(stream);
			stream.Close();
			return t;
		}

		/* TESTS */

		[Test]
		public void Collisions() // Test collisions (sprite mask)
		{
			Texture2D t1 = loadTexture("Collisions.1.png");
			tim_dodge.PhysicalObject o1 = new tim_dodge.PhysicalObject(new tim_dodge.Texture(t1), null, new Vector2(0f, 0f));
			Texture2D t2 = loadTexture("Collisions.2.png");
			tim_dodge.PhysicalObject o2 = new tim_dodge.PhysicalObject(new tim_dodge.Texture(t2), null, new Vector2(0f, 0f));
			//Debug.Assert(tim_dodge.Collision.object_collision(o1, o2) == null);
			Assert.IsTrue(tim_dodge.Collision.object_collision(o1, o2) == null);
			o2.Position = new Vector2(1.0f, 0.0f);
			//Debug.Assert(tim_dodge.Collision.object_collision(o1, o2) != null);
			Assert.IsFalse(tim_dodge.Collision.object_collision(o1, o2) == null);
		}

		[Test]
		public void EnvironmentAndDeath() // Simulate some games and check that the player expected lifetime match
		{
			// Should be dead at the end
			g.initializeHardLevel();
			int remainingFrames = 500;
			bool ok1 = false;
			while (remainingFrames > 0)
			{
				g.RunOneFrame();
				remainingFrames--;
				if (g.isGameTerminated())
				{
					ok1 = true;
					break;
				}
			}
			// Should be alive at the end
			g.initializeEasyLevel();
			remainingFrames = 250;
			bool ok2 = true;
			while (remainingFrames > 0)
			{
				g.RunOneFrame();
				remainingFrames--;
				if (g.isGameTerminated())
				{
					ok2 = false;
					break;
				}
			}

			Assert.IsTrue(ok1 && ok2);
		}
	}
}
