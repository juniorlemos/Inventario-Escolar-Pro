namespace InventarioEscolar.Communication.Response
{
    public record CreatedAssetResponse
    {
        public string Name { get; init; } = string.Empty;
        public long? PatrimonyCode { get; init; }
    }
}
