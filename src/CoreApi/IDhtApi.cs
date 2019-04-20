using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages the Distributed Hash Table.
    /// </summary>
    /// <remarks>
    ///   The DHT is a place to store, not the value, but pointers to peers who have 
    ///   the actual value.
    ///   <para>
    ///   See the ongoing DHT specification at <see href="https://github.com/libp2p/specs/pull/108"/>
    ///   </para>
    /// </remarks>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/DHT.md">DHT API spec</seealso>
    public interface IDhtApi : IPeerRouting, IContentRouting, IValueStore
    {
    }
}
