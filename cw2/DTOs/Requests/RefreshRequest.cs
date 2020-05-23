using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw2.DTOs.Requests
{
    public class RefreshRequest
    {
        public Guid refreshToken { set; get; }
    }
}
