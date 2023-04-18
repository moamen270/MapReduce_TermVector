using MapReduce;
using System.Collections.Generic;

static List<KeyValuePair<string, double>> Map(string document)
{
    // Split the document string doubleo individual words
    string[] words = document.Split();
    count.NumWords += words.Length;

    // Emit a key-value pair for each word, with the word as the key and a count of 1 as the value
    List<KeyValuePair<string, double>> wordCounts = new List<KeyValuePair<string, double>>();
    foreach (string word in words)
    {
        wordCounts.Add(new KeyValuePair<string, double>(word, 1));
    }
    return wordCounts;
}

static Dictionary<string, double> TermVector(List<KeyValuePair<string, double>> document)
{
    Dictionary<string, double> wordCounts = new();
    foreach (var pair in document)
    {
        if (wordCounts.ContainsKey(pair.Key))
        {
            wordCounts[pair.Key]++;
        }
        else
        {
            wordCounts[pair.Key] = 1;
        }
    }
    foreach (var pair in wordCounts)
    {
        wordCounts[pair.Key] = pair.Value / document.Count;
    }
    return wordCounts;
}

static Dictionary<string, double> Reduce(List<List<KeyValuePair<string, double>>> documents)
{
    // Aggregate the word counts from all the document dictionaries
    Dictionary<string, double> wordCounts = new Dictionary<string, double>();
    foreach (var document in documents)
    {
        foreach (var pair in document)
        {
            if (wordCounts.ContainsKey(pair.Key))
            {
                wordCounts[pair.Key] += pair.Value;
            }
            else
            {
                wordCounts[pair.Key] = pair.Value;
            }
        }
    }
    return wordCounts;
}

static Dictionary<string, double> TotalTermVector(Dictionary<string, double> Reduced)
{
    foreach (var pair in Reduced)
    {
        Reduced[pair.Key] = pair.Value / count.NumWords;
    }
    return Reduced;
}
static string clean(string document)
{
    var t = document.ToLower().Split(new char[] { '.', ',', ';', ':', '-', '(', ')', '[', ']', '{', '}', '<', '>', '/', '\\', '\'', '\"', '\n', '\r', '\t' });
    string result = string.Empty;
    foreach (var line in t)
    {
        result += line;
    }
    return result;
}

/*string data = File.ReadAllText("path/to/file.txt");
*/

List<string> HostName = new List<string>()
{
    "Moamen:19192","Ghanem:19015","Fouad:19204","Bassiony:19096","Younis:18300","Ziad:19035"
};
List<string> documents = new List<string>();
documents.Add("the quick brown fox jumps over the lazy dog. the dog barks at the fox, but the fox ignores it and continues running.");
documents.Add("dog cat dog cat dog cat cat dog cat");
documents.Add("dog dog dog cat dog cat cat dog cat");
documents.Add("cat dog cat cat dog cat cat dog cat");
documents.Add("dog dog cat cat dog cat cat dog cat");
documents.Add("dog dog cat dog dog dog cat dog cat");

Console.WriteLine("The BigData.txt:");
Console.WriteLine("----------------");

foreach (var document in documents)
{
    Console.WriteLine(document);
}

List<List<KeyValuePair<string, double>>> MapResults = new();

List<TermVector> TermVectorResults = new();

Dictionary<string, double> ReduceResult = new();

Console.WriteLine("==================================");
foreach (string document in documents)
{
    Console.WriteLine("MapResult:");
    Console.WriteLine("-----------");
    var result = Map(clean(document));
    foreach (var wrod in result)
    {
        Console.WriteLine($"Key: {wrod.Key}  Value: {wrod.Value}");
    }
    Console.WriteLine("==================================");

    MapResults.Add(result);
}
int name = 0;
foreach (var document in MapResults)
{
    TermVectorResults.Add(new TermVector { HostName = HostName[name], Terms = TermVector(document) });
    name++;
}

foreach (var term in TermVectorResults)
{
    Console.WriteLine("TermVectorResult:");
    Console.WriteLine("----------------");
    Console.WriteLine($"HostName: {term.HostName}");
    foreach (var dic in term.Terms)
    {
        Console.WriteLine($"Word: {dic.Key} Freq:{dic.Value.ToString("0.00")}");
    }
    Console.WriteLine("==================================");
}

Console.WriteLine("ReduceResult:");
Console.WriteLine("-------------");

ReduceResult = Reduce(MapResults);
foreach (var word in ReduceResult.OrderByDescending(p => p.Value))
{
    Console.WriteLine($"Key: {word.Key}  Value: {word.Value}");
}
Console.WriteLine("==================================");
Console.WriteLine("TotalTermVector:");
Console.WriteLine("----------------");

var totalTermVector = TotalTermVector(ReduceResult);
foreach (var word in totalTermVector.OrderByDescending(p => p.Value))
{
    Console.WriteLine($"Key: {word.Key}  Value: {word.Value.ToString("0.00")}");
}