package observer;

/**
 * Observer interface declares the update method used by subjects to notify observers.
 * 
 * @author BK Gumede, 223006166
 */
public interface Observer {
    /**
     * Receive update from subject.
     * @param message The message sent by the subject.
     */
    void update(String message);
}
