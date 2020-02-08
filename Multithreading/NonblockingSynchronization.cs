using System;
using System.Threading;

namespace Multithreading
{
    public class NonblockingSynchronization
    {
        int _answer;
        bool _complete;

        public static void Start(string[] args)
        {
            var nbs = new NonblockingSynchronization();
            new Thread(nbs.A).Start();
            nbs.B();
        }
 
        void A()
        {
            _complete = true;
            _answer = 123;
        }
 
        void B()
        {
            if (_complete) Console.WriteLine (_answer);
        }
    }
}