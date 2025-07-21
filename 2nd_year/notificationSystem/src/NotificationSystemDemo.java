import observer.NotificationService;
import observer.User;
import factory.Notifier;
import factory.NotifierFactory;
import command.Command;
import command.NotificationCommand;

/**
 * Demo class to showcase the Notification System using Observer, Factory, and Command patterns.
 * 
 * @author BK Gumede, 223006166
 */
public class NotificationSystemDemo {
    public static void main(String[] args) {
        // Create Notification Service (Subject)
        NotificationService service = new NotificationService();

        // Create Users (Observers)
        User alice = new User("Alice");
        User bob = new User("Bob");

        // Attach observers
        service.attach(alice);
        service.attach(bob);

        // Notify all observers
        service.notifyObservers("System maintenance at 2AM!");

        // Use Factory to get Notifiers
        Notifier emailNotifier = NotifierFactory.createNotifier("EMAIL");
        Notifier smsNotifier = NotifierFactory.createNotifier("SMS");

        // Use Command to send notifications
        Command emailCommand = new NotificationCommand(emailNotifier, "Your order has shipped.", alice);
        Command smsCommand = new NotificationCommand(smsNotifier, "Your OTP is 123456.", bob);

        // Execute Commands
        emailCommand.execute();
        smsCommand.execute();
    }
}
