using Services;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using System.Threading;

using Models.Dtos;


namespace Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CollectionController : ControllerBase
	{
		private static CollectionService _service;
		public CollectionController(CollectionService service)
		{
			_service = service;
		}

		// get collections
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet]
		public async Task<ActionResult<List<Collection>>> GetCollections()
		{
			string username = User.Identity.Name;
			Thread.Sleep(1000);
			var collections = await _service.GetCollections(username);

			if(collections == null) 
			{
				return NotFound();
			}

			return Ok(collections);
		}

		// get collection
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("{id}")]
		public async Task<ActionResult<Collection>> GetCollection(int id)
		{
			string username = User.Identity.Name;
			Thread.Sleep(1000);
			var collection = await _service.GetCollection(id);
			if(collection == null)
			{
				return NotFound();
			}

			return Ok(collection);
		}

		// create collection
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost]
		public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionDto collectionDto)
		{
			string username = User.Identity.Name;
			var newCollection = await _service.CreateCollection(collectionDto, username);
			return CreatedAtAction(nameof(GetCollection), new { id = newCollection.Id }, newCollection);
		}

		// update collection
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCollection(int id, Collection collection)
		{
			if (id != collection.Id)
			{
				return BadRequest();
			}

			try
			{
				await _service.UpdateCollection(collection);
			}
			catch (DbUpdateConcurrencyException)
			{
				throw;
			}

			return NoContent();
		}

		// delete collection
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpDelete("{collectionId}")]
		public async Task<IActionResult> DeleteCollection(int collectionId)
		{
			await _service.DeleteCollection(collectionId);
			return NoContent();
		}
	}
}