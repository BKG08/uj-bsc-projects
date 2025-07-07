package observer;

/**
 * Subject interface declares methods to attach, detach and notify observers.
 * 
 * @author BK Gumede, 223006166
 */
public interface Subject {
    /**
     * Attach an observer.
     * @param o The observer to attach.
     */
    void attach(Observer o);

    /**
     * Detach an observer.
     * @param o The observer to detach.
     */
    void detach(Observer o);

    /**
     * Notify all attached observers with a message.
     * @param message The message to send.
     */
    void notifyObservers(String message);
}
