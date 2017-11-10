using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testConsoleApp.Interfaces
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T record) where T : class;
        T Deserialize<T>(byte[] data) where T : class;
    }
}
