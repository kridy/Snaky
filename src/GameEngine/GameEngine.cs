using System;

namespace GameEngine
{
    public class GameEngine
    {
        private readonly Game m_game;
        private readonly GameLoop m_gameLoop;

        public GameEngine(Game game)
        {
            if (game == null)
                throw new ArgumentNullException("game");
				
            m_game = game;
            m_gameLoop = new GameLoop();
        }

        public void Initailize()
        {
            m_game.Initialize();
            m_gameLoop.Initialize(m_game);
        }
        public void Start()
        {
            m_gameLoop.Start();
        }
        public void Stop()
        {
            m_gameLoop.Stop();
        }
    }
}