using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using SharpDX.Direct3D9;
using System.Reflection.Metadata;

namespace Template
{
    public interface ILoadAssets
    {
        
        public static void Load(ContentManager content)
        {
            LoadSprites(content, GameWorld.sprites);
            LoadAnimations(content, GameWorld.animations);
            LoadSoundEffects(content, GameWorld.soundEffects);
            LoadMusic(content, GameWorld.music);
            GameWorld.gameFont = null;
        }

        private static void LoadSprites(ContentManager content, Dictionary<Enum, Texture2D> sprites)
        {
            //sprites.Add(LogicItems.MousePointer, content.Load<Texture2D>("Sprites\\GameItems\\mousePointer"));
        }

        private static void LoadAnimations(ContentManager content, Dictionary<Enum, Texture2D[]> animations)
        {

        }

        private static void LoadSoundEffects(ContentManager content, Dictionary<Enum, SoundEffect> soundEffects)
        {

        }

        private static void LoadMusic(ContentManager content, Dictionary<Enum, Song> music) 
        {

        }
    }
}
