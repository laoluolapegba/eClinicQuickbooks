using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace FCQBWebConnAPI.Services
{
   
    public class SessionStateManager

    {

        int counter;
        public SessionStateManager() { }

        public int Counter {
            get { return this.counter; }
            set { this.counter = value; }
        }

        public int Touch() { return Interlocked.Increment(ref this.counter); }

    }
}