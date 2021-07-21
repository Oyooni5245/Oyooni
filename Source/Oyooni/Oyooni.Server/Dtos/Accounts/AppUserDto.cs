namespace Oyooni.Server.Dtos.Accounts
{
    public class AppUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AppUserDto() { }
        public AppUserDto(string firstName, string lastName) => (FirstName, LastName) = (firstName, lastName);
    }
}
