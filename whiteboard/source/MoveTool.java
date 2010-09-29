import java.awt.*;

public  class MoveTool extends Tool {

    public void paint(Graphics g, int pos, boolean active) {
	super.paint(g, pos, active);
	g.drawLine(9, pos*20+3,9, pos*20+15);
	g.drawLine(3, pos*20+9,15, pos*20+9);

	g.drawLine(9, pos*20+3,11, pos*20+5);
	g.drawLine(9, pos*20+3,7, pos*20+5);
	g.drawLine(9, pos*20+15,11, pos*20+13);
	g.drawLine(9, pos*20+15,7, pos*20+13);

	g.drawLine(3, pos*20+9,5, pos*20+11);
	g.drawLine(3, pos*20+9,5, pos*20+7);
	g.drawLine(15, pos*20+9,13, pos*20+11);
	g.drawLine(15, pos*20+9,13, pos*20+7);
    }

    public Shape createShape(WhiteboardContext w, int x, int y, Color c) {
	return new MoveShape(w, x,y,c);
    }

    public Shape shapeFromString(String s) {
	if (s.charAt(0) != getToolID()) return null;
	int[] ts = tokenize(s.substring(1));
	return new MoveShape(ts[0], ts[1]);
    }

    public char getToolID() {
	return 'm';
    }

    public static class MoveShape extends Shape {

	int xoff, yoff;
	int tempX, tempY;

	public MoveShape(WhiteboardContext w, int x, int y, Color c) {
	    tempX=x+w.getXOffset();
	    tempY=y+w.getYOffset();
	    xoff=yoff=0;
	}

	public MoveShape(int xoff, int yoff) {
	    this.xoff=xoff;
	    this.yoff=yoff;
	}

	public boolean needsRepaint() {
	    return true;
	}

	public void updateState(WhiteboardContext w) {
	    w.setXOffset(xoff + w.getXOffset());
	    w.setYOffset(yoff + w.getYOffset());
	}

	public void paint(WhiteboardContext w, Graphics g) {}

	public void endPoint(WhiteboardContext w, int x, int y) {
	    xoff = (x+w.getXOffset()) - tempX;
	    yoff= (y+w.getYOffset()) - tempY;
	}

	public String shapeToString() {
	    return "m"+xoff+","+yoff;
	}
    }
}
