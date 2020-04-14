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