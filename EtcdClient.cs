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

        public string GetString( string path )
        {
            string fullResourcePath = EtcdKeysRootUrl + path;
            var result = _httpClient.GetAsync( fullResourcePath ).Result;
            result.EnsureSuccessStatusCode();

            var contents = result.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<EtcdEntry<string>>( contents ).Node.Value;
        }

        public void PutObject( string path, object value, int ttl = -1 )
        {
            PutString( path, JsonConvert.SerializeObject( value ), ttl );
        }

        public void PutString( string path, string value, int ttl = -1 )
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add( new KeyValuePair<string, string>( "value", value ) );

            if( ttl > 0 )
            {
                keyValues.Add( new KeyValuePair<string, string>( "ttl", ttl.ToString() ) );
            }

            var requestContent = new FormUrlEncodedContent( keyValues );

            string fullResourcePath = EtcdKeysRootUrl + path;
            _httpClient.PutAsync( fullResourcePath, requestContent ).Result.EnsureSuccessStatusCode();
        }

        public void Delete( string path )
        {
            string fullResourcePath = EtcdKeysRootUrl + path;
            _httpClient.DeleteAsync( fullResourcePath ).Result.EnsureSuccessStatusCode();
        }
    }
}
