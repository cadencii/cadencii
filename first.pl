@directories = ( "build", "build/win", "build/linux", "build/macosx", "build/java",
                 "build/doc", "build/java/org", "build/java/org/kbinani", 
                 "build/java/org/kbinani/windows", "build/java/org/kbinani/windows/forms",
                 "build/java/org/kbinani/xml", "build/java/org/kbinani/media",
                 "build/java/org/kbinani/apputil", "build/java/org/kbinani/componentmodel",
                 "build/java/org/kbinani/vsq", "build/java/org/kbinani/cadencii",
                 "build/java/resources", "build/win/resources" );

foreach my $d ( @directories ){
	if( !(-d $d) ){
		mkdir $d;
	}
}
