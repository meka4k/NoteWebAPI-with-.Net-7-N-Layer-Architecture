﻿using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Mapping;

public class MapProfile : Profile
{
	public MapProfile()
	{
		CreateMap<Note, NoteDto>().ReverseMap();
		CreateMap<Tag, TagDto>().ReverseMap();
	}
}