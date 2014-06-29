using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium;
using System.IO;
using System.Security.Permissions;
namespace Yuri
{
    class Yuri
    {
        static string URL=null;
        static IWebDriver driver = null;
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void watch(string path)
        {    
           List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
           Console.WriteLine("which file(s) to watch? Ex: *.html, index.html");
            string input=Console.ReadLine();
            string[] filters = input.Split(',');

            foreach (string filter in filters)
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path=path;
                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
                watcher.Filter = filter;
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watchers.Add(watcher);
            }

            foreach(FileSystemWatcher watcher in watchers)
            {
                watcher.EnableRaisingEvents = true;
            }
   
            Console.WriteLine("Press \'q\' to exit.");
            while (Console.Read() != 'q') ;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            RefreshBrowser();
        }

        private static void RefreshBrowser()
        {
            driver.Navigate().Refresh();
        }

        public static void LaunchBrowser(int selection)
        {
            Console.WriteLine("Lauching browser...");
            switch (selection)
            {
                case 1:
                    {
                        driver = new FirefoxDriver();
                        break;
                    }
                case 2:
                    {
                        driver = new ChromeDriver();
                        break;
                    }
                case 3:
                    {
                        driver = new InternetExplorerDriver();
                        break;
                    }
                case 4:
                    {
                        driver = new SafariDriver();
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Your input is invalid.");
                        break;
                    }

            }
            if (URL == null)
            {
                Console.WriteLine("No path specified...stopped!");
                Environment.Exit(0);
            }
            driver.Navigate().GoToUrl(URL);
        }
        public static void Run()
        {
            Console.WriteLine("Select browser to launch by providing the number. ex 1 for Firefox");
            string [] ar={"1) Firefox", "2) Chrome", "3) IE", "4) Safari"};
            foreach(string s in ar)
            {
                Console.WriteLine(s);
            }
            int browser = Int32.Parse(Console.ReadLine());
            Console.WriteLine("What is the path of the file to open in the browser? Ex: C:\\myfolder\\index.html");
            URL = Console.ReadLine();
           LaunchBrowser(browser);
           Console.WriteLine("Provide of the path of the directory where the files to watch are located. Ex:C:\\mywebsite\\");
           string path=Console.ReadLine();
           watch(path);
            
        }
        static void Main(string[] args)
        {
            Run();
        }
    }
}
