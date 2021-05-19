using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
	public class Horse
	{
		public int Id { get; set; }
		public string Name { get; set; }
		//this is a one-to-one relationship with the samurai model 
		public int SamuraiId { get; set; }
	}
}
