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

    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D _tankTexture;
        Texture2D _bulletTexture;
        float _secondsElapsed;
        float _tankRotation = 0.0f;
        Vector2 _tankPosition = new Vector2( 200.0f, 200.0f );
        IEnumerable<Tuple<Vector2, Vector2>> _bullets = new List<Tuple<Vector2, Vector2>>();
        float _rotationSpeed = 2.0f;
        float _bulletSpeed = 2.0f;
        MouseState _previousMouseState = new MouseState();

        public Game1( ) {
            graphics = new GraphicsDeviceManager( this );
            graphics.IsFullScreen = false;
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
            // TODO: Add your initialization logic here

            base.Initialize( );
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
            // TODO: use this.Content to load your game content here
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
                .Where( bullet => Vector2.Distance( _tankPosition, bullet.Item1 ) < 10004 );
            
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime ) {
            GraphicsDevice.Clear( Color.CornflowerBlue );
            spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise );

            spriteBatch.Draw( _tankTexture, _tankPosition, null, Color.White, _tankRotation, 
                new Vector2(184.0f,184.0f), 0.3f, SpriteEffects.None, 0.0f);
            foreach( Tuple<Vector2,Vector2> bullet in _bullets) {
                spriteBatch.Draw( _bulletTexture, bullet.Item1, null, Color.White, 0.0f,
                    new Vector2( 16.0f, 16.0f ), 0.3f, SpriteEffects.None, 0.0f );
            }
            spriteBatch.End( );
            base.Draw( gameTime );
        }
    }
}
