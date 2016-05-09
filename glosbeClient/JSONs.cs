using System.Runtime.Serialization;

namespace glosbeClient
{
    [DataContract]
    public class Response
    {
        [DataMember(Name = "result")]
        public string Result;

        [DataMember(Name = "tuc")]
        public Tuc[] Tuc;
    }

    [DataContract(Name = "tuc")]
    public class Tuc
    {
        [DataMember(Name = "phrase")]
        public Phrase Phrase;
    }

    [DataContract(Name = "phrase")]
    public class Phrase
    {
        [DataMember(Name = "text")]
        public string Text;
    }

}
