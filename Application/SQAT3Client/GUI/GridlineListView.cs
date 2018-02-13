using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Molmed.SQAT.GUI
{
    //This is a ListView class with a workaround for the gridline repaint bug.
    public partial class GridlineListView : System.Windows.Forms.ListView
    {
        private const int WM_PAINT = 0xF;
        private const int WM_VSCROLL = 0x115;
        private const int SB_LINEDOWN = 0x1;
        private const int SB_LINEUP = 0x0;
        private const int SB_PAGEDOWN = 0x3;
        private const int SB_PAGEUP = 0x2;
        private const int SB_ENDSCROLL = 0x8;

        private bool skipNextPainting = false;
        
        public GridlineListView()
        {
            InitializeComponent();
        }

        public GridlineListView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if (this.View != View.Details || !this.GridLines)
            {
                //No need to do anything special, just pass it on to the ListView base class.
                base.WndProc(ref m);
                return;
            }

            if (m.Msg == WM_VSCROLL)
            {
                int wParam = m.WParam.ToInt32();
                if (wParam == SB_LINEDOWN || wParam == SB_LINEUP)
                {
                    skipNextPainting = true;
                }

                base.WndProc(ref m);

                if (wParam == SB_PAGEDOWN || wParam == SB_PAGEUP)
                {
                    //The WM_VSCROLL message returned, the mouse button is still pressed.
                    //Invalidate to create a correct appearance.
                    this.Invalidate();
                }
                return;
            }

            if (skipNextPainting && m.Msg == WM_PAINT)
            {
                skipNextPainting = false;
                return;
            }

            base.WndProc(ref m);
        }


    }
}
