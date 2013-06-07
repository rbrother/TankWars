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

    abstract class Tank {
        public static Texture2D Texture;
        protected static readonly double RELOAD_SECONDS = 2.0;
        protected static readonly float ROTATION_SPEED = 2.0f;
        protected static readonly float Pi2 = (float) Math.PI * 2.0f;

        protected Vector2 position = new Vector2( 0.0f, 0.0f );
        protected float rotation = 0.0f;
        protected DateTime _lastBulletShoot = DateTime.Now;
        protected Bullet[] _bullets = new Bullet[] { };

        public Vector2 Position { get { return position; } }

        public Bullet[] Bullets { 
            get { return _bullets; }
            set { _bullets = value; }
        }

        protected Matrix TankRotationMatrix { get { return Matrix.CreateRotationZ( rotation ); } }

        protected void RotateLeft( float secondsElapsed ) {
            rotation -= ROTATION_SPEED * secondsElapsed;
            if (rotation < 0.0) rotation += Pi2;
        }

        protected void RotateRight( float secondsElapsed ) {
            rotation += ROTATION_SPEED * secondsElapsed;
            if (rotation > Pi2) rotation -= Pi2;
        }

        public void Draw( SpriteBatch spriteBatch, Vector2 objectOffset, Color color ) {
            spriteBatch.Draw( Texture, position + objectOffset, null, color, rotation,
                new Vector2( 184.0f, 184.0f ), 0.2f, SpriteEffects.None, 0.0f );
            foreach (Bullet bullet in _bullets) {
                bullet.Draw( spriteBatch, objectOffset );
            }
        }

        protected void ShootBullet( ) {
            var velocity = Vector2.Transform( -Vector2.UnitY, TankRotationMatrix );
            var bullet = new Bullet {
                Position = position + velocity * 55.0f,
                Speed = velocity
            };
            _bullets = _bullets.Concat( new Bullet[] { bullet } ).ToArray( );
            _lastBulletShoot = DateTime.Now;
        }

        public void MoveBullets( float secondsElapsed ) {
            foreach (Bullet bullet in _bullets) bullet.Move( secondsElapsed );
            _bullets = _bullets
                .Where( bullet => Vector2.Distance( position, bullet.Position ) < 1000 )
                .ToArray( );
        }

    }

    class EnemyTank : Tank {

        public void Update( float secondsElapsed, PlayerTank playerTank ) {
            var vectorToPlayer = playerTank.Position - this.position;
            var angleToPlayer = Math.Atan2( vectorToPlayer.Y, vectorToPlayer.X ) + Math.PI * 0.5;

            double clockwiseDist, counterDist;
            if (rotation > angleToPlayer) {
                clockwiseDist = Pi2 + angleToPlayer - rotation;
                counterDist = rotation - angleToPlayer;
            } else {
                clockwiseDist = angleToPlayer - rotation;
                counterDist = Pi2 + rotation - angleToPlayer;
            }
            if (clockwiseDist < counterDist) {
                RotateRight( secondsElapsed );
            } else {
                RotateLeft( secondsElapsed );
            }
        }

    }

    class PlayerTank : Tank {

        public void Update( KeyboardState keyboard, MouseState mouseState, float secondsElapsed,
            TerrainPiece[] terrain ) {
            if (keyboard.IsKeyDown( Keys.A )) {
                RotateLeft( secondsElapsed );
            } else if (keyboard.IsKeyDown( Keys.D )) {
                RotateRight( secondsElapsed );
            }
            if (keyboard.IsKeyDown( Keys.W ) || keyboard.IsKeyDown( Keys.S )) {
                var move = Vector2.Transform( -Vector2.UnitY, TankRotationMatrix ) * secondsElapsed * 100.0f;
                move = move * ( keyboard.IsKeyDown( Keys.W ) ? 1.0f : -1.0f );

                // Check collisions
                var closeBlocks = terrain
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

                position += move;
            }
            if (mouseState.LeftButton == ButtonState.Pressed &&
                ( DateTime.Now - _lastBulletShoot ).TotalSeconds > RELOAD_SECONDS) {
                ShootBullet( );
            }

        } // update
    }
}
