using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace notary_company.api.Controllers
{
    public class RequestsController : Controller
    {
        // GET: RequestsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RequestsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RequestsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RequestsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RequestsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RequestsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RequestsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
