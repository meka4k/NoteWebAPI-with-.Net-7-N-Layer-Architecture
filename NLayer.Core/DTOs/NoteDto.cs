﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs;

public class NoteDto
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Content { get; set; }
	public DateTime CreatedAt { get; set; }
}