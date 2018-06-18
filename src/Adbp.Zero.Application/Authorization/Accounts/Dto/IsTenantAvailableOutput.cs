using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.Authorization.Accounts.Dto
{
    public class IsTenantAvailableOutput
    {
        public TenantAvailabilityState State { get; set; }

        public int? TenantId { get; set; }

        public IsTenantAvailableOutput()
        {

        }

        public IsTenantAvailableOutput(TenantAvailabilityState state, int? tenantId = null)
        {
            State = state;
            TenantId = tenantId;
        }
    }

    public enum TenantAvailabilityState
    {
        Available = 1,
        InActive,
        NotFound
    }
}
