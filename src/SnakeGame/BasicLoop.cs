using System;
using System.Diagnostics;

namespace SnakeGame
{
    public class BasicLoop : LoopStrategy 
    {
        public override void Loop(Game game, LoopState state)
        {
            state.Tick();
            game.Update(state);
            game.Render(state);
        }
    }
}