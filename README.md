# MinimalApiSandbox

## About

Quick project to demonstrate a few key patterns that are commonly useful in ASP.NET APIs (and some patterns that I think are pretty neat).

## Learning points:

Throughout the project you will find `// Learning: {name}`. The `{name}` will reference a learning point included here.

### Primary Constructors

When classes only have one constructor they become a candidate for a primary constructor.

```
public class Address
{
	public string HouseNameNumber { get; set; }
	public string StreetName { get; set; }
	public string Postcode { get; set; }

	public Address(string houseNameNumber, string streetName, string postcode)
	{
		HouseNameNumber = houseNameNumber;
		StreetName = streetName;
		Postcode = postcode;
	}
}
```

Instead becomes:

```
public class Address(string houseNameNumber, string streetName, string postcode)
{
	public string HouseNameNumber { get; set; } = houseNameNumber;
	public string StreetName { get; set; } = streetName;
	public string Postcode { get; set; } = postcode;
}
```

Further reading: https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/primary-constructors

### Nullability handling

The two main categories of types in C# are [Value](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-types) types and [Reference](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/reference-types) types.

There're a few differences but this focuses on nullability and default values.

Default values:

```
public int Id { get; set; } // int is a value type so it defaults to 0. All value types have a non-null default eg bool default is false
public string Name { get; set; } // string is a reference type so it defaults to null. All reference types default to null. Gives Compiler warning: CS8625 as we have not specified nullable ie string?
```

Assigning null:

```
public int Id { get; set; } = null; // Compiler error: CS0037
public string Name { get; set; } = null; // Compiler warning: CS8625
```

You can write `public required string` or assign the string a value in the class constructor but there are some cases where external code will be providing the value so neither of these suit. (EF Core and Dapper most commonly). To deal with the null warning we can write:

```
public string Name { get; set; } = null!; // The bang operator ("!") tells the compiler to ignore the warning. Use sparingly, where you know the value will not actually be null.
```

[Further Reading](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/nullable-warnings)

### Null-coalescing operator

The null-coalescing operator ?? returns the value of its left-hand operand if it isn't null; otherwise, it evaluates the right-hand operand and returns its result. The ?? operator doesn't evaluate its right-hand operand if the left-hand operand evaluates to non-null. The null-coalescing assignment operator ??= assigns the value of its right-hand operand to its left-hand operand only if the left-hand operand evaluates to null. The ??= operator doesn't evaluate its right-hand operand if the left-hand operand evaluates to non-null.

```
List<int>? numbers = null;
int? a = null;

Console.WriteLine((numbers is null)); // expected: true
// if numbers is null, initialize it. Then, add 5 to numbers
(numbers ??= new List<int>()).Add(5);
Console.WriteLine(string.Join(" ", numbers));  // output: 5
Console.WriteLine((numbers is null)); // expected: false        


Console.WriteLine((a is null)); // expected: true
Console.WriteLine((a ?? 3)); // expected: 3 since a is still null 
// if a is null then assign 0 to a and add a to the list
numbers.Add(a ??= 0);
Console.WriteLine((a is null)); // expected: false        
Console.WriteLine(string.Join(" ", numbers));  // output: 5 0
Console.WriteLine(a);  // output: 0
```

This can also be very useful for throwing exceptions. For example:

```
var mustNotBeNull = GetItem(id) ?? throw new NullReferenceException(nameof(mustNotBeNull));
```

If GetItem does not find an item from the provided id we force this code to throw in a controlled manner rather than at some later point when it tries to use `mustNotBeNull`.

[Further Reading](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator)

### IDisposable

To understand IDisposable let's first understand C#'s Garbage Collector (GC). GC is the reason why, unlike C++ DEVs, C# DEVs don't have to interact with memory allocations. Here's an example:

```
for (int i = 0; i < myArray.Length; i++) // Memory is allocated for the integer i
{
	// Your really cool iterative code
}
// i is no longer in scope, effectively it no longer exists. GC comes along and frees up the allocated memory.
```

So with GC doing all that why do we need IDisposable? There are two important use cases:

- Unmanaged resources like `Stream` need to be told when to be disposed.
- Objects that require further functional behaviour to occur on disposal.

The latter is what we'll focus on here as it is more common to encounter. The most common scenarios being database connections and HTTP requests:

- With database connections we open a connection to perform a query. GC will just cleanup the memory allocation so we use Dispose to close the connection on cleanup.
- With HTTP requests we use HttpClients. These are given ports for making requests and if we don't dispose of them properly then eventually an exception will be thrown due to port exhaustion.

#### IDisposable - Best Practices

##### The `using` statement

The `using` statement effectively says "Ok GC, I'm going to use this unmanaged resource. When I'm done with it please call it's Dispose method".

There are two ways of writing a `using` statement:

```
// using statement - "brace" scoped
static IEnumerable<int> LoadNumbers(string filePath)
{
    var numbers = new List<int>();
    string line;

    using (StreamReader reader = File.OpenText(filePath))
    {
        while ((line = reader.ReadLine()) is not null)
        {
            if (int.TryParse(line, out int number))
            {
                numbers.Add(number);
            }
        }
    } // Closing brace of using statement. reader.Dispose is called here.

    return numbers;
}
```

```
// using declaration - "scope" scoped
static IEnumerable<int> LoadNumbers(string filePath)
{
    var numbers = new List<int>();
    string line;
    
    using StreamReader reader = File.OpenText(filePath);
    while ((line = reader.ReadLine()) is not null)
    {
        if (int.TryParse(line, out int number))
        {
            numbers.Add(number);
        }
    }

    return numbers;
} // End of scope that using declaration exists within. reader.Dispose is called here.
```

I have a strong preference for "scope" scoped. I dislike adding unnecessary indentation and if you're writing small, well segmented methods, then your using statement is typically disposing the resource at the same point anyway. Take the two examples above. While they may look different, in actual operation the using statement is basically being called at the end of the method in both instances.

##### Implementing `IDisposable`

Sometimes it makes more sense for your class to hold an IDisposable object as a property so that it can share that same instance across multiple scopes. A UnitOfWork is a good example of this:

```
public class UnitOfWork(IDbConnection connection) : IDisposable
{
    private IDbConnection _connection = connection;

    //
    // Your various repositories the UnitOfWork will provide the connection to
    //

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
```

So then in use:

```
    using UnitOfWork dc = new(connection);
    
    return await dc.Carparks.GetAllAsync();
```

Important to note here that IDbConnection is held as a property on multiple objects; the `UnitOfWork` itself and our various repositories. If one object in this family disposes the connection then any others will throw an exception if they try to use it. Best Practice here is to maintain a single point of ownership that implements IDisposable and is responsible for disposing the shared resource. In this case that is `UnitOfWork` as if it gets disposed we wouldn't expect further calls to be made to its repositories.

Further reading:
- [Garbage Collection](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals)
- [Cleaning up unmanaged resources](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/unmanaged)
- [The using statement](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/using)

### Interfaces

Interfaces can often seem a little pointless in their common uses. Take this block from the Bootstrapper:

```
    public static void AddServices(this IServiceCollection services)
        => services
            // App Services
            .AddScoped<ICarparkService, CarparkService>()
            .AddScoped<ISchoolService, SchoolService>()

            // Data Services
            .AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            ;
```

That's 4 instances of interfaces implemented exclusively by a single class. So if we only ever have one `ICarparkService` implementation then why even bother with the interface?

1. [SOLID design](https://www.freecodecamp.org/news/solid-design-principles-in-software-development/) specifically D, "The dependency inversion principle (DIP) states to depend upon abstractions, not concretes."

    Phrased differently high-level modules should not depend on low-level modules. In this case our endpoints are the high level while our services are the low level.

1. Unit Testing

    Take the `CarparkService`. To Unit Test it we would have to construct it with an instance of the `UnitOfWorkFactory` but then we're not Unit Testing `CarParkService`, we're integration testing a collection of classes. Instead we can construct `CarParkService` with a dummy `IUnitOfWorkFactory` (known as a stub) that will produce a set of controlled results on its methods thereby isolating our testing exclusively to how `CarParkService` handles those results.