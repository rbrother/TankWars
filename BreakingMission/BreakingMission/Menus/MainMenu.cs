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

    class MainMenu : Menu {

        public override void Draw( ContentManager contentManager, SpriteBatch spriteBatch, Rectangle screenSize ) {
            base.Draw( contentManager, spriteBatch, screenSize );
            DrawMenuLines(
                "Breaking Mission",
                "1 - Aloita",
                "2 - Asetukset",
                "Esc - Poistu" );
        }

        protected override object NumberKeyPressed( int number ) {
            if (number == 1) {
                return new Menus.ChooseSavingMenu( );
            } else if (number == 2) {
                return new Menus.SettingsMenu( );
            }
            return null;
        }

    }

}
