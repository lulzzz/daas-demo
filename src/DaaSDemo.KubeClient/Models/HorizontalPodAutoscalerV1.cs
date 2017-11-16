using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DaaSDemo.KubeClient.Models
{
    /// <summary>
    ///     configuration of a horizontal pod autoscaler.
    /// </summary>
    public class HorizontalPodAutoscalerV1 : KubeResource
    {
        /// <summary>
        ///     Standard object metadata. More info: https://git.k8s.io/community/contributors/devel/api-conventions.md#metadata
        /// </summary>
        [JsonProperty("metadata")]
        public ObjectMetaV1 Metadata { get; set; }

        /// <summary>
        ///     behaviour of autoscaler. More info: https://git.k8s.io/community/contributors/devel/api-conventions.md#spec-and-status.
        /// </summary>
        [JsonProperty("spec")]
        public HorizontalPodAutoscalerSpecV1 Spec { get; set; }

        /// <summary>
        ///     current information about the autoscaler.
        /// </summary>
        [JsonProperty("status")]
        public HorizontalPodAutoscalerStatusV1 Status { get; set; }
    }
}
