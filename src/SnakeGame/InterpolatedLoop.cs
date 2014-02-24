using System.Diagnostics;
using System.Threading;

namespace SnakeGame
{
    public class InterpolatedLoop : LoopStrategy
    {
        private const double frameCount = 60;
        private const double frameRate = (1/frameCount) * 1000;
        private const int maxCount = 25;
        private const bool yield = true;

        private double accumulatedTime = 0.0;
        private int updateCount = 0;
        
        public override void Loop(Game game, LoopState state)
        {
            state.Tick();
            accumulatedTime += state.NanoSeconds;
            updateCount = 0;

            while (accumulatedTime >= frameRate && updateCount < maxCount)
            {
                game.Update(state);
                updateCount++;
                accumulatedTime -= frameRate;
            }

            state.Interpolation = accumulatedTime/frameRate;

            game.Render(state);

            //var sleepTime = (frameRate - state.MilliSeconds);

            //if(sleepTime > 0)
            //    Thread.Sleep((int)sleepTime);
        }
    }
}