using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Allows you to publish messages to a given topic, and also to
    ///   subscribe to new messages on a given topic.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   This is an experimental feature. It is not intended in its current state
    ///   to be used in a production environment.
    ///   </para>
    ///   <para>
    ///   To use, the daemon must be run with '--enable-pubsub-experiment'.
    ///   </para>
    /// </remarks>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/PUBSUB.md">Pubsub API spec</seealso>
    public interface IPubSubApi
    {
        /// <summary>
        ///   Get the subscribed topics.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   a sequence of <see cref="string"/> for each topic.
        /// </returns>
        Task<IEnumerable<string>> SubscribedTopicsAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get the peers that are pubsubing with us.
        /// </summary>
        /// <param name="topic">
        ///   When specified, only peers subscribing on the topic are returned.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   a sequence of <see cref="Peer"/>.
        /// </returns>
        Task<IEnumerable<Peer>> PeersAsync(string topic = null, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Publish a message to a given topic.
        /// </summary>
        /// <param name="topic">
        ///   The topic name.
        /// </param>
        /// <param name="message">
        ///   The message to publish.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation.
        /// </returns>
        Task PublishAsync(string topic, string message, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Subscribe to messages on a given topic.
        /// </summary>
        /// <param name="topic">
        ///   The topic name.
        /// </param>
        /// <param name="handler">
        ///   The action to perform when a <see cref="IPublishedMessage"/> is received.
        /// </param>
        /// <param name="cancellationToken">
        ///   Is used to stop the topic listener.  When cancelled, the <see cref="OperationCanceledException"/>
        ///   is <b>NOT</b> raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation.
        /// </returns>
        /// <remarks>
        ///   The <paramref name="handler"/> is invoked on the topic listener thread.
        /// </remarks>
        Task SubscribeAsync(string topic, Action<IPublishedMessage> handler, CancellationToken cancellationToken);

    }
}
