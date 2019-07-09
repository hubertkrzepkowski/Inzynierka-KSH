using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHWP
{

    public class Connection
    {                                     
        public String ConnectionString = null;
        public Connection()
        {
           ConnectionString  = System.IO.File.ReadAllText("ConnectionString.txt");

        }


    }
}
