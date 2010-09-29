import java.awt.*;

public  class TextTool extends Tool {

    public void paint(Graphics g, int pos, boolean active) {
	super.paint(g, pos, active);
	g.drawLine(3, pos*20+3,15, pos*20+3);
	g.drawLine(9, pos*20+3,9, pos*20+15);
    }

    public Shape createShape(WhiteboardContext w, int x, int y, Color c) {
	return new TextShape(x,y, c, w.getText());
    }

    public Shape shapeFromString(String s) {
	if (s.charAt(0) != getToolID()) return null;
	int pos = s.indexOf(";");
	if (pos == -1) return null;
	int[] ts = tokenize(s.substring(1, pos));
	if (ts.length <3) return null;
	String text = hexdecode(s.substring(pos+1));
	return new TextShape(ts[0], ts[1], new Color(ts[2]), text);
    }

    public char getToolID() {
	return 't';
    }

    public static class TextShape extends Shape {

	Color c;
	int x,y;
	String text;

	public TextShape(int x, int y, Color c, String text) {
	    if (text.indexOf("\0") != -1 || text.indexOf("\n") != -1)
		throw new RuntimeException("Invalid text");
	    this.c=c;
	    this.x=x;
	    this.y=y;
	    this.text=text;
	}
	
	public void paint(WhiteboardContext w, Graphics g) {
	    int offsX=w.getXOffset();
	    int offsY=w.getYOffset();
	    g.setColor(c);
	    g.drawString(text, x+offsX, y+offsY);
	}

	public void updatePoint(WhiteboardContext w, int x, int y) {
	    this.x=x;
	    this.y=y;
	}
	public void endPoint(WhiteboardContext w, int x, int y) {
	    this.x=x;
	    this.y=y;
	}

	public String shapeToString() {
	    return "t"+x+","+y+","+c.getRGB()+";"+hexencode(text);
	}

    }

    public static String hexencode(String s) {
	StringBuffer sb = new StringBuffer();
	for (int i=0; i<s.length();i++) {
	    String hex = "0000"+Integer.toHexString(s.charAt(i));
	    hex=hex.substring(hex.length()-4);
	    sb.append(hex);
	}
	return sb.toString();
    }

    public static String hexdecode(String s) {
	StringBuffer sb = new StringBuffer();
	for (int i=0; i<s.length()-3;i+=4) {
	    try {
		sb.append((char)Integer.parseInt(s.substring(i,i+4), 16));
	    } catch (NumberFormatException ex) {
		ex.printStackTrace();
	    }
	}
	return sb.toString();	
    }
}
