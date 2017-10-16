﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace tim_dodge
{
	public class Sound
	{
		public SoundEffectInstance music; // Music played

		public SoundEffect[] sounds;
		public SoundEffect[] musics;

		public bool sfxmute;
		public bool musicmute;

		public Sound(SoundEffect[] sfx, SoundEffect[] musc)
		{
			sounds = sfx;
			musics = musc;
		}

		public enum SoundName
		{
			jump = 0,
			explosion = 1
		}


		public enum MusicName
		{
			cuphead = 0
		}


		public void playSound(SoundName son)
		{
			if (!sfxmute && false)
				sounds[(int)son].Play();
		}

		public void playMusic(MusicName mus)
		{
			music = musics[(int)mus].CreateInstance();
			music.IsLooped = true;
			if (!musicmute)
			{
				SoundEffectInstance playing = musics[(int)mus].CreateInstance();
				playing.Volume = 0.60f;
				playing.IsLooped = true;
				playing.Play();
			}
		}
	}
}
