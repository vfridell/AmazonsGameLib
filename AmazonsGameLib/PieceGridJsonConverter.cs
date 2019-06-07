using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class PieceGridJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(PieceGrid));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Newtonsoft.Json.Linq.JObject o = Newtonsoft.Json.Linq.JObject.Load(reader);
            int size = o.Value<int>("Size");
            PieceGrid newPieceGrid = new PieceGrid(size);
            var pieceGridJson = o.Value<JObject>("PointPieces");
            Dictionary<Point, Piece> pointPiecesDictionary = new Dictionary<Point, Piece>();
            foreach(KeyValuePair<string, JToken> kvp in pieceGridJson)
            {
                Point point = Point.Get(kvp.Key);
                Piece piece = Piece.Get(kvp.Value.Value<string>());
                pointPiecesDictionary.Add(point, piece);
            }
            newPieceGrid.Initialize(pointPiecesDictionary);
            return newPieceGrid;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PieceGrid pieceGrid = (PieceGrid)value;
            writer.WriteStartObject();
            writer.WritePropertyName("Size");
            writer.WriteValue(pieceGrid.Size);
            writer.WritePropertyName("PointPieces");
            serializer.Serialize(writer, pieceGrid.PointPieces);
            writer.WriteEndObject();
        }
    }
}
