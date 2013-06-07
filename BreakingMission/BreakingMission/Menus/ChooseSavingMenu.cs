using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakingMission.Menus {
    class ChooseSavingMenu : Menu {
        public override void Draw( Microsoft.Xna.Framework.Content.ContentManager contentManager, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle screenSize ) {
            base.Draw( contentManager, spriteBatch, screenSize );
            DrawMenuLines(
                "Valitse tallennus",
                "1 - Tallennus 1",
                "2 - Tallennus 2",
                "3 - Tallennus 3",
                "Esc - Poistu" );
        }
    }
}
