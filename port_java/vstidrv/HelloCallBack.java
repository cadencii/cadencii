public class HelloCallBack {

  public native void test(HelloCallBackMe hcMe);

  static {
    System.loadLibrary("HelloCallBack");
  }

  protected void start() {
    HelloCallBackMe hcMe = new HelloCallBackMe();
    this.test(hcMe);
  }

  public static void main( String[] args ) {
    new HelloCallBack().start();
  }
}
