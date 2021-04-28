using System;
using Controller;
using System.Threading;

namespace RaceSim
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Data.Initialize();
            Console.WriteLine(Data.CurrentRace.Track.Name);
            Data.CurrentRace.Start();
            Visualization.Initialize();
            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}