using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;

namespace Template
{
    public class GameWorld : Game, ILoadAssets
    {
        #region Fields

        private static bool gameRunning = true;

        #region Lists, assets and objects

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static ContentManager AddContent;
        internal static MousePointer mousePointer;
        private static List<GameObject> gameObjects = new List<GameObject>();
        private static List<GameObject> newGameObjects = new List<GameObject>();
        public static Dictionary<Enum, Texture2D> sprites = new Dictionary<Enum, Texture2D>();
        public static Dictionary<Enum, Texture2D[]> animations = new Dictionary<Enum, Texture2D[]>();
        public static Dictionary<Enum, SoundEffect> soundEffects = new Dictionary<Enum, SoundEffect>();
        public static Dictionary<Enum, Song> music = new Dictionary<Enum, Song>();
        public static SpriteFont gameFont;
        public static object syncGameObjects = new object();

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

            //Sets window resolution
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            //Instantiates mousePointer and makes "Content" static
            AddContent = Content;
            mousePointer = new MousePointer(LogicItems.MousePointer);

            base.Initialize();

        }


        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ILoadAssets.Load(Content);

        }


        protected override void Update(GameTime gameTime)
        {
            //Registers keyboard input
            var keyboardInput = Keyboard.GetState();

            //Closes threads connected to "GameRunning" property
            if (keyboardInput.IsKeyDown(Keys.Escape) && gameRunning)
            {
                gameRunning = false;
                Thread.Sleep(20);
                Exit();
            }

            //Update loop (if any objects present)
            if (gameObjects.Count > 0)
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(gameTime);
                }

            //Handles adding and removing objects from update-loop, is sync'ed with other method using same list via "syncGameObjects" lock object
            lock (syncGameObjects)
            {
                gameObjects.RemoveAll(obj => obj.IsAlive == false);
                gameObjects.AddRange(newGameObjects);
            }
            newGameObjects.Clear();

            base.Update(gameTime);

        }


        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Handles sorting from highest value front to lowest value in the back
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            //Draws custom mousepointer
            mousePointer.Draw(_spriteBatch);

            //Syncs with other method iterating gameObjects via "syncGameObjects" lock object
            lock (syncGameObjects)
            {
                //Iterates list only if any objects present
                if (gameObjects.Count > 0)
                    foreach (GameObject gameObject in gameObjects)
                    {
                        gameObject.Draw(_spriteBatch);
                    }
            }

            _spriteBatch.End();

            base.Draw(gameTime);

        }

        /// <summary>
        /// Method to add one or more objects and run its "LoadContent" method
        /// </summary>
        /// <param name="obj">Either single object, list or array</param>
        public static void AddObject(GameObject obj)
        {

            obj.LoadContent(AddContent);
            newGameObjects.Add(obj);

        }
        public static void AddObject(GameObject[] objs)
        {

            foreach (GameObject obj in objs)
            {
                obj.LoadContent(AddContent);
                newGameObjects.Add(obj);
            }

        }
        public static void AddObject(List<GameObject> objs)
        {

            foreach (GameObject obj in objs)
            {
                obj.LoadContent(AddContent);
                newGameObjects.Add(obj);
            }

        }

    }
}
