using System;

namespace BikeMgr.Core.Models
{
    public class PageParams : Model
    {
        private int _PageNo;
        private int _PageSize;
        public int PageNo 
        { 
            get => _PageNo;
            set
            {
                if(value < 1) throw new ArgumentOutOfRangeException($"{nameof(value)} must be greater than 0.");
                _PageNo = value;
            }
        }
        public int PageSize
        {
            get => _PageSize;
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException($"{nameof(value)} must be greater than 0.");
                _PageSize = value;
            }
        }
    }
}
