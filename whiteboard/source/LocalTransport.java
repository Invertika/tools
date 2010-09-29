import java.util.Vector;

public class LocalTransport extends Transport {
    
    private ObjectStore os = new ObjectStore();
    
    public void addShape(Tool.Shape s) {
	String str = s.shapeToString();
	if (str == null) throw new RuntimeException();
	System.out.println(str);
	os.add(str);
	notifyWhiteboard();
    }

    public Vector getAllShapes() {
	return os.getAllShapes();
    }

    public void start() {
	notifyWhiteboard();
    }
    public void stop() {
	os.clear();
	notifyWhiteboard();
    }

    public String getStatus() {
	return null;
    }
}
