# TCP Image Transfer System (Java)

## Overview

This project implements a TCP-based client–server application that enables uploading, downloading, and listing of image files through socket communication.  
The server manages image storage, while the client provides a JavaFX graphical interface for user interaction.

## Features

- Supports three primary operations: `LIST`, `UP`, and `DOWN`.
- Assigns a sequential ID to each uploaded image.
- Maintains persistent storage using `ImgList.txt` for server-side image metadata.
- JavaFX graphical client for image management.
- Multi-threaded server to handle concurrent client connections.

## Requirements

- **Java SE 17** or later  
- **JavaFX SDK 17** or later  
- Optional IDE: IntelliJ IDEA, Eclipse, or NetBeans  
- A command-line terminal to execute batch files

## Build and Execution Instructions

### Step 1: Clean Previous Builds
Removes old compiled `.class` files:
```1-cleanBin.bat```

### Step 2: Build the Server
Compiles all server-side source files:
```2-buildServer.bat```

### Step 3: Build the Client
Compiles all client-side source files:
```3-buildClient.bat```

### Step 4: Run the Server
Starts the server and waits for client connections

### Step 5: Run the Client
Launches the JavaFX GUI client

## System Operation

1. The client connects to the server through TCP on port 5432.
2. Commands such as LIST, UP, and DOWN are sent to the server.
3. The server responds by listing available images, accepting uploads, or sending requested files.
4. Uploaded images are stored in data/server/, while downloads are saved in data/client/.
5. The server maintains image records in ImgList.txt.

## Notes

- Ensure both data/server/ and data/client/ directories exist before running the application.
- If using an IDE, configure VM options to include:
```--module-path "path_to_javafx_lib" --add-modules javafx.controls,javafx.fxml```
- The GUI window title is: Women's Month Image Client.
- This program is intended for educational use as part of CSC2B practical assessment P04.

## Author

**Name:** BK Gumede  
**Student Number:** 223006166

## License

This project is for educational purposes. You may freely modify and adapt it for your university coursework and personal learning.

