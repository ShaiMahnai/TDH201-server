using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Managers
{
    public class FilesManager
    {
        public string Read(string _filePath)
        {
            return File.ReadAllText(@"Data/"+_filePath);
        }
        public void Write(string _filePath, object data)
        {
            File.WriteAllText(@"Data/" + _filePath, JsonConvert.SerializeObject(data));
        }
    }
}
