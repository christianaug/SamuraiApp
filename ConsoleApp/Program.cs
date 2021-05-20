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
			//RetrieveAndUpdateSamurai();
			//GetSamurais("After Add: ");
			//QueryFilters();
			//RetrieveAndUpdateMultipleSamurai();
			//RemoveSamurai("Billy");
			//InsertNewSamuraiWithAQuote();
			//AddQuoteToExistingSamuraiWhileTracked();
			//InsertMultipleSamurai();
			//EagerLoadSamuraiWithQuotes();
			AddNewSamuraiWithHorse();
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

		private static void InsertNewSamuraiWithAQuote()
		{
			var samurai = new Samurai
			{
				Name = "Kimbei Shimada",
				Quotes = new List<Quote>
				{
					new Quote { Text = "I've come to save you" }
				}
			};
			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

		private static void InsertNewSamuraiWithManyQuotes()
		{
			var samurai = new Samurai
			{
				Name = "Kyuzo",
				Quotes = new List<Quote>
				{
					new Quote { Text = "Watch for my sharp sword" },
					new Quote { Text = "kill them all!" }
				}
			};
			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

		private static void AddQuoteToExistingSamuraiWhileTracked()
		{
			var samurai = _context.Samurais.FirstOrDefault();
			samurai.Quotes.Add(new Quote { Text = "I Bet your are happy that i saved you" });
			_context.SaveChanges();
		}

		private static void AddQuoteToExistingSamuraiNotTrackined(int samuraiId)
		{
			//this version needs to call the samurai and then updates the samurai as well as creting a new Quote
			//var samurai = _context.Samurais.Find(samuraiId);
			//samurai.Quotes.Add(new Quote
			//{
			//	Text = "Now that I'm back, feed me dinner"
			//});

			//using (var newContext = new SamuraiContext())
			//{
			//	newContext.Samurais.Update(samurai);
			//	newContext.SaveChanges();

			//}

			var quote = new Quote
			{
				Text = "feed me now please",
				SamuraiId = samuraiId //this is th foreign key for the samurai
			};

			using (var newContext = new SamuraiContext())
			{
				newContext.Quotes.Add(quote);
				newContext.SaveChanges();
			}

		}

		private static void EagerLoadSamuraiWithQuotes()
		{
			var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
		}

		private static void ProjectSomeProperties()
		{
			//querying the samurais but creating a new type that only gets the id and name
			//SELECT Id, Name FROM Samurais
			var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
		}

		private static void ProjectSamuraisWithQuotes()
		{
			//var somePropertiesWithQuotes = _context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes }).ToList();

			var somePropertiesWithQuotes = _context.Samurais
				.Select(s => new { s.Id, s.Name, 
					FoodQuotes = s.Quotes.Where(q => q.Text.Contains("dinner")) })
				.ToList();
		}

		private static void ExplicitLoadQuotes()
		{
			var samurai = _context.Samurais.FirstOrDefault(s => s.Name.Contains("Julie"));
			_context.Entry(samurai).Collection(s => s.Quotes).Load();
			_context.Entry(samurai).Reference(s => s.Horse).Load();
		}

		private static void ModifyingRelatedDataWhenTracked()
		{
			var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);
			samurai.Quotes[0].Text = "Did you hear that?";
			_context.SaveChanges();
		}

		private static void ModifyingRelatedDataWhenNotTracked()
		{
			var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);
			var quote = samurai.Quotes[0];
			quote.Text += " did you hear that?";
			
			using (var newContext = new SamuraiContext())
			{
				//using this method not only updates the edited quote, but also updates all other quotes that the samurai might have whcih is uneccesary processing
				//newContext.Quotes.Update(quote);

				//this method specifically targets to only update the modified quote
				newContext.Entry(quote).State = EntityState.Modified;
				newContext.SaveChanges();
			}
		}

		private static void JoinBattleAndSamurai()
		{
			var sbJoin = new SamuraiBattle
			{
				SamuraiId = 1,
				BattleId = 3
			};
			_context.Add(sbJoin); //we can create a new SamuraiBattle Object with the foerign keys and hand it over to EF, it will then no what to do
			_context.SaveChanges();
		}

		private static void EnlistSamuraiIntoBattle()
		{
			var battle = _context.Battles.Find(1);
			battle.SamuraiBattles
				.Add(new SamuraiBattle { SamuraiId = 21 });
			_context.SaveChanges();
		}

		private static void RemoveJoinBetweenSamuraiAndBattleSimple()
		{
			var join = new SamuraiBattle
			{
				BattleId = 1,
				SamuraiId = 2
			};
			_context.Remove(join);
			_context.SaveChanges();
		}

		private static void GetSamuraiWithBattles()
		{
			var samuraiWithBattles = _context.Samurais
				.Include(s => s.SamuraiBattles)
				.ThenInclude(sb => sb.Battle)
				.FirstOrDefault(samurai => samurai.Id == 2);

			//using a projection
			var samuraiWithBattlesCleaner = _context.Samurais.Where(s => s.Id == 2)
				.Select(s => new
				{
					Samurai = s,
					Battles = s.SamuraiBattles.Select(sb => sb.Battle)
				})
				.FirstOrDefault();
		}

		private static void AddNewSamuraiWithHorse()
		{
			var samurai = new Samurai
			{
				Name = "Jina Ujichiki",
				Horse = new Horse
				{
					Name = "Silver"
				}
			};

			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

		private static void AddNewHorseToSamuraiUsingId()
		{
			var horse = new Horse
			{
				Name = "Scout",
				SamuraiId = 2
			};
			_context.Add(horse);
			_context.SaveChanges();
		}

		private static void AddNewHorseToSamuraiObject()
		{
			var samurai = _context.Samurais.Find(10);
			samurai.Horse = new Horse { Name = "Black Beauty" };
			_context.SaveChanges();
		}

		private static void AddNewHorseToDisconnectedSamuraiObject()
		{
			var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 5);
			samurai.Horse = new Horse
			{
				Name = "Mr. Ed"
			};

			using (var newContext = new SamuraiContext())
			{
				newContext.Attach(samurai);
				newContext.SaveChanges();
			}
		}
	}
}