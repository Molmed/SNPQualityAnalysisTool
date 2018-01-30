namespace Molmed.SQAT.DBObjects
{

    public class SampleGroup : IdentifiableGroup
    {

        public SampleGroup(int ID, string name, string description) 
            : base(ID, name, description)
        {
        }

        override public string GroupType
        {
            get
            {
                return "SampleGroup";
            }
        }

    }


}