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

    class PlayerCharacter : Character {
        
        public void MoveStraight( Vector2 dir, double secondsElapsed ) {
            Position += dir * CharacterSpeed * Convert.ToSingle(secondsElapsed);
        }

        public object Update( KeyboardState keyboard, MouseState mouseState, double secondsElapsed ) {
            if (keyboard.IsKeyDown( Keys.A ) && keyboard.IsKeyDown( Keys.W )) {
                MoveStraight( Vector2.Normalize(new Vector2(-1f,-1f)), secondsElapsed );
            } else if (keyboard.IsKeyDown( Keys.W ) && keyboard.IsKeyDown( Keys.D )) {
                MoveStraight( Vector2.Normalize( new Vector2( 1f, -1f ) ), secondsElapsed );
            } else if (keyboard.IsKeyDown( Keys.D ) && keyboard.IsKeyDown( Keys.S )) {
                MoveStraight( Vector2.Normalize( new Vector2( 1f, 1f ) ), secondsElapsed );
            } else if (keyboard.IsKeyDown( Keys.A ) && keyboard.IsKeyDown( Keys.S )) {
                MoveStraight( Vector2.Normalize( new Vector2( -1f, 1f ) ), secondsElapsed );
            } else if (keyboard.IsKeyDown( Keys.A )) {
                MoveStraight( Vector2.UnitX * -1f, secondsElapsed );
            } else if (keyboard.IsKeyDown( Keys.D )) {
                MoveStraight( Vector2.UnitX, secondsElapsed );
            } else if (keyboard.IsKeyDown( Keys.W )) {
                MoveStraight( Vector2.UnitY * -1f, secondsElapsed );
            } else if (keyboard.IsKeyDown( Keys.S )) {
                MoveStraight( Vector2.UnitY, secondsElapsed );
            }
            if (mouseState.LeftButton == ButtonState.Pressed &&
                ( DateTime.Now - _lastBulletShoot ).TotalSeconds > RELOAD_SECONDS) {
                return ShootBullet( );
            } else {
                return null;
            }
        } // update

        private KeyboardState MoveToHeadingDir( KeyboardState keyboard, double secondsElapsed ) {
            var move = Vector2.Transform( -Vector2.UnitY, RotationMatrix ) * Convert.ToSingle( secondsElapsed ) * CharacterSpeed;
            move = move * ( keyboard.IsKeyDown( Keys.W ) ? 1.0f : -1.0f );
            move = CheckCollisions( move );
            Position += move;
            return keyboard;
        }

        private Vector2 CheckCollisions( Vector2 move ) {
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

    } // class

} // namespace
