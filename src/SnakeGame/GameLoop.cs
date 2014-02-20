using System;
using System.Diagnostics;

namespace SnakeGame
{
    internal class GameLoop {
        private readonly LoopStrategy m_strategy;
        private Game m_game;
        private LoopState m_loopState;

        public GameLoop(LoopStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException("strategy");
				
            m_strategy = strategy;
            m_loopState = new LoopState();
        }

        public void Initialize(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            
            m_game = game;

            m_loopState.Initialize();
        }

        public void Start()
        {
            m_game.Start();           

            while (m_game.IsRunnig)
            {      
                m_strategy.Loop(m_game, m_loopState);
                Debug.WriteLine("UPS:{0} - RPS:{1}",
                                        m_loopState.Ups,
                                        m_loopState.Rps);
            }
        }
    }
}