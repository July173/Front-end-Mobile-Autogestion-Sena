using System.Collections.Generic;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSena.MAUI.Api.Dtos
{
    public class ModuleFormDto
    {
        public string Name { get; set; }
        public List<FormDto> Form { get; set; }
    }
}