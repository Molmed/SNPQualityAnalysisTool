namespace Molmed.SQAT.DBObjects
{

    public class PlateGroup : IdentifiableGroup
    {


        public PlateGroup(int ID, string name, string description)
            : base(ID, name, description)
        {
        }

        override public string GroupType
        {
            get
            {
                return "PlateGroup";
            }
        }
    }


}