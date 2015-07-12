using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MVC.App.Models;

namespace MVC.App.Controllers
{
	public class GradesController : Controller
	{
		public ActionResult Index (Grades model)
		{
			if (model == null) {
				model = new Grades ();
			}
			Decimal weight = 40 / (2);

			model.PracticeNote.Weight = 20;
			model.FinalTest.Weight = 40;

			model.Partials.Add (new Rating () { Weight = weight});
			model.Partials.Add (new Rating () { Weight = weight});
			return View (model);
		}

		public ActionResult AddPartial (int x)
		{
			var model = new Grades ();
			model.PracticeNote.Weight = 20;
			model.FinalTest.Weight = 40;
			Decimal weight = (Decimal)40 / (x + 1);
			for (var i = 0; i <= x; ++i) 
			{
				model.Partials.Add (new Rating () { Weight = weight});
			}
			RedirectToRouteResult red;
			try
			{
				red = RedirectToAction("Index","Grades",model);
			}
			catch (Exception e) 
			{
				throw e;
			}
			if (red.RouteName == "")
			{
				var r = View("Index",model);
				return r;
			}
			return red;
		}

		public ActionResult RemovePartial (int x)
		{
			var model = new Grades ();
			model.PracticeNote.Weight = 20;
			model.FinalTest.Weight = 40;
			Decimal weight = 0;
			if (x > 1) 
			{
				weight = (Decimal)40 / (x - 1);
			}
			for (var i = 0; i < x-1; ++i) 
			{
				model.Partials.Add (new Rating () { Weight = weight});
			}
			return View("Index",model);
		}

		public ActionResult Test (Grades model)
		{
			return View ("Index", model);
		}

		[HttpPost]
		public ActionResult Calculate (Grades model)
		{
			ViewBag.isCalc = true;
			model.FinalNote = 0;

			model.FinalNote += model.PracticeNote.Value * (model.PracticeNote.Weight/100);

			foreach (var part in model.Partials) 
			{
				model.FinalNote += part.Value * (part.Weight/100);
			}

			model.FinalNote += model.FinalTest.Value * (model.FinalTest.Weight/100);

			return View ("Index", model);	
		}
	}
}

