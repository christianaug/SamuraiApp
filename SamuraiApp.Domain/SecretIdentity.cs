using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
	public class SecretIdentity
	{
		public int Id { get; set; }
		public string RealName { get; set; }
		//integers are non nullable, therefore a secret identity cannot exist without a samurai
		//you can use "int?" to make it a nullable int
		//this acts as a foreign key
		public int SamuraiId { get; set; }
		//REMOVE navigation property
	}
}
