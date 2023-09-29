using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace FilmCutter
{
    internal class FilmCutter
    {
        internal static void CutFile(string videoFileName, string periodsFilename, int startIndex, string dimensions = "")
        {
            if (string.IsNullOrWhiteSpace(videoFileName))
            {
                throw new ArgumentException(nameof(videoFileName));
            }

            if (string.IsNullOrWhiteSpace(periodsFilename))
            {
                throw new ArgumentException(nameof(periodsFilename));
            }

            GetPeriodsFromFile(periodsFilename)
                .Select(period => new OutputFile(period, GenerateOutputFileName(videoFileName, startIndex++)))
                .ToList()
                .ForEach(fileSpan => CutMedia(videoFileName, fileSpan, dimensions));
        }

        private static string GenerateOutputFileName(string videoFileName, int index)
        {
            var fileName = videoFileName.Split(
                new[]
                {
                    ".mp4"
                },
                StringSplitOptions.None)[0];

            var outputFileName = $"{fileName}{"~" + (index + 1)}.mp4";
            return outputFileName;
        }

        private static void CutMedia(string inputFileName, OutputFile outputFile, string dimensions = "")
        {
            try
            {
                // period: 00:12:23 - 01:45:45
                var inputMediaFile = new MediaFile
                {
                    Filename = inputFileName
                };

                var outputMediaFile = new MediaFile
                {
                    Filename = outputFile.FileName
                };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputMediaFile);

                    var options = new ConversionOptions();

                    // This example will create a 25 second video, starting from the
                    // 30th second of the original video.
                    //// First parameter requests the starting frame to cut the media from.
                    //// Second parameter requests how long to cut the video.
                    options.CutMedia(outputFile.Start, outputFile.GetSpan());
                    options.VideoSize = VideoSize.Hd720;
                    options.VideoAspectRatio = VideoAspectRatio.R16_9;
                    if (!string.IsNullOrWhiteSpace(dimensions))
                    {
                        options.SourceCrop = GetCropRectangle(dimensions);
                    }

                    engine.CustomCommand("-deinterlace");
                    Console.WriteLine("Cutting and converting period {0}", outputFile);
                    engine.Convert(inputMediaFile, outputMediaFile, options);
                    Console.WriteLine("Done cutting period {0}", outputFile);
                }
            }
            catch (IndexOutOfRangeException indexOutOfRangeException)
            {
                Console.WriteLine($"{indexOutOfRangeException}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{exception}");
            }
        }

        private static CropRectangle GetCropRectangle(string dimensions)
        {
            // 0160:0004:0956:0712
            var array = dimensions.Split(':');
            if (array.Length != 4)
            {
                throw new ArgumentException($"Crop dimensions({dimensions}) not up to 4 elements. {array.Length} found.");
            }
            
            return new CropRectangle
            {
                X = Convert.ToInt32(array[0]),
                Y = Convert.ToInt32(array[1]),
                Width = Convert.ToInt32(array[2]),
                Height = Convert.ToInt32(array[3])
            };
        }

        private static IEnumerable<string> GetPeriodsFromFile(string periodsFilename)
        {
            using (var reader = new StreamReader(periodsFilename))
            {
                // 00:12:34 - 01:23:13
                var fileContent = reader.ReadToEnd();
                Console.WriteLine("File content");
                Console.WriteLine("------------");
                var periods = fileContent.Split(';').Where(period => !string.IsNullOrWhiteSpace(period)).ToList();
                periods.ForEach(period => Console.WriteLine("Period: {0}", period.Trim()));

                return periods;
            }
        }
    }
}