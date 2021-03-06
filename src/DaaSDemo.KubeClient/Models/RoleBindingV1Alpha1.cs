using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DaaSDemo.KubeClient.Models
{
    /// <summary>
    ///     RoleBinding references a role, but does not contain it.  It can reference a Role in the same namespace or a ClusterRole in the global namespace. It adds who information via Subjects and namespace information by which namespace it exists in.  RoleBindings in a given namespace only have effect in that namespace.
    /// </summary>
    public class RoleBindingV1Alpha1 : KubeResourceV1
    {
        /// <summary>
        ///     RoleRef can reference a Role in the current namespace or a ClusterRole in the global namespace. If the RoleRef cannot be resolved, the Authorizer must return an error.
        /// </summary>
        [JsonProperty("roleRef")]
        public RoleRefV1Alpha1 RoleRef { get; set; }

        /// <summary>
        ///     Subjects holds references to the objects the role applies to.
        /// </summary>
        [JsonProperty("subjects", NullValueHandling = NullValueHandling.Ignore)]
        public List<SubjectV1Alpha1> Subjects { get; set; } = new List<SubjectV1Alpha1>();
    }
}
