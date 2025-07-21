import java.io.IOException;
import java.net.InetAddress;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * Scans every 5th port on localhost from port 1 to 65535, attempting to establish a TCP connection.
 * <p>
 * Connection successes and failures are reported on the console.
 * </p>
 * 
 * @author 223006166, BK GUMEDE
 * @version P00
 */
public class Main {

    /**
     * Main method starts the port scanning process.
     * 
     * @param args command line arguments (not used)
     */
    public static void main(String[] args) {

        System.out.println("Starting port scan on localhost (every 5th port)...");

        // Loop over ports 1 to 65535 stepping by 5
        for (int port = 1; port < 65536; port += 5) {
            try (
                // Try to create a socket connection to localhost on the current port
                Socket socket = new Socket("localhost", port)
            ) {
                // Connection succeeded, print connection details
                System.out.println("Program connected to localhost port: " + port);
                System.out.println("Local port of the connection: " + socket.getLocalPort());
                System.out.println("Remote address of the connection: " + socket.getInetAddress());
                System.out.println();

            } catch (UnknownHostException ex) {
                // Exception if localhost cannot be resolved 
                System.err.println("Could not connect to localhost port: " + port);
            } catch (IOException ex) {
                // Exception for failed connections (most ports will trigger this)
                System.err.println("Could not connect to localhost port: " + port);
            }
        }

        // After scanning ports, display the IP address of the computer
        try {
            String ip = InetAddress.getLocalHost().getHostAddress();
            System.out.println("The computer IP Address is: " + ip);
        } catch (IOException ex) {
            System.out.println("Could not determine IP Address.");
        }

        System.out.println("Scan completed.");
    }
}
