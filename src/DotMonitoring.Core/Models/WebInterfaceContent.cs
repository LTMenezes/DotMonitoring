using System.Threading.Tasks;

namespace DotMonitoring.Core.Models
{
    public class WebInterfaceContent
    {
        public string ContentType { get; }

        public Task<string> ContentText { get; }

        public WebInterfaceContent(string contentType, Task<string> contentText)
        {
            this.ContentType = contentType;
            this.ContentText = contentText;
        }
    }
}
