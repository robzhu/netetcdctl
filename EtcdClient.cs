using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace EtcdCtl
{
    public class EtcdClient : IDisposable
    {
        private HttpClient _httpClient;
        private string EtcdKeysRootUrl;

        public EtcdClient( string etcdServerAddress )
        {
            _httpClient = new HttpClient();
            EtcdKeysRootUrl = string.Format( "http://{0}:4001/v2/keys/", etcdServerAddress );
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public bool Put( string relativeServiceDataPath, object serviceData, int ttl = -1 )
        {
            var jsonServiceSettings = JsonConvert.SerializeObject( serviceData );
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add( new KeyValuePair<string, string>( "value", jsonServiceSettings ) );

            if( ttl > 0 )
            {
                keyValues.Add( new KeyValuePair<string, string>( "ttl", ttl.ToString() ) );
            }

            var requestContent = new FormUrlEncodedContent( keyValues );

            string fullServiceDataPath = EtcdKeysRootUrl + relativeServiceDataPath;
            var result = _httpClient.PutAsync( fullServiceDataPath, requestContent ).Result;
            return result.IsSuccessStatusCode;
        }

        public bool Delete( string relativeServiceDataPath )
        {
            string fullServiceDataPath = EtcdKeysRootUrl + relativeServiceDataPath;
            var result = _httpClient.DeleteAsync( fullServiceDataPath ).Result;
            return result.IsSuccessStatusCode;
        }
    }
}
