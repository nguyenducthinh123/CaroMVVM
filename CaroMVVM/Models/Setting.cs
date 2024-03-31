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
                path = Environment.CurrentDirectory + "/config.json"; // trả về thư mục làm việc hiện tại của ứng dụng, sẽ gặp lỗi khi build Window App
                //path = AppDomain.CurrentDomain.BaseDirectory + "\\config.json"; // đây là đường dẫn tuyệt đối, trả về thư mục gốc chứa tệp thực thi (.exe)

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
