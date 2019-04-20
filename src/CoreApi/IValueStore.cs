using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   A basic Put/Get interface.
    /// </summary>
    public interface IValueStore
    {
        /// <summary>
        ///   Gets th value of a key.
        /// </summary>
        /// <param name="key">
        ///   A byte array representing the name of a key.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation that returns
        ///   the value of the key as a byte array.
        /// </returns>
        Task<byte[]> GetAsync(byte[] key, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Tries to get the value of a key.
        /// </summary>
        /// <param name="key">
        ///   A byte array representing the name of a key.
        /// </param>
        /// <param name="value">
        ///   A byte array containing the value of the <paramref name="key"/>
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation that returns
        ///   <b>true</b> if the key exists; otherwise, <b>false</b>.
        /// </returns>
        Task<bool> TryGetAsync(byte[] key, out byte[] value, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Put the value of a key.
        /// </summary>
        /// <param name="key">
        ///   A byte array representing the name of a key.
        /// </param>
        /// <param name="value">
        ///   A byte array containing the value of the <paramref name="key"/>
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation.
        /// </returns>
        Task PutAsync(byte[] key, out byte[] value, CancellationToken cancel = default(CancellationToken));
    }
}
