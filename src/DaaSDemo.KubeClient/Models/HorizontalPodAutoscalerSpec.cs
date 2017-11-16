using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DaaSDemo.KubeClient.Models
{
    /// <summary>
    ///     specification of a horizontal pod autoscaler.
    /// </summary>
    public class HorizontalPodAutoscalerSpecV1
    {
        /// <summary>
        ///     upper limit for the number of pods that can be set by the autoscaler; cannot be smaller than MinReplicas.
        /// </summary>
        [JsonProperty("maxReplicas")]
        public int MaxReplicas { get; set; }

        /// <summary>
        ///     lower limit for the number of pods that can be set by the autoscaler, default 1.
        /// </summary>
        [JsonProperty("minReplicas")]
        public int MinReplicas { get; set; }

        /// <summary>
        ///     reference to scaled resource; horizontal pod autoscaler will learn the current resource consumption and will set the desired number of pods by using its Scale subresource.
        /// </summary>
        [JsonProperty("scaleTargetRef")]
        public CrossVersionObjectReferenceV1 ScaleTargetRef { get; set; }
    }
}
