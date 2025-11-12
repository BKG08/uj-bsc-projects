package acsse.csc2b.client;

import javafx.application.Platform;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.image.Image;
import javafx.scene.image.ImageView;
import javafx.scene.layout.VBox;
import javafx.stage.FileChooser;
import javafx.stage.Stage;

import java.io.*;
import java.net.Socket;
import java.nio.file.Files;

/**
 * JavaFX GUI for the client to interact with the image server.
 * @author BK, Gumede 223006166
 * @version P04 
 */
public class ClientGUI {

    
    private DataOutputStream out;// Data output stream to send messages and files to the server 
    private DataInputStream in; // Data input stream to receive messages and files from the server 

    /**
     * Initializes the GUI and connects to the server.
     *
     * @param primaryStage Main JavaFX stage
     */
    public void start(Stage primaryStage) {
        try {
            // Connect to the server on localhost and port 5432
            Socket socket = new Socket("localhost", 5432);
            out = new DataOutputStream(socket.getOutputStream());
            in = new DataInputStream(socket.getInputStream());

            VBox root = new VBox(10); // Vertical layout with spacing 10

            // Buttons for user actions
            Button uploadBtn = new Button("Upload Image");
            Button listBtn = new Button("List Images");
            Button downloadBtn = new Button("Download Image");

            // Text area to display list of images
            TextArea listArea = new TextArea();
            listArea.setEditable(false);

            // ImageView to display downloaded images
            ImageView imageView = new ImageView();
            imageView.setFitWidth(400); // limit width to fit the GUI

            // Assign button actions
            uploadBtn.setOnAction(e -> uploadImage());
            listBtn.setOnAction(e -> listImages(listArea));
            downloadBtn.setOnAction(e -> downloadImage(imageView));

            // Add components to layout
            root.getChildren().addAll(uploadBtn, listBtn, listArea, downloadBtn, imageView);

            Scene scene = new Scene(root, 500, 600); // Window size
            primaryStage.setScene(scene);
            primaryStage.setTitle("Women's Month Image Client");
            primaryStage.show();

        } catch (IOException e) {
            System.err.println("GUI couldnt display");
        }
    }

    /**
     * Uploads an image to the server.
     * Prompts the user to select a file, sends the file to the server, and
     * receives a sequential ID assigned by the server. Displays a confirmation alert.
	 *
     */
    private void uploadImage() {
        try {
            FileChooser chooser = new FileChooser();
            chooser.setInitialDirectory(new File("data/client"));
            File file = chooser.showOpenDialog(null);

            if (file != null) {
                // Notify server of upload request
                out.writeUTF("UP");
                out.writeUTF(file.getName());       // send filename
                out.writeLong(file.length());       // send file size
                out.write(Files.readAllBytes(file.toPath())); // send file bytes

                // Receive the assigned sequential ID from the server
                int assignedId = in.readInt();

            }
        } catch (Exception e) {
            System.err.println("Couldnt upload image");
        }
    }

    /**
     * Requests the list of images from the server and displays it.
     *
     * @param listArea TextArea to display image list
     */
    private void listImages(TextArea listArea) {
        try {
            // Send request to server
            out.writeUTF("LIST");

            // Receive server response"
            String data = in.readUTF();
            String[] items = data.split("\n");

            StringBuilder sb = new StringBuilder();
            for (String item : items) {
                sb.append(item).append("\n"); // add each image on a new line
            }
            listArea.setText(sb.toString());
        } catch (Exception e) {
            System.err.println("Couldnt get list of images");
        }
    }

    /**
     * Downloads an image from the server by filename and displays it in the GUI.
	 *
     * Prompts the user to enter the full filename with extension.
     * Downloads the file from the server and saves it in the 'data/client' folder.
	 *
     * @param imageView ImageView to display the downloaded image
     */
    private void downloadImage(ImageView imageView) {
        try {
            String fileName = new TextInputDialog().showAndWait().orElse("");
            if (!fileName.isEmpty()) {
                // Send download request
                out.writeUTF("DOWN " + fileName);

                // Receive file size
                long size = in.readLong();

                if (size > 0) {
                    // Read file bytes
                    byte[] data = new byte[(int) size];
                    in.readFully(data);

                    // Save file to client folder
                    File savedFile = new File("data/client/" + fileName);
                    Files.write(savedFile.toPath(), data);

                    // Display the image in GUI
                    imageView.setImage(new Image(new ByteArrayInputStream(data)));
                } else {
                    System.out.println("Image not found on server.");
                }
            }
        } catch (Exception e) {
            System.err.println("Couldnt download image");
        }
    }
}
