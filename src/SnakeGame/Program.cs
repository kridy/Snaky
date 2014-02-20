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

        private Point position = new Point(0,25);
        private double velocity = 2;

        double x = 0;

        public void Update(LoopState state)
        {
            state.Updates();

            
            //for (uint i = 0; i < buffer.Height; i++)
            //{
            //    bufferRender.DrawHorizontalLine(new AsciiPen('O', AsciiColors.Red), 0, i, buffer.Width);
            //}


            x += (position.X) + (velocity*((double)1/(double)60));
           ;            ;
            position = new Point((uint)x, position.Y);
            Debug.WriteLine(position.X + "," + position.Y);

            bufferRender.DrawPoint(new AsciiPen('O', AsciiColors.Red), position);

            if (position.X >= 100 || position.X <= 0)
            {
                velocity *= -1;
            }
        }

        public void Render(LoopState state)
        {
            state.Renderings();

            
            
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
