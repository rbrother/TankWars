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

    public static class GameContent {

        private static Dictionary<string, Texture2D> _textures =
            new Dictionary<string, Texture2D>( );
        private static Dictionary<string, SoundEffect> _sounds =
            new Dictionary<string, SoundEffect>( );
        private static Dictionary<string, Song> _songs =
            new Dictionary<string, Song>();

        private static ContentManager _content;

        public static void Load( ContentManager content ) {
            _content = content;
        }

        public static Texture2D GetTexture( string name ) {
            if (!_textures.ContainsKey( name )) {
                _textures[name] = _content.Load<Texture2D>( name );
            }
            return _textures[name];
        }

        public static SoundEffect GetSound( string name ) {
            if (!_sounds.ContainsKey( name )) {
                _sounds[name] = _content.Load<SoundEffect>( name );
            }
            return _sounds[name];
        }

        public static Song GetSong(string name) {
            if (!_songs.ContainsKey(name)) {
                _songs[name] = _content.Load<Song>(name);
            }
            return _songs[name];
        }

    }

}
