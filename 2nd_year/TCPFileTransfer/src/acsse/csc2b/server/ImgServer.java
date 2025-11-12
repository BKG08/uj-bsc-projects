package acsse.csc2b.server;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

/**
 * Main server class that starts the image server on a specified port.
 * Handles incoming client connections by spawning a new thread for each client.
 *
 * @version P04
 * @author BK, GUMEDE 223006166
 */
public class ImgServer {

    private ServerSocket server;
    private boolean ready = false;

    /**
     * Constructs the server and binds it to the given port.
     *
     * @param port Port number to run the server on
     */
    public ImgServer(int port) {
        try {
            server = new ServerSocket(port);
            ready = true;
            System.out.println("Server started on port " + port);
        } catch (IOException e) {
            System.err.println("Error starting server on port " + port);
            e.printStackTrace();
        }
    }

    /**
     * Starts the server to listen for incoming client connections.
     */
    public void start() {
        while (ready) {
            try {
                Socket clientSocket = server.accept();
                Thread t = new Thread(new ConnectionHandler(clientSocket));
                t.start();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    /**
     * Entry point for the server application.
     *
     * @param args Command-line arguments (not used)
     */
    public static void main(String[] args) {
        ImgServer server = new ImgServer(5432);
        server.start();
    }
}
