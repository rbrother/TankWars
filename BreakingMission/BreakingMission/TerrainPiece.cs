using System;
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

    public class TerrainPiece {

        private string _terrainName;

        public TerrainPiece( string terrainName ) {
            _terrainName = terrainName;
        }

        public void Draw( SpriteBatch spriteBatch, Rectangle destination ) {
            spriteBatch.Draw( Textures.Get( "block_" + _terrainName), destination, Color.White );
        }

    }

}
