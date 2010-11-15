using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace RebarDotNet
{
	/// <summary>
	/// Summary description for BandCollectionEditor.
	/// </summary>
	public class BandCollectionEditor : System.ComponentModel.Design.CollectionEditor
	{
		public BandCollectionEditor(System.Type type): base(type)
		{
		}

		protected override object SetItems(object editValue, object[] value)
		{
			foreach(BandWrapper band in value)
			{
				if (!band.Created)
					((BandCollection)editValue).Add(band);
			}
			return editValue;
		}
		
	}
}
