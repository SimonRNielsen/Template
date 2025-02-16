using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Template
{
    public class GameWorld : Game, ILoadAssets
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        internal static MousePointer mousePointer;
        private static bool gameRunning = true;

        #region Lists and assets

        private static ContentManager AddContent;
        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        public static Dictionary<Enum, Texture2D> sprites = new Dictionary<Enum, Texture2D>();
        public static Dictionary<Enum, Texture2D[]> animations = new Dictionary<Enum, Texture2D[]>();
        public static Dictionary<Enum, SoundEffect> soundEffects = new Dictionary<Enum, SoundEffect>();
        public static Dictionary<Enum, Song> music = new Dictionary<Enum, Song>();
        public static SpriteFont gameFont;

        #endregion
        #endregion
        #region Properties

        public static bool GameRunning { get => gameRunning; }


        #endregion

        public GameWorld()
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            AddContent = Content;
            mousePointer = new MousePointer(LogicItems.MousePointer);

            base.Initialize();

        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ILoadAssets.Load(Content);

            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameRunning = false;
                Thread.Sleep(30);
                Exit();
            }

            if (gameObjects.Count > 0)
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(gameTime);
                }

            gameObjects.RemoveAll(obj => obj.IsAlive == false);
            gameObjects.AddRange(newGameObjects);
            newGameObjects.Clear();

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            mousePointer.Draw(_spriteBatch);
            if (gameObjects.Count > 0)
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Draw(_spriteBatch);
                }

            _spriteBatch.End();

            base.Draw(gameTime);

        }

        public static void AddObject(GameObject obj)
        {

            obj.LoadContent(AddContent);
            newGameObjects.Add(obj);

        }

        public static void AddObject(List<GameObject> objects)
        {

            foreach (GameObject obj in objects)
            {
                obj.LoadContent(AddContent);
                newGameObjects.Add(obj);
            }

        }

    }
}
