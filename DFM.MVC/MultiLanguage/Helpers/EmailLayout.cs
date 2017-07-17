using System;
using System.IO;
using System.Text;
using System.Web;

namespace DFM.MVC.MultiLanguage.Helpers
{
    public class EmailLayout
    {
        private static readonly String path = HttpContext.Current.Server.MapPath(@"~\MultiLanguage\EmailLayouts");

        public String this[String layout]
        {
            get
            {
                var filePath =
                    Path.Combine(path, PlainText.Language, layout + ".htm");

                if (!File.Exists(filePath))
                    return "";

                var reader = new StreamReader(filePath);
                var content = new StringBuilder();

                while (!reader.EndOfStream)
                {
                    content.AppendLine(reader.ReadLine());
                }

                return content.ToString();
            }
        }

    }
}