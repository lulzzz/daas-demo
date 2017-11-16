using HTTPlease;
using KubeNET.Swagger.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Net;

namespace DaaSDemo.KubeClient.Clients
{
    using Models;

    /// <summary>
    ///     A client for the Kubernetes Pods (v1) API.
    /// </summary>
    public class PodClientV1
        : KubeResourceClient
    {
        /// <summary>
        ///     Create a new <see cref="PodClientV1"/>.
        /// </summary>
        /// <param name="client">
        ///     The Kubernetes API client.
        /// </param>
        public PodClientV1(KubeApiClient client)
            : base(client)
        {
        }

        /// <summary>
        ///     Get all Pods in the specified namespace, optionally matching a label selector.
        /// </summary>
        /// <param name="labelSelector">
        ///     An optional Kubernetes label selector expression used to filter the Pods.
        /// </param>
        /// <param name="kubeNamespace">
        ///     The target Kubernetes namespace (defaults to <see cref="KubeApiClient.DefaultNamespace"/>).
        /// </param>
        /// <param name="cancellationToken">
        ///     An optional <see cref="CancellationToken"/> that can be used to cancel the request.
        /// </param>
        /// <returns>
        ///     The Pods, as a list of <see cref="V1Pod"/>s.
        /// </returns>
        public async Task<List<V1Pod>> List(string labelSelector = null, string kubeNamespace = null, CancellationToken cancellationToken = default)
        {
            V1PodList matchingPods =
                await Http.GetAsync(
                    Requests.Collection.WithTemplateParameters(new
                    {
                        Namespace = kubeNamespace ?? Client.DefaultNamespace,
                        LabelSelector = labelSelector
                    }),
                    cancellationToken: cancellationToken
                )
                .ReadContentAsAsync<V1PodList, UnversionedStatus>();

            return matchingPods.Items;
        }

        /// <summary>
        ///     Watch for events relating to Pods.
        /// </summary>
        /// <param name="labelSelector">
        ///     An optional Kubernetes label selector expression used to filter the Pods.
        /// </param>
        /// <param name="kubeNamespace">
        ///     The target Kubernetes namespace (defaults to <see cref="KubeApiClient.DefaultNamespace"/>).
        /// </param>
        /// <returns>
        ///     An <see cref="IObservable{T}"/> representing the event stream.
        /// </returns>
        public IObservable<ResourceEventV1<V1Pod>> WatchAll(string labelSelector = null, string kubeNamespace = null)
        {
            return ObserveEvents<V1Pod>(
                Requests.Collection.WithTemplateParameters(new
                {
                    Namespace = kubeNamespace ?? Client.DefaultNamespace,
                    LabelSelector = labelSelector,
                    Watch = true
                })
            );
        }

        /// <summary>
        ///     Get the Pod with the specified name.
        /// </summary>
        /// <param name="name">
        ///     The name of the Pod to retrieve.
        /// </param>
        /// <param name="kubeNamespace">
        ///     The target Kubernetes namespace (defaults to <see cref="KubeApiClient.DefaultNamespace"/>).
        /// </param>
        /// <param name="cancellationToken">
        ///     An optional <see cref="CancellationToken"/> that can be used to cancel the request.
        /// </param>
        /// <returns>
        ///     A <see cref="V1Pod"/> representing the current state for the Pod, or <c>null</c> if no Pod was found with the specified name and namespace.
        /// </returns>
        public async Task<V1Pod> Get(string name, string kubeNamespace = null, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'name'.", nameof(name));
            
            return await GetSingleResource<V1Pod>(
                Requests.ByName.WithTemplateParameters(new
                {
                    Name = name,
                    Namespace = kubeNamespace ?? Client.DefaultNamespace
                }),
                cancellationToken: cancellationToken
            );
        }

        /// <summary>
        ///     Get the combined logs for the Pod with the specified name.
        /// </summary>
        /// <param name="name">
        ///     The name of the target Pod.
        /// </param>
        /// <param name="kubeNamespace">
        ///     The target Kubernetes namespace (defaults to <see cref="KubeApiClient.DefaultNamespace"/>).
        /// </param>
        /// <param name="limitBytes">
        ///     Limit the number of bytes returned.
        /// </param>
        /// <param name="cancellationToken">
        ///     An optional <see cref="CancellationToken"/> that can be used to cancel the request.
        /// </param>
        /// <returns>
        ///     A string containing the logs.
        /// </returns>
        public async Task<string> Logs(string name, string kubeNamespace = null, int? limitBytes = null, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'name'.", nameof(name));
            
            HttpResponseMessage responseMessage = await Http.GetAsync(
                Requests.Logs.WithTemplateParameters(new
                {
                    Name = name,
                    Namespace = kubeNamespace ?? Client.DefaultNamespace,
                    LimitBytes = limitBytes
                }),
                cancellationToken
            );
            using (responseMessage)
            {
                if (responseMessage.IsSuccessStatusCode)
                    return await responseMessage.Content.ReadAsStringAsync();

                throw new HttpRequestException<UnversionedStatus>(responseMessage.StatusCode,
                    response: await responseMessage.ReadContentAsAsync<UnversionedStatus, UnversionedStatus>()
                );
            }
        }

        /// <summary>
        ///     Request creation of a <see cref="Pod"/>.
        /// </summary>
        /// <param name="newPod">
        ///     A <see cref="V1Pod"/> representing the Pod to create.
        /// </param>
        /// <param name="cancellationToken">
        ///     An optional <see cref="CancellationToken"/> that can be used to cancel the request.
        /// </param>
        /// <returns>
        ///     A <see cref="V1Pod"/> representing the current state for the newly-created Pod.
        /// </returns>
        public async Task<V1Pod> Create(V1Pod newPod, CancellationToken cancellationToken = default)
        {
            if (newPod == null)
                throw new ArgumentNullException(nameof(newPod));
            
            return await Http
                .PostAsJsonAsync(
                    Requests.Collection.WithTemplateParameters(new
                    {
                        Namespace = newPod?.Metadata?.Namespace ?? Client.DefaultNamespace
                    }),
                    postBody: newPod,
                    cancellationToken: cancellationToken
                )
                .ReadContentAsAsync<V1Pod, UnversionedStatus>();
        }

        /// <summary>
        ///     Request deletion of the specified Pod.
        /// </summary>
        /// <param name="name">
        ///     The name of the Pod to delete.
        /// </param>
        /// <param name="kubeNamespace">
        ///     The target Kubernetes namespace (defaults to <see cref="KubeApiClient.DefaultNamespace"/>).
        /// </param>
        /// <param name="cancellationToken">
        ///     An optional <see cref="CancellationToken"/> that can be used to cancel the request.
        /// </param>
        /// <returns>
        ///     An <see cref="UnversionedStatus"/> indicating the result of the request.
        /// </returns>
        public async Task<UnversionedStatus> Delete(string name, string kubeNamespace = null, CancellationToken cancellationToken = default)
        {
            return await Http
                .DeleteAsync(
                    Requests.ByName.WithTemplateParameters(new
                    {
                        Name = name,
                        Namespace = kubeNamespace ?? Client.DefaultNamespace
                    }),
                    cancellationToken: cancellationToken
                )
                .ReadContentAsAsync<UnversionedStatus, UnversionedStatus>(HttpStatusCode.OK, HttpStatusCode.NotFound);
        }

        /// <summary>
        ///     Request templates for the Pods (v1) API.
        /// </summary>
        public static class Requests
        {
            /// <summary>
            ///     A collection-level Pod (v1) request.
            /// </summary>
            public static readonly HttpRequest Collection = HttpRequest.Factory.Json("api/v1/namespaces/{Namespace}/pods?labelSelector={LabelSelector?}&watch={Watch?}", SerializerSettings);

            /// <summary>
            ///     A get-by-name Pod (v1) request.
            /// </summary>
            public static readonly HttpRequest ByName = HttpRequest.Factory.Json("api/v1/namespaces/{Namespace}/pods/{Name}", SerializerSettings);

            /// <summary>
            ///     A get-logs Pod (v1) request.
            /// </summary>
            public static readonly HttpRequest Logs = ByName.WithRelativeUri("log?limitBytes={LimitBytes?}");
        }
    }
}
