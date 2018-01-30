namespace Molmed.SQAT.DBObjects
{

    abstract public class SessionItem : Identifiable
    {
        private string MyDescription;

        public SessionItem(int ID, string name, string description)
            : base(ID, name)
        {
            MyDescription = description;
        }

        abstract public string SessionType
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

    public class SessionItemCollection : System.Collections.CollectionBase
    {
        public void Add(SessionItem newItem)
        {
            List.Add(newItem);
        }

        public SessionItem Item(int Index)
        {
            return (SessionItem)List[Index];
        }

        public void SetItem(int Index, SessionItem Item)
        {
            List[Index] = Item;
        }
    }

}