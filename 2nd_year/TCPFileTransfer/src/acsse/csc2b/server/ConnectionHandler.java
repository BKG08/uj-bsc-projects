package acsse.csc2b.server;

import java.io.*;
import java.net.Socket;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.List;

/**
 * Handles a single client connection.
 * Supports LIST, UP, and DOWN commands for image management.
 * 
 * @version P04
 * @author BK, GUMEDE 223006166
 */
public class ConnectionHandler implements Runnable {

    private Socket clientConnection;

    // Directory for server images
    private static final String SERVER_DATA = "data/server/";
    private static final String IMAGE_LIST = SERVER_DATA + "ImgList.txt";

    public ConnectionHandler(Socket connection) {
        this.clientConnection = connection;
    }

    @Override
    public void run() {
        try (DataInputStream in = new DataInputStream(clientConnection.getInputStream());
             DataOutputStream out = new DataOutputStream(clientConnection.getOutputStream())) {

            while (true) {
                // Read command from client
                String command = in.readUTF();

                if (command.startsWith("LIST")) {
                    // Send list of images to client
                    List<String> lines = readDataFile();
                    out.writeUTF(String.join("\n", lines));

                } else if (command.startsWith("DOWN")) {
                    // Download request, expects full filename
                    String filename = command.substring(5); // remove "DOWN "
                    sendImage(filename.trim(), out);

                } else if (command.startsWith("UP")) {
                    // Upload request
                    uploadImage(in, out);

                } else {
                    // Unknown command
                    out.writeUTF("FAILURE: Unknown command");
                }
            }

        } catch (IOException e) {
            System.out.println("Client disconnected.");
        }
    }

    /**
     * Reads the ImgList.txt file and returns the lines.
     */
    private List<String> readDataFile() throws IOException {
        File file = new File(IMAGE_LIST);
        if (!file.exists()) {
            // Create server directory and file if they do not exist
            new File(SERVER_DATA).mkdirs();
            file.createNewFile();
            return new ArrayList<>();
        }
        return Files.readAllLines(file.toPath());
    }

    /**
     * Sends an image to the client by filename.
     */
    private void sendImage(String filename, DataOutputStream out) throws IOException {
        File file = new File(SERVER_DATA + filename);
        if (file.exists()) {
            // Read entire file and send length + data
            byte[] data = Files.readAllBytes(file.toPath());
            out.writeLong(data.length);
            out.write(data);
        } else {
            // Send 0 length if file not found
            out.writeLong(0);
        }
    }

    /**
     * Receives an uploaded image from the client, saves it,
     * and appends info to ImgList.txt with a sequential ID.
     */
    private void uploadImage(DataInputStream in, DataOutputStream out) throws IOException {
        // Read filename and file size
        String name = in.readUTF();
        long size = in.readLong();

        // Read file bytes
        byte[] data = new byte[(int) size];
        in.readFully(data);

        // Save file to server folder
        File serverFile = new File(SERVER_DATA + name);
        try (FileOutputStream fos = new FileOutputStream(serverFile)) {
            fos.write(data);
        }

        // Append image info to ImgList.txt
        List<String> lines = readDataFile();
        int id = lines.size() + 1; // sequential ID
        try (BufferedWriter writer = new BufferedWriter(new FileWriter(IMAGE_LIST, true))) {
            writer.write(id + " " + name);
            writer.newLine();
        }

        // Confirm success to client
        out.writeUTF("SUCCESS");
    }
}
