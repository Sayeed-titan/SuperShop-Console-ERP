using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Infrastructure.Data
{
    public static class FilePaths
    {
        public static string DataRoot { get; } =
            Path.Combine(AppContext.BaseDirectory, "Data");

        public static string PathFor<T>() =>
            Path.Combine(DataRoot, $"{typeof(T).Name.ToLower()}s.json");
    }
}
