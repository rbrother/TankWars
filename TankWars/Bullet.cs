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

namespace TankWars {

    public class MovingObject {
        public Vector2 Position;
        public Vector2 Speed;
        public void Move( float secondsElapsed ) {
            Position = Position + Speed * secondsElapsed * 600.0f;
        }
    }

    public class Bullet : MovingObject {
        public static Texture2D texture;

        public static void Load( ContentManager content ) {
            texture = content.Load<Texture2D>( "bullet" );
        }

        public void Draw( SpriteBatch spriteBatch, Vector2 objectOffset ) {
            spriteBatch.Draw( texture, Position + objectOffset, null, Color.White, 0.0f,
                new Vector2( 16.0f, 16.0f ), 0.3f, SpriteEffects.None, 0.0f );
        }
    }
}
