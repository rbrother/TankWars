using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BreakingMission {
    /// <summary>
    /// Base class for different game screens like menus,
    /// gameplay, bonus scenes, etc.
    /// </summary>
    abstract class Screen {
        protected ContentManager contentManager;
        protected Rectangle screenSize;
        protected SpriteBatch spriteBatch;
        protected static KeyboardState keystate, prevKeystate;
        protected static Keys[] newKeyPresses = new Keys[] {};

        public virtual void Draw( ContentManager contentManager, SpriteBatch spriteBatch, Rectangle screenSize ) {
            this.contentManager = contentManager;
            this.screenSize = screenSize;
            this.spriteBatch = spriteBatch;
        }

        public object UpdateOuter( ) {
            keystate = Keyboard.GetState();
            newKeyPresses = keystate.GetPressedKeys( ).
                Except( prevKeystate.GetPressedKeys( ) ).ToArray();
            object result = Update( );
            prevKeystate = keystate;
            return result;
        }

        public abstract object Update( );

    }
}
