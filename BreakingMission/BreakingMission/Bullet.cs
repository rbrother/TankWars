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

    public class MovingObject {

        public Vector2 Position;
        public Vector2 Speed;

        public virtual void Move( double secondsElapsed ) {
            Position = Position + Speed * Convert.ToSingle(secondsElapsed * 600.0);
        }
    }

    public class Bullet : MovingObject {
        public bool Old; // Bullet has been flying too long, should be removed
        private DateTime fireTime;

        public Bullet( ) {
            Old = false;
            fireTime = DateTime.Now;
        }

        public void Draw( SpriteBatch spriteBatch ) {
            spriteBatch.Draw( Textures.Get("bullet"), Position, null, Color.White, 0.0f,
                new Vector2( 16.0f, 16.0f ), 0.3f, SpriteEffects.None, 0.0f );
        }

        public override void Move( double secondsElapsed ) {
            base.Move( secondsElapsed );
            Old = ( DateTime.Now - fireTime ).TotalSeconds > 1.0;
        }
    }
}
