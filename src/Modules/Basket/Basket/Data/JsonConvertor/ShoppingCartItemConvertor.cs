using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.JsonConvertor
{
    public class ShoppingCartItemConvertor : JsonConverter<ShoppingCartItem>
    {
        public override ShoppingCartItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;

            var id = jsonObject.GetProperty("Id").GetGuid();
            var shoppingCartId = jsonObject.GetProperty("ShoppingCartId").GetGuid();
            var productId = jsonObject.GetProperty("ProductId").GetGuid();
            var quantity = jsonObject.GetProperty("Quantity").GetInt32();
            var color = jsonObject.GetProperty("Color").GetString() ?? string.Empty;
            var price = jsonObject.GetProperty("Price").GetDecimal();
            var productName = jsonObject.GetProperty("ProductName").GetString() ?? string.Empty;

            return new ShoppingCartItem(id, shoppingCartId, productId, quantity, color, price, productName);

        }

        public override void Write(Utf8JsonWriter writer, ShoppingCartItem value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Id", value.Id);
            writer.WriteString("ShoppingCartId", value.ShoppingCartId);
            writer.WriteString("ProductId", value.ProductId);
            writer.WriteNumber("Quantity", value.Quantity);
            writer.WriteString("Color", value.Color);
            writer.WriteNumber("Price", value.Price);
            writer.WriteString("ProductName", value.ProductName);
            writer.WriteEndObject();
        }
    }
}
