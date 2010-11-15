using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using WindowsUtilities;
using System.ComponentModel;

namespace RebarDotNet
{
	/// <summary>
	/// Summary description for BandCollection.
	/// </summary>
	
	[Editor(typeof(RebarDotNet.BandCollectionEditor), 
		 typeof(System.Drawing.Design.UITypeEditor))]
	public class BandCollection: CollectionBase, IEnumerable
	{
		private RebarWrapper _rebar;
		private int _idCounter = 0;

		public BandCollection(RebarWrapper rebar)
		{
			_rebar = rebar;
		}

		public BandWrapper this[int Index]
		{
			get
			{
				return (BandWrapper)List[Index];
			}
		}

		public BandWrapper this[string Key]
		{
			get
			{
				if(Key == null) return null;
				foreach (BandWrapper band in this)
				{
					if (band.Key == Key) return band;
				}
				return null;
			}
		}

		public BandWrapper Add(BandWrapper band)
		{
			List.Add(band);
			band.Bands = this;
			return band;
		}

		public BandWrapper BandFromID(int ID)
		{
			foreach(BandWrapper band in this)
			{
				if(band.ID == ID)
					return band;
			}
			return null;
		}

		public new void Clear()
		{
			for(;List.Count > 0;)
			{
				Remove(0);
			}
			base.Clear();
		}

		public new BandEnumerator GetEnumerator()
		{
			return new BandEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int IndexOf(BandWrapper band)
		{
			return List.IndexOf(band);
		}

		internal int NextID()
		{
			return _idCounter++;
		}

		public RebarWrapper Rebar
		{
			get
			{
				return _rebar;
			}
		}

		public void Remove(int Index)
		{
			Remove(this[Index]);
		}

		public void Remove(string Key)
		{
			Remove(this[Key]);
		}

		public void Remove(BandWrapper band)
		{
			band.DestroyBand();
			List.Remove(band);
			band.Dispose();
		}

		public class BandEnumerator: IEnumerator
		{
			private int Index;
			private BandCollection Collection;

			public BandEnumerator(BandCollection Bands)
			{
				Collection = Bands;
				Index = -1;
			}

			public bool MoveNext()
			{
				Index++;
				return (Index < Collection.Count);
			}

			public void Reset()
			{
				Index = -1;
			}

			public BandWrapper Current
			{
				get
				{
					return Collection[Index];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}
		}
	}
}
