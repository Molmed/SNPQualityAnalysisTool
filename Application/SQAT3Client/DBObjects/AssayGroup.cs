namespace Molmed.SQAT.DBObjects
{

    public class AssayGroup : IdentifiableGroup
    {
        public AssayGroup(int ID, string name, string description) 
            : base(ID, name, description)
        {
        }

        override public string GroupType
        {
            get
            {
                return "AssayGroup";
            }
        }
    }


}