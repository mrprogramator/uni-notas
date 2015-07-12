using System;
using System.Collections.Generic;

namespace MVC.App.Models
{
	public class Grades
	{
		public Rating PracticeNote { get; set; }

		public List<Rating> Partials { get; set; }

		public Rating FinalTest { get; set; }

		public Decimal FinalNote { get; set; }

		public Grades()
		{
			PracticeNote = new Rating ();
			Partials = new List<Rating> ();
			FinalTest = new Rating ();
		}
	}
}

