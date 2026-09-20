using System.Text.Json;
using freecurrencyapi;
using MenuKit;
namespace AssetTrackingSystem;

class Program
{
    static void Main(string[] args)
    {
       // Test();
       CurrencyTest();
    }
    
    static void Test()
    {
        AssetTracker assetTracker;
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
        {
            assetTracker = new AssetTracker();

            assetTracker.AddAsset(AssetType.Desktop, "Apple", "Mac Mini M4", new DateTime(2024, 6, 6),
                1100, 990, Location.London);
            assetTracker.AddAsset(AssetType.Desktop, "Apple", "Mac Mini M5", new DateTime(2026, 6, 6),
                1100, 990, Location.London);
            assetTracker.AddAsset(AssetType.Mobile, "Apple", "iPhone 17", new DateTime(2023, 1, 7),
                800, 750, Location.Frankfurt);
            assetTracker.AddAsset(AssetType.Laptop, "Lenovo", "Think Pad", new DateTime(2026, 5, 6),
                800, 750, Location.NewYork);
            assetTracker.AddAsset(AssetType.Tablet, "Apple", "iPad Air 4", new DateTime(2024, 1, 10),
                900, 850, Location.Frankfurt);
            assetTracker.AddAsset(AssetType.Tablet, "Apple", "iPad Air 3", new DateTime(2022, 1, 10),
                900, 850, Location.Frankfurt);
        }
        


        assetTracker.PrintByType();
        Console.WriteLine();
        assetTracker.PrintByBrand();
        Console.WriteLine();
        assetTracker.PrintByOffice();
        Console.WriteLine();
        assetTracker.PrintByModel();
        Console.WriteLine();
        assetTracker.PrintByPurchaseDate();
        Console.WriteLine();
        assetTracker.PrintByID();
        Console.WriteLine();
        try
        {
            Utilities.WritelnGreen($"Saving to {"assets.json"}");
            string jsonString = JsonSerializer.Serialize(assetTracker);
            File.WriteAllText("assets.json", jsonString);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public static void CurrencyTest()
    {
        var fx = new Freecurrencyapi("fca_live_GyuUHAV7ziIvpoHqYrSx5lGpSgmmme8xkXbOepyN");
        Console.WriteLine(fx.Status());

    }
}