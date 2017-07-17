using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Qs2Xml
{
    static class ListEx
    {
        public static void ModifyEach<T>(this IList<T> source,
                                 Func<T, T> projection)
        {
            for (int i = 0; i < source.Count; i++)
            {
                source[i] = projection(source[i]);
            }
        }
    }

    internal class Program
    {

        private static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    throw new Exception("You must specify a quick select text file to convert.");
                }
                string filename = args[0];
                if (!File.Exists(filename))
                {
                    throw new Exception($"file {filename} does not exist.");
                }

                var s4 = "Vente ID_QCKSEL_NUMERO_VERSION ( 6 ; 6513 ) uint 26";
                var pattern = "\\( [0-9]+ ; [0-9]+ \\)";
                //var pattern = "\\( [0-9]+ ; [0-9]";
                var result = Regex.Replace(s4, pattern, "");

                var linelist = File.ReadAllLines(filename).ToList();
                linelist.ModifyEach(x=> Regex.Replace(x, "Vente ID_QCKSEL_", ""));
                linelist.ModifyEach(x => Regex.Replace(x, " string| date| uint| sint", ""));
                linelist.ModifyEach(x => Regex.Replace(x, " \\( [0-9]+ ; [0-9]+ \\)", ""));
                linelist.ModifyEach(x => Regex.Replace(x, "PRODUCT struct", "<q "));
                linelist.ModifyEach(x => Regex.Replace(x, "CODE", "code="));
                linelist.ModifyEach(x => Regex.Replace(x, "ORIGIN", "o="));
                linelist.ModifyEach(x => Regex.Replace(x, "DESTINATION", "d="));
                linelist.ModifyEach(x => Regex.Replace(x, "END_DATE", "u="));
                linelist.ModifyEach(x => Regex.Replace(x, "START_DATE", "f="));
                linelist.ModifyEach(x => Regex.Replace(x, "ROUTE", "r="));
                linelist.ModifyEach(x => Regex.Replace(x, "TICKET", "t="));
                linelist.ModifyEach(x => Regex.Replace(x, "RESTRICTION", "res="));
                linelist.ModifyEach(x => Regex.Replace(x, "ADULT_FARE", "fare="));
                linelist.ModifyEach(x => Regex.Replace(x, "FLAG", "flag="));
                linelist.ModifyEach(x => Regex.Replace(x, "CROSS_LONDON_IND", "cli="));
                linelist.ModifyEach(x => Regex.Replace(x, "STATUS", "status="));
                linelist.ModifyEach(x => Regex.Replace(x, "ORIENTATION", "orient="));
                linelist.ModifyEach(x => Regex.Replace(x, "DATEBAND_NAME", "dband="));
                linelist.ModifyEach(x => Regex.Replace(x, "TIMEBAND_NAME", "tband="));
                linelist.ModifyEach(x => Regex.Replace(x, "TIMEBAND_TABLE", "struct"));
                linelist.ModifyEach(x => Regex.Replace(x, "TIMEBAND_START", "start="));
                linelist.ModifyEach(x => Regex.Replace(x, "TIMEBAND_END", "end="));
                linelist.ModifyEach(x => Regex.Replace(x, "TIMEBAND_NAME", "name="));
                linelist.ModifyEach(x => Regex.Replace(x, "TIMEBAND_ARRAY array struct", "@"));



                linelist.ForEach(s => Console.WriteLine($"{s}"));
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
