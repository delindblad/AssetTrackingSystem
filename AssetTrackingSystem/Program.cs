using System.Text.Json;
using freecurrencyapi;
using MenuKit;
namespace AssetTrackingSystem;

class Program
{
    static void Main(string[] args)
    {   
        Run();
        
    }

    static void Run()
    {
        //Create asset tracker
        AssetTracker assetTracker;
        //If there's an assets.json file, load it
        if (File.Exists("assets.json"))
        {
            try
            {
                Utilities.WritelnGreen($"Loading from assets.json");
                string jsonString = File.ReadAllText("assets.json");
                assetTracker = JsonSerializer.Deserialize<AssetTracker>(jsonString)!;
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }

        }
        else
        //Otherwise, create a new one
        {
            assetTracker = new AssetTracker();
            
        }
        //Update currency data
        assetTracker.UpdateCurrecyData();
        //Create root page
        var root = new RootPage("ASSET TRACKER type 'Q' TO QUIT", assetTracker);
        
        root.AddChildPage(new ShowAssetsPage("SHOW ASSETS", root, assetTracker));
        root.AddChildPage(new AddAssetPage("ADD ASSET", root, assetTracker));

       
       AbstractMenuPage? context = root;
       while (true)
       {
           context = context.Run();
           if (context == null!)
           {
               return;
           }
       }
    }
}