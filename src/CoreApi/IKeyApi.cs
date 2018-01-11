using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages cryptographic keys.
    /// </summary>
    /// <remarks>
    ///   <note>
    ///   The Key API is work in progress! There be dragons here.
    ///   </note>
    /// </remarks>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/KEY.md">Key API spec</seealso>
    public interface IKeyApi
    {
        /// <summary>
        ///   Creates a new key.
        /// </summary>
        /// <param name="name">
        ///   The local name of the key.
        /// </param>
        /// <param name="keyType">
        ///   The type of key to create; "rsa" or "ed25519".
        /// </param>
        /// <param name="size">
        ///   The size, in bits, of the key.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   The information on the newly created key.
        /// </returns>
        Task<IKey> CreateAsync(
            string name,
            string keyType,
            int size,
            CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   List all the keys.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A sequence of IPFS keys.
        /// </returns>
        Task<IEnumerable<IKey>> ListAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Delete the specified key.
        /// </summary>
        /// <param name="name">
        ///   The local name of the key.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A sequence of IPFS keys that were deleted.
        /// </returns>
        Task<IEnumerable<IKey>> RemoveAsync(string name, CancellationToken cancel = default(CancellationToken));
    }
}
