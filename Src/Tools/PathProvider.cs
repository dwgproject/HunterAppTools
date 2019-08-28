using System;
using System.IO;
using Gravityzero.Console.Utility.Engine;

namespace Gravityzero.Console.Utility.Tools
{
    public static class PathProvider
    {
        public static string SettingsPath()
        {
            string appName = ConsoleEngine.GetAppInfo().ProductName.Replace(" ",  string.Empty);
            //string settings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName);
            string settings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),appName);
            if (!Directory.Exists(settings))
            {
                try
                {
                    DirectoryInfo info = Directory.CreateDirectory(settings);
                }
                catch (IOException ex)
                {
                    return string.Empty;
                }
                catch (UnauthorizedAccessException ex)
                {
                    return string.Empty;
                }
                catch (ArgumentException ex)
                {
                    return string.Empty;
                }
                catch (NotSupportedException ex)
                {
                    return string.Empty;
                }
            }

            return string.Concat(settings,"\\" , appName,".json");
        }
    }
}