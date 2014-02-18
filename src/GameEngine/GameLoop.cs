using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class GameEngine
    {
        private readonly Game m_game;
        private readonly GameLoop m_gameLoop;

        public GameEngine(Game game)
            :this(game, )

        public GameEngine(Game game, GameLoop gameLoop)
        {

            if (game == null)
                throw new ArgumentNullException("game");
				
            m_game = game;


            if (gameLoop == null)
                throw new ArgumentNullException("gameLoop");				

            m_gameLoop = gameLoop;
        }

        public void Initailize() {}
        public void Start() {}
        public void Stop() {}
    }

    public abstract class Game {}

    public abstract class GameLoop
    {
        private Game _game;

        public void Initialize(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            _game = game;
        }

        public void Start()
        {
            RunLoop();
        }

        public void Stop() {}
        public abstract void RunLoop();
    }

    class BasicGameLoop : GameLoop {
        public override void RunLoop()
        {
            
        }
    }
}
