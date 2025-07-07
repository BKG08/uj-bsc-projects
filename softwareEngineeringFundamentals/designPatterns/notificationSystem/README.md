# Notification System (Java)

## Overview

This project implements a **Notification System in Java** using three key design patterns:

- **Observer Pattern**: Manages subscribers (`User`) who receive notifications from the system (`NotificationService`).
- **Factory Pattern**: Dynamically creates different notifier types (`EmailNotifier`, `SMSNotifier`).
- **Command Pattern**: Encapsulates notification requests (`NotificationCommand`) for execution.

The system allows sending notifications to users via different channels (Email, SMS) while demonstrating clean object-oriented design principles.

## Features

- Uses the Observer Pattern for real-time notifications to multiple users.
- Uses the Factory Pattern for flexible notification channel creation.
- Uses the Command Pattern to encapsulate notification actions.
- Includes a batch file for compiling and running
- Structured for beginner-friendly practice with Java design patterns.

## How to Compile, Run, and Generate Javadoc

### Using `build.bat` (Recommended)

1. Ensure Java JDK 17 or higher is installed and on your system `PATH`.
2. Double-click `build.bat` or run:
The batch file will:
- Compile all Java files into the `bin/` folder.
- Run `NotificationSystemDemo`.

## Expected Output

Running
Copy
Edit
Alice received: System maintenance at 2AM!
Bob received: System maintenance at 2AM!
Sending EMAIL: Your order has shipped.
Alice received: [Email] Your order has shipped.
Sending SMS: Your OTP is 123456.
Bob received: [SMS] Your OTP is 123456.

## Author

BK Gumede  
Student Number: 223006166

## License

This project is for educational purposes. You may freely modify and adapt it for your university coursework and personal learning.

## Notes

- This project is designed to clearly demonstrate the Observer, Factory, and Command design patterns in a practical Java system.
- You may extend the system with additional notifier types or a GUI layer for advanced practice.

