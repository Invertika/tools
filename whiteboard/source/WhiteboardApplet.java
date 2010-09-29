import java.awt.*;
import java.applet.*;
public class WhiteboardApplet extends Applet {

    private Whiteboard w;
    private Transport t;
    public void start () {
	t.start();
    }

    public void stop () {
	t.stop();
    }
    
    public void init() {
	boolean readOnly = "true".equals(getParameter("readonly"));
	String url = getParameter("url");
	if (url != null) {
	    t=new PHTTPTransport(url);
	} else {
	    String host=getParameter("host");
	    String port = getParameter("port");
	    if (host == null || port == null) {
		t = new LocalTransport();
	    } else {
		t = new ServerTransport(host, Integer.parseInt(port));
	    }
	}
	w=new Whiteboard(t, readOnly);
	w.buildWhiteboard(this);
    }    
}
