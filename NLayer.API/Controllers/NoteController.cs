using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NoteController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IService<Note> _service;

		public NoteController(IService<Note> service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}


		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var notes = await _service.GetAllAsync();
			var notesDto = _mapper.Map<List<NoteDto>>(notes);
			return Ok(notesDto);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var notes = await _service.GetByIdAsync(id);
			var notesDto = _mapper.Map<NoteDto>(notes);
			return Ok(notesDto);
		}

		[HttpPost]
		public async Task<IActionResult> Save(NoteDto noteDto)
		{
			var notes = await _service.AddAsync(_mapper.Map<Note>(noteDto));
			var notesDto = _mapper.Map<NoteDto>(notes);
			return Ok(notesDto);
		}

		[HttpPut]
		public async Task<IActionResult> Update(NoteDto noteDto)
		{
			await _service.UpdateAsync(_mapper.Map<Note>(noteDto));
			return Ok();
		}

		[HttpDelete]
		public async Task<IActionResult> Remove(int id)
		{
			var note = await _service.GetByIdAsync(id);
			await _service.RemoveAsync(note);
			return Ok();
		}
	}
}
