﻿using Microsoft.AspNetCore.Mvc;
using ToDo.Data;
using ToDo.Models;
using ToDo.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Controllers
{
    public class EditDto
    {
        public int Id { get; set; }
        public bool Done { get; set; }
        public string? Text { get; set; }
        public string? GroupName { get; set; }
    }
    public class ItemsController : Controller
    {
        public readonly ToDoContext _context;

        public ItemsController(ToDoContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> HomeScreen()
        {
            var item = await _context.Items.ToListAsync();
            return View(item);
        }

        public async Task<IActionResult> Test()
        {
            var item = await _context.Items.ToListAsync();
            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, GroupToDo, TextToDo, CheckedDone")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("HomeScreen");
            }
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewTask([FromBody] EditDto dto)

        {

            Item item = new Item();
            item.TextToDo = dto.Text;
            item.GroupToDo = dto.GroupName;
            int resultNum = 0;

            try
            {
                _context.Items.Add(item);
                resultNum = await _context.SaveChangesAsync(); //result contains 
                                                               //number of state entries written to the database

                Console.WriteLine(resultNum);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            if (resultNum >= 1) return Ok();
            else return NotFound();
        }
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDone([FromBody] EditDto dto)
        {
            var item = await _context.Items.FindAsync(dto.Id);
            if (item == null)
                return NotFound();

            item.CheckedDone = dto.Done;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateText([FromBody] EditDto dto)
        {
            var item = await _context.Items.FindAsync(dto.Id);
            if (item == null)
                return NotFound();

            item.TextToDo = dto.Text;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id, GroupToDo, TextToDo, CheckedDone")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("HomeScreen");
            }
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }
        [HttpPost, ActionName("Delete")]//this atribute is used to mask the name ConfirmedDelete,
                                        //because you are not allowed to have two Delete methods
                                        //with the same parameter (int id)
        public async Task<IActionResult> ConfirmedDelete(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("HomeScreen");

        }

    }
}
