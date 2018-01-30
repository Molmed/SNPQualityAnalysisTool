namespace Molmed.SQAT.DBObjects
{

    public class ApprovedSession : SessionItem
    {

        public ApprovedSession(int ID, string name, string description)
            : base(ID, name, description)
        {
        }

        public override string SessionType
        {
            get
            {
                return "LockedSession";
            }
        }

    }

    public class OldApprovedSession : SessionItem
    {

        public OldApprovedSession(int ID, string name, string description)
            : base(ID, name, description)
        {
        }

        public override string SessionType
        {
            get
            {
                return "SQATv2ApprovedSession";
            }
        }

    }

}