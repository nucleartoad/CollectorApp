using Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CollectionController : ControllerBase
	{
		private static AppDbContext _context;
		public CollectionController(AppDbContext context)
		{
			_context = context;
		}

		// get collections
		[HttpGet]
		public async Task<IActionResult> GetCollections()
		{
			var collections = await _context.Collections.Include( e => e.Items ).ToListAsync();
			return Ok(collections);
		}

		// get collection
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCollection(int id)
		{
			var collection = await _context.Collections.FirstOrDefaultAsync(e => e.Id == id);
			return Ok(collection);
		}

		// create collection
		[HttpPost]
		public async Task CreateCollection(Collection collection)
		{
			await _context.Collections.AddAsync(collection);
		}

		// update collection
		[HttpPut]
		public async Task UpdateCollection(Collection collection)
		{
			_context.Entry(collection).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		// delete collection
		[HttpDelete]
		public async Task DeleteCollection(int id)
		{
			var collection = await _context.Collections.FindAsync(id);
			_context.Collections.Remove(collection);
			await _context.SaveChangesAsync();
		}
	}
}