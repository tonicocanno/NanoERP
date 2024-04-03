using NanoERP.API.Domain.interfaces;

namespace NanoERP.API.Domain.Entities;

public class Product : MasterData, IEntity
{
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public string? BarCode { get; set; }
    public string? UOM { get; set; }
    public decimal? Cost { get; set; }
    public decimal? Price { get; set; }
    public decimal? Stock { get; set; }
    public decimal? MinStock { get; set; }
    public decimal? MaxStock { get; set; }
    public string? Image { get; set; }
}
