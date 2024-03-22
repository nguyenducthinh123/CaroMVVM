using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class Setting : Document
    {
        string path;
        public Setting()
        {
            if (path == null)
            {
                path = Environment.CurrentDirectory + "/config.json";
            }
            try
            {
                var content = System.IO.File.ReadAllText(path);
                var doc = Parse(content);
                Copy(doc);
            }
            catch
            {
                Name = "Debug";
                Size = 21;
                CellSize = 20;
            }
        }
        public void Save(Document src) => System.IO.File.WriteAllText(path, src.ToString());
    }
}
