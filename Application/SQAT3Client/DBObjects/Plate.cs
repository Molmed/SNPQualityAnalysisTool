namespace Molmed.SQAT.DBObjects
{

    public class Plate : Identifiable
    {
        private string MyDescription;

        public Plate(int ID, string name, string description)
            : base(ID, name)
        {
            MyDescription = description;
        }

        public string Description
        {
            get
            {
                return MyDescription;
            }
        }
    }

    public class PlateCollection : System.Collections.CollectionBase
    {
        public void Add(Plate newItem)
        {
            List.Add(newItem);
        }

        public Plate Item(int Index)
        {
            return (Plate)List[Index];
        }

        public void SetItem(int Index, Plate Item)
        {
            List[Index] = Item;
        }
    }

}