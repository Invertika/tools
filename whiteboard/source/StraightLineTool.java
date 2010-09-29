import java.awt.*;

public  class StraightLineTool extends Tool {

    public void paint(Graphics g, int pos, boolean active) {
	super.paint(g, pos, active);
	g.drawLine(3, pos*20+9,15, pos*20+9);
    }

    public Shape createShape(WhiteboardContext w, int x, int y, Color c) {
	return new StraightLineShape(x,y,c);
    }

    public Shape shapeFromString(String s) {
	if (s.charAt(0) != getToolID()) return null;
	int[] ts = tokenize(s.substring(1));
	return new StraightLineShape(ts[0], ts[1], ts[2], ts[3],
				     new Color(ts[4]));
    }

    public char getToolID() {
	return 's';
    }

    public static class StraightLineShape extends TwoPointShape {

	public StraightLineShape(int x, int y, Color c) {
	    super(x,y,c);
	}

	public StraightLineShape(int x1, int y1, int x2, int y2, Color c) {
	    super(x1,y1,x2,y2,c);
	}
	
	
	public void paint(WhiteboardContext w, Graphics g) {
	    int offsX=w.getXOffset();
	    int offsY=w.getYOffset();
	    g.setColor(c);
	    g.drawLine(x1+offsX, y1+offsY, x2+offsX, y2+offsY);
	}
	
	public void endPoint(WhiteboardContext w, int x, int y) {
	    x2=x;
	    y2=y;
	    if (Math.abs(x2-x1) > Math.abs(y2-y1)) {
		y2=y1;
	    } else {
		x2=x1;
	    }
	}

	public char getToolID() {
	    return 's';
	}
    }    
}
