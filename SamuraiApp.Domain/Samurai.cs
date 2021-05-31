using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
	public class Samurai
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Quote> Quotes { get; set; }
		public Clan Clan { get; set; }
		public List<SamuraiBattle> SamuraiBattles { get; set; }
		//this is a one-to-one relationship with the horse model
		public Horse Horse { get; set; }
		public SecretIdentity SecretIdentity { get; set; }

		public Samurai()
		{
			Quotes = new List<Quote>();
			SamuraiBattles = new List<SamuraiBattle>();
		}
	}
}
