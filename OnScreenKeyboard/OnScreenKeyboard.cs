#region File Description
//-----------------------------------------------------------------------------
// SafeAreaOverlay.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System;
#endregion

namespace TopHatHacker.Tools
{
    
    /// <summary>
    /// on screen keyboard
    /// </summary>
    public class OnScreenKeyboard : DrawableGameComponent
    {
        string[] quertyList = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "^", "z", "x", "c", "v", "b", "n", "m", "<--", "?123", "Space","Done"};
        Keys[] quertyKeyList = { Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P, Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.H, Keys.J, Keys.K, Keys.L, Keys.CapsLock, Keys.Z, Keys.X, Keys.C, Keys.V, Keys.B, Keys.N, Keys.M, Keys.Back, Keys.SelectMedia, Keys.Space, Keys.Enter };
        /// <summary>
        /// Set or get the font file.
        /// </summary>
        public SpriteFont Font;
        TimeSpan lastClick;
        Keys lastKeyDown;
        public bool Enabled = true;
        Keys[] keysDown;
        public Color BackgroundColor;
        public Color ButtonColor1;
        public Color ButtonColor2;
        public Color TextColor;
        SpriteBatch spriteBatch;
        Texture2D dummyTexture;
        List<singleKey> keys;
        int keyboardWidth;
        int keyboardHeight;
        /// <summary>
        /// Constructor.
        /// </summary>
        public OnScreenKeyboard(Game game)
            : base(game)
        {
            keysDown = new Keys[0];
            // Choose a high number, so we will draw on top of other components.
            DrawOrder = 1000;
            ButtonColor1 = Color.Gray;
            ButtonColor2 = Color.Gray;
            BackgroundColor = Color.LightGray;
            TextColor = Color.White;
        }

        public KeyboardState GetState()
        {
            KeyboardState temp = new KeyboardState(keysDown);
            keysDown = new Keys[0];
            return temp;
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - lastClick.TotalMilliseconds < 50)
                return;
            keysDown = new Keys[0];
            TouchCollection collection = TouchPanel.GetState();
            MouseState mouse = Mouse.GetState();
            foreach (TouchLocation location in collection)
            {
                if(touched(location.Position,gameTime))
                {
                    System.Console.WriteLine("touch at" + location.Position.ToString());
                }
            }
            if(mouse.LeftButton == ButtonState.Pressed)
                if(touched(new Vector2(mouse.X,mouse.Y),gameTime))
                {
                        System.Console.WriteLine("touch at" + new Vector2(mouse.X,mouse.Y).ToString());
                }
            base.Update(gameTime);
        }

        private bool touched(Vector2 position, GameTime gameTime)
        {
            foreach (singleKey key in keys)
                {
                    if(key.Bounds.Intersects(new Rectangle((int)position.X,(int)position.Y,1,1)))
                    {
                        if(gameTime.TotalGameTime.TotalMilliseconds -  lastClick.TotalMilliseconds < 100)
                            if (lastKeyDown == key.key)
                                return false;
                        lastKeyDown = key.key;
                        lastClick = gameTime.TotalGameTime;
                        Keys[] keysDownTemp = new Keys[keysDown.Length+1];
                        keysDown.CopyTo(keysDownTemp, 0);
                        keysDownTemp[keysDownTemp.Length - 1] = key.key;
                        keysDown = keysDownTemp;
                        return true;
                    }
                }
            return false;
        }

        public void UpdateKeyboardSize(object o, System.EventArgs e)
        {
            System.Console.WriteLine("Should do some code here");
        }

        protected void figureOutKeyboardSize()
        {
            keyboardWidth = Game.GraphicsDevice.Viewport.Width;
            keyboardHeight = Game.GraphicsDevice.Viewport.Height * 40/100;
            int keyWidth = keyboardWidth / 10;
            int keyHeight = keyboardHeight / 4;
            keys = null;
            for (int i = 0; i < quertyList.Length; i++ )
            {
                singleKey key = new singleKey();
                if (keys == null)
                {
                    keys = new List<singleKey>();
                    key.KeyChar = quertyList[i];
                    key.key = quertyKeyList[i];
                    key.Bounds = new Rectangle(0, GraphicsDevice.Viewport.Height - keyboardHeight, keyWidth, keyHeight);
                    key.DrawingBounds = new Rectangle(key.Bounds.X + key.Bounds.Width * 5 / 100, key.Bounds.Y + key.Bounds.Height * 5 / 100, keyWidth - key.Bounds.Width * 5 / 100 - key.Bounds.Width * 5 / 100, keyHeight - key.Bounds.Height * 5 / 100 - key.Bounds.Height * 5 / 100);
                    keys.Add(key);
                }
                else
                {
                    if (i < 10)
                    {
                        key.KeyChar = quertyList[i];
                        key.key = quertyKeyList[i];
                        key.Bounds = new Rectangle(keys[keys.Count - 1].Bounds.X + keyWidth, GraphicsDevice.Viewport.Height - keyboardHeight, keyWidth, keyHeight);
                        key.DrawingBounds = new Rectangle(key.Bounds.X + key.Bounds.Width * 5 / 100, key.Bounds.Y + key.Bounds.Height * 5 / 100, keyWidth - key.Bounds.Width * 5 / 100 - key.Bounds.Width * 5 / 100, keyHeight - key.Bounds.Height * 5 / 100 - key.Bounds.Height * 5 / 100);
                        keys.Add(key);
                    }
                    else if (i == 10)
                    {
                        key.KeyChar = quertyList[i];
                        key.key = quertyKeyList[i];
                        key.Bounds = new Rectangle((GraphicsDevice.Viewport.Width - keyWidth * 9)/2, GraphicsDevice.Viewport.Height - keyboardHeight + keyHeight, keyWidth, keyHeight);
                        key.DrawingBounds = new Rectangle(key.Bounds.X + key.Bounds.Width * 5 / 100, key.Bounds.Y + key.Bounds.Height * 5 / 100, keyWidth - key.Bounds.Width * 5 / 100 - key.Bounds.Width * 5 / 100, keyHeight - key.Bounds.Height * 5 / 100 - key.Bounds.Height * 5 / 100);
                        keys.Add(key);
                    }
                    else if (i < 19)
                    {
                        key.KeyChar = quertyList[i];
                        key.key = quertyKeyList[i];
                        key.Bounds = new Rectangle(keys[keys.Count - 1].Bounds.X + keyWidth, GraphicsDevice.Viewport.Height - keyboardHeight + keyHeight, keyWidth, keyHeight);
                        key.DrawingBounds = new Rectangle(key.Bounds.X + key.Bounds.Width * 5 / 100, key.Bounds.Y + key.Bounds.Height * 5 / 100, keyWidth - key.Bounds.Width * 5 / 100 - key.Bounds.Width * 5 / 100, keyHeight - key.Bounds.Height * 5 / 100 - key.Bounds.Height * 5 / 100);
                        keys.Add(key);
                    }
                    else if (i == 19)
                    {
                        key.KeyChar = quertyList[i];
                        key.key = quertyKeyList[i];
                        key.Bounds = new Rectangle((GraphicsDevice.Viewport.Width - keyWidth * 9) / 2, GraphicsDevice.Viewport.Height - keyboardHeight + keyHeight * 2, keyWidth, keyHeight);
                        key.DrawingBounds = new Rectangle(key.Bounds.X + key.Bounds.Width * 5 / 100, key.Bounds.Y + key.Bounds.Height * 5 / 100, keyWidth - key.Bounds.Width * 5 / 100 - key.Bounds.Width * 5 / 100, keyHeight - key.Bounds.Height * 5 / 100 - key.Bounds.Height * 5 / 100);
                        keys.Add(key);
                    }
                    else if (i < 28)
                    {
                        key.KeyChar = quertyList[i];
                        key.key = quertyKeyList[i];
                        key.Bounds = new Rectangle(keys[keys.Count - 1].Bounds.X + keyWidth, GraphicsDevice.Viewport.Height - keyboardHeight + keyHeight * 2, keyWidth, keyHeight);
                        key.DrawingBounds = new Rectangle(key.Bounds.X + key.Bounds.Width * 5 / 100, key.Bounds.Y + key.Bounds.Height * 5 / 100, keyWidth - key.Bounds.Width * 5 / 100 - key.Bounds.Width * 5 / 100, keyHeight - key.Bounds.Height * 5 / 100 - key.Bounds.Height * 5 / 100);
                        keys.Add(key);
                    }
                    else if (i == 28)
                    {
                        key.KeyChar = quertyList[i];
                        key.key = quertyKeyList[i];
                        key.Bounds = new Rectangle((GraphicsDevice.Viewport.Width - keyWidth * 9) / 2, GraphicsDevice.Viewport.Height - keyboardHeight + keyHeight * 3, keyWidth*2, keyHeight);
                        key.DrawingBounds = new Rectangle(key.Bounds.X + key.Bounds.Width * 5 / 100, key.Bounds.Y + key.Bounds.Height * 5 / 100, key.Bounds.Width - key.Bounds.Width * 5 / 100 - key.Bounds.Width * 5 / 100, key.Bounds.Height - key.Bounds.Height * 5 / 100 - key.Bounds.Height * 5 / 100);
                        keys.Add(key);
                    }
                    else if (i == 29)
                    {
                        key.KeyChar = quertyList[i];
                        key.key = quertyKeyList[i];
                        key.Bounds = new Rectangle(keys[keys.Count - 1].Bounds.X + keys[keys.Count - 1].Bounds.Width, GraphicsDevice.Viewport.Height - keyboardHeight + keyHeight * 3, keyWidth * 4, keyHeight);
                        key.DrawingBounds = new Rectangle(key.Bounds.X + key.Bounds.Width * 5 / 100, key.Bounds.Y + key.Bounds.Height * 5 / 100, key.Bounds.Width - key.Bounds.Width * 5 / 100 - key.Bounds.Width * 5 / 100, key.Bounds.Height - key.Bounds.Height * 5 / 100 - key.Bounds.Height * 5 / 100);
                        keys.Add(key);
                    }

                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Game.GraphicsDevice.DeviceReset +=new System.EventHandler<System.EventArgs>(UpdateKeyboardSize);
            this.figureOutKeyboardSize();
        }

        /// <summary>
        /// Creates the graphics resources needed to draw the overlay.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a 1x1 white texture.
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);

            dummyTexture.SetData(new Color[] { Color.White });
        }


        /// <summary>
        /// Draws the keyboard
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if (!Enabled)
                return;

            // Look up the current viewport and safe area dimensions.
            Viewport viewport = GraphicsDevice.Viewport;

            Rectangle safeArea = viewport.TitleSafeArea;

            int viewportRight = viewport.X + viewport.Width;
            int viewportBottom = viewport.Y + viewport.Height;

            // Compute four border rectangles around the edges of the safe area.
            Rectangle Area = new Rectangle(0,viewport.Height - keyboardHeight,keyboardWidth,keyboardHeight);

            Rectangle rightBorder = new Rectangle(100, 100, 50, 50);

            Rectangle topBorder = new Rectangle(120, 120, 50, 50);

            Rectangle bottomBorder = new Rectangle(200, 100, 20, 20);

            // Draw the safe area borders.
            Color translucentRed = Color.Red * 0.5f;

            spriteBatch.Begin();

            spriteBatch.Draw(dummyTexture, Area, BackgroundColor);
            foreach (singleKey myKey in keys)
            {
                spriteBatch.Draw(dummyTexture, myKey.DrawingBounds, ButtonColor1);
                spriteBatch.DrawString(Font,myKey.KeyChar,new Vector2(myKey.DrawingBounds.X + myKey.DrawingBounds.Width * 10/100,myKey.DrawingBounds.Y + myKey.DrawingBounds.Height * 10/100),Color.White);
            }
            spriteBatch.End();
        }
    }
}
