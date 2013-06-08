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

    abstract class Character {

        protected static readonly double RELOAD_SECONDS = 2.0;
        protected static readonly float ROTATION_SPEED = 4.0f;
        protected static readonly float Pi2 = (float)Math.PI * 2.0f;

        protected double rotation = 0.0f;
        protected DateTime _lastBulletShoot = DateTime.Now;

        /// <summary>
        /// Position in units of blocks from (0,0) to (60,60)
        /// </summary>
        public Vector2 Position { get; set; }

        public Texture2D Texture { get; set; }

        protected Matrix RotationMatrix { 
            get { 
                return Matrix.CreateRotationZ( Convert.ToSingle(rotation) ); 
            } 
        }

        public float CharacterSpeed { get { return 10.0f; } }

        protected void RotateLeft( double secondsElapsed ) {
            rotation -= ROTATION_SPEED * secondsElapsed;
            if (rotation < 0.0) rotation += Pi2;
        }

        protected void RotateRight( double secondsElapsed ) {
            rotation += ROTATION_SPEED * secondsElapsed;
            if (rotation > Pi2) rotation -= Pi2;
        }

        public void Draw( SpriteBatch spriteBatch, Vector2 mapOffset, float blockSize ) {
            var scale = blockSize / Texture.Height * 2f;
            spriteBatch.Draw( Texture, mapOffset + Position * blockSize, null, Color.White, 
                Convert.ToSingle( rotation ), new Vector2(Texture.Width, Texture.Height) * 0.5f, 
                scale, SpriteEffects.None, 0.0f );
        }

        protected Bullet ShootBullet( ) {
            var velocity = Vector2.Transform( -Vector2.UnitY, RotationMatrix );
            var bullet = new Bullet {
                Position = Position + velocity * 55.0f,
                Speed = velocity
            };
            _lastBulletShoot = DateTime.Now;
            return bullet;
        }

    }

}
