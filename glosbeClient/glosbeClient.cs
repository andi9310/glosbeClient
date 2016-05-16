using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;

namespace glosbeClient
{
    public class GlosbeClient
    {
        private readonly LimitedSizeDictionary<string, IEnumerable<string>> _knownTranslations = new LimitedSizeDictionary<string, IEnumerable<string>>(1000);

        public IEnumerable<string> GetTranslations(string word)
        {
            if (_knownTranslations.ContainsKey(word))
            {
                return _knownTranslations[word];
            }
            var response = SendRequest(word);
            if (response == null || response.Result != "ok")
            {
                return new string[] { };
            }
            var translations = from tuc in response.Tuc where tuc.Phrase != null select tuc.Phrase.Text;
            var enumerable = translations as string[] ?? translations.ToArray();
            _knownTranslations.Add(word, enumerable);
            return enumerable;
        }

        public Response SendRequest(string word)
        {
            var requestUrl = $"https://glosbe.com/gapi/translate?from=eng&dest=pol&format=json&phrase={word}&pretty=true";
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (var response = request?.GetResponse() as HttpWebResponse)
                {
                    if (response != null && response.StatusCode != HttpStatusCode.OK)
                        throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
                    var jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                    var stream = response?.GetResponseStream();
                    if (stream == null) return null;
                    var objResponse = jsonSerializer.ReadObject(stream);
                    var jsonResponse = objResponse as Response;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
