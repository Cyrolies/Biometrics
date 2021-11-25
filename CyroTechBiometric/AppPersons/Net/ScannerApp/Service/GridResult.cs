using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    [Serializable]
    public class GridResult<T>
    {
        IEnumerable<T> items;
        int totalcount = 0;
        int totalfilteredcount = 0;
        public IEnumerable<T> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }

        public int TotalCount
        {
            get
            {
                return totalcount;
            }

            set
            {
                totalcount = value;
            }
        }

        public int TotalFilteredCount
        {
            get
            {
                return totalfilteredcount;
            }

            set
            {
                totalfilteredcount = value;
            }
        }
    }
}
