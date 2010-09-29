import java.awt.*;
import java.awt.event.*;

public class Main {

    public static final boolean DEBUG=false;
    
    public static void main(String[] args) {
	Frame f = new Frame("Whiteboard");
	Transport t;
	boolean readOnly=false;
	if (args.length==0) {
	    t = new LocalTransport();
	} else if (args.length == 2) {
	    t = new ServerTransport(args[0],
				    Integer.parseInt(args[1]));
	} else if (args.length == 1) {
	    if (args[0].equals("-ro")) {
		readOnly=true;
		t=new LocalTransport();
	    } else {
		t = new PHTTPTransport(args[0]);
	    }
	} else {
	    return;
	}
	new Whiteboard(t, readOnly).buildWhiteboard(f);
	f.pack();
	f.addWindowListener(new WindowAdapter() {
		public void windowClosing(WindowEvent evt) {
		    System.exit(0);
		}
	    });
	if (DEBUG) {
	    final Transport _t = t;
	    new Thread() {
		public void run() {
		    Runtime r = Runtime.getRuntime();
		    try {
			while(true) {
			    Thread.sleep(5000);
//			    _t._debug_notify();
//			    System.gc();
			    System.out.println("Total: "+r.totalMemory()+
					       ", Free: "+r.freeMemory()+
					       ", Max: "+r.maxMemory());
			}
		    } catch (Throwable t) { // yes, really!
			t.printStackTrace();
		    }
		}
	    }.start();
	}
	t.start();
	f.show();
    }
}
