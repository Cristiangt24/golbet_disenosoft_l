using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolBet.Entities.Common
{
    /// <summary>
    /// Base class for all domain entities.
    /// Provides identity, audit timestamps and logical activation state.
    /// </summary>
    public abstract class AuditableEntity
    {
        public int Id { get; set; }

        /// <summary>Set automatically when the entity is first saved.</summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>Set automatically on every update. Null until first modification.</summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>Logical state. Inactive entities are hidden, not deleted.</summary>
        public bool IsActive { get; set; } = true;
    }
}
