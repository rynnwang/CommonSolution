using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.AutoConfigurator
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error : only accept 2 args : [folder path] [configuration]");
                return -1;
            }
            string folder = args[0];
            string configuration = args[1];
            try
            {
                Run(args[0], args[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
            return 0;
        }

        static void Run(string folder, string configuration)
        {
            var dic = new Dictionary<string, byte[]>();
            var list = new List<string>();

            foreach (var filePath in Directory.EnumerateFiles(folder, "*.json"))
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (fileName.EndsWith(configuration, StringComparison.CurrentCultureIgnoreCase))
                {
                    string destFileName = fileName.Substring(0, fileName.Length - configuration.Length) + "json";
                    dic.Add(Path.Combine(folder, destFileName), File.ReadAllBytes(filePath));
                }
                list.Add(filePath);
            }

            if (dic.Count == 0) return;

            foreach (var filePath in list)
            {
                File.Delete(filePath);
            }

            foreach (var pair in dic)
            {
                File.WriteAllBytes(pair.Key, pair.Value);
            }
        }

    }
}
