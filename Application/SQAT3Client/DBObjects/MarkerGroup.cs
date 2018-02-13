namespace Molmed.SQAT.DBObjects
{

    public class MarkerGroup : IdentifiableGroup
    {
        private string MyDescription;

        public MarkerGroup(int ID, string name, string description) 
            : base(ID, name, description)
        {
            MyDescription = description;
        }

        override public string GroupType
        {
            get
            {
                return "MarkerGroup";
            }
        }

    }


}