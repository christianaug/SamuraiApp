using System;
using System.Linq;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace ConsoleApp
{
	internal class Program
	{
		//instance of samurai context
		private static SamuraiContext context = new SamuraiContext();
		static void Main(string[] args)
		{
			//calling database context method to ensure that the database has been created (DON'T USE IN REAL APPLIACTIONS) 
			/*
			 * reads the database context to see if it exists, if it doesnt then reads the context and finds the relationships to build the new database
			 */
			context.Database.EnsureCreated();
			GetSamurais("Before Add: ");
			AddSamurai();
			GetSamurais("After Add: ");
			Console.Write("Press any key...");
			Console.ReadKey();
		}

		private static void GetSamurais(string text)
		{
			//uses a LINQ to retrieve all samurais in the DB
			var samurais = context.Samurais.ToList();
			Console.WriteLine($"{text}: Samurai count is {samurais.Count}");

			foreach (var samurai in samurais)
			{
				Console.WriteLine(samurai.Name);
			}
		}

		private static void AddSamurai()
		{
			var samurai = new Samurai { Name = "Isabel" };
			context.Samurais.Add(samurai);
			context.SaveChanges();
		}
	}
}
