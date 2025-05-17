# Talkster
Talkster is a solution to secure private messaging, employing multiple layers of encryption and stream compression.

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
![image](https://github.com/user-attachments/assets/7ac1f26b-bef6-47e2-9314-e457814cce82)

### Contacts
![image](https://github.com/user-attachments/assets/47612d1c-9736-48f0-81fc-e96add2a6649)

### Messaging
![image](https://github.com/user-attachments/assets/b3f73059-fd74-43a8-8113-4da6b7043f29)

### Tray Icon
![image](https://github.com/user-attachments/assets/0c5f9c6f-fed4-4875-a425-fc5ff3ee5542)

### Voice Chat
![image](https://github.com/user-attachments/assets/11baa0aa-3d98-4697-b33b-856006032819)

### Settings
![image](https://github.com/user-attachments/assets/9f10ed90-739a-487d-9325-af1a9dd2ed62)

### Roots
This saga started in 2001, pictured here is the 5th revision from 2003! 🫣
![WhatsApp Image 2025-05-15 at 12 14 24_2ea7d1d2](https://github.com/user-attachments/assets/9479afa9-b5ca-48b9-835a-02543ea0d32a)
