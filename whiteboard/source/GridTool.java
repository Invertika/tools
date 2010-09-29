import java.awt.*;

public  class GridTool extends Tool {

    private static final int GRIDWIDTH=20;

    public void paint(Graphics g, int pos, boolean active) {
	super.paint(g, pos, active);
	g.drawLine(9, pos*20+3,9, pos*20+12);
	g.drawLine(6, pos*20+3,6, pos*20+12);
	g.drawLine(3, pos*20+6,12, pos*20+6);
	g.drawLine(3, pos*20+9,12, pos*20+9);
    }

    public Shape createShape(WhiteboardContext w, int x, int y, Color c) {
	return new GridShape(x,y,c);
    }

    public Shape shapeFromString(String s) {
	if (s.charAt(0) != getToolID()) return null;
	int[] ts = tokenize(s.substring(1));
	return new GridShape(ts[0], ts[1], ts[2], ts[3],
			     new Color(ts[4]));
    }


    public char getToolID() {
	return 'G';
    }

    public static class GridShape extends Shape {

	Color c;
	int x, y, w, h;

	public GridShape(int x, int y, Color c) {
	    this.c=c;
	    this.x=x;
	    this.y=y;
	    w=h=GRIDWIDTH;
	}

	public GridShape(int x, int y, int w, int h, Color c) {
	    this.c=c;
	    this.x=x;
	    this.y=y;
	    this.w=w;
	    this.h=h;
	}
	
	public void paint(WhiteboardContext wb, Graphics g) {
	    g.setColor(c);
	    if (w > 0) {
		int baseX=(x+wb.getXOffset()) % w;
		for (int i=baseX;i<Whiteboard.WIDTH; i+=w) {
		    g.drawLine(i,0,i,Whiteboard.HEIGHT);
		}
	    }
	    if (h > 0) {
		int baseY=(y+wb.getYOffset()) % h;
		for (int i=baseY;i<Whiteboard.HEIGHT; i+=h) {
		    g.drawLine(0,i,Whiteboard.WIDTH,i);
		}
	    }
	}
	
	public void endPoint(WhiteboardContext wb, int xx, int yy) {
	    w = Math.abs(x-xx);
	    h = Math.abs(y-yy);
	    if (w<GRIDWIDTH/2) w=0;
	    if (h<GRIDWIDTH/2) h=0;
	    if (w==0 && h == 0) w=h=GRIDWIDTH;
	}
	
	public String shapeToString() {
	    return "G"+x+","+y+","+w+","+h+","+c.getRGB();
	}
    }
}
