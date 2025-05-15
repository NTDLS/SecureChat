# Secure Chat
Secure Chat is a solution to secure private messaging, employing multiple layers of encryption and stream compression.

## Want to chat? 🗨️
If you want to chat with me, download the client, navigate to the "Contacts" menu and click "Find People". Search for me: NOP.

## Server ⚙️
The **default** server is hosted by me, but you can install your own server and point the client to your server in the client settings window.
However it should be noted that the server is merely a passthough as the chat encryption is end-to-end (client-to-client).

## Encryption 🛡️
Each client connects to a central server using two pairs of 4096-bit RSA public-private key pairs. Communication begins with data encrypted using symmetric 256-bit AES, with the AES key itself encrypted using RSA for secure key exchange. When a chat session starts, each client generates 32 sets of 1024-bit Diffie-Hellman key pairs (providing a total of 8,192 bits of entropy) to negotiate a single shared secret. This shared key is then used for symmetric end-to-end encryption of all communication. Messages are encrypted using all negotiated layers and decrypted directly within the recipient’s chat dialog, ensuring in-flight confidentiality. Message history is not persistent and is lost upon logging out of the chat application.

## Accounts 🧍
Accounts are stored at the server (which you can easily host, or use mine). The only thing stored is your preferred username, the SHA-256 of your chosen password, and your status (if you choose to have one).

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
