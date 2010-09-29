import java.util.Vector;
import java.io.*;
import java.net.*;

public class ServerTransport extends Transport implements Runnable{
    
    private ObjectStore os = new ObjectStore();

    private PrintWriter out = null;
    private Socket sock = null;

    private boolean running=false;
    private String host;
    private int port;

    public ServerTransport(String host, int port) {
	this.host=host;
	this.port=port;
    }

	
    public void addShape(Tool.Shape s) {
	String str = s.shapeToString();
	if (str == null) throw new RuntimeException();
	if (out != null) {
	    out.println(str);
	} else {
	    System.err.println("Could not send to server!");
	}
	os.addTemp(str);
    }

    public Vector getAllShapes() {
	return os.getAllShapes();
    }

    public void start() {
	running=true;
	new Thread(this).start();
	notifyWhiteboard();
    }
    public void stop() {
	running=false;
	if (sock != null) {
	    try {
		sock.close();
	    } catch (IOException ex) {
		ex.printStackTrace();
	    }
	    sock=null;
	}
	os.clear();
	notifyWhiteboard();
    }

    public String getStatus() {
	return null;
    }

    public void run() {
	try {
	    sock = new Socket(host, port);
	    BufferedReader r = new BufferedReader
		(new InputStreamReader(sock.getInputStream()));
	    out = new PrintWriter
		(new OutputStreamWriter(sock.getOutputStream()),
		 true);
	    String line;
	    while(running && (line=r.readLine()) != null) {
		os.add(line);
		notifyWhiteboard();
	    }
	    sock.close();
	} catch (IOException ex) {
	    ex.printStackTrace();
	}
    }
}
