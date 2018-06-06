using System;
using System.Net.Http;

namespace Lykke.Service.Security.Client.AutorestClient //Do not change namespace
{
    public partial class SecurityAPI
    {
        public SecurityAPI(Uri baseUri, HttpClient client) : base(client)
        {
            Initialize();
            BaseUri = baseUri ?? throw new ArgumentNullException("baseUri");
        }
    }
}
