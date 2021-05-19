using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
	public class SamuraiBattle
	{
		//this class is used a join to represent a many-to-many relationship
		public int SamuraiId { get; set; }
		public int BattleId { get; set; }
		public Samurai Samurai { get; set; }
		public Battle Battle { get; set; }
	}
}
