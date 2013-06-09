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

    public enum HitState {
        None1,
        LeftHand,
        None2,
        RightHand
    }

    class PlayerCharacter : Character {

        private Vector2 _targetPos;
        private HitState _hitState = HitState.None1;
        private DateTime _hitTime;
 
        protected override string TextureName {
            get {
                return _hitState == HitState.LeftHand ? "ukko-hit-vas" :
                    _hitState == HitState.RightHand ? "ukko-hit-oik" :
                    "ukko2";
            }
        }

        public void MoveStraight( Vector2 dir, double secondsElapsed ) {
            Position += dir * CharacterSpeed * Convert.ToSingle(secondsElapsed);
        }

        public object Update( KeyboardState keyboard, MouseState mouseState, bool newMouseLeftClick, bool newMouseRightClick, double secondsElapsed ) {
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
            _targetPos = new Vector2( mouseState.X, mouseState.Y );

            if (_hitState == HitState.None1 && newMouseRightClick) {
                Hit( HitState.LeftHand );
            } else if (_hitState == HitState.LeftHand && IsHitOld) {
                _hitState = HitState.None2;
            } else if (_hitState == HitState.None2 && newMouseRightClick) {
                Hit( HitState.RightHand );
            } else if (_hitState == HitState.RightHand && IsHitOld) {
                _hitState = HitState.None1;
            }

            if (mouseState.LeftButton == ButtonState.Pressed &&
                ( DateTime.Now - _lastBulletShoot ).TotalSeconds > RELOAD_SECONDS) {
                return ShootBullet( );
            } else {
                return null;
            }
        } // update

        private void Hit( HitState state ) {
            _hitState = state;
            _hitTime = DateTime.Now;
            GameContent.GetSound( "hit" ).Play( );
        }

        private bool IsHitOld { get { return ( DateTime.Now - _hitTime ).TotalSeconds > 0.3; } }

        private KeyboardState MoveToHeadingDir( KeyboardState keyboard, double secondsElapsed ) {
            var move = Vector2.Transform( -Vector2.UnitY, RotationMatrix ) * Convert.ToSingle( secondsElapsed ) * CharacterSpeed;
            move = move * ( keyboard.IsKeyDown( Keys.W ) ? 1.0f : -1.0f );
            move = CheckCollisions( move );
            Position += move;
            return keyboard;
        }

        public override void Draw( SpriteBatch spriteBatch, Vector2 mapOffset, float blockSize ) {
            var targetBlockPos = ( _targetPos - mapOffset ) / blockSize;
            var vectorToTarget = targetBlockPos - Position;
            var angleToTarget = Math.Atan2( vectorToTarget.Y, vectorToTarget.X ) + Math.PI * 0.5;
            this.rotation = angleToTarget;
            base.Draw( spriteBatch, mapOffset, blockSize );
        }

    } // class

} // namespace
