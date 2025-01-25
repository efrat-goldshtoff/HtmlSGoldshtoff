using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;


namespace HTMLSerializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            string str = File.ReadAllText("HtmlTags/HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(str);

            string str2 = File.ReadAllText("HtmlTags/HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(str2);

        }
    }
}
