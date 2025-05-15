# Secure Chat
Secure Chat is a solution to secure private messaging, employing multiple layers of encryption and stream compression.

# Server
The **default** server is hosted by me, but you can install your own server and point to it in the chat client settings window.
However it should be noted that the server is merely a passthough as the chat encryption is end-to-end (client to client).

# Security Blurb
Each client connects to a central server using two pairs of 4096-bit public-private key pairs. The data is initially encrypted with asymmetric 4096-bit AES, where the key is RSA encrypted. When a chat session starts, each peer creates 32 sets of 1024-bit Diffie-Hellman negotiation keys to agree on a single shared private key. This key is then used for symmetric encryption of the messages. The encrypted messages go through all the mentioned layers of encryption and are decrypted directly in the remote peer chat dialog.
Message history is not persistent and is lost when logging out of the chat application.

### Login
![image](https://github.com/user-attachments/assets/ef329f08-2276-44a4-b563-b1124306eecb)

### Contacts
![image](https://github.com/user-attachments/assets/0694ba0d-61d0-49f9-8149-a23b96c526a1)

### Messaging
![image](https://github.com/user-attachments/assets/b6e696aa-1891-4610-bcfa-f6d750af13e9)

### Tray Icon
![image](https://github.com/user-attachments/assets/0c5f9c6f-fed4-4875-a425-fc5ff3ee5542)

### Voice Chat
![image](https://github.com/user-attachments/assets/74aa49b7-21bd-4ded-ae78-21b8292320c4)

### Settings
![image](https://github.com/user-attachments/assets/ea8d8ce8-4bfe-4916-82e4-0e08c768d26c)
