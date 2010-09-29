import java.awt.*;
import java.util.*;

public abstract class Tool {

    public void paint(Graphics g, int pos, boolean active) {
	g.setColor(Color.black);
	g.drawLine(0, pos*20+19, 20, pos*20+19);
	if (active) {
	    g.drawRect(1, pos*20+1, 17, 16);
	}
    }

    public abstract Shape createShape(WhiteboardContext w, 
				      int x, int y, Color c);

    public abstract Shape shapeFromString(String s);

    public abstract char getToolID();

    public static abstract class Shape {
	public boolean needsRepaint() {
	    return false;
	}

	public void updateState(WhiteboardContext w) {}

	public abstract void paint(WhiteboardContext w, Graphics g);

	public void updatePoint(WhiteboardContext w, int x, int y) {
	    endPoint(w,x,y);
	}

	public abstract void endPoint(WhiteboardContext w, int x, int y);

	public abstract String shapeToString();
	
	/*
	public abstract boolean containsPoint(int x, int y);

	public abstract void moveShape(int deltaX, int deltaY);
	*/
    }

    public static abstract class TwoPointShape extends Shape {
	Color c;
	int x1, y1, x2, y2;

	public abstract char getToolID();

	public TwoPointShape(int x, int y, Color c) {
	    this.c=c;
	    x1=x2=x;
	    y1=y2=y;
	}

	public TwoPointShape(int x1, int y1, int x2, int y2, Color c) {
	    this.c=c;
	    this.x1=x1;
	    this.y1=y1;
	    this.x2=x2;
	    this.y2=y2;
	}
	
	public void endPoint(WhiteboardContext w, int x, int y) {
	    x2=x;
	    y2=y;
	}
	
	public String shapeToString() {
	    return getToolID()+""+x1+","+y1+","+x2+","+y2+","+c.getRGB();
	}
    }
    
    public static class DeleteShape extends Shape {
	Shape toDelete;

	public DeleteShape(Shape s) {
	    toDelete=s;
	}

	public void endPoint(WhiteboardContext w, int x, int y) {
	    throw new RuntimeException("Not supported");
	}

	public void paint(WhiteboardContext w, Graphics g){
	    throw new RuntimeException("Not supported");
	}

	public String shapeToString() {
	    return "~"+toDelete.shapeToString();
	}
    }

    public static int[] tokenize(String s) {
	StringTokenizer st = new StringTokenizer(s, ",");
	int[] result = new int[st.countTokens()];
	int count=0;
	while (st.hasMoreTokens()) {
	    int num=0;
	    try {
	     num = Integer.parseInt(st.nextToken());
	    } catch (NumberFormatException ex) {
		ex.printStackTrace();
	    }
	    result[count++] = num;
	}
	return result;
    }

    public static final Tool[] TOOLS = {
	new MoveTool(), new FreehandTool(),
	new LineTool(), new StraightLineTool(), new BoxTool(),
	new FilledBoxTool(), new EllipseTool(), new FilledEllipseTool(),
	new TextTool(), new GridTool()
    };

    public static final Tool RESET=new ResetTool();

    public static Shape getShapeFromString(String s) {
	if (s.length()==0) return RESET.shapeFromString(s);
	for (int i=0;i<TOOLS.length;i++) {
	    if (s.charAt(0) == TOOLS[i].getToolID()) {
		Tool.Shape sh = TOOLS[i].shapeFromString(s);
		if (sh == null) throw new RuntimeException
				    ("Null shape returned by "+
				     TOOLS[i].getClass());
		return sh;
	    }
	}
	return null;
    }
}
