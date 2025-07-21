package observer;

import java.util.ArrayList;
import java.util.List;

/**
 * Concrete Subject that maintains a list of observers and notifies them of events.
 * Implements Subject interface.
 * 
 * @author BK Gumede, 223006166
 */
public class NotificationService implements Subject {
    private List<Observer> observers = new ArrayList<>();

    /**
     * Attach an observer.
     * @param o Observer to attach.
     */
    public void attach(Observer o) {
        observers.add(o);
    }

    /**
     * Detach an observer.
     * @param o Observer to detach.
     */
    public void detach(Observer o) {
        observers.remove(o);
    }

    /**
     * Notify all observers with a message.
     * @param message The notification message.
     */
    public void notifyObservers(String message) {
        for (Observer o : observers) {
            o.update(message);
        }
    }
}
