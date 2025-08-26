using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public static class LoginManager
    {
        static DataTable dt =CRUD.Read("SELECT * FROM Users");
      
    }
}
