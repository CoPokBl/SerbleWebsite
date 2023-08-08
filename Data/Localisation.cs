using YamlDotNet.RepresentationModel;

namespace SerbleWebsite.Data;

public class Localiser {
    public string this[string key] => !Localisation.Localisations.ContainsKey(key) ? key : Localisation.Localisations[key];
}

public static class Localisation {
    
    public static readonly Dictionary<string, string> Localisations = new();

    public static void AddLocalisations(this IServiceCollection services) {
        services.AddSingleton<Localiser>();
    }

    public static async Task SetLanguage(this IServiceProvider prov, string code) {
        // Create a list of all files in the directory Languages and all its subdirectories at the url /assets/translations
        HttpClient http = prov.GetRequiredService<HttpClient>();
        string manifest = await http.GetStringAsync("/assets/translations/manifest.txt");
        string[] files;
        try {
            files = manifest.Split('\n').Where(f => f.EndsWith(code) || f.EndsWith("default")).ToArray();
        }
        catch (Exception e) {
            Console.WriteLine("Localisation error: " + e);
            files = new[] { "default" };
        }

        // Sort so all files that end in default are at the top
        List<string> filesOrdered = new();
        foreach (string f in files) {
            if (f.EndsWith("default")) {
                filesOrdered.Add(f);
                continue;
            }
            filesOrdered.Insert(0, f);
        }
        files = filesOrdered.ToArray().Reverse().ToArray();

        // Loop through all files
        foreach (string file in files) {
            Console.WriteLine("Loading file: " + file);
            string content = await http.GetStringAsync("/assets/translations/" + file + ".yaml");
            StringReader reader = new(content);
            YamlStream yaml = new();
            yaml.Load(reader);
            YamlMappingNode root = (YamlMappingNode)yaml.Documents[0].RootNode;
            foreach (KeyValuePair<YamlNode, YamlNode> entry in root.Children) {
                string key = entry.Key.ToString();
                string value = entry.Value.ToString();
                Localisations[key] = value;
            }
        }
    }
    
}