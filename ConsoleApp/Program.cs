using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace ConsoleApp
{
	internal class Program
	{
		//instance of samurai context
		private static SamuraiContext _context = new SamuraiContext();
		static void Main(string[] args)
		{
			//calling database context method to ensure that the database has been created (DON'T USE IN REAL APPLIACTIONS) 
			/*
			 * reads the database context to see if it exists, if it doesnt then reads the context and finds the relationships to build the new database
			 */
			_context.Database.EnsureCreated();
			//GetSamurais("Before Add: ");
			//AddSamurai();
			RetrieveAndUpdateSamurai();
			GetSamurais("After Add: ");
			QueryFilters();
			RetrieveAndUpdateMultipleSamurai();
			RemoveSamurai("Billy");
			GetSamurais("After operations: ");
			//InsertMultipleSamurai();
			Console.Write("Press any key...");
			Console.ReadKey();
		}

		private static void InsertMultipleSamurai()
		{
			List<string> names = new List<string>() { "Christain", "Camille", "Celine", "Ben", "Betty", "Billy" };
			List<Samurai> samurais = new List<Samurai>();
			foreach (var name in names)
			{
				samurais.Add(new Samurai { Name = name });
			}
			_context.Samurais.AddRange(samurais);
			_context.SaveChanges(); //make sure to use this after making changes!
		}

		private static void GetSamurais(string text)
		{
			//uses a LINQ to retrieve all samurais in the DB
			var samurais = _context.Samurais.ToList();
			Console.WriteLine($"{text}: Samurai count is {samurais.Count}");

			foreach (var samurai in samurais)
			{
				Console.WriteLine(samurai.Name);
			}
		}

		private static void AddSamurai()
		{
			var samurai = new Samurai { Name = "Isabel" };
			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

		private static void QueryFilters()
		{
			var name = "Christian";
			var filter = "C%";
			var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, filter)).ToList();
			Console.WriteLine($"Filter: {filter}");
			foreach(var s in samurais)
			{
				Console.WriteLine(s.Name);
			}
		}

		private static void RetrieveAndUpdateSamurai()
		{
			var samurai = _context.Samurais.FirstOrDefault();
			samurai.Name += "San";
			_context.SaveChanges();
		}

		private static void RetrieveAndUpdateMultipleSamurai()
		{
			var samurais = _context.Samurais.Skip(1).Take(3).ToList();
			samurais.ForEach(s => s.Name += "Boko");
			_context.SaveChanges();
		}

		private static void RemoveSamurai(string name)
		{
			var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
			_context.Samurais.Remove(samurai);
			_context.SaveChanges();
		}
	}
}
