package factory;

import observer.Observer;

/**
 * Concrete Notifier that sends SMS notifications.
 * 
 * @author BK Gumede, 223006166
 */
public class SMSNotifier implements Notifier {
    /**
     * Send SMS notification.
     * @param message The message content.
     * @param user The user to notify.
     */
    public void send(String message, Observer user) {
        System.out.println("Sending SMS: " + message);
        user.update("[SMS] " + message);
    }
}
