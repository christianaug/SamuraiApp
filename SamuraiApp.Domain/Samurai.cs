﻿using System;
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

		public Samurai()
		{
			Quotes = new List<Quote>();
		}
	}
}
