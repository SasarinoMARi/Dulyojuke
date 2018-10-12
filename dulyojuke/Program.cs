using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace dulyojuke
{
    class Program
    {
        [STAThread]
        static void Main(string[] arguments)
        {
            // Resolve merged libraries from resource
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(new Program().GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };

            CopyDownloader();

            // application entrypoint
            var application = new Application
            {
                StartupUri = new Uri("Windows/MainWindow.xaml", UriKind.RelativeOrAbsolute)
            };

            application.Run();
        }

        static void CopyDownloader()
        {
            var path = MediaDownloader.GetDownloadClientPath();
            if (File.Exists(path)) return;

            string resourceName = new AssemblyName("youtube-dl.exe").ToString();

            string resource = Array.Find(new Program().GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));
            
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            {
                using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(file);
                }
            }
        }
    }
}
