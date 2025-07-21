package observer;

/**
 * Concrete observer that implements the Observer interface.
 * Receives updates and processes them.
 * 
 * @author BK Gumede, 223006166
 */
public class User implements Observer {
    private String name;

    /**
     * Constructor for User observer.
     * @param name The user's name.
     */
    public User(String name) {
        this.name = name;
    }

    /**
     * Receive and handle update messages.
     * @param message The message received from subject.
     */
    public void update(String message) {
        System.out.println(name + " received: " + message);
    }
}
