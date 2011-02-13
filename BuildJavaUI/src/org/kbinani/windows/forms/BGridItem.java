package org.kbinani.windows.forms;

import java.awt.Component;
import java.util.Vector;
import org.kbinani.componentmodel.TypeConverter;
import org.kbinani.xml.XmlMember;

public class BGridItem
{
    public XmlMember member;
    public Vector<XmlMember> memberStack = new Vector<XmlMember>();
    public Component editor;
    public Vector<BGridItem> children = new Vector<BGridItem>();
    public TypeConverter<?> converter = null;
    public Component expandMark = null;

    public boolean isExpandable()
    {
        return (expandMark != null);
    }
    
    public boolean isExpanded()
    {
        if( expandMark == null ){
            return false;
        }else{
            return ((BPropertyGridExpandMark)expandMark).isExpanded();
        }
    }
}
