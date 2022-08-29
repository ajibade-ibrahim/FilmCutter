using System;

namespace FilmCutter
{
    public class Program
    {
        private static void Main()
        {
            const string VideoFileName = @"D:\Matches\Ajax\Ajax vs Feyenoor - 1995\AFC Ajax vs. Feyenoord 1995.mp4";
            const string PeriodsFilename = @"D:\text files\Kanu vs Feyenoord - 1995.txt";

            FilmCutter.CutFile(VideoFileName, PeriodsFilename, 0);

            Console.WriteLine("Finished Cutting Periods");
            Console.ReadLine();
        }
    }
}