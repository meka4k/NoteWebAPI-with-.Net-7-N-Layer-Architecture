using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs;

public class CreateNoteDto
{
	public string? Tag { get; set; }
	public string? Title { get; set; }
	public string Content { get; set; }
	public DateTime CreatedAt { get; set; }
}
