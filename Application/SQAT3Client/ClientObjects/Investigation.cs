using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ClientObjects
{

    public enum InvestigationMode
    {
        Zygosity = 1,
        Genotype = 2,
        FreeText = 3
    }

    public class InvestigationStatusEventArgs : EventArgs
    {
        private string MyStatus;

        public InvestigationStatusEventArgs(string status)
        {
            MyStatus = status;
        }

        public string Status
        {
            get { return MyStatus; }
        }
    }

    public class Investigation
    {

        public delegate void InvestigationStatusChangeHandler(InvestigationStatusEventArgs status);

        public event InvestigationStatusChangeHandler StatusChange;

        protected void OnStatusChange(string status)
        {
            if (StatusChange != null)
            {
                StatusChange(new InvestigationStatusEventArgs(status));
            }
        }
    
    }
}
