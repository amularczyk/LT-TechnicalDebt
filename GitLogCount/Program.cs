using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace GitLogCount
{
    class Program
    {
        static void Main(string[] args)
        {
            //var fileName = @"C:\github\LT-TechnicalDebt\simpleReport.txt";
            //GenerateSimpleReport(fileName);

            //var fileName1 = @"C:\github\LT-TechnicalDebt\backend-commits.txt";
            //GenerateSimpleReport(fileName1);

            //var fileName2 = @"C:\github\LT-TechnicalDebt\frontend-commits.txt";
            //GenerateSimpleReport(fileName2);

            //var fileName3 = @"C:\github\LT-TechnicalDebt\react-commits.txt";
            //GenerateSimpleReport(fileName3);

            //var fileName4 = @"C:\github\LT-TechnicalDebt\efcore-commits.txt";
            //GenerateSimpleReport(fileName4);

            var fileName = @"C:\github\LT-TechnicalDebt\advancedReport.txt";
            GenerateAdvancedReport(fileName);
        }

        public static void GenerateSimpleReport(string fileName)
        {
            // git log --pretty=format:" " --name-only > commits.txt
            var lines = File.ReadAllLines(fileName);
            var commits = new Dictionary<string, int>();

            foreach (var line in lines)
            {
                var lineToCheck = line.Trim();
                if (!string.IsNullOrEmpty(lineToCheck))
                {
                    if (!commits.ContainsKey(lineToCheck))
                    {
                        commits.Add(lineToCheck, 0);
                    }
                    commits[lineToCheck] += 1;
                }
            }
            
            var linesToWrite = commits.OrderByDescending(c => c.Value).Select(c => $"{c.Key};{c.Value}").ToList();

            var newFileName = GenerateNewFileName(fileName, "data");
            File.WriteAllLines(newFileName, linesToWrite);
        }

        public static void GenerateAdvancedReport(string fileName)
        {
            // git log --pretty=format:"Author:%an " --name-only > commits.txt
            var lines = File.ReadAllLines(fileName);
            var commits = new Dictionary<string, AdvancedData>();

            var author = string.Empty;
            foreach (var line in lines)
            {
                var lineToCheck = line.Trim();
                if (!string.IsNullOrEmpty(lineToCheck))
                {
                    if (lineToCheck.StartsWith("Author:"))
                    {
                        author = new string(lineToCheck.Skip(7).ToArray());
                    }
                    else
                    {
                        if (!commits.ContainsKey(lineToCheck))
                        {
                            commits.Add(lineToCheck, new AdvancedData());
                        }

                        commits[lineToCheck].CommitsCount += 1;
                        if (!commits[lineToCheck].Authors.Contains(author))
                        {
                            commits[lineToCheck].Authors.Add(author);
                        }
                    }
                }
            }

            var linesToWrite = commits.OrderByDescending(c => c.Value.CommitsCount).Select(c => $"{c.Key};{c.Value.CommitsCount};{c.Value.Authors.Count}").ToList();

            var newFileName = GenerateNewFileName(fileName, "advanced");
            File.WriteAllLines(newFileName, linesToWrite);
        }

        private static string GenerateNewFileName(string fileName, string suffix)
        {
            var fileNameSplitted = fileName.Split('.');
            var newFileName = fileNameSplitted.Take(fileNameSplitted.Length - 1).Append($"_{suffix}.csv")
                .Aggregate((seed, elem) => seed + elem);
            return newFileName;
        }
    }

    public class AdvancedData
    {
        public int CommitsCount { get; set; } = 0;
        public List<String> Authors { get; set; } = new List<string>();
    }
}
