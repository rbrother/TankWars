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

    public class Explosion {
        public Vector2 Position;
        public TimeSpan StartTime;
        private double _age;
                
        private static Texture2D texture;

        public static void Load( ContentManager content ) {
            texture = content.Load<Texture2D>( "explosion" );
        }

        public Explosion( Bullet bullet, TimeSpan now ) {
            Position = bullet.Position;
            StartTime = now;
        }

        public void Update( TimeSpan gameTime ) {
            _age = gameTime.TotalSeconds - StartTime.TotalSeconds;
        }

        private float Magnitude { get { return (float)Math.Max( 1.0 - _age * 2.0, 0.0 ); } }

        public bool Ended { get { return Magnitude < 0.1f; } }

        private Color CurrentColor {
            get {
                return new Color( Magnitude, Magnitude, Magnitude, Magnitude );
            }
        }

        public void Draw( SpriteBatch spriteBatch, Vector2 objectOffset ) {
            spriteBatch.Draw( texture, Position + objectOffset, null, CurrentColor, 0.0f,
                new Vector2( 40.0f, 40.0f ), 1.0f, SpriteEffects.None, 0.0f );
        }
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
        Texture2D _stoneTexture, _woodTexture;
        Texture2D _backgroundTexture;
        float _secondsElapsed;
        TimeSpan _gameTime;

        PlayerTank playerTank;
        EnemyTank enemyTank;

        Explosion[] _explosions = new Explosion[] { };

        TerrainPiece[] _terrain;
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
                hitPoints = typ == TerrainType.Wood ? 2 : 5,
                location = RandomPosition() * TerrainPiece.BLOCK_SIZE
            };
        }

        private Vector2 RandomPosition( ) {
            var pos = new Vector2( rand.Next( 1, 20 ) - 10, rand.Next( 1, 20 ) - 10 );
            return pos.LengthSquared() > 3.0 ? pos : RandomPosition( );
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent( ) {
            spriteBatch = new SpriteBatch( GraphicsDevice );
            Bullet.Load( this.Content );
            playerTank = new PlayerTank( );
            enemyTank = new EnemyTank( );
            Tank.Texture = this.Content.Load<Texture2D>( "tank" );
            _stoneTexture = this.Content.Load<Texture2D>( "Block_tile" );
            _woodTexture = this.Content.Load<Texture2D>( "Block_wood" );
            _backgroundTexture = this.Content.Load<Texture2D>( "sand" );
            Explosion.Load( this.Content );

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
            this.Content.Unload( );
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
            _gameTime = gameTime.TotalGameTime;
            var keyboard = Keyboard.GetState( );
            var mouseState = Mouse.GetState( );

            playerTank.Update( keyboard, mouseState, _secondsElapsed, _terrain );
            playerTank.MoveBullets( _secondsElapsed );
            playerTank.Bullets = CheckBulletHits( playerTank.Bullets );

            enemyTank.Update( _secondsElapsed, playerTank );

            foreach( Explosion explosion in _explosions ) explosion.Update( _gameTime );
            _explosions = _explosions.Where( explosion => !explosion.Ended ).ToArray();

            _previousMouseState = mouseState;
            base.Update( gameTime );
        }

        private Bullet[] CheckBulletHits( Bullet[] bullets ) {
            var hitBullets = bullets.Where( bullet => BulletHitsBlock( bullet ) != null );

            if (hitBullets.Count( ) > 0) {
                _explosions = _explosions.
                    Concat( hitBullets.Select( bullet => new Explosion( bullet, _gameTime ) ) ).ToArray( );
                foreach (TerrainPiece terrain in hitBullets.Select( bullet => BulletHitsBlock( bullet ) )) {
                    terrain.TakeHit( );
                }
                _terrain = _terrain.Where( piece => !piece.Destroyed ).ToArray( );
            }
            return bullets.Except( hitBullets ).ToArray( );
        }

        private TerrainPiece BulletHitsBlock( Bullet bullet ) {
            var blocks = _terrain.Where( block => block.HitsBullet( bullet.Position ) );
            if (blocks.Count( ) > 0) {
                return blocks.First( );
            } else {
                return null;
            }
        }

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
            GraphicsDevice.Clear( Color.Black );
            spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise );
            var objectOffset = ScreenCenter - playerTank.Position;
            spriteBatch.Draw( _backgroundTexture, objectOffset, null, Color.White, 0.0f,
                    new Vector2( 700f, 500f ), 1.0f, SpriteEffects.None, 0.0f );

            playerTank.Draw( spriteBatch, objectOffset, Color.White );
            enemyTank.Draw( spriteBatch, objectOffset, new Color(255,180,180));

            foreach (TerrainPiece block in _terrain) {
                spriteBatch.Draw( block.texture, block.location + objectOffset /* block.location */ , 
                    null, Color.White, 0.0f,
                    new Vector2( 34.0f, 34.0f ), 1.0f, SpriteEffects.None, 0.0f );
            }
            foreach (Explosion explosion in _explosions) {
                explosion.Draw( spriteBatch, objectOffset );
            }
            spriteBatch.End( );
            base.Draw( gameTime );
        }
    }
}
