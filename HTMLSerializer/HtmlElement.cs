using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace HTMLSerializer
{
    internal class HtmlElement
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement Parent { get; set; } = null;
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                HtmlElement element = queue.Dequeue();
                yield return element;
                foreach (HtmlElement child in element.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement element = this;
            while (element.Parent != null)
            {
                yield return element;
                element = element.Parent;
            }
        }
        public List<HtmlElement> GetElementBySelector(Selector selector)
        {
            HashSet<HtmlElement> elements = new HashSet<HtmlElement>();
            GetElementRec(this, selector, elements);
            return elements.ToList();
        }
        public void GetElementRec(HtmlElement element, Selector selector, HashSet<HtmlElement> htmlElements)
        {
            var descendants = this.Descendants();
            var match = descendants.Where(element =>
                (string.IsNullOrEmpty(selector.TagName) || selector.TagName == element.Name)
                && (string.IsNullOrEmpty(selector.Id) || selector.Id == element.Id)
                && (!selector.Classes.Any() || selector.Classes.All(
                    class1 =>
                    element.Classes.Contains(class1)
                    ))).ToList();
            if (selector.Child == null)
            {
                foreach (var m in match)
                {
                    htmlElements.Add(m);
                }
                return;
            }
            foreach (var m in match)
            {
                GetElementRec(element, selector.Child, htmlElements);
            }
        }
    }
}
