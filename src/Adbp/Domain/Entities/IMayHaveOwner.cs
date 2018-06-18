using System;
using System.Collections.Generic;
using System.Text;

namespace Adbp.Domain.Entities
{
    public interface IMayHaveOwner
    {
        long? OwnerId { get; set; }
    }
}
