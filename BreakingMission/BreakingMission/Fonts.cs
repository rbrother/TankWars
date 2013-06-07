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

    static class Fonts {

        public static void Load(ContentManager contentManager) {
            bigArial = contentManager.Load<SpriteFont>( "BigArial" );
        }

        private static SpriteFont bigArial;

        public static SpriteFont BigArial { get { return bigArial; } }
 
    }

}
