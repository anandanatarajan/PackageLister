using System;
using System.Text;
using System.Text.RegularExpressions;

namespace PackageLister
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            CreatePacakgesFromFolder("E:\\personal\\Sandvik_Mehsana\\CalibproWebApp\\packages\\");
            Console.ReadKey();
        }


        public static void CreatePacakgesFromFolder(string folderpath)
        {
            string FileName = Path.Combine(folderpath, "packages.config");
            string pattern = @"^(?<package_name>.+)[.](?<version>\d+\.\d+\.\d+)[.]nupkg$";


            IEnumerable<string> SubDirectories = Directory.EnumerateDirectories(folderpath);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(@"<? xml version = ""1.0"" encoding = ""utf-8"" ?>");
            stringBuilder.AppendLine(@"<packages>");


            foreach (string subdir in SubDirectories)
            {
                if (Directory.Exists(subdir))
                {
                    var dn = new DirectoryInfo(subdir);
                    string fname=dn.EnumerateFiles("*.nupkg").FirstOrDefault()?.Name ?? "";
                    if (!string.IsNullOrWhiteSpace(fname))
                    {
                        Match match = Regex.Match(fname, pattern);
                        if (match.Success)
                        {
                            string packageName = match.Groups["package_name"].Value;
                            string version = match.Groups["version"].Value;

                            Console.WriteLine("Package Name: " + packageName);
                            Console.WriteLine("Version: " + version);
                            stringBuilder.AppendLine($@"<package id = ""{packageName}"" version = ""{version}"" targetFramework = ""net40"" />");
                        }
                        else
                        {
                            Console.WriteLine("Invalid filename format.");
                        }
                    }
                }
                

            }
            stringBuilder.AppendLine("</packages>");

            File.WriteAllText(FileName,stringBuilder.ToString());
            Console.WriteLine("File Created Successfully");

        }
    }
}
