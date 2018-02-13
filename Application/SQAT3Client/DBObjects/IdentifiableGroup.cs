namespace Molmed.SQAT.DBObjects
{

    abstract public class IdentifiableGroup : Identifiable
    {
        private string MyDescription;

        public IdentifiableGroup(int ID, string name, string description)
            : base(ID, name)
        {
            MyDescription = description;
        }

        abstract public string GroupType
        {
            get;
        }

        public string Description
        {
            get
            {
                return MyDescription;
            }
        }

    }

    public class IdentifiableGroupCollection : System.Collections.CollectionBase
    {
        public void Add(IdentifiableGroup newItem)
        {
            List.Add(newItem);
        }

        public IdentifiableGroup Item(int Index)
        {
            return (IdentifiableGroup)List[Index];
        }

        public void SetItem(int Index, IdentifiableGroup Item)
        {
            List[Index] = Item;
        }
    }

}