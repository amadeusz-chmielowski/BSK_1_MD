[BSK]
GUI:


Flow:

	Client:
	- Get ip address and port from gui.
	- Create endpoint( ip, port)
	- Socket.BeginConnection
	- Socket.EndConnect 
	Socket not connected:
	-retry
	Socket Connected:
	- Get text/ get file
	Create queue for text/files
	text (synchrounous):
	-send text via socket
	file (synchrounous):
	-send "File {filename.XXX}, size {fileSize} being send"
	-send file byte data
	-send "File {filename.XXX} sent"



	Server:
	- Create local endpoint
	- Craate tcp listener socket
	- Bind endpoint to listener
	- Listen for connection/s
	- Socket.BeginAccept
	- Socket.EndAccept
	- Socket.Recive (synchrounous)
	-Check if massage contains "File {filename.XXX}, size {fileSize}being send"
	Yes:
	-recive data until recived data size is equal to message size.
	-check if postBuffer is equal to "File {filename.XXX} sent"
	No:
	-recive and encode message, to console.

Cipher:

	Client:
	- Generate RSA pub and private key
	- Encrypt rsa keys with SHA-256 function of user-friendly paswd
	- Recive server RSA.pub key
	- Generate psuedorandom session key
	- Encrypt session key with server RSA.pub key
	- Send to server session key
	- Set algorithm type, key size, block size, cipher mode, initial vector
	- Encrypt algorithm type, key size, block size, cipher mode, initial vector with server RSA.pub key
	- Send to server algorithm type, key size, block size, cipher mode, initial vector
	- Create encryptor 



	Server:
	- Generate RSA pub and private key
	- Send RSA.pub key to client
	- Encrypt rsa keys with SHA-256 function of user-friendly paswd
	- Recive session key
	- Decrypt session key with RSA priv key
	- Recive algorithm type, key size, block size, cipher mode, initial vector
	- Decrypt algorithm type, key size, block size, cipher mode, initial vector with RSA priv 	key
	- Create algorith decryptor with decrypted data

