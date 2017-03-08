namespace Molmed.SQAT.DBObjects
{

    public class SavedSession : SessionItem
    {

        public SavedSession(int ID, string name, string description)
            : base(ID, name, description)
        {
        }

        public override string SessionType
        {
            get 
            {
                return "SavedSession";
            }
        }
    }

    public class OldSavedSession : SessionItem
    {

        public OldSavedSession(int ID, string name, string description)
            : base(ID, name, description)
        {
        }

        public override string SessionType
        {
            get
            {
                return "SQATv2SavedSession";
            }
        }
    }

    public class SavedSessionCollection : System.Collections.CollectionBase
    {
        public void Add(SavedSession newItem)
        {
            List.Add(newItem);
        }

        public SavedSession Item(int Index)
        {
            return (SavedSession)List[Index];
        }

        public void SetItem(int Index, SavedSession Item)
        {
            List[Index] = Item;
        }
    }

}