using System;
using System.Diagnostics;
using System.Threading;
using ADIPlus.Drawing;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var disp = new ConsoleDisplay(100, 50);
            var game= new SnakeGame();
            var engine = new GameEngine(game, disp);

            engine.Initialize();
            engine.Start();
        }
    }

    public class GameEngine
    {
        private readonly Game m_game;
        private readonly ConsoleDisplay m_display;
        private readonly GameLoop m_gameLoop;

        public GameEngine(Game game, ConsoleDisplay display)
        {
            if (game == null)
                throw new ArgumentNullException("game");
                
            m_game = game;


            if (display == null)
                throw new ArgumentNullException("display");
                
            m_display = display;

            m_gameLoop = new GameLoop(new InterpolatedLoop());
        }

        public void Initialize()
        {
            m_display.Initialize();
            m_game.Initialize();
            m_gameLoop.Initialize(m_game);
        }

        public void Start()
        {
            m_gameLoop.Start();
        }
    }

    internal class SnakeGame : Game
    {
        public SnakeGame()
        {
            
        }
    }

    public class GameObject
    {
        private double x;
        private double y;

        private double ax;
        private double ay;

        private int xVelocity;
        private int yVelocity;
        private double lastX;
        private double lastY;

        //private double xDist;
        //private double yDist;
        

        public GameObject()
        {
            xVelocity = 10;
            yVelocity = 0;
            x = 0;
            y = 25;

            ax = x;
            ay = y;
        }

        public void Update(LoopState state)
        {
            lastX = x;
            lastY = y;
            x += xVelocity * state.Seconds;
            y += yVelocity * state.Seconds;

            HandelCollition();
        }

        private void HandelCollition()
        {
            if (x > 10)
            {
                xVelocity = -xVelocity;
                x = 10;
            }
        }

        public void Render(LoopState state, AsciiGraphics g)
        {
            ax = x;
            ay = y;

            g.DrawPoint(new AsciiPen('O', AsciiColors.Red), new Point((uint)ax, (uint)ay));
        }
    }

    public class Game {

        private Image buffer;
        private AsciiGraphics bufferRender;
        private AsciiGraphics dispRender;        
        
        public void Initialize()
        {
            IsRunnig = false;
            buffer = new Image(100, 50);
            bufferRender = AsciiGraphics.FromCharImage(buffer);
            dispRender = AsciiGraphics.FromUnManagedConsole();

            bufferRender.Clear();
        }

        public bool IsRunnig { get; protected set; }

        GameObject dot = new GameObject();

        public void ReadInput(LoopState state) 
        {}

        public void Update(LoopState state)
        {
            state.Updates();
            bufferRender.Clear();

            dot.Update(state);
        }

        public void Render(LoopState state)
        {
            state.Renderings(); 

            dot.Render(state, bufferRender);                     
            dispRender.DrawImage(buffer);
        }

        public void Start()
        {
            IsRunnig = true;
        }
    }

    public class ConsoleDisplay : Display
    {
        private readonly int m_width;
        private readonly int m_height;

        public override int Width { get { return m_width; } }
        public override int Height { get { return m_height; } }
        
        public ConsoleDisplay(int width, int height)
        {
            m_width = width;
            m_height = height;
        }

        public void Initialize()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = m_width;
            Console.BufferHeight = m_height;
            Console.WindowWidth = m_width;
            Console.WindowHeight = m_height;            
        }
    }

    public abstract class Display
    {
        public abstract int Width { get; }
        public abstract int Height { get;}        
    }
}
