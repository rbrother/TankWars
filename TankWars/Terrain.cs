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
        public static int BLOCK_SIZE = 66;

        public TerrainType type;
        public Texture2D texture;
        public Vector2 location;
        public int hitPoints;

        public bool HitsBullet( Vector2 bulletLocation ) {
            return Math.Abs( this.location.X - bulletLocation.X ) < BLOCK_SIZE * 0.5f &&
                Math.Abs( this.location.Y - bulletLocation.Y ) < BLOCK_SIZE * 0.5f;
        }

        internal void TakeHit( ) {
            hitPoints = hitPoints - 1;
        }

        public bool Destroyed { get { return hitPoints <= 0; } }
    }
}
