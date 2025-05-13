# Secure Chat
Secure Chat is a solution to secure private messaging, employing multiple layers of encryption and stream compression.

# Server
The **default** server is hosted by me, but you can install your own server and point to it in the chat client settings window.
However it should be noted that the server is merely a passthough as the chat encryption is end-to-end (client to client).

# Security Blurb
Each client connects to a central server using two pairs of 4096-bit public-private key pairs. The data is initially encrypted with asymmetric 4096-bit AES, where the key is RSA encrypted. When a chat session starts, each peer creates 32 sets of 1024-bit Diffie-Hellman negotiation keys to agree on a single shared private key. This key is then used for symmetric encryption of the messages. The encrypted messages go through all the mentioned layers of encryption and are decrypted directly in the remote peer chat dialog.
Message history is not persistent and is lost when logging out of the chat application.

### Login
![image](https://github.com/user-attachments/assets/ca709763-b5a1-41d6-a950-39935a8421a1)

### Messaging
![image](https://github.com/user-attachments/assets/eafb793c-cdb2-427b-9c6e-933048442f01)

### Contacts
![image](https://github.com/user-attachments/assets/3a1ad18d-5d01-4d76-a0e6-42c60aa52d08)

### Tray Icon
![image](https://github.com/user-attachments/assets/0c5f9c6f-fed4-4875-a425-fc5ff3ee5542)

### Voice Chat
![image](https://github.com/user-attachments/assets/5b279caf-f390-4f35-90ae-0ed407c3be00)

### Settings
![image](https://github.com/user-attachments/assets/5382a51e-38ef-4652-9ae9-036adb9171fc)
