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

        protected Matrix RotationMatrix { 
            get { 
                return Matrix.CreateRotationZ( Convert.ToSingle(rotation) ); 
            } 
        }

        public float CharacterSpeed { get { return 8f; } }

        protected void RotateLeft( double secondsElapsed ) {
            rotation -= ROTATION_SPEED * secondsElapsed;
            if (rotation < 0.0) rotation += Pi2;
        }

        protected void RotateRight( double secondsElapsed ) {
            rotation += ROTATION_SPEED * secondsElapsed;
            if (rotation > Pi2) rotation -= Pi2;
        }

        protected virtual string TextureName { get { return "ukko2"; } }

        public virtual void Draw( SpriteBatch spriteBatch, Vector2 mapOffset, float blockSize ) {
            var texture = GameContent.GetTexture( TextureName );
            var scale = blockSize / texture.Height * 2f;
            spriteBatch.Draw( texture, mapOffset + Position * blockSize, null, Color.White,
                Convert.ToSingle( rotation ), new Vector2( texture.Width, texture.Height ) * 0.5f, 
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

        protected Vector2 CheckCollisions( Vector2 move ) {
            // Check collisions
            /*            var closeBlocks = terrain
                            .Where( block => Vector2.DistanceSquared( block.location, position ) < 100 * 100 );
                        var stopDistance = 66f;
                        foreach (TerrainPiece block in closeBlocks) {
                            if (Math.Abs( position.X - block.location.X ) < stopDistance - 2) {
                                if (( position.Y > block.location.Y &&
                                    position.Y < block.location.Y + stopDistance &&
                                    move.Y < 0 ) ||
                                   ( position.Y < block.location.Y &&
                                    position.Y > block.location.Y - stopDistance &&
                                    move.Y > 0 )) move = new Vector2( move.X, 0f );
                            }
                            if (Math.Abs( position.Y - block.location.Y ) < stopDistance - 2) {
                                if (( position.X > block.location.X &&
                                    position.X < block.location.X + stopDistance &&
                                    move.X < 0 ) ||
                                    ( position.X < block.location.X &&
                                    position.X > block.location.X - stopDistance &&
                                    move.X > 0 )) move = new Vector2( 0f, move.Y );
                            }
                        }
             */
            return move;
        }

    }

}
