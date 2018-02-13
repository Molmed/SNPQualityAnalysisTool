namespace Molmed.SQAT.ClientObjects
{

    public class FormattedColumn
    {
        private string MyFullName, MyDisplayName;

        public FormattedColumn(string fullName)
        {
            MyFullName = fullName;
            //Only take the part before the formatting code.
            MyDisplayName = fullName.Split(new string[1] { "//" }, System.StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public string FullName { get { return MyFullName; } }
        public string DisplayName { get { return MyDisplayName; } }

        public string GetFormatCode()
        {
            string[] separatedParts;

            separatedParts = MyFullName.Split(new string[1] { "//" }, System.StringSplitOptions.RemoveEmptyEntries);
            if (separatedParts.GetLength(0) > 1)
            {
                return separatedParts[1];
            }
            else
            {
                return "";
            }
        }

        public override string ToString()
        {
            return MyDisplayName;
        }

    }


}