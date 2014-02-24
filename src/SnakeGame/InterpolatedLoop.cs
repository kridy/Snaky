using System.Threading;

namespace SnakeGame
{
    public class InterpolatedLoop : LoopStrategy
    {
        private const double frameCount = 60;
        private const double frameRate = 1/frameCount;
        private const int maxCount = 25;
        private const bool yield = true;

        private double accumulatedTime = 0.0;
        private int updateCount = 0;
        
        public override void Loop(Game game, LoopState state)
        {
            state.Tick();
            accumulatedTime += state.Seconds;
            updateCount = 0;

            while (accumulatedTime >= frameRate && updateCount < maxCount)
            {
                game.Update(state);
                updateCount++;
                accumulatedTime = accumulatedTime - frameRate;
            }
            
            game.Render(state);

            Thread.Yield();
        }
    }
}