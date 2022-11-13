using System.ComponentModel.DataAnnotations;

namespace Data.EntityModel
{
    public class PhoneBookEntryPagination
    {
        private const int maxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        /// <summary>
        /// Sorting by PhoneBook Name
        /// </summary>
        /// <example>desc</example>    
        public string? NameOrderBy { get; set; }
        /// <summary>
        /// Sorting by PhoneBook BirthDay
        /// </summary>
        /// <example>asc</example>
        public string? DateOfBirthOrderBy { get; set; }
    }
}
