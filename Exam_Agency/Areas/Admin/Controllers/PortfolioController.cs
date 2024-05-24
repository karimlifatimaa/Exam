using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Data;

namespace Exam_Agency.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class PortfolioController : Controller
    {
       
        private readonly IPortfolioServices _portfolioServices;

        public PortfolioController(IPortfolioServices portfolioServices)
        {
            _portfolioServices = portfolioServices;
        }

        public IActionResult Index()
        {
            var list = _portfolioServices.GetAllPortfolio();
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Portfolio portfolio)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(portfolio == null)
            {
                return NotFound();
            }
            try
            {
                _portfolioServices.AddPortfolio(portfolio);
            }
            catch (EntityNullException ex)
            {

                ModelState.AddModelError("",ex.Message);
                return View();
            }
            catch(FileSIzeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var portfolio=_portfolioServices.GetPortfolio(x=>x.Id == id);
            if(portfolio == null)
            {
                return NotFound();
            }

            try
            {
                _portfolioServices.DeletePortfolio(portfolio.Id);
            }
            catch (EntityNullException ex)
            {

                ModelState.AddModelError("", ex.Message);
                return BadRequest(ex.Message);
            }
            catch(FileNotFoundExceptio ex)
            {
                ModelState.AddModelError(ex.PropertyName,ex.Message);
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var portfolio=_portfolioServices.GetPortfolio(x=> x.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }
        [HttpPost]
        public IActionResult Update(Portfolio portfolio)
        {
            if (!ModelState.IsValid)
            {
                return View(portfolio);
            }

            try
            {
                _portfolioServices.UpdatePortfolio(portfolio.Id,portfolio);
            }
            catch (EntityNullException ex)
            {

                ModelState.AddModelError("",ex.Message);
                return View();
            }
            catch(FileNotFoundExceptio ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return BadRequest(ex.Message);
            }
            catch(FileSIzeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return BadRequest(ex.Message);
            }
            catch(ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
