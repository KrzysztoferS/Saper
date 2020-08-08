using Saper.Intrfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Classes
{
    class StopWatch : ITime
    {
        public event EventHandler TimePassed;

        private bool isTimerOn = false;
        private int timeThreshold = 1000;
        private int sleepTime = 100;

        public int TimeThreshold { get
            {
                return timeThreshold;
            }
            set
            {
                if(value>0)
                timeThreshold = value;
            } }

        public int SleepTime { get
            {
                return sleepTime;
            }
            set
            {
                if (value > 0 && value < timeThreshold)
                    timeThreshold = value;
            }
        }

        //metoda startuje licznik i po osiagnieciu limitu czasu wywoluje event TimePassed;
        public void Start()
        {
            isTimerOn = true;
            int timePassed = 0;

            while (isTimerOn)
            {
                if (timePassed > timeThreshold)
                {
                    timePassed = 0;
                    TimePassed?.Invoke(this, new EventArgs());
                }

                System.Threading.Thread.Sleep(sleepTime);
                timePassed += sleepTime;
            }
        }

        public void Stop()
        {
            isTimerOn = false;
        }
    }
}
