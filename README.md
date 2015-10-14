# net-ipfs-core ![build status](https://ci.appveyor.com/api/projects/status/github/richardschneider/net-ipfs-core?branch=master&svg=true)

The core objects of the [IPFS](https://github.com/ipfs/ipfs) (Inter Planetary File System)  for .Net (C#, VB, F# etc.)

## MultiHash

All hashes in IPFS are encoded with [multihash](https://github.com/jbenet/multihash/), a self-describing hash format. The actual hash function used depends on security requirements. The cryptosystem of IPFS is upgradeable, meaning that as hash functions are broken, networks can shift to stronger hashes. There is no free lunch, as objects may need to be rehashed, or links duplicated. But ensuring that tools built do not assume a pre-defined length of hash digest means tools that work with today's hash functions will also work with tomorrows longer hash functions too.

## MultiAddress

## Base58

Most binary data (objects) in IPFS is represented as a [Base-58](https://en.wikipedia.org/wiki/Base58) string; the BitCoin alphabet is used.

> Base58 is a group of binary-to-text encoding schemes used to represent large integers as alphanumeric text. It is similar to Base64 but has been modified to avoid both non-alphanumeric characters and letters which might look ambiguous when printed. It is therefore designed for human users who manually enter the data, copying from some visual source, but also allows easy copy and paste because a double-click will usually select the whole string. 

