import javafx.application.Application;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.layout.GridPane;
import javafx.scene.layout.VBox;
import javafx.scene.text.Font;
import javafx.scene.text.FontWeight;
import javafx.stage.FileChooser;
import javafx.stage.Stage;

import java.io.*;
import java.net.*;

public class UDPClient extends Application {

    private DatagramSocket dSocket;
    private InetAddress serverAddr;
    private int serverPort = 1234;

    private GridPane togglePane;
    private Button btnSwitch;
    private String mode = "L";

    private GridPane lGridpane;
    private Label lblHost;
    private Label lblPort;
    private TextField txtHost;
    private TextField txtPort;
    private Button btnConnect;
    private Button btnRetrList;
    private Button btnDownLoad;
    private ObservableList<String> fileList;
    private ListView<String> listView;

    private GridPane sGridpane;
    private Button btnAddFile;
    private Button btnHostFiles;
    private Button btnRemoveFile;

    public static void main(String[] args) {
        launch(args);
    }

    @Override
    public void start(Stage primaryStage) throws Exception {
        primaryStage.setTitle("UDP client");

        // initialize panes (assign to instance fields)
        togglePane = createPane();
        sGridpane = createPane();
        lGridpane = createPane();

        setUIControls(lGridpane, sGridpane, togglePane, primaryStage);

        VBox vbox = new VBox();
        vbox.setSpacing(10);
        vbox.setPadding(new Insets(10, 10, 10, 10));
        vbox.getChildren().addAll(togglePane, sGridpane, lGridpane);

        Scene scene = new Scene(vbox, 800, 600);
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    private void setUIControls(GridPane lgridpane, GridPane sgridpane, GridPane tgridpane, Stage primaryStage) {

        btnSwitch = new Button("Switch Mode");
        Label theaderLabel = new Label("Select a Mode:");
        theaderLabel.setFont(Font.font("Arial", FontWeight.BOLD, 16));

        fileList = FXCollections.observableArrayList();
        listView = new ListView<>(fileList);
        listView.setPrefHeight(250);

        tgridpane.add(theaderLabel, 0, 0, 1, 1);
        tgridpane.add(btnSwitch, 0, 1);
        tgridpane.add(listView, 0, 2);

        lblHost = new Label("Host:");
        lblPort = new Label("Port:");
        txtHost = new TextField();
        txtPort = new TextField();
        btnConnect = new Button("Connect");
        btnRetrList = new Button("Retrieve List");
        btnDownLoad = new Button("Download");

        Label lheaderLabel = new Label("Leecher");
        lheaderLabel.setFont(Font.font("Arial", FontWeight.BOLD, 16));
        lgridpane.add(lheaderLabel, 0, 0, 1, 1);

        lgridpane.add(lblHost, 0, 1);
        lgridpane.add(txtHost, 1, 1);
        lgridpane.add(lblPort, 0, 2);
        lgridpane.add(txtPort, 1, 2);
        lgridpane.add(btnConnect, 0, 3);
        lgridpane.add(btnRetrList, 0, 4);
        lgridpane.add(btnDownLoad, 1, 4);

        btnAddFile = new Button("Add File");
        btnHostFiles = new Button("Host Files");
        btnRemoveFile = new Button("Remove File");

        Label sheaderLabel = new Label("Seeder");
        sheaderLabel.setFont(Font.font("Arial", FontWeight.BOLD, 16));
        sgridpane.add(sheaderLabel, 0, 0, 1, 1);

        sgridpane.add(btnAddFile, 0, 1);
        sgridpane.add(btnHostFiles, 1, 1);
        sgridpane.add(btnRemoveFile, 2, 1);

        // initial visibility
        if (mode.equals("L")) {
            sgridpane.setVisible(false);
            lgridpane.setVisible(true);
        } else {
            lgridpane.setVisible(false);
            sgridpane.setVisible(true);
        }

        btnSwitch.setOnAction((event) -> {
            if (mode.equals("L")) {
                // switching from leecher to seeder
                lgridpane.setVisible(false);
                sgridpane.setVisible(true);
                mode = "S";
            } else {
                // switching from seeder to leecher
                sgridpane.setVisible(false);
                lgridpane.setVisible(true);
                mode = "L";
            }
        });

        // Leecher: Connect to a peer
        btnConnect.setOnAction((event) -> {
            try {
                serverAddr = InetAddress.getByName(txtHost.getText().trim());
                serverPort = Integer.parseInt(txtPort.getText().trim());

                dSocket = new DatagramSocket();
                System.out.println("Started on port: " + dSocket.getLocalPort());

                String response = sendTextMessage("CONNECT");
                System.out.println("Response: " + response);
            } catch (UnknownHostException e) {
                showAlert("Connection error", "Unknown host: " + e.getMessage());
            } catch (NumberFormatException e) {
                showAlert("Input error", "Port must be a number.");
            } catch (SocketException e) {
                showAlert("Socket error", e.getMessage());
            }
        });

        // Leecher: Retrieve file list from peer
        btnRetrList.setOnAction((event) -> {
            if (dSocket == null || serverAddr == null) {
                showAlert("Not connected", "Please connect to a peer first.");
                return;
            }
            String list = sendTextMessage("LIST");
            fileList.clear();
            if (list != null && !list.trim().isEmpty()) {
                String[] items = list.split("#");
                for (String str : items) {
                    if (!str.trim().isEmpty()) fileList.add(str);
                }
            }
            listView.refresh();
        });

        // Leecher: Download selected file
		btnDownLoad.setOnAction((event) -> {
			if (dSocket == null || serverAddr == null) {
				showAlert("Not connected", "Please connect to a peer first.");
				return;
			}
			int idx = listView.getSelectionModel().getSelectedIndex();
			if (idx == -1) {
				showAlert("No selection", "Please select a file in the list to download.");
				return;
			}

			String request = "FILE " + idx;
			try {
				byte[] buffer = request.getBytes();
				DatagramPacket packet = new DatagramPacket(buffer, buffer.length, serverAddr, serverPort);
				dSocket.send(packet);

				// receive the file (single packet)
				buffer = new byte[65507]; // maximum UDP payload size
				packet = new DatagramPacket(buffer, buffer.length);
				dSocket.receive(packet);

				int len = packet.getLength();

				// Ensure the directory exists
				File leecherDir = new File("data/leecher");
				if (!leecherDir.exists()) {
					leecherDir.mkdirs();
				}

				File f = new File(leecherDir, "File_" + idx);
				try (DataOutputStream dos = new DataOutputStream(new FileOutputStream(f))) {
					dos.write(buffer, 0, len);
				}

				showAlert("Download complete", "Saved as: " + f.getAbsolutePath());
			} catch (IOException e) {
				showAlert("I/O error", e.getMessage());
			}
		});

        // Seeder: add files to share
        btnAddFile.setOnAction((event) -> {
            FileChooser fileChooser = new FileChooser();
            File selectedFile = fileChooser.showOpenDialog(primaryStage);
            if (selectedFile != null) {
                String path = selectedFile.getAbsolutePath();
                fileList.add(path);
                listView.refresh();
            }
        });

        // Seeder: remove selected shared file
        btnRemoveFile.setOnAction((event) -> {
            int idx = listView.getSelectionModel().getSelectedIndex();
            if (idx != -1) {
                fileList.remove(idx);
                listView.refresh();
            }
        });

        // Seeder: start hosting the files (runs on a background thread)
        btnHostFiles.setOnAction((event) -> {
            if (fileList.isEmpty()) {
                showAlert("No files", "Add files to host before starting.");
                return;
            }
            // Start hosting in a background thread so UI stays responsive
            Thread hostThread = new Thread(this::runSeederLoop);
            hostThread.setDaemon(true);
            hostThread.start();
            showAlert("Hosting", "Seeder is running on port " + serverPort);
        });
    }

    /**
     * Runs the seeder loop that listens for requests and responds.
     * This runs on its own thread to avoid blocking the JavaFX UI thread.
     */
    private void runSeederLoop() {
        try {
            serverAddr = InetAddress.getLocalHost();
            dSocket = new DatagramSocket(serverPort);
            System.out.println("Seeder started on port: " + dSocket.getLocalPort());

            while (true) {
                byte[] buffer = new byte[65507];
                DatagramPacket packet = new DatagramPacket(buffer, buffer.length);
                dSocket.receive(packet);
                String request = new String(packet.getData(), 0, packet.getLength()).trim().toLowerCase();

                InetAddress requestAddr = packet.getAddress();
                int requestPort = packet.getPort();

                if (request.startsWith("connect")) {
                    String response = "HELLO";
                    byte[] resBuffer = response.getBytes();
                    DatagramPacket resPacket = new DatagramPacket(resBuffer, resBuffer.length, requestAddr, requestPort);
                    dSocket.send(resPacket);
                } else if (request.startsWith("list")) {
                    StringBuilder list = new StringBuilder();
                    for (String item : fileList) {
                        list.append(item).append("#");
                    }
                    byte[] resBuffer = list.toString().getBytes();
                    DatagramPacket resPacket = new DatagramPacket(resBuffer, resBuffer.length, requestAddr, requestPort);
                    dSocket.send(resPacket);
                } else if (request.startsWith("file")) {
                    System.out.println("Request received: " + request);
                    String[] parts = request.split(" ");
                    if (parts.length >= 2) {
                        int fileNo;
                        try {
                            fileNo = Integer.parseInt(parts[1]);
                            if (fileNo >= 0 && fileNo < fileList.size()) {
                                File f = new File(fileList.get(fileNo));
                                byte[] fileBuffer = new byte[(int) Math.min(f.length(), 65507)]; // ensure within UDP limit

                                try (DataInputStream dis = new DataInputStream(new FileInputStream(f))) {
                                    dis.readFully(fileBuffer);
                                }

                                DatagramPacket filePacket = new DatagramPacket(fileBuffer, fileBuffer.length, requestAddr, requestPort);
                                dSocket.send(filePacket);
                            } else {
                                String err = "ERROR: invalid file index";
                                dSocket.send(new DatagramPacket(err.getBytes(), err.getBytes().length, requestAddr, requestPort));
                            }
                        } catch (NumberFormatException | IOException e) {
                            String err = "ERROR: " + e.getMessage();
                            dSocket.send(new DatagramPacket(err.getBytes(), err.getBytes().length, requestAddr, requestPort));
                        }
                    }
                } else {
                    String err = "UNKNOWN COMMAND";
                    dSocket.send(new DatagramPacket(err.getBytes(), err.getBytes().length, requestAddr, requestPort));
                }
            }
        } catch (SocketException e) {
            System.err.println("Seeder socket error: " + e.getMessage());
        } catch (IOException e) {
            System.err.println("Seeder I/O error: " + e.getMessage());
        } finally {
            if (dSocket != null && !dSocket.isClosed()) dSocket.close();
        }
    }

    /**
     * Sends a short text request and waits for a short text response.
     * Returns the response string (or empty string on error).
     */
    private String sendTextMessage(String request) {
        String response = "";
        if (serverAddr == null || dSocket == null) {
            return response;
        }
        try {
            byte[] buffer = request.getBytes();
            DatagramPacket packet = new DatagramPacket(buffer, buffer.length, serverAddr, serverPort);
            dSocket.send(packet);

            buffer = new byte[65507];
            packet = new DatagramPacket(buffer, buffer.length);
            dSocket.receive(packet);
            response = new String(packet.getData(), 0, packet.getLength()).trim();
        } catch (IOException e) {
            System.err.println("sendTextMessage error: " + e.getMessage());
        }
        return response;
    }

    private GridPane createPane() {
        GridPane gridPane = new GridPane();
        gridPane.setAlignment(Pos.CENTER);
        gridPane.setHgap(20);
        gridPane.setVgap(20);
        return gridPane;
    }

    private void showAlert(String title, String message) {
        // Simple alert helper for the UI thread
        javafx.application.Platform.runLater(() -> {
            Alert alert = new Alert(Alert.AlertType.INFORMATION);
            alert.setTitle(title);
            alert.setHeaderText(null);
            alert.setContentText(message);
            alert.showAndWait();
        });
    }
}
