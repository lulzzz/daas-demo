using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DaaSDemo.KubeClient.Models
{
    /// <summary>
    ///     Initializers tracks the progress of initialization.
    /// </summary>
    public class InitializersV1
    {
        /// <summary>
        ///     Pending is a list of initializers that must execute in order before this object is visible. When the last pending initializer is removed, and no failing result is set, the initializers struct will be set to nil and the object is considered as initialized and visible to all clients.
        /// </summary>
        [JsonProperty("pending")]
        public List<InitializerV1> Pending { get; set; }
    }
}
