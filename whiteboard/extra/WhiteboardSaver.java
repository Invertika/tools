import javax.imageio.*;
import java.io.*;
import java.awt.*;
import java.awt.image.*;
import java.util.*;
import java.util.List;

public class WhiteboardSaver implements WhiteboardContext {

    private int xOffset, yOffset;
    public int getXOffset() { return xOffset; }
    public int getYOffset() { return yOffset; }
    
    public void setXOffset(int newValue) {xOffset=newValue; }
    public void setYOffset(int newValue) {yOffset=newValue; }

    public String getText() {
	return "";
    }

    public void savePNG(String filename) throws IOException {
	List shapes = new ArrayList();
	BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
	WhiteboardContext wb=this;
	String line;
	while((line=br.readLine()) != null) {
	    Tool.Shape ss = Tool.getShapeFromString(line);
	    if (ss == null) System.out.println("Unknown shape: "+line);
	    shapes.add(ss);
	}
	//now save all shapes...
	BufferedImage img = new BufferedImage(800,600, BufferedImage.TYPE_INT_RGB);
	Graphics g=img.getGraphics();
	g.setColor(Color.white);
	g.fillRect(0,0,800,600);
	wb.setXOffset(0);
	wb.setYOffset(0);
	Iterator it = shapes.iterator();
	while (it.hasNext()) {
	    Tool.Shape s = (Tool.Shape)it.next();
	    s.updateState(wb);
	}
	it = shapes.iterator();
	while (it.hasNext()) {
	    Tool.Shape s = (Tool.Shape)it.next();
	    s.paint(wb, g);
	}
	ImageIO.write(img, "png", new File(filename));
    }

    public static void main(String[] args) throws Exception {
	WhiteboardSaver ws = new WhiteboardSaver();
	String filename=args[0]; // e.g. "1.png"
	ws.savePNG(filename);
    }
}
