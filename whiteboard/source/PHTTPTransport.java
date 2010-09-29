import java.util.Vector;
import java.io.*;
import java.net.*;

public class PHTTPTransport extends Transport implements Runnable {
    
    private ObjectStore os = new ObjectStore();

    private int lastID=0;

    private boolean running=false;
    private String url;

    public PHTTPTransport(String url) {
	this.url=url;
    }

    // from interface
    public synchronized void addShape(Tool.Shape s) {
	String str = s.shapeToString();
	if (str == null) throw new RuntimeException();
	sendShape(str);
	os.addTemp(str);
    }

    public void sendShape(String s) {
	try {
	    URLConnection uc = new URL(url+"&add="+s).openConnection();
	    InputStream in = uc.getInputStream();
	    int b;
	    while((b=in.read()) != -1) {}
	    in.close();
	} catch (IOException ex) {
	    ex.printStackTrace();
	}
    }

    // from interface
    public synchronized Vector getAllShapes() {
	return os.getAllShapes();
    }

    // from interface
    public synchronized void start() {
	running=true;
	new Thread(this).start();
	notifyWhiteboard();
    }

    //from interface
    public synchronized void stop() {
	running=false;
	os.clear();
	notifyWhiteboard();
    }

    //from interface
    public String getStatus() {
	return null;
    }

    public void run() {
	try {
	  while(running) {
	    Thread.sleep(1000);
	    if (!running) break;
	    boolean notify=false;
	    synchronized(this) {
		URLConnection c = new URL(url+"&after="+lastID)
		    .openConnection();
		BufferedReader br = new BufferedReader
		    (new InputStreamReader(c.getInputStream(), "ISO-8859-1"));
		String line;
		int maxID=0;
		while((line=br.readLine()) != null) {
		    if (line.startsWith("::")) {
			line=line.substring(2);
			int pos = line.indexOf(":");
			if (pos == -1) {
			    System.out.println("!> "+line);
			    continue;
			}
			try {
			    int id = Integer.parseInt(line.substring(0, pos));
			    if (id > lastID) {
				if (id > maxID) maxID=id;
				String shape=line.substring(pos+1);
				if (os.add(shape))
				    notify=true;
			    }
			} catch (NumberFormatException ex) {
			    ex.printStackTrace();
			}
		    } else {
			System.out.println(">>"+line);
		    }
		}
		if (maxID > lastID) lastID=maxID;
		br.close();
		br = null;
		c = null;
	    }
	    if (notify) {
		notifyWhiteboard();
	    }
	    System.gc(); // aggressive GC
	  }
	} catch (InterruptedException ex) {
	    ex.printStackTrace();
	} catch (IOException ex) {
	    ex.printStackTrace();
	}
    }
}
