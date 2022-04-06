using System;

namespace Kafka.Dto
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Foo Foo { get; set; }

    }

    public class Foo
    {
        public string Bar { get; set; }
    }
}
