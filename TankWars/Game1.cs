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

    public enum TerrainType {
        Wood,
        Stone
    }

    public class TerrainPiece {
        public TerrainType type;
        public Texture2D texture;
        public Vector2 location;
        public int hitPoints;
    }

    public class BlockLocationComparer : IEqualityComparer<TerrainPiece> {
        public bool Equals( TerrainPiece a, TerrainPiece b ) {
            return a.location == b.location;
        }
        public int GetHashCode( TerrainPiece a ) {
            return a.location.GetHashCode( );
        }
    }

    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D _tankTexture;
        Texture2D _bulletTexture;
        Texture2D _stoneTexture;
        Texture2D _woodTexture;
        float _secondsElapsed;
        float _tankRotation = 0.0f;
        Vector2 _tankPosition = new Vector2( 0.0f, 0.0f );
        IEnumerable<Tuple<Vector2, Vector2>> _bullets = new List<Tuple<Vector2, Vector2>>();
        IEnumerable<TerrainPiece> _terrain;
        float _rotationSpeed = 2.0f;
        float _bulletSpeed = 2.0f;
        MouseState _previousMouseState = new MouseState();
        Random rand = new Random( );

        public Game1( ) {
            graphics = new GraphicsDeviceManager( this );
            graphics.IsFullScreen = true;
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize( ) {
            base.Initialize( );
        }

        private TerrainPiece RandomTerrain( ) {
            var typ = rand.Next( 0, 2 ) == 0 ? TerrainType.Wood : TerrainType.Stone;
            return new TerrainPiece {
                type = typ,
                texture = typ == TerrainType.Wood ? _woodTexture : _stoneTexture,
                hitPoints = typ == TerrainType.Wood ? 2 : 10,
                location = new Vector2( ( rand.Next( 1, 20 ) - 10 ) * 66, ( rand.Next( 1, 20 ) - 10 ) * 66 )
            };
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent( ) {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( GraphicsDevice );

            _tankTexture = this.Content.Load<Texture2D>( "tank" );
            _bulletTexture = this.Content.Load<Texture2D>( "bullet" );
            _stoneTexture = this.Content.Load<Texture2D>( "Block_tile" );
            _woodTexture = this.Content.Load<Texture2D>( "Block_wood" );

            _terrain = Enumerable.Range( 1, 100 )
                .Select( i => RandomTerrain( ) )
                .ToArray( )
                .Distinct( new BlockLocationComparer( ) )
                .ToArray( );
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent( ) {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update( GameTime gameTime ) {
            // Allows the game to exit
            if (GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed)
                this.Exit( );
            _secondsElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboard = Keyboard.GetState( );
            if (keyboard.IsKeyDown( Keys.A )) {
                _tankRotation -= _rotationSpeed * _secondsElapsed; 
            } else if (keyboard.IsKeyDown( Keys.D )) {
                _tankRotation += _rotationSpeed * _secondsElapsed; 
            }
            if (keyboard.IsKeyDown( Keys.W ) || keyboard.IsKeyDown( Keys.S )) {
                var move = Vector2.Transform( -Vector2.UnitY, TankRotationMatrix ) * _secondsElapsed * 100.0f;
                move = move * ( keyboard.IsKeyDown( Keys.W ) ? 1.0f : -1.0f );

                // Check collisions
                var closeBlocks = _terrain
                    .Where( block => Vector2.DistanceSquared( block.location, _tankPosition ) < 100 * 100 );
                var stopDistance = 66f;
                foreach (TerrainPiece block in closeBlocks) {
                    if (Math.Abs( _tankPosition.X - block.location.X ) < stopDistance - 2 ) {
                        if ( ( _tankPosition.Y > block.location.Y &&
                            _tankPosition.Y < block.location.Y + stopDistance &&
                            move.Y < 0 ) ||
                           (_tankPosition.Y < block.location.Y &&
                            _tankPosition.Y > block.location.Y - stopDistance &&
                            move.Y > 0) ) move = new Vector2( move.X, 0f );
                    }
                    if (Math.Abs( _tankPosition.Y - block.location.Y ) < stopDistance - 2 ) {
                        if ( ( _tankPosition.X > block.location.X &&
                            _tankPosition.X < block.location.X + stopDistance &&
                            move.X < 0) || 
                            (_tankPosition.X < block.location.X &&
                            _tankPosition.X > block.location.X - stopDistance &&
                            move.X > 0) ) move = new Vector2( 0f, move.Y );
                    }
                }

                _tankPosition += move;                
            }
            var mouseState = Mouse.GetState( );
            var leftClick = mouseState.LeftButton == ButtonState.Pressed && 
                _previousMouseState.LeftButton == ButtonState.Released;
            if (leftClick) {
                var velocity = Vector2.Transform( -Vector2.UnitY, TankRotationMatrix );
                var bullet = Tuple.Create( _tankPosition + velocity * 55.0f, velocity );
                _bullets = _bullets.Concat( new Tuple<Vector2, Vector2>[] { bullet } );
            }

            _bullets = _bullets
                .Select( bullet => MoveBullet(bullet) )
                .Where( bullet => Vector2.Distance( _tankPosition, bullet.Item1 ) < 1000 )
                .ToArray();
            
            _previousMouseState = mouseState;
            base.Update( gameTime );
        }

        private Tuple<Vector2, Vector2> MoveBullet( Tuple<Vector2, Vector2> bullet ) {
            return new Tuple<Vector2, Vector2>(
                bullet.Item1 + bullet.Item2 * _secondsElapsed * 300.0f * _bulletSpeed,
                bullet.Item2
                );
        }

        private Matrix TankRotationMatrix { get { return Matrix.CreateRotationZ( _tankRotation ); } }

        private Vector2 ScreenCenter {
            get {
                return new Vector2( GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 );
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime ) {
            GraphicsDevice.Clear( Color.CornflowerBlue );
            spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise );

            spriteBatch.Draw( _tankTexture, ScreenCenter, null, Color.White, _tankRotation, 
                new Vector2(184.0f,184.0f), 0.2f, SpriteEffects.None, 0.0f);

            var objectOffset = ScreenCenter - _tankPosition;

            foreach( Tuple<Vector2,Vector2> bullet in _bullets) {
                spriteBatch.Draw( _bulletTexture, bullet.Item1 + objectOffset, null, Color.White, 0.0f,
                    new Vector2( 16.0f, 16.0f ), 0.3f, SpriteEffects.None, 0.0f );
            }
            foreach (TerrainPiece block in _terrain) {
                spriteBatch.Draw( block.texture, block.location + objectOffset /* block.location */ , 
                    null, Color.White, 0.0f,
                    new Vector2( 34.0f, 34.0f ), 1.0f, SpriteEffects.None, 0.0f );
            }
            spriteBatch.End( );
            base.Draw( gameTime );
        }
    }
}
