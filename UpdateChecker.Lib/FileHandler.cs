using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UpdateChecker.Lib
{
    static public class FileHandler
    {
        static public bool Save(string path, object file)
        {
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, file);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return true;
            }
        }

        static public object Read(string path)
        {
            object obj = null;
            try
            {
                using (FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    if (stream.Length > 0)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        obj = formatter.Deserialize(stream);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return obj;
        }
    }
}