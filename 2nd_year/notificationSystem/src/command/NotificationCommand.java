package command;

import factory.Notifier;
import observer.Observer;

/**
 * Concrete Command to send a notification using a Notifier to a user.
 * 
 * @author BK Gumede, 223006166
 */
public class NotificationCommand implements Command {
    private Notifier notifier;
    private String message;
    private Observer user;

    /**
     * Constructor for NotificationCommand.
     * @param notifier The notifier to send notification.
     * @param message The message to send.
     * @param user The user to notify.
     */
    public NotificationCommand(Notifier notifier, String message, Observer user) {
        this.notifier = notifier;
        this.message = message;
        this.user = user;
    }

    /**
     * Execute sending notification.
     */
    public void execute() {
        notifier.send(message, user);
    }
}
