# Content ID

A [Cid](xref:Ipfs.Cid) is a self-describing content-addressed identifier for distributed systems. It doesn't indicate *where* 
the content is stored, but it forms a kind of address based on the content itself. 

For background information, see [cid concept](https://docs.ipfs.io/guides/concepts/cid/).

# Encoding

In June 2019 the IPFS ecosystem switched from **base58btc** encoding
to the case-insensitive **base32** encoding. This plays much
better with DNS and URL based systems.
