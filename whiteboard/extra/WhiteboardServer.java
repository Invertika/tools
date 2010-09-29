import java.net.*;
import java.io.*;
import java.util.*;

public class WhiteboardServer extends Thread {
    
    public static ArrayList ostreams = new ArrayList();

    private Socket s;
    public WhiteboardServer(Socket s) {
	this.s=s;
	start();
    }

    public void run() {
	try {
	    run1();
	} catch (IOException ex) {
	    ex.printStackTrace();
	}
    }

    public void run1() throws IOException {
	BufferedWriter out = new BufferedWriter(new OutputStreamWriter(s.getOutputStream(), "ISO-8859-1"));
	synchronized(ostreams) {
	    ostreams.add(out);
	}
	BufferedReader br = new BufferedReader(new InputStreamReader(s.getInputStream(), "ISO-8859-1"));
	String line;
	while((line=br.readLine()) != null) {
	    synchronized(ostreams) {
		for(Iterator it=ostreams.iterator(); it.hasNext(); ) {
		    BufferedWriter bw = (BufferedWriter) it.next();
//		    if (out == bw) continue;
		    bw.write(line+"\n");
		    bw.flush();
		}
	    }
	}
    }

    public static void main (String[] args) throws Exception {
	ServerSocket ss = new ServerSocket(Integer.parseInt(args[0]));
	while(true) {
	    Socket s = ss.accept();
	    new WhiteboardServer(s);
	}
    }
}
