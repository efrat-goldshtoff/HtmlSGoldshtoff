using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HTMLSerializer
{
    internal class Selector
    {
        public string TagName { get; set; } = "";
        public string Id { get; set; } = "";
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; } = null;
        public Selector Child { get; set; } = null;
        public static Selector ConvertToS(string str)
        {
            List<string> selectors = str.Split(" ").ToList();
            Selector root = new Selector();
            Selector crnt = root;
            foreach (string selector in selectors)
            {
                List<string> strings = new List<string>();
                string s1 = "";
                char ch = '.';
                for (int ind = 0; ind < selector.Length; ind++)
                {
                    ch = selector[ind];
                    if (ch != '.' && ch != '#' && ind != selector.Length - 1)
                    {
                        s1 += ch;
                    }
                    else
                    {
                        strings.Add(s1);
                        s1 = "" + ch;
                    }
                }
                strings[strings.Count - 1] += ch;
                //foreach (string s2 in strings)
                //{
                //    Console.WriteLine(s2);
                //}
                foreach (string s in strings)
                {
                    if (s.StartsWith("."))
                    {
                        crnt.Classes.Add(s.Substring(1));
                    }
                    else if (s.StartsWith("#"))
                    {
                        crnt.Id = s.Substring(1);
                    }
                    else
                    {
                        if (HtmlHelper.Instance.HtmlTags.Contains(s))
                        {
                            crnt.TagName = s;
                        }
                    }
                }
                Selector newH = new Selector();
                crnt.Child = newH;
                newH.Parent = crnt;
                //Console.WriteLine("id: " + crnt.Id);
                //Console.WriteLine("name: " + crnt.TagName);
                //Console.WriteLine("classes: ");
                //foreach (var item in crnt.Classes)
                //{
                //    Console.Write("     " + item);
                //}
                //Console.WriteLine();
                //Console.WriteLine("parent: " + crnt.Parent?.TagName);
                //Console.WriteLine("child: " + crnt.Child?.TagName);
                //Console.WriteLine("====================");
                crnt = newH;

            }
            return root;
        }
    }
}
