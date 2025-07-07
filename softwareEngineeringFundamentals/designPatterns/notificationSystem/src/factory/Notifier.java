package factory;

import observer.Observer;

/**
 * Notifier interface defines sending notification messages.
 * 
 * @author BK Gumede, 223006166
 */
public interface Notifier {
    /**
     * Send a notification message to a user.
     * @param message The notification message.
     * @param user The observer/user to notify.
     */
    void send(String message, Observer user);
}
