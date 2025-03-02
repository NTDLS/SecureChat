## 👀 THIS IS A WORK IN PROGRESS AND IS NOT YET READY FOR PROCUTION USE 👀

# SecureChat
Secure Chat is a solution to secure private messaging, employing multiple layers of encryption and stream compression.

# Saftey Blurb
Each client connects to a central server using two pairs of 4096-bit public-private key pairs. The data is initially encrypted with asymmetric 4096-bit AES, where the key is RSA encrypted. When a chat session starts, each peer creates 32 sets of 1024-bit Diffie-Hellman negotiation keys to agree on a single shared private key. This key is then used for symmetric encryption of the messages. The encrypted messages go through all the mentioned layers of encryption and are decrypted directly in the remote peer chat dialog.
Message history is not persistent and is lost when logging out of the chat application.
