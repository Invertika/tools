import java.awt.*;

public  class LineTool extends Tool {

    public void paint(Graphics g, int pos, boolean active) {
	super.paint(g, pos, active);
	g.drawLine(3, pos*20+3,15, pos*20+15);
    }

    public Shape createShape(WhiteboardContext w, int x, int y, Color c) {
	return new LineShape(x,y,c);
    }

    public Shape shapeFromString(String s) {
	if (s.charAt(0) != getToolID()) return null;
	int[] ts = tokenize(s.substring(1));
	return new LineShape(ts[0], ts[1], ts[2], ts[3],
			     new Color(ts[4]));
    }

    public char getToolID() {
	return 'l';
    }

    public static class LineShape extends TwoPointShape {

	public LineShape(int x, int y, Color c) {
	    super(x,y,c);
	}

	public LineShape(int x1, int y1, int x2, int y2, Color c) {
	    super(x1,y1,x2,y2,c);
	}
	
	public void paint(WhiteboardContext w, Graphics g) {
	    int offsX=w.getXOffset();
	    int offsY=w.getYOffset();
	    g.setColor(c);
	    g.drawLine(x1+offsX, y1+offsY, x2+offsX, y2+offsY);
	}
	
	public char getToolID() {
	    return 'l';
	}
    }
}
