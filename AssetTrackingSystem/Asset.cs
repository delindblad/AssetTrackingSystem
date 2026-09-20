using System.Data.Common;
using System.Text.Json.Serialization;
using AssetTrackingSystem;
// ReSharper disable InconsistentNaming


namespace AssetTrackingSystem;

//Class storing asset data
public class Asset
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PriceUSD { get; set; }
    public decimal PriceLocal { get; set; }
    public Location Office { get; set; }
    public AssetType Type { get; set; }
    public long ID { get; set; }
    

    public Asset(string brand, string model, DateTime purchaseDate, decimal priceUSD,
        decimal priceLocal, Location office, long id, AssetType type)
    {
        Brand = brand;
        Model = model;
        PurchaseDate = purchaseDate;
        PriceUSD = priceUSD;
        PriceLocal = priceLocal;
        Office = office;
        ID = id;
        Type = type;
    }
}

