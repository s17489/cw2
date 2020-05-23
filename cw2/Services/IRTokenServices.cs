using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw2.Models;

namespace cw2.Services
{
    public interface IRTokenServices
    {
        void  SetToken(Guid token);
        bool  CheckToken(Guid token);
       
    }
}
