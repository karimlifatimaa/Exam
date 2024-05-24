using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class PortfolioServices : IPortfolioServices
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PortfolioServices(IPortfolioRepository portfolioRepository, IWebHostEnvironment webHostEnvironment)
        {
            _portfolioRepository = portfolioRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddPortfolio(Portfolio portfolio)
        {
            if (portfolio == null)
                throw new EntityNullException(" ","Entity not found");
            if (portfolio.PhotoFile == null)
                throw new EntityNullException(" ", "Entity not found");
            if (!portfolio.PhotoFile.ContentType.Contains("image/"))
                throw new ContentTypeException("PhotoFile", "Content type error");
            if (portfolio.PhotoFile.Length > 2097152)
                throw new FileSIzeException("PhotoFile", "File size error");


            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(portfolio.PhotoFile.FileName);
            string path = _webHostEnvironment.WebRootPath + @"\Uploads\Portfolio\" + fileName;

            using(FileStream stream =new FileStream(path, FileMode.Create))
            {
                portfolio.PhotoFile.CopyTo(stream);
            }

            portfolio.ImgUrl = fileName;
            _portfolioRepository.Add(portfolio);
            _portfolioRepository.Commit();

        }

        public void DeletePortfolio(int id)
        {
            var portfolio=_portfolioRepository.Get(x=>x.Id == id);
            if(portfolio == null)
                throw new EntityNullException(" ", "Entity not found");
            string path=_webHostEnvironment.WebRootPath +@"\Uploads\Portfolio\"+portfolio.ImgUrl;
            if (!File.Exists(path))
                throw new FileNotFoundExceptio("PhotoFile", "PhotoFile not found");
            File.Delete(path);
            _portfolioRepository.Delete(portfolio);
            _portfolioRepository.Commit();
        }

        public List<Portfolio> GetAllPortfolio(Func<Portfolio, bool>? func=null)
        {
            return _portfolioRepository.GetAll(func);
        }

        public Portfolio GetPortfolio(Func<Portfolio, bool>? func = null)
        {
            return _portfolioRepository.Get(func);
        }

        public void UpdatePortfolio(int id, Portfolio portfolio)
        {
            var oldPortfolio=_portfolioRepository.Get(x=> x.Id == id);
            if(oldPortfolio == null)
                throw new EntityNullException(" ", "Entity not found");
            if (portfolio.PhotoFile != null)
            {
                if(!portfolio.PhotoFile.ContentType.Contains("image/"))
                    throw new ContentTypeException("PhotoFile", "Content type error");
                if(portfolio.PhotoFile.Length> 2097152)
                    throw new FileSIzeException("PhotoFile", "File size error");

                var oldPaht = _webHostEnvironment.WebRootPath + @"\Uploads\Portfolio\" + oldPortfolio.ImgUrl;
                if(!File.Exists(oldPaht))
                    throw new FileNotFoundExceptio("PhotoFile", "PhotoFile not found");
                File.Delete(oldPaht);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(portfolio.PhotoFile.FileName);
                string path = _webHostEnvironment.WebRootPath + @"\Uploads\Portfolio\" + fileName;
                using(FileStream stream=new FileStream(path, FileMode.Create))
                {
                    portfolio.PhotoFile.CopyTo(stream);
                }
                oldPortfolio.ImgUrl = fileName;
            }
            oldPortfolio.Name=portfolio.Name;
            oldPortfolio.Description=portfolio.Description;
            _portfolioRepository.Commit();
        }
    }
}
