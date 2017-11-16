using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DaaSDemo.KubeClient.Models
{
    /// <summary>
    ///     ReplicaSetStatus represents the current status of a ReplicaSet.
    /// </summary>
    public class ReplicaSetStatusV1Beta1
    {
        /// <summary>
        ///     The number of available replicas (ready for at least minReadySeconds) for this replica set.
        /// </summary>
        [JsonProperty("availableReplicas")]
        public int AvailableReplicas { get; set; }

        /// <summary>
        ///     Represents the latest available observations of a replica set's current state.
        /// </summary>
        [JsonProperty("conditions")]
        public List<ReplicaSetConditionV1Beta1> Conditions { get; set; }

        /// <summary>
        ///     The number of pods that have labels matching the labels of the pod template of the replicaset.
        /// </summary>
        [JsonProperty("fullyLabeledReplicas")]
        public int FullyLabeledReplicas { get; set; }

        /// <summary>
        ///     ObservedGeneration reflects the generation of the most recently observed ReplicaSet.
        /// </summary>
        [JsonProperty("observedGeneration")]
        public int ObservedGeneration { get; set; }

        /// <summary>
        ///     The number of ready replicas for this replica set.
        /// </summary>
        [JsonProperty("readyReplicas")]
        public int ReadyReplicas { get; set; }
    }
}
