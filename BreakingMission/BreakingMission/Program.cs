using System;

namespace BreakingMission {
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BreakingMissionGame game = new BreakingMissionGame())
            {
                game.Run();
            }
        }
    }
#endif
}

