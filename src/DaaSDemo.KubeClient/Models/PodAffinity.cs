using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DaaSDemo.KubeClient.Models
{
    /// <summary>
    ///     Pod affinity is a group of inter pod affinity scheduling rules.
    /// </summary>
    public class PodAffinityV1
    {
        /// <summary>
        ///     The scheduler will prefer to schedule pods to nodes that satisfy the affinity expressions specified by this field, but it may choose a node that violates one or more of the expressions. The node that is most preferred is the one with the greatest sum of weights, i.e. for each node that meets all of the scheduling requirements (resource request, requiredDuringScheduling affinity expressions, etc.), compute a sum by iterating through the elements of this field and adding "weight" to the sum if the node has pods which matches the corresponding podAffinityTerm; the node(s) with the highest sum are the most preferred.
        /// </summary>
        [JsonProperty("preferredDuringSchedulingIgnoredDuringExecution")]
        public List<WeightedPodAffinityTermV1> PreferredDuringSchedulingIgnoredDuringExecution { get; set; }
    }
}