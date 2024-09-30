using CompanyX.API.DataAccess.Entities;

namespace CompanyX.API.DataAccess.Utilities
{
    public static class CustomerComparer
    {
        public static bool AreCustomersEqual(Customer c1, Customer c2)
        {
            return c1.FirstName == c2.FirstName &&
                   c1.LastName == c2.LastName &&
                   c1.BirthDate == c2.BirthDate &&
                   c1.Email == c2.Email &&
                   c1.PhoneNumber == c2.PhoneNumber &&
                   AreAddressesEqual(c1.HomeAddress, c2.HomeAddress);
        }

        public static bool AreAddressesEqual(CustomerHomeAddress a1, CustomerHomeAddress a2)
        {
            return a1.StreetName == a2.StreetName &&
                   a1.HouseNumber == a2.HouseNumber &&
                   a1.City == a2.City &&
                   a1.PostalCode == a2.PostalCode;
        }
    }
}
