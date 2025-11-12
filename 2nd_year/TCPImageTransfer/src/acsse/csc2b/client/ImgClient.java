package acsse.csc2b.client;

import javafx.application.Application;
import javafx.stage.Stage;

/**
 * Main client application class.
 * Launches the JavaFX GUI for interacting with the image server.
 *
 * @version P04
 * @author BK, GUMEDE 223006166
 */
public class ImgClient extends Application {

    @Override
    public void start(Stage primaryStage) throws Exception {
        ClientGUI gui = new ClientGUI();
        gui.start(primaryStage);
    }

    /**
     * Entry point for the client application.
	 *
     * @param args Command-line arguments (not used)
     */
    public static void main(String[] args) {
        launch(args);
    }
}
