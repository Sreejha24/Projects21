using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC21.Data;
using MVC21.Models;
using MVC21.Models.Views;

namespace MVC21.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Person.Include(p => p.Address);
            return View(await applicationDbContext.ToListAsync());
        }


        public IActionResult RawQuery(int id)
        {
            var sql = "SELECT * FROM Person WHERE PersonID = {0}";

            var data = _context.Person.FromSqlRaw(sql, id).FirstOrDefault();

            return View(data);
        }


        public IActionResult RawQueryComplex(int id)
        {
            IList<PersonAddressView> list = new List<PersonAddressView>();
            using (var conn = _context.Database.GetDbConnection())
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    var sql = "SELECT P.PersonID,P.FirstName,P.LastName,P.Email,P.Mobile,P.AddressID,A.AddressLine,A.City,A.Pin FROM People P,PAddresss A" +
                        "WHERE P.AddressID = A.AddressID";

                    //var sql = "SELECT P.PersonID, P.FirstName, P.LastName, P.Email, P.Mobile, P.AddressID, A.AddressLine, A.City, A.Pin FROM People P  left join PAddress A on P.AddressID = A.AddressID";

                    command.CommandText = sql;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var record = new PersonAddressView()
                                {
                                    PersonID = reader.GetInt32(0),
                                    FirstName = reader.GetString(1),
                                    LastName = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Mobile = reader.GetInt64(4),
                                    AddressID = reader.GetInt32(5),
                                    AddressLine = reader.GetString(6),
                                    City = reader.GetString(7),
                                    Pin = reader.GetInt32(8)
                                };
                                list.Add(record);
                            }
                        }
                    } // reader
                } // command
                conn.Close();
            } // connection

            return View(list);
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.Address)
                .FirstOrDefaultAsync(m => m.PersonID == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Set<PAddress>(), "AddressID", "AddressID");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonID,FirstName,LastName,Email,Mobile,AddressId")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Set<PAddress>(), "AddressID", "AddressID", person.AddressId);
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Set<PAddress>(), "AddressID", "AddressID", person.AddressId);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("PersonID,FirstName,LastName,Email,Mobile,AddressId")] Person person)
        {
            if (id != person.PersonID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Set<PAddress>(), "AddressID", "AddressID", person.AddressId);
            return View(person);
        }

        //public async Task<ActionResult> Search()
        //{
        //    return View(new List<Person>());
        //}

        //[HttpPost]
        //public async Task<ActionResult> Search(IFormCollection form)
        //{
        //    var keyword = form["keyword"];
        //    var data = await _context.Person.Where(d => d.FirstName.Contains(keyword) || d.LastName.Contains(keyword)).ToListAsync();
        //    ViewBag.Keyword = keyword;
        //    return View(data);
        //}




        public JsonResult GetResult(string searchBy, string searchValue)
        {
            List<Person> PersonDetails = new List<Person>();
            if (searchBy == "PersonID")
            {
                int id = Convert.ToInt32(searchValue);
                PersonDetails = _context.Person.Where(e => e.PersonID == id || searchValue == null).ToList();
                return Json(PersonDetails);

            }
            else if (searchBy == "FirstName")
            {
                PersonDetails = _context.Person.Where(e => e.FirstName.StartsWith(searchValue) || searchValue == null).ToList();
                return Json(PersonDetails);
            }
            else if (searchBy == "LastpName")
            {
                PersonDetails = _context.Person.Where(e => e.LastName.StartsWith(searchValue) || searchValue == null).ToList();
                return Json(PersonDetails);
            }
            else if (searchBy == "Email")
            {
                PersonDetails = _context.Person.Where(e => e.FirstName.StartsWith(searchValue) || searchValue == null).ToList();
                return Json(PersonDetails);
            }
            else if (searchBy == "Mobile")
            {
                long mobile = Convert.ToInt64(searchValue);
                PersonDetails = _context.Person.Where(e => e.Mobile == mobile || searchValue == null).ToList();
                return Json(PersonDetails);
            }
            else if (searchBy == "AddressID")
            {
                int aid = Convert.ToInt32(searchValue);
                PersonDetails = _context.Person.Where(e => e.Address.AddressID == aid || searchValue == null).ToList();
                return Json(PersonDetails);
            }
            else
            {
                var Persons = _context.Person;
                return Json(Persons);
            }
        }



        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .Include(p => p.Address)
                .FirstOrDefaultAsync(m => m.PersonID == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int? id)
        {
            return _context.Person.Any(e => e.PersonID == id);
        }
    }
}
