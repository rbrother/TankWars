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

    abstract class Menu : Screen {
        protected Texture2D _background;
        protected int menuLinePos;

        protected Texture2D Background {
            get {
                if (_background == null) {
                    _background = contentManager.Load<Texture2D>( "boxes" );
                }
                return _background;
            }
        }

        private Keys[] NumberKeys = new Keys[] { 
            Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5,
            Keys.D6, Keys.D7, Keys.D8, Keys.D9
        };

        public override object Update( ) {
            object result = null;
            if ( newKeyPresses.Contains( Keys.Escape )) {
                result = "ReturnHigherLevel";
            } else {
                var numberKey = NumberKeys.FirstOrDefault( key => newKeyPresses.Contains(key) );
                if (numberKey != Keys.None ) {
                    result = NumberKeyPressed( Array.IndexOf( NumberKeys, numberKey ) + 1 );
                }
            }
            return result;
        }

        protected virtual object NumberKeyPressed( int number ) {
            return null;
        }

        public override void Draw( ContentManager contentManager, SpriteBatch spriteBatch, Rectangle screenSize ) {
            base.Draw( contentManager, spriteBatch, screenSize );
            spriteBatch.Draw( Background, screenSize, Color.White );
            menuLinePos = 100;
        }

        protected void DrawMenuLines( params string[] lines ) {
            foreach (string line in lines) {
                DrawMenuLine( line );
            }
        }

        private void DrawMenuLine( string text ) {
            DrawTextCenteredShadow( Fonts.BigArial, text, new Vector2( screenSize.Width * 0.5f, menuLinePos ), Color.Black, Color.Green );
            menuLinePos += 120;
        }

        private void DrawTextCenteredShadow( SpriteFont font, string text, Vector2 textCenterPos, Color color, Color color2 ) {
            DrawTextCentered( font, text, textCenterPos, color );
            DrawTextCentered( font, text, textCenterPos + new Vector2( -3, -3 ), color2 );
        }

        private Vector2 CenterPos {
            get {
                return new Vector2( screenSize.Width * 0.5f, screenSize.Height * 0.5f );
            }
        }

        private void DrawTextCentered( SpriteFont font, string text, Vector2 textCenterPos, Color color ) {
            Vector2 size = font.MeasureString( text );
            Vector2 pos = textCenterPos - size * 0.5f;
            spriteBatch.DrawString( font, text, pos, color );
        }

    }

}
