using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakingMission {

    class EnemyCharacter : Character {

        public object Update( double secondsElapsed, PlayerCharacter player ) {
            var vectorToPlayer = player.Position - this.Position;
            var angleToPlayer = Math.Atan2( vectorToPlayer.Y, vectorToPlayer.X ) + Math.PI * 0.5;

            double clockwiseDist, counterDist;
            if (rotation > angleToPlayer) {
                clockwiseDist = Pi2 + angleToPlayer - rotation;
                counterDist = rotation - angleToPlayer;
            } else {
                clockwiseDist = angleToPlayer - rotation;
                counterDist = Pi2 + rotation - angleToPlayer;
            }
            if (clockwiseDist < counterDist) {
                RotateRight( secondsElapsed );
            } else {
                RotateLeft( secondsElapsed );
            }
            return null;
        }

    } // class

} // namespace
