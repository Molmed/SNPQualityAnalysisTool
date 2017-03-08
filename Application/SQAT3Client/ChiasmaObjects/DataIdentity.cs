using System;
using System.Collections.Generic;
using System.Text;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.ChiasmaObjects
{
    public interface IDataIdentity : IDataIdentifier
    {
        Int32 GetId();
    }

    public abstract class DataIdentity : DataIdentifier, IDataIdentity
    {
        private Int32 MyId;

        public DataIdentity(DataReader dataReader)
            : base(dataReader)
        {
            MyId = dataReader.GetInt32(DataIdentityData.ID);
        }

        public Int32 GetId()
        {
            return MyId;
        }
    }

    public class DataIdentityList : DataIdentifierList
    {
        public IDataIdentity GetById(Int32 id)
        {
            foreach (IDataIdentity dataIdentity in this)
            {
                if (dataIdentity.GetId() == id)
                {
                    return dataIdentity;
                }
            }
            return null;
        }

        public Int32 GetIndex(IDataIdentity dataIdentity)
        {
            Int32 index;

            for (index = 0; index < this.Count; index++)
            {
                if ((((IDataIdentity)(this[index])).GetDataType() == dataIdentity.GetDataType()) &&
                     (((IDataIdentity)(this[index])).GetId() == dataIdentity.GetId()))
                {
                    return index;
                }
            }
            return -1;
        }

        public Boolean IsMember(IDataIdentity dataIdentity)
        {
            return (this.GetIndex(dataIdentity) >= 0);
        }

        public Boolean IsNotMember(IDataIdentity dataIdentity)
        {
            return (this.GetIndex(dataIdentity) < 0);
        }

        public void Remove(IDataIdentity dataIdentity)
        {
            Int32 index;

            index = this.GetIndex(dataIdentity);
            if (index >= 0)
            {
                this.RemoveAt(index);
            }
        }
    }
}
