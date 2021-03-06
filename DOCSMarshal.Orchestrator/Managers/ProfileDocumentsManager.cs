﻿using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DocsMarshal.Entities;
using Newtonsoft.Json;

namespace DocsMarshal.Orchestrator.Managers
{
    public class ProfileDocumentManager: DocsMarshal.Interfaces.Managers.Profile.IProfileDocumentManager
    {
        private Manager Orchestrator = null;

        public ProfileDocumentManager(DocsMarshal.Orchestrator.Manager manager)
        {
            Orchestrator = manager;

        }

        public void Dispose()
        {
            if (Orchestrator != null) Orchestrator = null;
        }

        public ProfileDocumentResponse GetDefaultDocument(Guid objectId)
        {
            return GetDefaultDocumentAsync(objectId).Result;
        }

        public async Task<ProfileDocumentResponse> GetDefaultDocumentAsync (Guid objectId)
        {
            if (Guid.Empty == objectId) throw new ArgumentNullException("ObjectId cannot be empty");
            string defaultUrl = string.Format("{0}/{1}", Orchestrator.DocsMarshalUrl, "/DMDocuments/GetProfileDefaultDocumentByObjectId");
            var serializedItem = JsonConvert.SerializeObject(new { sessionID= Orchestrator.SessionId, objectID = objectId});
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(defaultUrl, new StringContent(serializedItem, Encoding.UTF8, "application/json"));
                string rit = await response.Content.ReadAsStringAsync();
                var ritO = JsonConvert.DeserializeObject<RootFile>(rit);
                return ritO.result;
            }
        }

        public async Task<FileValue> GetDocumentByExternalFieldId(Guid objectId, string externalId)
        {
            if (Guid.Empty == objectId)
                throw new ArgumentNullException("ObjectId cannot be empty");
            var url = $"{Orchestrator.DocsMarshalUrl}/Profile/GetDocumentByFieldExternalId";
            var serializedItem = JsonConvert.SerializeObject(new
            {
                SessionId = Orchestrator.SessionId,
                ObjectId = objectId,
                FieldExternalId = externalId });
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, new StringContent(serializedItem, Encoding.UTF8, "application/json"));
                return await GetFileFromResponse(response);
            }
        }

        private async Task<FileValue> GetFileFromResponse(HttpResponseMessage response)
        {
            var contentDisposition = response.Headers.GetValues("content-disposition").FirstOrDefault();
            var fileName = "file";
            if (!String.IsNullOrEmpty(contentDisposition))
            {
                var lookFor = "filename=";
                var index = contentDisposition.IndexOf(lookFor, StringComparison.CurrentCultureIgnoreCase);
                if (index >= 0)
                    fileName = contentDisposition.Substring(index + lookFor.Length);
            }
            var content = await response.Content.ReadAsByteArrayAsync();
            return new FileValue
            {
                FileName = fileName,
                Content = content
            };
        }

        public Task<FileValue> GetDocumentByFieldId(Guid DmObjectId, int fieldId)
        {
            throw new NotImplementedException();
        }

        class RootFile
        {
            public DocsMarshal.Entities.ProfileDocumentResponse result { get; set; }
        }
    }
}
