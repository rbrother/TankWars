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

    public class BreakingMissionGame : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Stack<Screen> ScreenStack = new Stack<Screen>( );

        public BreakingMissionGame( ) {
            graphics = new GraphicsDeviceManager( this );
            graphics.IsFullScreen = true;
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
        }

        private Rectangle ScreenSize {
            get {
                return new Rectangle(
                    0, 0,
                    GraphicsDevice.DisplayMode.Width,
                    GraphicsDevice.DisplayMode.Height );
            }
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
            Fonts.Load( Content );
            GameContent.Load( Content );
            ScreenStack.Push( new MainMenu( ) );
            // TODO: use this.Content to load your game content here
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

            object result = CurrentScreen.UpdateOuter( gameTime.ElapsedGameTime.TotalSeconds );
            if (result != null) {
                if (result.Equals( "ReturnHigherLevel" )) {
                    ScreenStack.Pop( );
                    if (ScreenStack.Count == 0) Exit( );
                }
                if (result is Screen) {
                    ScreenStack.Push( result as Screen );
                }
            }

            base.Update( gameTime );


        }

        private Screen CurrentScreen { get { return ScreenStack.Peek( ); } } 


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime ) {
            GraphicsDevice.Clear( Color.Black );
            spriteBatch.Begin( );
            // TODO: Add your drawing code here
            CurrentScreen.Draw( Content, spriteBatch, ScreenSize );

            spriteBatch.End( );
            base.Draw( gameTime );
        }
    }
}
