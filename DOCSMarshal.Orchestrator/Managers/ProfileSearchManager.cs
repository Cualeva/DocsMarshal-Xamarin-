﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DocsMarshal.Entities;
using DocsMarshal.Interfaces.Managers.Profile;
using Newtonsoft.Json;

namespace DocsMarshal.Orchestrator.Managers
{
    public class ProfileSearchManager: DocsMarshal.Interfaces.Managers.Profile.IProfileSearchManager
    {
        private Manager Orchestrator = null;

        public ProfileSearchManager(DocsMarshal.Orchestrator.Manager manager)
        {
            Orchestrator = manager;
            Query = new ProfileQueryManager(manager);
        }

        public IProfileQueryManager Query { get; private set; }

        public Task<Entities.ProfileSearchResult> ByDynAssExternalId(string dynAssExternalId, Guid objectId)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.ProfileSearchResult> ByDynAssExternalId(string dynAssExternalId, List<Guid> objectIds)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.ProfileSearchResult> ByDynAssId(Guid dynAssId, Guid objectId)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.ProfileSearchResult> ByDynAssId(Guid dynAssId, List<Guid> objectIds)
        {
            throw new NotImplementedException();
        }

        public async Task<Entities.ProfileSearchResult> ById(Guid Id)
        {
            CallById c = new CallById();
            c.ObjectId = Id.ToString();
            c.sessionID = Orchestrator.SessionId;
            var serializedItem = JsonConvert.SerializeObject(c);
            using (var client = new HttpClient())
            {
                var url = string.Format("{0}/DMSearch/GetProfileByObjectId", Orchestrator.DocsMarshalUrl);
                var response = await client.PostAsync(url, new StringContent(serializedItem, Encoding.UTF8, "application/json"));
                string rit = response.Content.ReadAsStringAsync().Result;
                //var ritO = JsonConvert.DeserializeObject<Root>(rit);
                //return ritO.Result;
                throw new NotImplementedException();
            }
        }

        private class CallById
        {
            public string sessionID { get; set; }
            public string ObjectId { get; set; }
        }


        public void Dispose()
        {
            if (Query != null) { Query.Dispose(); Query = null; }
            if (Orchestrator != null) Orchestrator = null;
        }
    }
}
