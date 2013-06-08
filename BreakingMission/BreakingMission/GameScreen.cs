﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BreakingMission {

    class GameScreen : Screen {

        readonly static int AREA_SIZE = 60;

        private TerrainPiece[,] _terrain = new TerrainPiece[AREA_SIZE, AREA_SIZE];

        protected Bullet[] _bullets = new Bullet[] { };

        protected PlayerCharacter _player;
        protected EnemyCharacter[] _enemies = new EnemyCharacter[] { };

        public GameScreen( ) {
            for (int x = 0; x < AREA_SIZE; x++) {
                for (int y = 0; y < AREA_SIZE; y++) {
                    _terrain[x, y] = new TerrainPiece( ( x + y ) % 2 == 0 ? "grass" : "sand" );
                }
            }
            _player = new PlayerCharacter { 
                Texture = Textures.Get("ukko2"),
                Position = Vector2.One * AREA_SIZE * 0.5f
            };
        }

        public Vector2 MapSize {
            get { return new Vector2( screenSize.Height, screenSize.Height ); }
        }

        public Vector2 MapOffset {
            get { return new Vector2( 0.5f * ( screenSize.Width - screenSize.Height ), 0f ); }
        }

        public float BlockSize { get { return MapSize.X / AREA_SIZE; } }

        public Bullet[] Bullets {
            get { return _bullets; }
            set { _bullets = value; }
        }

        public override object Update( ) {
            var playerResult = _player.Update( keystate, mouseState, secondsElapsed );
            if (playerResult is Bullet) AddBullet( playerResult as Bullet );
            foreach (var enemy in _enemies) {
                enemy.Update( secondsElapsed, _player );
            }
            MoveBullets( secondsElapsed );
            return null;
        }

        public void AddBullet( Bullet bullet ) {
            _bullets = _bullets.Concat( new Bullet[] { bullet } ).ToArray( );
        }

        public void MoveBullets( double secondsElapsed ) {
            foreach (Bullet bullet in _bullets) bullet.Move( secondsElapsed );
            _bullets = _bullets.Where( bullet => !bullet.Old ).ToArray( );
        }

        public override void Draw( ContentManager contentManager, SpriteBatch spriteBatch, Rectangle screenSize ) {
            base.Draw( contentManager, spriteBatch, screenSize );            
            for (int x = 0; x < AREA_SIZE; x++) {
                for (int y = 0; y < AREA_SIZE; y++) {
                    double blockSize = screenSize.Height / Convert.ToDouble( AREA_SIZE );
                    var destination = new Rectangle( 
                        Convert.ToInt32( MapOffset.X + x * blockSize ), 
                        Convert.ToInt32( y * blockSize), 
                        Convert.ToInt32( blockSize ), 
                        Convert.ToInt32( blockSize ) );
                    _terrain[x, y].Draw( spriteBatch, destination );
                }
            }
            _player.Draw( spriteBatch, MapOffset, BlockSize );
            foreach (var bullet in _bullets) {
                bullet.Draw( spriteBatch );
            }
            var mouse = Mouse.GetState( );
            spriteBatch.Draw( Textures.Get( "target" ), new Vector2( mouse.X, mouse.Y ), null,
                Color.White, 0f, Vector2.One * 100f, 2f * BlockSize / 100f, SpriteEffects.None, 0f );
            
        }

    }

}
