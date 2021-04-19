using ApiCRUDProject.Models;
using ApiCRUDProject.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCRUDProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SchoolApiContext _context;

        public StudentController(SchoolApiContext context)
        {
            this._context = context;
        }
        [HttpGet("GetMarks")]
        public async Task<ActionResult<List<MarksViewModel>>> GetMarks()
        {

            /* var result = (from s in _context.Student
                           from sub in _context.Subject
                           select new
                           {
                               StudentID = s.Id,
                               Name = s.Name,
                               Subject = sub.Name,
                               SubjectID = sub.Id

                           }).ToList();*/
            var result =  _context.Student.SelectMany(s => _context.Subject, (x, y) => new StudentViewModel
            {
                StudentID = x.Id,
                Name = x.Name,
                Subject = y.Name,
                SubjectID = y.Id
            });

            var res = (from m in _context.Marks
                     from st in result
                     where m.StudentId == st.StudentID  && m.SubjectId==st.SubjectID 
                     select new MarksViewModel
                     {
                         MarksID = m.Id,
                         Name = st.Name,
                         Subject = st.Subject,
                         Marks = m.Marks1 
                     });
            

            //var res = _context.Marks.ToList();
            return await Task.FromResult(res.ToList());
        }


        // GET: api/Products/5
        [HttpGet("GetMarksByStudentID/{id}")]
        public async Task<ActionResult<IEnumerable<Marks>>> Get(int id)
        {
            var result = _context.Student.SelectMany(s => _context.Subject, (x, y) => new StudentViewModel
            {
                StudentID = x.Id,
                Name = x.Name,
                Subject = y.Name,
                SubjectID = y.Id
            });

            var res = (from m in _context.Marks
                       from st in result
                       where m.StudentId == st.StudentID && m.SubjectId == st.SubjectID && st.StudentID==id
                       select new MarksViewModel
                       {
                           MarksID = m.Id,
                           Name = st.Name,
                           Subject = st.Subject,
                           Marks = m.Marks1
                       });


           
            var products = await Task.FromResult(res.ToList());

            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }



        // PUT: api/Products/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(Marks marks)
        {
            if (marks.Id == 0)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _context.Marks.Update(marks);

                await _context.SaveChangesAsync();
                return Ok();
            }
            
            

            
            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Add")]
        public async Task<ActionResult<Marks>> Post(Marks products)
        {
            _context.Marks.Add(products);
            await _context.SaveChangesAsync();

            return Ok("Added");
        }

        // DELETE: api/Products/5
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<Marks>> Delete(int id)
        {
            var result = _context.Marks.Where(x => x.Id == id ).FirstOrDefault();
            var products = await Task.FromResult(result);
            if (products == null)
            {
                return NotFound();
            }

            _context.Marks.Remove(products);
            await _context.SaveChangesAsync();

            return products;
        }

        



    }
}
