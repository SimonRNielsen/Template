using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Content;

namespace Template
{
    public abstract class GameObject
    {
        #region Fields

        private object animationLock = new object();
        protected Enum type;
        protected Texture2D sprite;
        protected Texture2D[] sprites;
        protected Color color = Color.White;
        protected Vector2 position;
        protected SpriteEffects[] spriteEffects = new SpriteEffects[3] { SpriteEffects.None, SpriteEffects.FlipHorizontally, SpriteEffects.FlipVertically };
        protected float fps = 20;
        protected float layer = 0.5f;
        protected float scale = 1;
        protected float timeElapsed;
        protected int spriteEffectIndex;
        private float rotation;
        private int currentIndex;
        private bool isAlive = true;

        #endregion

        #region Properties
        public virtual Rectangle CollisionBox
        {
            get
            {
                if (sprite != null)
                    return new Rectangle((int)(Position.X - (sprite.Width / 2) * scale), (int)(Position.Y - (sprite.Height / 2) * scale), (int)(sprite.Width * scale), (int)(sprite.Height * scale));
                else
                    return new Rectangle();
            }
        }
        public Texture2D Sprite { get => sprite; }
        public Texture2D[] Sprites
        {
            get => sprites;
            set
            {
                lock (animationLock)
                {
                    sprites = value;
                }
            }
        }
        public float Rotation { get => rotation; set => rotation = value; }
        public Vector2 Position { get => position; set => position = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public int SpriteEffectIndex { get => spriteEffectIndex; set => spriteEffectIndex = value; }
        public Enum Type { get => type; protected set => type = value; }
        public float Layer { get => layer; }

        #endregion

        #region Constructor


        public GameObject(Enum type, Vector2 spawnPos)
        {
            Type = type;
            position = spawnPos;
        }

        #endregion
        #region Methods


        public abstract void LoadContent(ContentManager content);


        public virtual void Update(GameTime gameTime)
        {
            if (sprites != null)
                Animate(gameTime);
        }


        protected virtual void Animate(GameTime gameTime)
        {

            lock (animationLock)
            {
                if (currentIndex >= sprites.Length - 1)
                {
                    timeElapsed = 0;
                    currentIndex = 0;
                }
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentIndex = (int)(timeElapsed * fps);
                sprite = sprites[currentIndex];
            }

        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
                spriteBatch.Draw(sprite, position, null, color, rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, spriteEffects[spriteEffectIndex], layer);
        }

        #endregion

    }
}
