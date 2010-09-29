public interface WhiteboardContext {

    public static final int WIDTH=800, HEIGHT=550;

    public int getXOffset();
    public int getYOffset();
    
    public void setXOffset(int newValue);
    public void setYOffset(int newValue);

    public String getText();
}
