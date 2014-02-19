using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
            const double FrameCount = 60;
            const double FrameTime = 1.0/FrameCount; 

            double timeLast = 0.0;
            double timeStart = 0.0;
            double timeDelta = 0.0;
            double timeAccu = 0.0;
            double interplation = 0.0;

            double fpsTime = 0.0;
            int fpsCount = 0;

            while (m_game.IsRunnig)
            {
                timeStart = Time.GetHighResolutionTime();
                timeDelta = timeStart - timeLast;
                timeLast = timeStart;

                if (timeDelta > 0.25)
                    timeDelta = 0.25;

                timeAccu += timeDelta;
                while (timeAccu >= FrameTime)
                {
                    m_game.Update(FrameTime);
                    timeAccu -= FrameTime;
                }

                interplation = 0.0;

                if (fpsTime + Time.GetOneSecond() < timeStart)
                {
                    Console.Title = string.Format("FPS: {0}", fpsCount);
                    fpsCount = 0;
                    fpsTime = timeStart;
                }

                m_game.Render(interplation);
                fpsCount++;
            }
        }
    }

    internal class Time {
        
        private static readonly long Frequency;

        static Time()
        {
            if (QueryPerformanceFrequency(out Frequency) == false)
            {
                // Frequency not supported
                throw new Win32Exception();
            }
        }

        public static double GetOneSecond()
        {
            return Frequency;
        }

        public static double GetHighResolutionTime()
        {
            long counter;
            QueryPerformanceCounter(out counter);

            return counter/(double)Frequency;
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
        }

        public bool IsRunnig { get; protected set; }

        public void Update(double frameTime)
        {
            
        }

        public void Start()
        {
            IsRunnig = true;
        }

        public void Render(double interplation)
        {           
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
