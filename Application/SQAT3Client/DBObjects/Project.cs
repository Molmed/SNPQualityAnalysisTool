namespace Molmed.SQAT.DBObjects
{

    public class Project : Identifiable
    {

        public Project(int ID, string name)
            : base(ID, name)
        {
        }

    }

    public class ProjectCollection : System.Collections.CollectionBase
    {
        public void Add(Project newItem)
        {
            List.Add(newItem);
        }

        public Project Item(int Index)
        {
            return (Project)List[Index];
        }

        public void SetItem(int Index, Project Item)
        {
            List[Index] = Item;
        }
    }

}