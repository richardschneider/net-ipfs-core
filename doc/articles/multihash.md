# MultiHash

All hashes in IPFS are encoded as a [MultiHash](xref:Ipfs.MultiHash), a self-describing hash format. 
The actual [hash function](#algorithms) used depends on security requirements; "sha2-256" is todays default.

A multihash is used to identify a [peer](xref:Ipfs.Peer), [key](xref:Ipfs.IKey) and [content](cid.md). 
For background information, see [hash concept](https://docs.ipfs.io/guides/concepts/hashes/).

```csharp
var hello = Encoding.UTF8.GetBytes("Hello world");
var mh = MultiHash.ComputeHash(hello, "sha2-256");
```

## Format

The binary representation consists of the [hash code](xref:Ipfs.MultiHash.Algorithm), the [digest's](xref:Ipfs.MultiHash.Digest) 
length and value. The code and length are encoded as [varints](varint.md).

The textual representation is usually the [Base58](xref:Ipfs.MultiHash.ToBase58*) encoding of the 
binary format. [Base32](xref:Ipfs.MultiHash.ToBase32*) encoding is used when case insensity is required.

From the above example, the following is produced

| Name | Value |
| ---- | ----- |
| hash code | 0x12 |
| digest length | 0x20 |
| digest value | 64ec88ca00b268e5ba1a35678a1b5316d212f4f366b2477232534a8aeca37f3c |
| binary | 12 20 64ec88ca00b268e5ba1a35678a1b5316d212f4f366b2477232534a8aeca37f3c |
| base 58 | QmV8cfu6n4NT5xRr2AHdKxFMTZEJrA44qgrBCr739BN9Wb |
| base 32 | ciqgj3eiziale2hfxindkz4kdnjrnuqs6tzwnmshoizfgsuk5srx6pa |

## Algorithms

IPFS assigns a unique [Name](xref:Ipfs.Registry.HashingAlgorithm.Name) and [Code](xref:Ipfs.Registry.HashingAlgorithm.Code) 
to a hashing algorithm. See [hashtable.csv](https://github.com/multiformats/multicodec/blob/master/table.csv") 
for the currently defined hashing algorithms.

These algorithms are implemented:

- blake2b-160, blake2b-256 blake2b-384 and blake2b-512
- blake2s-128, blake2s-160, blake2s-224 a nd blake2s-256
- keccak-224, keccak-256, keccak-384 and keccak-512
- md4 and md5
- sha1
- sha2-256, sha2-512 and dbl-sha2-256
- sha3-224, sha3-256, sha3-384 and sha3-512
- shake-128 and shake-256

The identity hash is also implemented; which just returns the input bytes. This is used to inline a small amount of 
data into a [CID](cid.md).

## Registry

The [hashing registry](xref:Ipfs.Registry.HashingAlgorithm) contains the metadata on hashing algorithms.  You can use
[Register](xref:Ipfs.Registry.HashingAlgorithm.Register*) to add a new hashing algorithm.

### Example

Using an hashing algorithm.  Note that `ComputeHash` can take a byte array or a `Stream`.

```csharp
public void GetHasher()
{
    using (var hasher = HashingAlgorithm.GetAlgorithm("sha3-256"))
    {
        Assert.IsNotNull(hasher);
        var input = new byte[] { 0xe9 };
        var expected = "f0d04dd1e6cfc29a4460d521796852f25d9ef8d28b44ee91ff5b759d72c1e6d6".ToHexBuffer();

        var actual = hasher.ComputeHash(input);
        CollectionAssert.AreEqual(expected, actual);
    }
}
```
