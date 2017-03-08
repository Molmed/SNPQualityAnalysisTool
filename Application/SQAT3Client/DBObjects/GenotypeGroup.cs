namespace Molmed.SQAT.DBObjects
{

    public class GenotypeGroup : IdentifiableGroup
    {

        public GenotypeGroup(int ID, string name, string description)
            : base(ID, name, description)
        {
        }

        override public string GroupType
        {
            get
            {
                return "GenotypeGroup";
            }
        }

    }


}
