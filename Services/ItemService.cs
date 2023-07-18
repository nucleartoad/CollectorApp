using Data;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Services
{
	public class ItemService
	{
		private static AppDbContext _context;
		public ItemService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Item> GetItem(int id)
		{
			var item = await _context.Items.FirstOrDefaultAsync( e => e.Id == id);
			return item;
		}

		public async Task<Item> AddItem(Item item)
		{
			_context.Items.Add(item);
			await _context.SaveChangesAsync();
			return item;
		}

		public async Task UpdateItem(Item item)
		{
			_context.Entry(item).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteItem(int id)
		{
			var item = await _context.Items.FindAsync(id);
			_context.Items.Remove(item);
			await _context.SaveChangesAsync();
		}
	}
}