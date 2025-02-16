using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Template
{
    internal class MousePointer
    {
        #region Fields

        private Texture2D sprite;
        private GameObject tempObject;
        private Vector2 position;
        private bool leftClick;
        private bool rightClick;
        private bool ranLeftClick = false;
        private bool ranRightClick = false;
        private Thread inputThread;
        private LogicItems thisIs;

        #endregion
        #region Properties

        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, 1, 1); }
        }
        public Vector2 Position { get => position; }
        public bool LeftClick
        {
            get => leftClick;
            private set
            {
                leftClick = value;
                if (value == true)
                    LeftClickEvent();
                else
                    ranLeftClick = false;
            }
        }
        public bool RightClick
        {
            get => rightClick;
            private set
            {
                rightClick = value;
                if (value == true)
                    RightClickEvent();
                else
                    ranRightClick = false;
            }
        }

        #endregion
        #region Constructor

        public MousePointer(LogicItems type)
        {

            thisIs = type;
            try
            {
                sprite = GameWorld.sprites[thisIs];
            }
            catch { }
            inputThread = new Thread(HandleInput);
            inputThread.IsBackground = true;
            inputThread.Start();

        }

        #endregion
        #region Methods

        /// <summary>
        /// Draws a custom mousecursor at the location its detected to be in and
        /// </summary>
        /// <param name="spriteBatch">GameWorld logic</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
                spriteBatch.Draw(sprite, position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Method to run when left mouse is clicked
        /// </summary>
        private void LeftClickEvent()
        {
            if (!ranLeftClick)
            {

            }
            ranLeftClick = true;
        }

        /// <summary>
        /// Method to be run when right mouse is clicked
        /// </summary>
        private void RightClickEvent()
        {
            if (!ranRightClick)
            {

            }
            ranRightClick = true;
        }

        /// <summary>
        /// Thread function to continuously loop HandleInput which translates player input
        /// </summary>
        private void HandleInput()
        {

            while (GameWorld.GameRunning)
            {
                var mouseState = Mouse.GetState();
                position = mouseState.Position.ToVector2();
                LeftClick = mouseState.LeftButton == ButtonState.Pressed;
                RightClick = mouseState.RightButton == ButtonState.Pressed;
            }

        }

        #endregion
    }
}
