import java.awt.*;

public  class FilledEllipseTool extends Tool {

    public void paint(Graphics g, int pos, boolean active) {
	super.paint(g, pos, active);
	g.fillOval(3, pos*20+3,12, 12);
    }

    public Shape createShape(WhiteboardContext w, int x, int y, Color c) {
	return new FilledEllipseShape(x,y,c);
    }

    public Shape shapeFromString(String s) {
	if (s.charAt(0) != getToolID()) return null;
	int[] ts = tokenize(s.substring(1));
	return new FilledEllipseShape(ts[0], ts[1], ts[2], ts[3],
				  new Color(ts[4]));
    }


    public char getToolID() {
	return 'E';
    }

    public static class FilledEllipseShape extends TwoPointShape {

	public FilledEllipseShape(int x, int y, Color c) {
	    super(x,y,c);
	}
	
	public FilledEllipseShape(int x1, int y1, int x2, int y2, Color c) {
	    super(x1,y1,x2,y2,c);
	}
		
	public void paint(WhiteboardContext w, Graphics g) {
	    int offsX=w.getXOffset();
	    int offsY=w.getYOffset();
	    int xx1=x1, xx2=x2, yy1=y1, yy2=y2;
	    if (xx1 > xx2) {
		xx1=x2; xx2=x1;
	    }
	    if (yy1 > yy2) {
		yy1=y2; yy2=y1;
	    }
	    g.setColor(c);
	    g.fillOval(xx1+offsX, yy1+offsY, xx2-xx1, yy2-yy1);
	}
	
	public char getToolID() {
	    return 'E';
	}
    }
}
