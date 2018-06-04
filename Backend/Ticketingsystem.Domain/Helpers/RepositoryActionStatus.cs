using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketingsystem.Domain.Helpers
{
    public enum RepositoryActionStatus
    {
        Ok,
        Created,
        Updated,
        NotFound,
        Deleted,
        NothingModified,
        PartialContent,
        BadRequest,
        ConcurrencyConflict,
        Error
    }
}
