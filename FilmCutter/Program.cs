using System;
using System.IO;
using System.Linq;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace FilmCutter
{
    public class Program
    {
        private static void CutFile(string filename, string period, int index)
        {
            try
            {
                // period: 00:12:23 - 01:45:45
                var inputFile = new MediaFile
                {
                    Filename = filename
                };

                var outputFileName = filename.Split(
                    new[]
                    {
                        ".mp4"
                    },
                    StringSplitOptions.None)[0];

                var outputFile = new MediaFile
                {
                    Filename = $"{outputFileName}{"~" + (index + 11)}.mp4"
                };

                var t1 = period.Split('-')[0];
                Console.WriteLine("Time t1 : {0}", t1);

                var t2 = period.Split('-')[1];
                Console.WriteLine("Time t2 : {0}", t2);

                var span1 = GetTimeSpan(t1.Trim());
                var span2 = GetTimeSpan(t2.Trim());

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);

                    var options = new ConversionOptions();

                    // This example will create a 25 second video, starting from the
                    // 30th second of the original video.
                    //// First parameter requests the starting frame to cut the media from.
                    //// Second parameter requests how long to cut the video.
                    var span3 = span2 - span1;
                    options.CutMedia(span1, span3);
                    options.VideoSize = VideoSize.Hd720;
                    options.VideoAspectRatio = VideoAspectRatio.R16_9;
                    engine.CustomCommand("-deinterlace");
                    Console.WriteLine("Cutting and converting period {0}", period);
                    engine.Convert(inputFile, outputFile, options);
                    Console.WriteLine("Done cutting period {0}", period);
                }
            }
            catch (IndexOutOfRangeException indexOutOfRangeException)
            {
                Console.WriteLine($"{indexOutOfRangeException.Message}:{indexOutOfRangeException.StackTrace}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{exception.Message}:{exception.StackTrace}");
            }
        }

        private static int GetNumber(string seconds)
        {
            try
            {
                return string.IsNullOrWhiteSpace(seconds) ? 0 : Convert.ToInt32(seconds);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{exception.Message}{'-'}{exception.StackTrace}");
            }

            return 0;
        }

        private static string[] GetPeriodsFromFile(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                // 00:12:34 - 01:23:13
                var fileContent = reader.ReadToEnd();
                Console.WriteLine("File content: {0}", fileContent);
                var periods = fileContent.Split(';');

                foreach (var item in periods)
                {
                    Console.WriteLine("Period: {0}", item);
                }

                return periods;
            }
        }

        private static TimeSpan GetTimeSpan(string time)
        {
            var timeParts = time.Split(':').Reverse().ToList();
            var seconds = GetNumber(timeParts.ElementAtOrDefault(0));
            var minutes = GetNumber(timeParts.ElementAtOrDefault(1));
            var hours = GetNumber(timeParts.ElementAtOrDefault(2));

            return new TimeSpan(hours, minutes, seconds);
        }

        private static void Main()
        {
            const string VideoFileName =
                @"D:\Documents\Soccer\Okocha\Batch 5\Fenerbahce vs Manchester United\Fenerbahce vs Manchester United.mp4";
            const string PeriodsFilename = @"D:\text files\Jay Jay Feb vs ManU.txt";

            var periods = GetPeriodsFromFile(PeriodsFilename);

            foreach (var period in periods)
            {
                Console.WriteLine("Abt to cut period {0}", period);
                CutFile(VideoFileName, period, periods.ToList().IndexOf(period));
            }

            Console.WriteLine("Finished Cutting Periods".GroupBy(x => x));
            Console.ReadLine();
        }
    }
}