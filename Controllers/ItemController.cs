using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemController : ControllerBase
	{
		private static ItemService _service;
		public ItemController(ItemService service)
		{
			_service = service;
		}
		
		// get item
		[HttpGet]
		public async Task<ActionResult<Item>> GetItem(int id)
		{
			var item = await _service.GetItem(id);
			if(item == null)
			{
				return NotFound();
			}

			return item;
		}

		// add item
		[HttpPost]
		public async Task<IActionResult> AddItem(Item item)
		{
			var newItem = await _service.AddItem(item);
			return CreatedAtAction(nameof(newItem), new { id = newItem.Id}, newItem);
		}

		// update item
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateItem(int id, Item item)
		{
			if(id != item.Id)
			{
				return BadRequest();
			}

			try
			{
				await _service.UpdateItem(item);
			}
			catch (DbUpdateConcurrencyException)
			{
				throw;
			}

			return NoContent();
		}
		
		// delete item
		[HttpDelete]
		public async Task<IActionResult> DeleteItem(int id)
		{
			await _service.DeleteItem(id);
			return NoContent();
		}
	}
}