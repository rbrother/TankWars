using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakingMission.Menus {

    class SettingsMenu : Menu {

        public override void Draw( Microsoft.Xna.Framework.Content.ContentManager contentManager, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle screenSize ) {
            base.Draw( contentManager, spriteBatch, screenSize );
            DrawMenuLines(
                "Asetukset",
                "1 - Näppäimistöohjaus",
                "2 - Hiiriohjaus",
                "Esc - Poistu" );
        }

    }

}
