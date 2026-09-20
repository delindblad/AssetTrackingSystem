// ReSharper disable InconsistentNaming
// ReSharper disable RedundantDefaultMemberInitializer

using System.Collections;
using System.Text.Json;
using freecurrencyapi;

namespace AssetTrackingSystem;

//Class provides an interface for tracking assets
public class AssetTracker
{
    public Hashtable CurrencyData { get; set; } = new();
    public List<Asset> Assets { get; set; } = new();

    //Used for unique ID, gets incremented when used
    public long IdStore
    {
        get => field++;
        set => field = value;
    } = 0;


    //Used to check asset age
    readonly TimeSpan _yellowSpan = new TimeSpan(913, 0, 0, 0, 0); //2.5 years
    readonly TimeSpan _redSpan = new TimeSpan(1004, 0, 0, 0, 0); //2.75 years

    //Adds a new asset
    public void AddAsset(AssetType type, string brand, string model, DateTime purchaseDate, decimal priceUSD,
         Location office)
    {
        decimal priceLocal;
        string prefix = "$";
        switch (office)
        {
            case Location.Copenhagen:
                priceLocal = ConvertCurrency("DKK", priceUSD);
                break;
            case Location.NewYork:
                priceLocal = priceUSD;
                break;
            case Location.Frankfurt:
                priceLocal = ConvertCurrency("EUR", priceUSD);
                break;
            case Location.Tokyo:
                priceLocal = ConvertCurrency("JPY", priceUSD);
                break;
            case Location.London:
                priceLocal = ConvertCurrency("GBP", priceUSD);
                break;
            default:
                priceLocal = priceUSD;
                break;
        }
        Assets.Add(new Asset(brand, model, purchaseDate, priceUSD, Decimal.Round(priceLocal, 2), office, IdStore, type));

    }

    //Removes asset from list
    public void RemoveAsset(int index)
    {
        Assets.RemoveAt(index);
    }


    //Print sorted by asset type
    public void PrintByType()
    {
        var data = Assets.AsEnumerable();
        //LINQ query
        var sortedData =
            from t in data
            orderby t.Type
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY TYPE");
        Console.WriteLine("");
        Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Type", "Brand", "Model", "Purchase Date", "Price (USD)", "Price (Local)", "Office", "ID");
        Console.WriteLine("-----------------------------------------------------------------------------" +
                          "--------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)
        {   
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            //Print with color depending on time since purchase
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.Type, asset.Brand, asset.Model, asset.PurchaseDate.ToShortDateString(), "$" + asset.PriceUSD,
                localPrefix + asset.PriceLocal, asset.Office, asset.ID);
            Console.ResetColor();
        }

    }

    

    //Print sorted by ID
    public void PrintByID()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.ID
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY ID");
        Console.WriteLine("");
        Console.WriteLine("{0,-5} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-15}",
            "ID", "Brand", "Model", "Purchase Date", "Price (USD)", "Price (Local)", "Office", "Type");
        Console.WriteLine("----------------------------------------------------------------------------" +
                          "---------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)

        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            //Print with color depending on time since purchase
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-5} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -15}",
                asset.ID, asset.Brand, asset.Model, asset.PurchaseDate.ToShortDateString(), "$" + asset.PriceUSD,
                localPrefix + asset.PriceLocal, asset.Office, asset.Type);
            Console.ResetColor();
        }

    }

    //Print sorted by brand
    public void PrintByBrand()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.Brand
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY BRAND");
        Console.WriteLine("");
        Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Brand", "Model", "Purchase Date", "Price (USD)", "Price (Local)", "Office", "Type", "ID");
        Console.WriteLine("----------------------------------------------------------------------------" +
                          "---------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)
        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            //Print with color depending on time since purchase
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.Brand, asset.Model, asset.PurchaseDate.ToShortDateString(), "$" + asset.PriceUSD,
                localPrefix + asset.PriceLocal, asset.Office, asset.Type, asset.ID);
            Console.ResetColor();
        }

    }

    //Print sorted by office
    public void PrintByOffice()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.Office
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY OFFICE");
        Console.WriteLine("");
        Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Office", "Brand", "Model", "Purchase Date", "Price (USD)", "Price (Local)", "Type", "ID");
        Console.WriteLine("-----------------------------------------------------------------------------" +
                          "--------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)

        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
                //Print with color depending on time since purchase
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.Office, asset.Brand, asset.Model, asset.PurchaseDate.ToShortDateString(), "$" + asset.PriceUSD,
                localPrefix + asset.PriceLocal, asset.Type, asset.ID);
            Console.ResetColor();
        }

    }

    //Print sorted by model
    public void PrintByModel()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.Model
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY MODEL");
        Console.WriteLine("");
        Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Model", "Office", "Brand", "Purchase Date", "Price (USD)", "Price (Local)", "Type", "ID");
        Console.WriteLine("-----------------------------------------------------------------------------" +
                          "--------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)
        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
                //Print with color depending on time since purchase
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.Model, asset.Office, asset.Brand, asset.PurchaseDate.ToShortDateString(), "$" + asset.PriceUSD,
                localPrefix + asset.PriceLocal, asset.Type, asset.ID);
            Console.ResetColor();
        }

    }

    //Print sorted by purchase date
    public void PrintByPurchaseDate()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.PurchaseDate
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY PURCHASE DATE");
        Console.WriteLine("");
        Console.WriteLine("{0,-25} | {1,-15} | {2,-15} | {3,-15} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Purchase Date", "Model", "Office", "Brand", "Price (USD)", "Price (Local)", "Type", "ID");
        Console.WriteLine("-----------------------------------------------------------------------------" +
                          "--------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)

        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            //Print with color depending on time since purchase
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-25} | {1,-15} | {2,-15} | {3,-15} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.PurchaseDate.ToShortDateString(), asset.Model, asset.Office, asset.Brand, "$" + asset.PriceUSD,
                localPrefix + asset.PriceLocal, asset.Type, asset.ID);
            Console.ResetColor();
        }

    }
    //Updates the  currency data
    public void UpdateCurrecyData()
    {   decimal d;
        //Get latest currency data
        var fx = new Freecurrencyapi("fca_live_GyuUHAV7ziIvpoHqYrSx5lGpSgmmme8xkXbOepyN");
        var latest = fx.Latest();
        var root = JsonDocument.Parse(latest).RootElement;
        var data = root.GetProperty("data");
        CurrencyData.Clear();
        CurrencyData.Add("SEK", data.GetProperty("SEK").GetDecimal());
        CurrencyData.Add("DKK", data.GetProperty("DKK").GetDecimal());
        CurrencyData.Add("EUR", data.GetProperty("EUR").GetDecimal());
        CurrencyData.Add("GBP", data.GetProperty("GBP").GetDecimal());
        CurrencyData.Add("JPY", data.GetProperty("JPY").GetDecimal());
    }
    private string getCurrencyPrefix(Asset asset)
    {
        switch (asset.Office)
        {
            case Location.Frankfurt:
                return "EUR ";
            case Location.London:
                return "£";
            case Location.NewYork:
                return "$";
            case Location.Tokyo:
                return "¥";
            case Location.Copenhagen:
                return "DKK ";
            default:
                return "$";
        }
        
    }
    public decimal ConvertCurrency(string currency, decimal amount)
    {

        var value =  CurrencyData["GBP"];
        if (value == null)
        {
            return 0;
        }
        return (decimal) value * amount;
    }
}