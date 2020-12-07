using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Api.Service
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }

        public bool Revert { get; set; }

        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
            Revert = revert;
        }
    }
}
