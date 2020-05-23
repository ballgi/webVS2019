﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webVS2019.Models;

namespace webVS2019.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CoursesController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _context.Course.Where(o=>o.IsDeleted == null || o.IsDeleted ==false).ToListAsync();
        }

        // GET: api/Courses/CourseStudents
        [HttpGet("CourseStudents")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetvwCourseStudents()
        {
            return await _context.VwCourseStudents.ToListAsync();
        }

        // GET: api/Courses/CourseStudentCount
        [HttpGet("CourseStudentCount")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetvwCourseStudentCount()
        {
            return await _context.VwCourseStudentCount.ToListAsync();
        }

        /// <summary>
        /// Raw SQL Query
        /// </summary>
        /// <returns></returns>
        // GET: api/Courses/DepartmentCourseCount
        [HttpGet("DepartmentCourseCount")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetvwDepartmentCourseCount()
        {
            var vw1 = await _context.VwDepartmentCourseCount.FromSqlRaw("select * from VwDepartmentCourseCount").ToListAsync();
            return vw1;
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null || (course.IsDeleted.HasValue && course.IsDeleted.Value))
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            course.DateModified = DateTime.Now;
            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null || (course.IsDeleted.HasValue && course.IsDeleted.Value))
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
