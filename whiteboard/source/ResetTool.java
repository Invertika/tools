import java.awt.*;

public  class ResetTool extends Tool {

    public void paint(Graphics g, int pos, boolean active) {
	super.paint(g, pos, active);
	g.drawLine(9, pos*20+3,7, pos*20+9);
	g.drawLine(7, pos*20+9,10, pos*20+8);
	g.drawLine(10, pos*20+8,9, pos*20+15);

	g.drawLine(9, pos*20+15,11, pos*20+13);
	g.drawLine(9, pos*20+15,7, pos*20+13);
    }

    public Shape createShape(WhiteboardContext w, int x, int y, Color c) {
	return new ResetShape(w, c);
    }

    public Shape shapeFromString(String s) {
	if (s.length() == 0) return new ResetShape(Color.white);
	if (s.charAt(0) != getToolID()) return null;
	int[] ts = tokenize(s.substring(1));
	return new ResetShape(new Color(ts[0]));
    }

    public char getToolID() {
	return 'x';
    }

    public static class ResetShape extends Shape {

	Color c;

	public ResetShape(WhiteboardContext w, Color c) {
	    this.c=c;
	}

	public ResetShape(Color c) {
	    this.c=c;
	}

	public boolean needsRepaint() {
	    return true;
	}

	public void updateState(WhiteboardContext w) {
	    w.setXOffset(0);
	    w.setYOffset(0);
	}

	public void paint(WhiteboardContext w, Graphics g) {
	    g.setColor(c);
	    g.fillRect(0, 0, WhiteboardContext.WIDTH, 
		       WhiteboardContext.HEIGHT);
	}

	public void endPoint(WhiteboardContext w, int x, int y) {}

	public String shapeToString() {
	    return "x"+c.getRGB();
	}
    }
}
