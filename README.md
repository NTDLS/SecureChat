# Secure Chat
Secure Chat is a solution to secure private messaging, employing multiple layers of encryption and stream compression.

# Server
The **default** server is hosted by me, but you can install your own server and point to it in the chat client settings window.
However it should be noted that the server is merely a passthough as the chat encryption is end-to-end (client to client).

# Security Blurb
Each client connects to a central server using two pairs of 4096-bit public-private key pairs. The data is initially encrypted with asymmetric 4096-bit AES, where the key is RSA encrypted. When a chat session starts, each peer creates 32 sets of 1024-bit Diffie-Hellman negotiation keys to agree on a single shared private key. This key is then used for symmetric encryption of the messages. The encrypted messages go through all the mentioned layers of encryption and are decrypted directly in the remote peer chat dialog.
Message history is not persistent and is lost when logging out of the chat application.

### Login
![image](https://github.com/user-attachments/assets/dfbccd30-5b19-46db-826b-25f07f6108e9)

### Messaging
![image](https://github.com/user-attachments/assets/ace0f54d-0467-4ba0-aec7-c3baf2dfee07)

### Contacts
![image](https://github.com/user-attachments/assets/4016f170-311a-44b7-9cd0-679d95061cdd)

### Tray Icon
![image](https://github.com/user-attachments/assets/59819cfc-9200-48aa-bd07-02212265e142)

### Settings
![image](https://github.com/user-attachments/assets/e6052b84-f5c5-4eba-87c9-151a698ab735)

