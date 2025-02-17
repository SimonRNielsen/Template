using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Collections.Generic;
using System;

namespace Template
{
    internal class MousePointer
    {
        #region Fields

        private Texture2D sprite;                                           //Custom mouse cursor texture
        private List<GameObject> tempObjects = new List<GameObject>();      //For handling more than one item at a time
        private Vector2 position;
        private bool leftClick;
        private bool rightClick;
        private bool ranLeftClick = false;                                  //Blocks more than one run of event
        private bool ranRightClick = false;                                 //Blocks more than one run of event
        private Thread inputThread;                                         //Thread for running mouse input non-stop
        private LogicItems type;                                            //Enum for defining object

        public Action LeftClickEventHandler;                                //Left click eventhandler
        public Action RightClickEventHandler;                               //Right click eventhandler
        #endregion
        #region Properties

        /// <summary>
        /// CollisionBox for registering mouse/objects overlap
        /// </summary>
        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, 1, 1); }
        }

        /// <summary>
        /// Used externally for mouse position referrencing
        /// </summary>
        public Vector2 Position { get => position; }

        /// <summary>
        /// Handles mouse left-click event and external detection thereof
        /// </summary>
        public bool LeftClick
        {
            get => leftClick;
            private set
            {
                leftClick = value;
                if (leftClick && !ranLeftClick)
                {
                    LeftClickEventHandler?.Invoke();
                    ranLeftClick = true;
                }
                else
                    ranLeftClick = false;
            }
        }

        /// <summary>
        /// Handles mouse right-click event and external detection thereof
        /// </summary>
        public bool RightClick
        {
            get => rightClick;
            private set
            {
                rightClick = value;
                if (rightClick && !ranRightClick)
                {
                    RightClickEventHandler?.Invoke();
                    ranRightClick = true;
                }
                else
                    ranRightClick = false;
            }

        }

        #endregion
        #region Constructor

        /// <summary>
        /// Creates an instance for custom mouse handling
        /// </summary>
        /// <param name="type">Enum to define mouse and set sprite</param>
        public MousePointer(LogicItems type)
        {

            this.type = type;
            try
            {
                sprite = GameWorld.sprites[type];
            }
            catch { }
            inputThread = new Thread(HandleInput);
            inputThread.IsBackground = true;
            inputThread.Start();
            LeftClickEventHandler += LeftClickEvent;
            RightClickEventHandler += RightClickEvent;

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


            
        }

        /// <summary>
        /// Method to run when right mouse is clicked
        /// </summary>
        private void RightClickEvent()
        {
            


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
