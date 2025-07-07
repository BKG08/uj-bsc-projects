package factory;

import observer.Observer;

/**
 * Concrete Notifier that sends Email notifications.
 * 
 * @author BK Gumede, 223006166
 */
public class EmailNotifier implements Notifier {
    /**
     * Send email notification.
     * @param message The message content.
     * @param user The user to notify.
     */
    public void send(String message, Observer user) {
        System.out.println("Sending EMAIL: " + message);
        user.update("[Email] " + message);
    }
}
