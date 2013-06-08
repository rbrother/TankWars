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

    public static class Textures {

        private static Dictionary<string, Texture2D> _textures =
            new Dictionary<string, Texture2D>( );

        private static ContentManager _content;

        public static void Load( ContentManager content ) {
            _content = content;
        }

        public static Texture2D Get( string name ) {
            if ( !_textures.ContainsKey(name) ) {
                _textures[name] = _content.Load<Texture2D>(name);
            }
            return _textures[name];
        }

    }

}
