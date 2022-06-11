using Bogus;

namespace FC.Common.Domain.FakeData;

public static class UsersExtension
{
    public static User GetFakeData(this User user)
    {
        var personFake = new Faker<User>()
            .RuleFor(o => o.FirstName, f => f.Person.FirstName)
            .RuleFor(o => o.LastName, f => f.Person.LastName)
            //Replaces symbols with numbers and letters. # = number, ? = letter, * = number or letter.
            .RuleFor(o => o.PasswordHash, f => f.Random.Replace("???-**-##???"));
        
        var person = personFake.Generate();
        return person;
    }
}

public static class MyExtensions
{
    public static int WordCount(this string str)
    {
        return str.Split(new char[] { ' ', '.', '?' },
            StringSplitOptions.RemoveEmptyEntries).Length;
    }
}