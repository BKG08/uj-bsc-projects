# Java Port Scanner

## Overview

This Java Port Scanner:

- Scans every 5th TCP port on `localhost` from ports 1 to 65535.
- Attempts to establish a TCP connection to each port.
- Reports successful connections and failed connection attempts to the console.
- Displays the computer’s IP address after completing the scan.

This project is useful for practicing Java networking and understanding TCP connection principles.

## Author

- 223006166, BK GUMEDE
- Version: P00

## Files

- `Main.java` – Contains the main port scanning logic.
- `build.bat` (located in the `docs` folder) – Compiles and runs the program using the command line.

## Requirements

- Java JDK 8 or higher
- Windows Command Prompt or a compatible terminal

## How to Compile and Run

1. Open Command Prompt.
2. Navigate to your `docs` folder:
    ```cmd
    cd path\to\your\project\docs
    ```
3. Run:
    ```cmd
    build.bat
    ```

The batch file will compile `Main.java` and run the compiled program.

## Expected Output

- Displays connection attempts for every 5th port.
- Indicates whether each connection attempt succeeded or failed.
- Displays your local machine’s IP address.
- Confirms when the scan has completed.

## Example Output

Starting port scan on localhost (every 5th port)...
Could not connect to localhost port: 1
Could not connect to localhost port: 6
Program connected to localhost port: 135
Local port of the connection: 49153
Remote address of the connection: localhost/127.0.0.1

The computer IP Address is: 192.168.1.100
Scan completed.

pgsql
Copy
Edit

## Notes

- Most ports will fail to connect unless services are actively listening on them, which is expected behavior.
- Successful connections indicate open ports on your local machine.
- This scanner runs only on `localhost` (127.0.0.1) and does not scan external hosts.

## License

This project is provided for educational purposes to practice Java networking and basic port scanning concepts.