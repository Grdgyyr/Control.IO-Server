using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Control_IO
{
    public class ControlIO
    {
        public Socket _Socket { get; set; }
        public string _Name { get; set; }

        public ControlIO(Socket socket)
        {
            this._Socket = socket;
        }

    }
}
