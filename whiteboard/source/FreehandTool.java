import java.awt.*;
import java.util.*;

public  class FreehandTool extends Tool {

    public void paint(Graphics g, int pos, boolean active) {
	super.paint(g, pos, active);
	g.drawLine(3, pos*20+3,11, pos*20+9);
	g.drawLine(11, pos*20+9,15, pos*20+15);
    }

    public Shape createShape(WhiteboardContext w, int x, int y, Color c) {
	return new FreehandShape(x,y,c);
    }

    public Shape shapeFromString(String s) {
	if (s.charAt(0) != getToolID()) return null;
	int[] ts = tokenize(s.substring(1));
	Vector ps = new Vector();
	for (int i=0; i<ts.length-1;i+=2) {
	    ps.addElement(new Point(ts[i],ts[i+1]));
	}
	return new FreehandShape(ps, new Color(ts[ts.length-1]));
    }

    public char getToolID() {
	return 'f';
    }

    public static class FreehandShape extends Shape {

	Color c;
	Vector points;

	public FreehandShape(int x, int y, Color c) {
	    this.c=c;
	    points=new Vector();
	    points.addElement(new Point(x,y));
	}
	
	public FreehandShape(Vector points, Color c) {
	    this.c=c;
	    this.points=points;
	}
	
	public void paint(WhiteboardContext w, Graphics g) {
	    int offsX=w.getXOffset();
	    int offsY=w.getYOffset();
	    g.setColor(c);
	    Point s = (Point) points.elementAt(0);
	    for (int i=1; i<points.size(); i++) {
		Point e = (Point) points.elementAt(i);
		g.drawLine(s.x+offsX, s.y+offsY,
			   e.x+offsX, e.y+offsY);
		s=e;
	    }
	}

	public void endPoint(WhiteboardContext w, int x, int y) {
	    points.addElement(new Point(x, y));
	}

	public String shapeToString() {
	    StringBuffer sb = new StringBuffer("f");
	    for (int i=0;i<points.size(); i++) {
		Point e = (Point) points.elementAt(i);
		sb.append(e.x+","+e.y+",");
	    }
	    return sb.append(c.getRGB()).toString();
	}

    }
}
