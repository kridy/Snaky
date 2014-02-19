using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ADIPlus.Drawing;

namespace Game
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

    internal class GameEngine
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

            m_gameLoop = new GameLoop();
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

    internal class GameLoop {
        private Game m_game;

        public void Initialize(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            m_game = game;
        }

        public void Start()
        {
            m_game.Start();
            const double frameCount = 60.0;
           const double frameTime = 1.0e9 / frameCount;

            double timeLast = Time.GetNanoSeconds();
            double timeStart = 0.0;
            double timeDelta = 0.0;
            double timeAccu = 0.0;
            double interplation = 0.0;

            double fpsTime = Time.GetNanoSeconds();
            int fpsCount = 0;
            int updCounter = 0;

            while (m_game.IsRunnig)
            {
                timeStart = Time.GetNanoSeconds();
                timeDelta = timeStart - timeLast;
                timeLast = timeStart;

                if (timeDelta > 0.25)
                    timeDelta = 0.25;

                timeAccu += timeDelta;
                while (timeAccu >= frameTime)
                {
                    m_game.Update(frameTime);
                    timeAccu -= frameTime;
                    updCounter++;
                }
                // soh cah toa
                interplation = 0.0;

                if (timeStart > (fpsTime + 1.0e9 ))
                {
                    Console.Title = string.Format("FPS: {0} UPD: {1}", fpsCount, updCounter);
                    updCounter = 0;
                    fpsCount = 0;
                    fpsTime = timeStart;
                }

                m_game.Render(interplation);
                fpsCount++;
            }
        }
    }

    internal class Time {

        private static readonly long frequency;
        private static readonly double multiplier = 1.0e9;
        static Time()
        {
            if (QueryPerformanceFrequency(out frequency) == false)
            {
                // Frequency not supported
                throw new Win32Exception();
            }
        }

        public static double GetNanoSeconds(int iterations)
        {
            long counter;

            QueryPerformanceCounter(out counter);

            return ((((double)counter * (double)multiplier) / (double)frequency) / iterations);
        }

        public static double GetNanoSeconds()
        {
            return GetNanoSeconds(1);
        }

        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

    }

    internal class SnakeGame : Game
    {
        public SnakeGame()
        {
            
        }
    }

    internal class Game {
        public void Initialize()
        {
            IsRunnig = false;
            bufferRender = AsciiGraphics.FromCharImage(buffer);
            dispRender = AsciiGraphics.FromUnManagedConsole();
        }

        public bool IsRunnig { get; protected set; }

        public void Update(double frameTime)
        {
            
        }

        public void Start()
        {
            IsRunnig = true;
        }

        private static Image buffer = new Image(100,50);
        private AsciiGraphics bufferRender;
        private AsciiGraphics dispRender;


        public void Render(double interplation)
        {
            for (uint i = 0; i < buffer.Height; i++)
            {
                bufferRender.DrawHorizontalLine(new AsciiPen('*', AsciiColors.Yellow),0,i, buffer.Width);
            }
            dispRender.DrawImage(buffer);
        }
    }

    internal class ConsoleDisplay : Display
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

    internal abstract class Display
    {
        public abstract int Width { get; }
        public abstract int Height { get;}
    }
}
