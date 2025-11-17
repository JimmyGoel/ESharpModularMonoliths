using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.JsonConvertor
{
    internal class ShoppingCartConvertor : JsonConverter<ShoppingCart>
    {
        public override ShoppingCart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;

            var id = jsonObject.GetProperty("Id").GetGuid();
            var userName = jsonObject.GetProperty("UserName").GetString() ?? string.Empty;
            var itemsElemnet = jsonObject.GetProperty("Items");

            var shoppingCart = ShoppingCart.CreateNew(userName);

            var items = itemsElemnet.Deserialize<List<ShoppingCartItem>>(options);
            if (items != null)
            {
                var ItemField = typeof(ShoppingCart).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
                if (ItemField != null)
                {
                    ItemField.SetValue(shoppingCart, items);
                }
            }
            return shoppingCart;
        }

        public override void Write(Utf8JsonWriter writer, ShoppingCart value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Id", value.Id);
            writer.WriteString("UserName", value.UserName);

            writer.WritePropertyName("Items");
            JsonSerializer.Serialize(writer, value.Items, options);

            writer.WriteEndObject();
        }
    }
}
