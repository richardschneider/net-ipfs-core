# MultiHash

All hashes in IPFS are encoded with as a [MultiHash](xref:Ipfs.MultiHash), a self-describing hash format. 
The actual hash function used depends on security requirements. The cryptosystem of IPFS is upgradeable, 
meaning that as hash functions are broken, networks can shift to stronger hashes. There is no free lunch, as objects 
may need to be rehashed, or links duplicated. But ensuring that tools built do not assume a pre-defined length of 
hash digest means tools that work with today's hash functions will also work with tomorrows longer hash functions too.

```csharp
var hello = Encoding.UTF8.GetBytes("Hello, world.");
var mh = MultiHash.ComputeHash(hello, "sha2-512);
```

For background information, see [hash concept](https://docs.ipfs.io/guides/concepts/hashes/).

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

The [hashing registry](xref:Ipfs.Registry.HashingAlgorithm) contains the metadata on a hashing algorithm.  You can use
[Register](xref:Ipfs.Registry.HashingAlgorithm.Register*) to add a new hashing algorithm.
