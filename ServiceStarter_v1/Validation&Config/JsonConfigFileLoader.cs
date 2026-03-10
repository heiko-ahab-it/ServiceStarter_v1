using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceStarter_v1
{
    internal class JsonConfigFileLoader : IConfigLoader
    {
        private readonly JsonSerializerOptions _options;
        public JsonConfigFileLoader()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowOutOfOrderMetadataProperties = true,
                Converters = {new JsonStringEnumConverter()}

            };
        }
        public T getRootObject<T>(string path) where T : class
        {
            try
            {
                if (!File.Exists(path)) { throw new FileNotFoundException($"Config File could not be Found under Path: {path}"); }
                string jsonContent = File.ReadAllText(path);
                T? result = JsonSerializer.Deserialize<T>(jsonContent, _options);
                if (result == null) { throw new InvalidOperationException("The Config File ist empty or invalid"); }
                return result;
            }catch(JsonException ex)
            {
                Console.WriteLine($"Fehler im JSON!");
                Console.WriteLine($"Nachricht: {ex.Message}");
                Console.WriteLine($"Pfad: {ex.Path}"); // Zeigt z.B. $.sequenceObjects.check_sql_port
                Console.WriteLine($"Zeile: {ex.LineNumber}, Position: {ex.BytePositionInLine}");
                Console.WriteLine($"quelle: {ex.Source}");
                throw;

            }
            
        }
    }
}
