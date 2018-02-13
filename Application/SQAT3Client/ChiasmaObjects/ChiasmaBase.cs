using System;
using System.Collections;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public class ChiasmaBase
    {
        protected ChiasmaBase()
        {
        }

        protected static Boolean AreEqual(String string1, String string2)
        {
            return String.Compare(string1, string2, true) == 0;
        }

        protected static Boolean AreNotEqual(String string1, String string2)
        {
            return String.Compare(string1, string2, true) != 0;
        }

        protected static Boolean IsEmpty(ICollection collection)
        {
            return ((collection == null) || (collection.Count == 0));
        }

        protected static Boolean IsEmpty(String testString)
        {
            return (testString == null) || (testString.Trim().Length == 0);
        }

        protected static Boolean IsNotEmpty(String testString)
        {
            return (testString != null) && (testString.Trim().Length > 0);
        }

        protected static Boolean IsNotEmpty(ICollection collection)
        {
            return ((collection != null) && (collection.Count > 0));
        }

        protected static Boolean IsNotNull(Object testObject)
        {
            return (testObject != null);
        }

        protected static Boolean IsNull(Object testObject)
        {
            return (testObject == null);
        }

        protected static Boolean IsToLong(String testString, Int32 maxLength)
        {
            if (IsEmpty(testString))
            {
                return false;
            }
            return testString.Length > maxLength;
        }

        protected String JoinComments(String comment1, String comment2)
        {
            comment1 = TrimString(comment1);
            comment2 = TrimString(comment2);
            if (IsEmpty(comment1))
            {
                return comment2;
            }
            if (IsEmpty(comment2))
            {
                return comment1;
            }
            return comment1 + " | " + comment2;
        }

        protected static String TrimString(String trimString)
        {
            if (IsEmpty(trimString))
            {
                return null;
            }
            else
            {
                return trimString.Trim();
            }
        }
    }
}
