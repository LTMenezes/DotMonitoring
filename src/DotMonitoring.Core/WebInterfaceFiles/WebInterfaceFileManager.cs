using DotMonitoring.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotMonitoring.Core.WebInterfaceFiles
{
    public class WebInterfaceFileManager
    {
        private IDictionary<string, string> PathToResourceNameDict { get; set; }

        public WebInterfaceFileManager()
        {
            this.PathToResourceNameDict = BuildResourceDictionary();
        }

        public async Task<WebInterfaceContent> GetResource(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.PathToResourceNameDict[path]);
            StreamReader reader = new StreamReader(stream);

            return new WebInterfaceContent(this.GetContentType(path), reader.ReadToEndAsync());
        }

        public bool DoesResourceExists(string path)
        {
            return this.PathToResourceNameDict.ContainsKey(path);
        }

        private string GetContentType(string path)
        {
            string[] splitedResourceName = this.PathToResourceNameDict[path].Split('.', StringSplitOptions.RemoveEmptyEntries);
            string fileEnding = splitedResourceName[splitedResourceName.Length - 1];

            switch (fileEnding)
            {
                case "js":
                    return "text/javascript;";

                case "html":
                    return "text/html";

                case "css":
                    return "text/css";

                case "svg":
                    return "image/svg+xml";

                case "ico":
                    return "image/x-icon";

                default:
                    return "text/plain";
            }
        }

        private IDictionary<string, string> BuildResourceDictionary()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string currentNamespace = typeof(WebInterfaceFileManager).Namespace;
            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach(string resourceName in resourceNames)
            {
                if (!resourceName.StartsWith(currentNamespace))
                    continue;

                string resourceNameWithoutNamespace = resourceName.Remove(0, currentNamespace.Length);
                string[] splitedResourceName = resourceNameWithoutNamespace.Split('.', StringSplitOptions.RemoveEmptyEntries);
                StringBuilder pathBuilder = new StringBuilder();

                pathBuilder.Append("/monitoring/");
                bool hasFileNameStarted = false;

                foreach(string resourceNamePart in splitedResourceName)
                {
                    if(hasFileNameStarted == false && (resourceNamePart == "assets" || resourceNamePart == "brand" || resourceNamePart == "css"
                        || resourceNamePart == "js" || resourceNamePart == "vendors"))
                    {
                        pathBuilder.Append($"{resourceNamePart}/");
                        continue;
                    }

                    if (hasFileNameStarted)
                    {
                        pathBuilder.Append($".{resourceNamePart}");
                    }
                    else
                    {
                        hasFileNameStarted = true;
                        pathBuilder.Append($"{resourceNamePart}");
                    }
                }

                dict.Add(pathBuilder.ToString(), resourceName);
            }

            return dict;
        }
    }
}
