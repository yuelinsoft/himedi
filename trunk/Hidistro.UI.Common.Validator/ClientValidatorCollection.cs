using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Hidistro.UI.Common.Validator
{
    public class ClientValidatorCollection : ICollection, IEnumerable
    {
        ValidateTarget owner;
        ArrayList validators;

        public ClientValidatorCollection(ValidateTarget owner, ArrayList validators)
        {
            this.owner = owner;
            this.validators = validators;
        }

        public void Add(ClientValidator validator)
        {
            AddAt(-1, validator);
        }

        public void AddAt(int index, ClientValidator validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException("validator");
            }
            if (index == -1)
            {
                validators.Add(validator);
            }
            else
            {
                validators.Insert(index, validator);
            }
            validator.SetOwner(owner);
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            IEnumerator enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                array.SetValue(enumerator.Current, index++);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return validators.GetEnumerator();
        }

        public int IndexOf(ClientValidator validator)
        {
            if (validator != null)
            {
                return validators.IndexOf(validator);
            }
            return -1;
        }

        public void Remove(ClientValidator validator)
        {
            int index = IndexOf(validator);
            if (index >= 0)
            {
                RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            validators.RemoveAt(index);
        }

        [Browsable(false)]
        public int Count
        {
            get
            {
                return validators.Count;
            }
        }

        [Browsable(false)]
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        [Browsable(false)]
        public ClientValidator this[int index]
        {
            get
            {
                return (ClientValidator)validators[index];
            }
        }

        [Browsable(false)]
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
    }
}

