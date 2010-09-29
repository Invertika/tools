import java.util.Vector;

public abstract class Transport {

    protected Whiteboard wb;
    public void setWhiteboard(Whiteboard w) {
	wb=w;
    }

    protected void notifyWhiteboard() {
	wb.newShapesAvailable();
    }

    public void _debug_notify() {
	wb.newShapesAvailable();
    }
	
    public abstract void addShape(Tool.Shape s);

    public abstract Vector getAllShapes();

    public abstract void start();
    public abstract void stop();

    public abstract String getStatus();
}
