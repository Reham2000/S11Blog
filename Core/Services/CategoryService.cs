using Domain.Models;
using Infrasitructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Core.Services
{
    public class CategoryService
    {
        private readonly IBaseRepository<Category> _catRepo;
        private readonly Action<string> _logAction;

        public CategoryService(IBaseRepository<Category> catRepo)
        {
            _catRepo = catRepo;
            _logAction = message => Console.WriteLine(message);
        }


        public async Task<IEnumerable<SelectListItem>> GetCategoriesWithSelectListItem()
        {
            var categories = await _catRepo.GetAll();
            return categories.Select(c => new SelectListItem {
                Value = c.Id.ToString(),
                Text = c.Name
            });
        }
    }
}
// category   {id, name , description}
// in view make a select list
// text for user  => name
// value for Db => id
// class SelectListItem{
//{  text ;
// value;
// }

// new SelectListItem (value: id,text: name)


// post       {id, title, content, categoryId}