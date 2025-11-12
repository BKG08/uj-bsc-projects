# UDP Peer-to-Peer File Sharing

## Overview

This JavaFX program implements a peer-to-peer (P2P) file sharing system using UDP sockets.

Each instance of the program can operate in two modes:
- Seeder – Shares local files and responds to requests from other peers.
- Leecher – Connects to a Seeder to retrieve a list of available files and download selected files.

The same application can switch between Seeder and Leecher modes at runtime.

## Features

- Add, host, and remove files in Seeder mode.
- Connect to a peer and retrieve a file list in Leecher mode.
- Download selected files from a peer.
- Graphical interface using JavaFX.
- Dynamic mode switching without restarting the application.

## Architecture

This program is peer-to-peer:
- Every instance can act as client (Leecher) or server (Seeder).
- Communication is done directly over UDP sockets.
- Protocol commands used:
	- CONNECT – Establishes a connection handshake.
	- LIST – Requests list of files from a Seeder.
	- FILE <index> – Requests a specific file by its index in the list.

Unlike traditional client-server systems, this P2P design allows multiple peers to share files without a central server.

## Requirements

- Java 17 or later
- JavaFX SDK configured in the IDE or classpath
- Basic understanding of UDP networking

## How to Run

1. Compile the program:
```javac UDPClient.java```
	
	Or

	Run batch file:
```build.bat```

2. Launch **two instances** of the program:
- First instance: Start as Seeder (or switch to Seeder mode).
- Second instance: Start as Leecher to connect and download files.

Note: The Leecher must connect after the Seeder has started and hosted files.

## Usage

### Seeder Mode

- Add File: Select files to share.
- Host Files: Start listening for requests from Leechers.
- Remove File: Stop sharing specific files.

### Leecher Mode

- Enter the Seeder's host IP and port.
- Connect: Establish a handshake with the Seeder.
- Retrieve List: View available files on the Seeder.
- Download: Select a file from the list to download it locally.

### Switching Modes

Click Switch Mode to toggle between Leecher and Seeder.

## Notes

- Uses UDP, so file transfers are limited to single-packet files (~64 KB max).
- GUI messages are shown via console; downloaded files are saved in the program directory.
- Designed for educational purposes to demonstrate UDP networking and JavaFX GUI.

## Author

**Name:** BK Gumede
**Student Number:** 223006166

## License

This project is for educational purposes. You may freely modify and adapt it for your university coursework and personal learning.
