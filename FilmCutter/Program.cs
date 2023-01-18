using System;

namespace FilmCutter
{
    public class Program
    {
        private static void Main()
        {
            const string VideoFileName = @"D:\Documents\Soccer\Ronaldinho\Ronaldinho ● The Most Skillful Player Ever ● Grêmio.MKV";
            const string PeriodsFilename = @"D:\text files\dinho step.txt";

            FilmCutter.CutFile(VideoFileName, PeriodsFilename, 0);

            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Finished Cutting Periods");
            Console.ReadLine();
        }
    }
}