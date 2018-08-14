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
            //var fileName = @"C:\github\LT-TechnicalDebt\commits.txt";
            //GenerateSimpleRaport(fileName);

            var fileName1 = @"C:\github\LT-TechnicalDebt\backend-commits.txt";
            GenerateSimpleRaport(fileName1);

            var fileName2 = @"C:\github\LT-TechnicalDebt\frontend-commits.txt";
            GenerateSimpleRaport(fileName2);
        }

        public static void GenerateSimpleRaport(string fileName)
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

            var fileNameSplitted = fileName.Split('.');
            var newFileName = fileNameSplitted.Take(fileNameSplitted.Length - 1).Append("_data.csv").Aggregate((seed, elem) => seed + elem);
            File.WriteAllLines(newFileName, linesToWrite);
        }
    }
}
