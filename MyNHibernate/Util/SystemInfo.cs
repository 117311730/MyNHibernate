using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyNHibernate.Util
{
    public static class SystemInfo
    {
        public static string ApplicationBaseDirectory
        {
            get
            {
#if NETCF
				return System.IO.Path.GetDirectoryName(SystemInfo.EntryAssemblyLocation) + System.IO.Path.DirectorySeparatorChar;
#else
                return AppDomain.CurrentDomain.BaseDirectory;
#endif
            }
        }

        public static string ConvertToFullPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            string baseDirectory = "";
            try
            {
                string applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                if (applicationBaseDirectory != null)
                {
                    // applicationBaseDirectory may be a URI not a local file path
                    Uri applicationBaseDirectoryUri = new Uri(applicationBaseDirectory);
                    if (applicationBaseDirectoryUri.IsFile)
                    {
                        baseDirectory = applicationBaseDirectoryUri.LocalPath;
                    }
                }
            }
            catch
            {
                // Ignore URI exceptions & SecurityExceptions from SystemInfo.ApplicationBaseDirectory
            }

            if (baseDirectory != null && baseDirectory.Length > 0)
            {
                // Note that Path.Combine will return the second path if it is rooted
                return Path.GetFullPath(Path.Combine(baseDirectory, path));
            }
            return Path.GetFullPath(path);
        }
    }
}