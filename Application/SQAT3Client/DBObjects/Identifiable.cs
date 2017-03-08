
namespace Molmed.SQAT.DBObjects 
{

	public class Identifiable
	{
		int MyId;
		string MyName;

		public Identifiable(int ID, string name)
		{
			MyId = ID;
			MyName = name;
		}

		public int ID
		{
			get
			{
				return MyId;
			}
		}

		public string Name
		{
			get
			{
				return MyName;
			}
		}

		override public string ToString()
		{
			return MyName;
		}
	}

    public class IdentifiableCollection : System.Collections.CollectionBase
    {
        public void Add(Identifiable newItem)
        {
            List.Add(newItem);
        }

        public Identifiable Item(int Index)
        {
            return (Identifiable)List[Index];
        }

        public void SetItem(int Index, Identifiable Item)
        {
            List[Index] = Item;
        }
    }

}