using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public static class Serializer
    {
        public static string Serialize(Game game)
        {
            return JsonConvert.SerializeObject(game);
        }

        public static T Deserialize<T>(string json)
        {
            ITraceWriter traceWriter = new MemoryTraceWriter();
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TraceWriter = traceWriter } );
        }
    }
}
