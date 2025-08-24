using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SuperShop.Infrastructure.Data
{
    internal static class JsonStore
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static List<T> LoadList<T>()
        {
            Directory.CreateDirectory(FilePaths.DataRoot);
            var path = FilePaths.PathFor<T>();
            if (!File.Exists(path)) return new List<T>();
            var json = File.ReadAllText(path);
            return string.IsNullOrWhiteSpace(json)
                ? new List<T>()
                : (JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>());
        }

        public static void SaveList<T>(List<T> data)
        {
            Directory.CreateDirectory(FilePaths.DataRoot);
            var path = FilePaths.PathFor<T>();
            var json = JsonSerializer.Serialize(data, Options);
            File.WriteAllText(path, json);
        }
    }
}
