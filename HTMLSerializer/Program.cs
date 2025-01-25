
using System.Text.RegularExpressions;
using HTMLSerializer;

//get the html from the web
var html = await Load("https://learn.malkabruk.co.il/courses");
//clean the html to be without spaces
var cleanHtml = new Regex("\\s").Replace(html, "");
//get the html in lines
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToList();

//var htmlElement = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";
//var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
//Console.ReadLine();

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

//return the name of the tag
static string getTagName(string attribute)
{
    string s = "";
    for (int u = 0; u < attribute.Length && attribute[u] != '='; u++)
    {
        if (HtmlHelper.Instance.HtmlTags.Contains(attribute.Substring(0, u)))
        {
            s = attribute.Substring(0, u);
        }
        else
            s = "";
    }
    return s;
}
static string getVoidTagName(string attribute)
{
    string s = "";
    for (int u = 0; u < attribute.Length && attribute[u] != '='; u++)
    {
        if (HtmlHelper.Instance.HtmlVoidTags.Contains(attribute.Substring(0, u)))
        {
            s = attribute.Substring(0, u);
        }
    }
    return s;
}
//convert MatchCollection to list
static List<string> CreateList(MatchCollection attributesM)
{
    List<string> attributes = new List<string>();
    for (int z = 0; z < attributesM.Count; z++)
    {
        attributes.Add(attributesM[z].ToString());
    }
    return attributes;
}

static void CreateNewElement(HtmlElement crnt, List<string> attributes, string line)
{
    if (attributes.Count > 0)
    {
        crnt.Name = getTagName(attributes[0]);
        attributes[0] = attributes[0].Substring(crnt.Name.Length);
        foreach (var attribute in attributes)
        {
            if (attribute.StartsWith("id"))
                crnt.Id = attribute.Substring("id".Length + 1);
            if (attribute.StartsWith("class"))
                crnt.Classes = attribute.Substring("class".Length + 1).Split(' ').ToList();
            crnt.Attributes.Add(attribute);
        }
    }
    else
        crnt.Name = getTagName(line);
}

//main function
static void Func(List<string> htmlLines, int index)
{
    //get the root - html
    var htmlElement = htmlLines[index];
    //create the root html
    HtmlElement root = new HtmlElement();
    //CreateNewElement(root, attributes);
    HtmlElement current = root;

    for (int i = index; i < htmlLines.Count; i++)
    {
        string line = htmlLines[i];
        var cAttributesM = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
        //convert the attributes to list
        List<string> cAttributes = CreateList(cAttributesM);
        if (line.StartsWith("/html"))
            break;
        if (line.StartsWith("/"))
        {
            if (current.Parent != null)
                current = current.Parent;
            Console.WriteLine("Id: " + current.Id);
            Console.WriteLine("name: " + current.Name);
            Console.WriteLine("inner html: " + current.InnerHtml);
            Console.WriteLine("Attributes: ");
            foreach (var item in current.Attributes)
            {
                Console.Write(item + ", ");
            }
            Console.WriteLine("parent: " + current.Parent?.Name);
            Console.WriteLine("children: ");
            for (int g = 0; g < current.Children.Count; g++)
            {
                Console.WriteLine(current.Children[g].Name);
            }
            Console.WriteLine();

        }
        else if (cAttributes != null && getTagName(line) != "")
        {
            HtmlElement newH = new HtmlElement();
            CreateNewElement(newH, cAttributes, line);
            current.Children.Add(newH);
            newH.Parent = current;
            if (getVoidTagName(line) == "" && line.EndsWith("/"))
            {
                current = newH;
            }
        }
        else
        {
            current.InnerHtml = line;
        }
    }

}
int i = 1;

Func(htmlLines, i);
//Selector root = Selector.ConvertToS("div .p pre.hhh");
//Selector crnt = root;
//while (crnt.Child != null)
//{
//    Console.WriteLine("id: " + crnt.Id);
//    Console.WriteLine("name: " + crnt.TagName);
//    Console.WriteLine("classes: ");
//    foreach (var item in crnt.Classes)
//    {
//        Console.Write("     " + item);
//    }
//    Console.WriteLine();
//    Console.WriteLine("parent: " + crnt.Parent?.TagName);
//    Console.WriteLine("child: " + crnt.Child?.TagName);
//    Console.WriteLine("====================");
//    crnt = crnt.Child;
//}





