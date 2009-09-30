public class HelloCallBackMe {

  public int testMe( String args[], int nNum ) {
    for ( int i=0; i<args.length; i++) {
      System.out.println(args[i] + ": " + nNum );
    }
    return nNum*10;
  }
}
