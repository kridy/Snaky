namespace SnakeGame
{
    public class LoopState
    {
        private double m_start;
        private double m_last;
        private double m_delta;

        private double m_updateTime;
        private double m_renderingTime;

        private int m_updates;
        private int m_renderings;

        /// <summary>
        /// Elapsed time in seconds
        /// </summary>
        public double Seconds;
        /// <summary>
        /// Elapsed time in milliseconds
        /// </summary>
        public double MilliSeconds;
        /// <summary>
        /// Elapsed time in microseconds
        /// </summary>
        public double MicroSeconds;
        /// <summary>
        /// Elapsed time in nanoseconds
        /// </summary>
        public double NanoSeconds;        
        /// <summary>
        /// Interpolation factor between rendering
        /// </summary>
        public double Interpolation;        
        /// <summary>
        /// Updates pr. second.
        /// </summary>
        public int Ups;
        /// <summary>
        /// Renderings pr. seconds.
        /// </summary>
        public int Rps;                
        /// <summary>
        /// Updates Ups
        /// </summary>
        internal void Updates()
        {
            if ((m_updateTime + 1.0e9) <= m_start)
            {
                Ups = m_updates;
                m_updateTime = m_start;
                m_updates = 0;
                return;
            }

            m_updates++;
        }
        /// <summary>
        /// Updates Rps
        /// </summary>
        internal void Renderings()
        {
            if ((m_renderingTime + 1.0e9) <= m_start)
            {
                Rps = m_renderings ;
                m_renderingTime = m_start;
                m_renderings = 0;
                return;
            }

            m_renderings++;
        }
        /// <summary>
        /// Updates Seconds, MilliSeconds MicroSeconds and NanoSeconds
        /// </summary>
        public void Tick()
        {
            m_start = Time.GetNanoSeconds();
            m_delta = m_start - m_last;
            m_last = m_start;

            NanoSeconds = m_delta;
            MicroSeconds = m_delta / 1000.0;
            MilliSeconds = m_delta / 1000000.0;
            Seconds = m_delta / 1.0e-9;

        }
        /// <summary>
        /// Initialize the LoopState
        /// </summary>
        public void Initialize()
        {
            m_updateTime    = Time.GetNanoSeconds();
            m_renderingTime = Time.GetNanoSeconds();
            m_last          = Time.GetNanoSeconds();
            m_start         = 0.0;
            m_delta         = 0.0;           
            m_updates       = 0;
            m_renderings    = 0;

            Ups             = 0;
            Rps             = 0;
            Seconds         = 0.0;
            MilliSeconds    = 0.0;
            MicroSeconds    = 0.0;
            NanoSeconds     = 0.0;
            Interpolation   = 0.0;
        }        
    }
}