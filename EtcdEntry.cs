
namespace EtcdCtl
{
    //example content from HTTP GET etcd key:
    //{
    //  "action":"get",
    //  "node": {
    //      "key":"/haproxy",
    //      "value":"asdfasdfasdf",
    //      "modifiedIndex":7,
    //      "createdIndex":7
    //}}

    public class EtcdEntry<T>
    {
        public class ValueNode<U>
        {
            public string Key { get; set; }
            public U Value { get; set; }
            public int ModifiedIndex { get; set; }
            public int CreatedIndex { get; set; }
        }

        public string Action { get; set; }
        public ValueNode<T> Node { get; set; }
    }
}
