namespace Molmed.SQAT.DBObjects
{

    public class IndividualGroup : IdentifiableGroup
    {

        public IndividualGroup(int ID, string name, string description) 
            : base(ID, name, description)
        {
        }

        override public string GroupType
        {
            get
            {
                return "IndividualGroup";
            }
        }
    }

}