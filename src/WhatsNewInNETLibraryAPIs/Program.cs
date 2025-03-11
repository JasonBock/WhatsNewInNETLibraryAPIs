using Microsoft.Extensions.Time.Testing;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using WhatsNewInNETLibraryAPIs;

/*
Note that in .NET 9, BinaryFormatter is gone.
If you're still using it...get rid of it :)
*/

/*
Collections are core to many .NET applications,
and changes haven't stopped.
*/

//DemonstrateCollectionsPriorityQueue();

static void DemonstrateCollectionsPriorityQueue()
{
	Console.WriteLine(nameof(DemonstrateCollectionsPriorityQueue));
	Console.WriteLine();

	// Before: A queue is FIFO, but there's no way
	// to prioritize them.
	var beforeQueue = new Queue<int>(6);
	beforeQueue.Enqueue(1);
	beforeQueue.Enqueue(2);
	beforeQueue.Enqueue(3);
	beforeQueue.Enqueue(4);
	beforeQueue.Enqueue(5);
	beforeQueue.Enqueue(6);

	while (beforeQueue.TryDequeue(out var beforeItem))
	{
		Console.WriteLine($"Queue: {beforeItem}");
	}

	Console.WriteLine();

	// After: PriorityQueue gives you a way
	// to do prioritization.
	var afterQueue = new PriorityQueue<int, int>(6);
	afterQueue.Enqueue(1, 3);
	afterQueue.Enqueue(2, 7);
	afterQueue.Enqueue(3, 5);
	afterQueue.Enqueue(4, 1);
	afterQueue.Enqueue(5, -1);
	afterQueue.Enqueue(6, 5);

	// From the docs:
	// "Note that the type does not guarantee first-in-first-out semantics
	// for elements of equal priority."
	while (afterQueue.TryDequeue(out var afterItem, out var afterPriority))
	{
		Console.WriteLine($"PriorityQueue: {afterItem}, {afterPriority}");
	}
}

//DemonstrateFrozenCollections();

static void DemonstrateFrozenCollections()
{
	Console.WriteLine(nameof(DemonstrateFrozenCollections));
	Console.WriteLine();

	var guitars = new Guitar[]
	{
		new Guitar("PRS", 7),
		new Guitar("Ovation", 12),
		new Guitar("Warwick", 5),
		new Guitar("Charvel", 6),
	};

	var immutableGuitars = guitars.ToImmutableArray();
	var newImmutableGuitars = immutableGuitars.Add(new Guitar("Gibson", 6));

	Console.WriteLine($"immutableGuitars.Count = {immutableGuitars.Length}");
	Console.WriteLine($"newImmutableGuitars.Count = {newImmutableGuitars.Length}");

	var frozenGuitars = guitars.ToFrozenSet();
	// There is no "Add".
	Console.WriteLine($"frozenGuitars.Count = {frozenGuitars.Count}");
}

//DemonstrateShuffling();

static void DemonstrateShuffling()
{
	Console.WriteLine(nameof(DemonstrateShuffling));
	Console.WriteLine();

	var items = Enumerable.Range(0, 100).ToArray();

	Console.WriteLine($"Original items order: {string.Join(", ", items)}");
	Console.WriteLine();
	RandomNumberGenerator.Shuffle(items.AsSpan());
	Console.WriteLine($"Shuffled items order: {string.Join(", ", items)}");
	Console.WriteLine();
	Console.WriteLine();

	string[] names = ["Jane", "Joe", "Tim", "Jason", "Sheila", "Liz", "Mike", "Hayden", "Ryan"];

	Console.WriteLine($"Original names order: {string.Join(", ", names)}");
	Console.WriteLine();
	RandomNumberGenerator.Shuffle(names.AsSpan());
	Console.WriteLine($"Shuffled names order: {string.Join(", ", names)}");
}

/*
LINQ and collections go hand in hand. A lot of performance improvements
have been made with LINQ, along with making small but useful
API improvements.
*/

//DemonstrateLinqImprovementsDefaultValues();

static void DemonstrateLinqImprovementsDefaultValues()
{
	Console.WriteLine(nameof(DemonstrateLinqImprovementsDefaultValues));
	Console.WriteLine();

	var items = new List<int>() { 1, 2, 3, 4, 5, 6 };

	// Before: The "default" value wasn't settable for those
	// LINQ functions that provide a default value, so in this case, we get 0
	// (the default value of int).
	Console.WriteLine($"Before: {items.FirstOrDefault(_ => _ < 0)}");

	// After: You can provide a default value.
	Console.WriteLine($"After: {items.FirstOrDefault(_ => _ < 0, -1)}");
}

//DemonstrateLinqImprovementsByOperators();

// https://github.com/dotnet/runtime/issues/27687
static void DemonstrateLinqImprovementsByOperators()
{
	Console.WriteLine(nameof(DemonstrateLinqImprovementsByOperators));
	Console.WriteLine();

	var guitars = new Guitar[]
	{
		new Guitar("PRS", 7),
		new Guitar("Ovation", 12),
		new Guitar("Warwick", 5),
		new Guitar("Charvel", 6),
	};

	// Before: With Min(), you'd actually get the key value.
	Console.WriteLine($"Before: {guitars.Min(_ => _.StringCount)}");

	// After: You can now use MinBy() which will return
	// the object itself.
	Console.WriteLine($"After: {guitars.MinBy(_ => _.StringCount)}");
}

//DemonstrateLinqImprovementsOrder();

// https://devblogs.microsoft.com/dotnet/performance_improvements_in_net_7/#linq
static void DemonstrateLinqImprovementsOrder()
{
	Console.WriteLine(nameof(DemonstrateLinqImprovementsOrder));
	Console.WriteLine();

	var items = new[] { 3, 9, 12, 4, -2, 6 };

	// Previously, you had to do this:
	Console.WriteLine("OrderBy");
	var oldOrder = items.OrderBy(static x => x);
	foreach (var oldItem in oldOrder)
	{
		Console.WriteLine(oldItem);
	}

	Console.WriteLine();

	// Now, it's a little simpler:
	Console.WriteLine("Order");
	var newOrder = items.Order();
	foreach (var newItem in newOrder)
	{
		Console.WriteLine(newItem);
	}

	Console.WriteLine();

	var names = new[] { "Jane", "Joe", "Tim", "Jason", "Sheila", "Liz", "Mike", "Hayden", "Ryan" };

	foreach (var name in names.Order())
	{
		Console.WriteLine(name);
	}
}

/*
Static abstract members in interfaces (SAMIs) is a major
change to interfaces.
*/

//DemonstrateMath();

// int -> IBinaryInteger -> IBinaryNumber -> INumber -> INumberBase -> IAdditionOperators
static void DemonstrateMath()
{
	Console.WriteLine(nameof(DemonstrateMath));
	Console.WriteLine();

	static T Add<T>(T left, T right)
		where T : INumber<T> => left + right;

	static T AddCustom<T>(T left, T right)
		where T : IAdditionOperators<T, T, T> => left + right;

	Console.WriteLine(Add(3, 4));
	Console.WriteLine(Add(3.4, 4.3));
	Console.WriteLine(Add(
		BigInteger.Parse("49043910940940104390", CultureInfo.CurrentCulture),
		BigInteger.Parse("59839583901984390184", CultureInfo.CurrentCulture)));

	Console.WriteLine();

	Console.WriteLine(Math.Abs(-4));
	Console.WriteLine(int.Abs(-4));
}

//DemonstrateParseable();

// int -> IBinaryInteger -> IBinaryNumber -> INumber -> INumberBase -> 
// ISpanParsable -> IParsable
static void DemonstrateParseable()
{
	Console.WriteLine(nameof(DemonstrateParseable));
	Console.WriteLine();

	var value = "3";
	Console.WriteLine(int.Parse(value, CultureInfo.CurrentCulture));

	if (int.TryParse(value, out var result))
	{
		Console.WriteLine(result);
	}
}

/*
Cryptography has been around since 1.0, and in recent history,
APIs have been added to reduce the amount of code necessary
to perform cryptographic operations.
*/

//DemonstrateOneShotCryptography();

static void DemonstrateOneShotCryptography()
{
	Console.WriteLine(nameof(DemonstrateOneShotCryptography));
	Console.WriteLine();

	var data = RandomNumberGenerator.GetBytes(256);
	var key = RandomNumberGenerator.GetBytes(16);
	var iv = RandomNumberGenerator.GetBytes(16);

	// Before: For cryptographic operations, you used to have to do more:
	using (var hash = SHA512.Create())
	{
#pragma warning disable CA1850 // Prefer static 'HashData' method over 'ComputeHash'
		var beforeDigest = hash.ComputeHash(data);
#pragma warning restore CA1850 // Prefer static 'HashData' method over 'ComputeHash'
		Console.WriteLine("Before hash: [{0}]", string.Join(", ", beforeDigest));
		Console.WriteLine();

		using var beforeAes = Aes.Create();
#pragma warning disable CA5401 // Do not use CreateEncryptor with non-default IV
		using var transform = beforeAes.CreateEncryptor(key, iv);
#pragma warning restore CA5401 // Do not use CreateEncryptor with non-default IV
		var beforeEncrypted = transform.TransformFinalBlock(data, 0, data.Length);
		Console.WriteLine("Before encrypted: [{0}]", string.Join(", ", beforeEncrypted));
		Console.WriteLine();
	}

	// After: Now, there are "one-shot" operations.
	var afterDigest = SHA512.HashData(data);
	Console.WriteLine($"After hash: [{string.Join(", ", afterDigest)}]");
	Console.WriteLine();

	// .NET added SHA3 support, though it's new, some OSes don't support it yet.
	if (SHA3_256.IsSupported)
	{
		var sha3Digest = SHA3_256.HashData(data);
		Console.WriteLine($"SHA3 hash: [{string.Join(", ", sha3Digest)}]");
	}
	else
	{
		Console.WriteLine("SHA3 hash is not supported");
	}

	Console.WriteLine();

	using var afterAes = Aes.Create();
	afterAes.Key = key;
	var afterEncrypted = afterAes.EncryptCbc(data, iv);
	Console.WriteLine($"After encrypted: [{string.Join(", ", afterEncrypted)}]");
}

/*
Dates and times can be a thorny issue, especially as it related to
time zones, date or time representation, and testing.
*/

//DemonstrateDatesAndTimes();

static void DemonstrateDatesAndTimes()
{
	Console.WriteLine(nameof(DemonstrateDatesAndTimes));
	Console.WriteLine();

	// Before: If you wanted to represent just a date or time...
	// you couldn't. You had to use a Date, or a TimeSpan for time.
	// But timezones and "overflow" can make for unexpected results.
	var beforeDate = new DateTime(2022, 1, 5);
	var beforeTime = new DateTime(2022, 1, 5, 13, 13, 0);
	var beforeTimeSpan = new TimeSpan(13, 13, 0);
	Console.WriteLine($"Before: Date is {beforeDate}");
	Console.WriteLine($"Before: Time is {beforeTime}");
	Console.WriteLine($"Before: Time + Time is {beforeTime.Add(beforeTime.TimeOfDay)}");
	Console.WriteLine($"Before: TimeSpan is {beforeTimeSpan}");
	Console.WriteLine($"Before: TimeSpan + TimeSpan is {beforeTimeSpan.Add(beforeTimeSpan)}");

	Console.WriteLine();

	// After: Now you can use DateOnly and TimeOnly.
	var afterDate = new DateOnly(2022, 1, 5);
	var afterTime = new TimeOnly(13, 13, 0);
	Console.WriteLine($"After: Date is {afterDate}");
	Console.WriteLine($"After: Time is {afterTime}");
	Console.WriteLine($"After: Time + Time is {afterTime.Add(afterTime.ToTimeSpan())}");
}

//DemonstrateTimeProvider();

static void DemonstrateTimeProvider()
{
	Console.WriteLine(nameof(DemonstrateTimeProvider));
	Console.WriteLine();

	var oldIssue = new OldIssue(IssueLevel.Bug);
	Console.WriteLine($"Old issue priority (should be Concerning): {oldIssue.GetPriority()}");

	var utcNow = DateTimeOffset.UtcNow;
	var concerningFakeTimeProvider = new FakeTimeProvider(utcNow);
	var newBugConcerningIssue = new NewIssue(IssueLevel.Bug, concerningFakeTimeProvider);
	Console.WriteLine($"New issue bug priority (should be Concerning): {newBugConcerningIssue.GetPriority()}");

	var immediateFakeTimeProvider = new FakeTimeProvider(utcNow);
	var newBugImmediateIssue = new NewIssue(IssueLevel.Bug, immediateFakeTimeProvider);
	immediateFakeTimeProvider.Advance(TimeSpan.FromDays(2));
	Console.WriteLine($"New issue bug priority (should be Immediate): {newBugImmediateIssue.GetPriority()}");
}

/*
Similar to the cryptography example above, exception creation
is getting smaller with "ThrowIf" static method patterns.
*/

//DemonstrateOneLineThrows();

static void DemonstrateOneLineThrows()
{
	Console.WriteLine(nameof(DemonstrateOneLineThrows));
	Console.WriteLine();

	static string GetGuitarManufacturer(Guitar guitar)
	{
		ArgumentNullException.ThrowIfNull(guitar);
		return guitar.Manufacturer;
	}

	try
	{
		Console.WriteLine(GetGuitarManufacturer(new("PRS", 7)));
	}
	catch (ArgumentNullException e)
	{
		Console.WriteLine(e.Message);
	}

	try
	{
		Console.WriteLine(GetGuitarManufacturer(null!));
	}
	catch (ArgumentNullException e)
	{
		Console.WriteLine(e.Message);
	}

	Console.WriteLine();

	static int GetLength(string value)
	{
		ArgumentException.ThrowIfNullOrEmpty(value);
		return value.Length;
	}

	try
	{
		Console.WriteLine(GetLength("Jason"));
	}
	catch (ArgumentException e)
	{
		Console.WriteLine(e.Message);
	}

	try
	{
		Console.WriteLine(GetLength(null!));
	}
	catch (ArgumentException e)
	{
		Console.WriteLine(e.Message);
	}
}

//DemonstrateObjectDisposeException();

static void DemonstrateObjectDisposeException()
{
	Console.WriteLine(nameof(DemonstrateObjectDisposeException));
	Console.WriteLine();

	var resource = new DisposableResource();
	Console.WriteLine(resource.GetStreamSize());
	resource.Dispose();

	try
	{
		resource.GetStreamSize();
	}
	catch (ObjectDisposedException e)
	{
		Console.WriteLine(e.Message);
	}

	try
	{
		resource.GetStreamSizeThrowIf();
	}
	catch (ObjectDisposedException e)
	{
		Console.WriteLine(e.Message);
	}
}

/*
One nice improvement with C# 11 is raw string literals.
As strings get larger and representation well-known formats (e.g. JSON),
it's nice that we can check string literals at compile time
for issues.
*/

//DemonstrateStringSyntax();

static void DemonstrateStringSyntax()
{
	static int SumJson([StringSyntax(StringSyntaxAttribute.Json)] string json)
	{
		var content = JsonDocument.Parse(json);
		var sum = 0;

		foreach (var value in content.RootElement.GetProperty("values").EnumerateArray())
		{
			sum += value.GetInt32();
		}

		return sum;
	}

	Console.WriteLine(nameof(DemonstrateStringSyntax));
	Console.WriteLine();

	Console.WriteLine(SumJson(
		"""
		{
			"values": [1, 3, 7, 22]
		}
		"""));
}

/*
Source generators are everywhere. Minimal APIs, logging, JSON serialization, configuration,
along with numerous OSS projects. Regular expressions is just one of many 
examples to dive into.
*/

DemonstrateRegularExpressions();

static void DemonstrateRegularExpressions()
{
	Console.WriteLine(nameof(DemonstrateRegularExpressions));
	Console.WriteLine();

	var id = Guid.NewGuid();
	Console.WriteLine(GuidRegex.Regex.IsMatch(id.ToString()));
	Console.WriteLine(GuidRegex.Regex.IsMatch(id.ToString("N")));

	// .NET 9 added a new way to make Guids
	var version7Id = Guid.CreateVersion7();
	Console.WriteLine(GuidRegex.Regex.IsMatch(version7Id.ToString()));
}

/*
And so much more! Resiliency, numerics and intrinsics, streams,
garbage collection, DI, metrics - there's a lot to take a look at.
*/