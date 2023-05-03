using System;

namespace eVoucher.Models
{
    public abstract class Entity
    {
        private Guid _id;

        public virtual Guid Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        protected Guid dummyID = Guid.NewGuid();
        public virtual Guid DummyID
        {
            get { return dummyID; }
        }
    }
}
