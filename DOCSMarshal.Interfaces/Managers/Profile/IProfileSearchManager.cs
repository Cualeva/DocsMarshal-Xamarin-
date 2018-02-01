﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocsMarshal.Interfaces.Managers.Profile
{
    public interface IProfileSearchManager: IDisposable
    {
        Task<Entities.ProfileSearchResult> ById(Guid Id);
        Task<Entities.ProfileSearchResult> ByDynAssId(Guid dynAssId, Guid objectId);
        Task<Entities.ProfileSearchResult> ByDynAssId(Guid dynAssId, List<Guid> objectIds);
        Task<Entities.ProfileSearchResult> ByDynAssExternalId(string dynAssExternalId, Guid objectId);
        Task<Entities.ProfileSearchResult> ByDynAssExternalId(string dynAssExternalId, List<Guid> objectIds);
        IProfileQueryManager Query { get; }
    }
}
