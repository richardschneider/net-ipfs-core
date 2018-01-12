using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
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
        Task<IKey> RemoveAsync(string name, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Rename the specified key.
        /// </summary>
        /// <param name="oldName">
        ///   The local name of the key.
        /// </param>
        /// <param name="newName">
        ///   The new local name of the key.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A sequence of IPFS keys that were deleted.
        /// </returns>
        Task<IKey> RenameAsync(string oldName, string newName, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Export a key to a PEM encoded password protected PKCS #8 container.
        /// </summary>
        /// <param name="name">
        ///   The local name of the key.
        /// </param>
        /// <param name="password">
        ///   The PEM's password.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///    A task that represents the asynchronous operation. The task's value
        ///    the password protected PEM string.
        /// </returns>
        Task<string> Export(string name, SecureString password, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Import a key from a PEM encoded password protected PKCS #8 container.
        /// </summary>
        /// <param name="name">
        ///   The local name of the key.
        /// </param>
        /// <param name="pem">
        ///   The PEM encoded PKCS #8 container.
        /// </param>
        /// <param name="password">
        ///   The <paramref name="pem"/>'s password.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///    A task that represents the asynchronous operation. The task's value
        ///    the password protected PEM string.
        /// </returns>
        Task<IKey> Import(string name, string pem, SecureString password = null, CancellationToken cancel = default(CancellationToken));
    }
}
