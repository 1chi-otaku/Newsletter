using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsletter
{
    using System;

    namespace Newsletter
    {
        public class Country
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public class City
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int CountryID { get; set; }
        }

        public class Customer
        {
            public int ID { get; set; }
            public string FullName { get; set; }
            public DateTime BirthDate { get; set; }
            public bool Gender { get; set; }
            public string Email { get; set; }
            public int CityID { get; set; }
        }

        public class Section
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public class CustomerSection
        {
            public int ID { get; set; }
            public int CustomerID { get; set; }
            public int SectionID { get; set; }
        }

        public class PromotionalItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        public class PromotionalSection
        {
            public int ID { get; set; }
            public int PromotionalItemID { get; set; }
            public int SectionID { get; set; }
        }
    }

}
