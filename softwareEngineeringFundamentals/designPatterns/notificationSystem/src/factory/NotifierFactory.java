package factory;

/**
 * Factory class to create Notifier objects based on type.
 * 
 * @author BK Gumede, 223006166
 */
public class NotifierFactory {
    /**
     * Create a Notifier based on the given type.
     * @param type The type of notifier ("EMAIL", "SMS")
     * @return Notifier instance.
     * @throws IllegalArgumentException if unknown type.
     */
    public static Notifier createNotifier(String type) {
        if (type.equalsIgnoreCase("EMAIL")) {
            return new EmailNotifier();
        } else if (type.equalsIgnoreCase("SMS")) {
            return new SMSNotifier();
        } else {
            throw new IllegalArgumentException("Unknown notifier type: " + type);
        }
    }
}
