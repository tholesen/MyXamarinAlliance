using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyXamarinAlliance.Controllers
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
}
