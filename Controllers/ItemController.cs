using Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemController : ControllerBase
	{
		private static AppDbContext _context;
		public ItemController(AppDbContext context)
		{
			_context = context;
		}
		
		// get item
		[HttpGet]
		public async Task<IActionResult> GetItem(int id)
		{
			var items = await _context.Items.ToListAsync();
			return Ok(items.FirstOrDefault(e => e.Id == id));
		}

		// add item
		[HttpPost]
		public async Task AddItem(Item item)
		{
			_context.Items.Add(item);
			await _context.SaveChangesAsync();
		}

		// update item
		[HttpPut]
		public async Task UpdateItem(Item item)
		{
			_context.Entry(item).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
		
		// delete item
		[HttpDelete]
		public async Task DeleteItem(int id)
		{
			var item = await _context.Items.FindAsync(id);
			_context.Items.Remove(item);
			await _context.SaveChangesAsync();
		}
	}
}