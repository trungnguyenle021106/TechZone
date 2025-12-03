namespace Ordering.Domain.ValueObjects
{
    public record Address(string FirstName, string LastName, string EmailAddress, string AddressLine, string Country, string ZipCode);
}