using System;
using System.Collections.Generic;
using System.Text;

namespace Molmed.SQAT.ChiasmaObjects
{
    public interface IDataComment
    {
        void AddComment(String comment);
        String GetComment();
        Boolean HasComment();
        void SetComment(String comment);
    }

    public abstract class DataComment : DataIdentity, IDataComment
    {
        private String MyComment;

        public DataComment(DataReader dataReader)
            : base(dataReader)
        {
            MyComment = dataReader.GetString(DataCommentData.COMMENT);
        }

        public void AddComment(String comment)
        {
            SetComment(JoinComments(MyComment, comment));
        }

        public String GetComment()
        {
            return MyComment;
        }

        public Boolean HasComment()
        {
            return IsNotEmpty(MyComment);
        }

        public abstract void SetComment(String comment);

        public virtual void UpdateComment(String comment)
        {
            // Update this object.
            MyComment = comment;
        }
    }
}
