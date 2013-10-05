#if !MONO
/*
 * RebarDesigner.cs
 * Copyright © Anthony Baraff
 * Copyright © 2010-2011 kbinani
 *
 * This file is part of cadencii.windows.forms.
 *
 * cadencii.windows.forms is free software; you can redistribute it and/or
 * modify it under the terms of the BSD License.
 *
 * cadencii.windows.forms is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;
using cadencii;

namespace cadencii.windows.forms
{
    /// <summary>
    /// Summary description for RebarDesigner.
    /// </summary>
    public class RebarDesigner : System.Windows.Forms.Design.ParentControlDesigner
    {
        private Rebar _rebar;
        private IDesignerHost _host;
        private ISelectionService _selService;
        private DesignerVerbCollection _verbs;

        public RebarDesigner()
        {
            _verbs = new DesignerVerbCollection();
            _verbs.Add(new DesignerVerb("Add Band", new EventHandler(mnuAddBand)));
            _verbs.Add(new DesignerVerb("Remove Band", new EventHandler(mnuRemoveBand)));
            _verbs[1].Enabled = false;
        }

        public override ICollection AssociatedComponents
        {
            get
            {
                ArrayList assocComponents = new ArrayList(20);
                foreach (RebarBand band in _rebar.Bands) {
                    assocComponents.Add(band);
                    if (band.Child != null) {
                        assocComponents.Add(band.Child);
                    }
                }
                return assocComponents;
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                return _verbs;
            }
        }
        /*
                public void Host_OnLoadComplete(object sender, EventArgs e)
                {	
                    if(!_rebar.Created) _rebar.CreateControl();
                }
        */
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (Control is Rebar) {
                _rebar = (Rebar)Control;
                /*
                mPropertyTree.PaneActivated += 
                    new PropertyTree.PaneActivatedEventHandler(ptPaneActivated); 
                    */
                _host = (IDesignerHost)
                    GetService(typeof(IDesignerHost));
                _selService = (ISelectionService)
                    GetService(typeof(ISelectionService));
                //_host.LoadComplete += new EventHandler(Host_OnLoadComplete);
                //if(!_rebar.Created) _rebar.CreateControl();
            }
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                if (_rebar != null) {
                    if (_rebar.Orientation == Orientation.Horizontal) {
                        return SelectionRules.LeftSizeable |
                            SelectionRules.RightSizeable |
                            SelectionRules.Moveable |
                            SelectionRules.Visible;
                    } else {
                        return SelectionRules.TopSizeable |
                            SelectionRules.BottomSizeable |
                            SelectionRules.Moveable |
                            SelectionRules.Visible;
                    }
                } else {
                    return SelectionRules.AllSizeable
                        | SelectionRules.Moveable
                        | SelectionRules.Visible;
                }
            }
        }

        public void mnuAddBand(object sender, EventArgs e)
        {
            RebarBand band = (RebarBand)_host.CreateComponent(typeof(RebarBand));
            _rebar.Bands.Add(band);
        }

        public void mnuRemoveBand(object sender, EventArgs e)
        {
            //BandWrapper band = (BandWrapper)_host.CreateComponent(typeof(BandWrapper));
            //_rebar.Bands.Remove(band);
        }

        protected override bool EnableDragRect
        {
            get
            {
                return _rebar.Dock == DockStyle.None;
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (_rebar.Created) {
                if (m.Msg == (int)win32.WM_LBUTTONDBLCLK) {
                    RebarBand band = _rebar.BandHitTest(new Point(m.LParam.ToInt32()));
                    if (band != null)
                        _selService.SetSelectedComponents(new Component[] { band });
                    return;
                }
            }
            base.WndProc(ref m);
            if (m.Msg == (int)win32.WM_LBUTTONDOWN) //No silly rectangle selection when you tinker with the bands
			{
                win32.SendMessage(_rebar.Handle, (int)win32.WM_LBUTTONUP, m.WParam, m.LParam);
            }
        }


    }
}
#endif
