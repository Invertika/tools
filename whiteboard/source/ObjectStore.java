import java.util.*;

public class ObjectStore {

    private Vector v = new Vector();
    private boolean removeLast=false;
    private Whiteboard wb;

    public Vector getAllShapes() {
	return v;
    }

    public boolean add(String shape) {
	if (removeLast) {
	    removeLast=false;
	    v.removeElementAt(v.size()-1);
	}
	if (shape.startsWith("~")) {
	    for (int i=v.size()-1; i>=0;i--) {
		Tool.Shape s = (Tool.Shape) v.elementAt(i);
		if (shape.equals("~"+s.shapeToString())) {
		    v.removeElementAt(i);
		    return true;
		}
	    }
	    System.out.println("Could not find shape: "+shape);
	    return false;
	}
	Tool.Shape ss = Tool.getShapeFromString(shape);
	if (ss == null) {
	    System.out.println("Unknown shape: "+shape);
	    return false;
	} else {
	    v.addElement(ss);
	    return true;
	}
    }

    public void addTemp(String shape) {
	if (shape.startsWith("~")) return;
	if (!removeLast) {
	    Tool.Shape ss = Tool.getShapeFromString(shape);
	    if (ss == null) return;
	    removeLast=true;
	    v.addElement(ss);
	}
    }

    public void clear() {
	v.clear();
    }
}
