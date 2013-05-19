using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.NRefactory.CSharp;
using System.Linq;

namespace com.github.cadencii
{
    abstract class ScriptProcessor
    {
        protected abstract string getPrefix();

        protected abstract string getSuffix();

        public string process( string code )
        {
            var targetCode = getPrefix() + code + getSuffix();
            var parser = new CSharpParser();
            var root = parser.Parse( targetCode, "" );
            root
                .Descendants
                .OfType<Identifier>()
                .Where( ( id ) => { var n = GetSiblingNode( id, 1 ); return n != null && n.GetType() == typeof( CSharpTokenNode ); } )
                .ToList().ForEach( ( id ) => {
                    var childIdentifier = GetSiblingNode( id, 2 ) as Identifier;
                    var grandChildIdentifier = GetSiblingNode( id, 4 ) as Identifier;
                    if ( GetSiblingNode( id, 3 ) != null && GetSiblingNode( id, 3 ).GetType() != typeof( CSharpTokenNode ) ) {
                        grandChildIdentifier = null;
                    }
                    if ( (id.Name == "Boare" && childIdentifier.Name == "Lib") ||
                         (id.Name == "org" && childIdentifier.Name == "kbinani") ) {
                        id.Name = "com";
                        if ( grandChildIdentifier != null && grandChildIdentifier.Name.ToLower() != "cadencii" ) {
                            childIdentifier.Name = "github.cadencii";
                        } else {
                            childIdentifier.Name = "github";
                        }
                        if ( grandChildIdentifier != null ) {
                            grandChildIdentifier.Name = grandChildIdentifier.Name.ToLower();
                        }
                    }

                    if ( id.Name == "Boare" && childIdentifier.Name == "Cadencii" ) {
                        id.Name = "com";
                        childIdentifier.Name = "github.cadencii";
                    }
                    if ( id.Name == "bocoree" ) {
                        id.Name = "com.github.cadencii.java";
                    }
                    if ( id.Name == "bocoreex" ) {
                        id.Name = "com.github.cadencii.javax";
                    }
                    if ( id.Name == "InputBox" ) {
                        id.Name = typeof( com.github.cadencii.windows.forms.InputBox ).FullName;
                    }
                } );
            root
                .Descendants
                .ToList()
                .ForEach( ( node ) => {
                    if ( node.Children.Count() == 0 ) {
                        if ( node.GetText() == "InputBox" && node.GetNextNode().GetType() != typeof( CSharpTokenNode ) ) {
                            (node as Identifier).Name = typeof( com.github.cadencii.windows.forms.InputBox ).FullName;
                        }
                    }
                } );
            return root.GetText();
        }

        AstNode GetSiblingNode( AstNode node, int count )
        {
            if ( count == 0 ) {
                return node;
            } else {
                if ( node == null ) {
                    return null;
                } else {
                    return GetSiblingNode( node.GetNextNode(), count - 1 );
                }
            }
        }
    }

    class ScriptProcessorVersion1 : ScriptProcessor
    {
        protected override string getPrefix() {
            return
                "using System;" +
                "using System.IO;" +
                "using Boare.Lib.Vsq;" +
                "using Boare.Cadencii;" +
                "using bocoree;" +
                "using bocoree.io;" +
                "using bocoree.util;" +
                "using bocoree.awt;" +
                "using Boare.Lib.Media;" +
                "using Boare.Lib.AppUtil;" +
                "using System.Windows.Forms;" +
                "using System.Collections.Generic;" +
                "using System.Drawing;" +
                "using System.Text;" +
                "using System.Xml.Serialization;" +
                "namespace com.github.cadencii.plugin {";
        }
        protected override string getSuffix() { return "}"; }
    }

    class ScriptProcessorVersion2 : ScriptProcessor
    {
        protected override string getPrefix() {
            return
                "using System;" + 
                "using System.IO;" +
                "using org.kbinani.vsq;" +
                "using org.kbinani.cadencii;" +
                "using org.kbinani;" +
                "using org.kbinani.java.io;" +
                "using org.kbinani.java.util;" +
                "using org.kbinani.java.awt;" +
                "using org.kbinani.media;" +
                "using org.kbinani.apputil;" +
                "using System.Windows.Forms;" +
                "using System.Collections.Generic;" +
                "using System.Drawing;" +
                "using System.Text;" +
                "using System.Xml.Serialization;" +
                "namespace com.github.cadencii.plugin {";
        }
        protected override string getSuffix() { return "}"; }
    }

    class ScriptProcessorVersion3 : ScriptProcessor
    {
        protected override string getPrefix()
        {
            return
                "using System;" +
                "using System.IO;" +
                "using com.github.cadencii.vsq;" +
                "using com.github.cadencii;" +
                "using com.github.cadencii.java.io;" +
                "using com.github.cadencii.java.util;" +
                "using com.github.cadencii.java.awt;" +
                "using com.github.cadencii.media;" +
                "using com.github.cadencii.apputil;" +
                "using System.Windows.Forms;" +
                "using System.Collections.Generic;" +
                "using System.Drawing;" +
                "using System.Text;" +
                "using System.Xml.Serialization;" +
                "namespace com.github.cadencii.plugin {";
        }
        protected override string getSuffix() { return "}"; }
    }
}
