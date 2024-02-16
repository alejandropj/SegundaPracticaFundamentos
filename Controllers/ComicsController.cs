using Microsoft.AspNetCore.Mvc;
using SegundaPracticaFundamentos.Models;
using SegundaPracticaFundamentos.Repositories;

namespace SegundaPracticaFundamentos.Controllers
{
    public class ComicsController : Controller
    {
        IRepositoryComics repo;
        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }
        public IActionResult CreateLAMBDA()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateLAMBDA(Comic comic)
        {
            this.repo.InsertComicLambda(comic);
            return RedirectToAction("Index");
        }        
        public IActionResult CreatePROCEDURE()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreatePROCEDURE(Comic comic)
        {
            this.repo.InsertComicProcedure(comic);
            return RedirectToAction("Index");
        }
    }
}
