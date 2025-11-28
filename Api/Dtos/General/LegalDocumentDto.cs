using System;

namespace AutogestionSena.MAUI.Api.Dtos.General
{
 public class LegalDocumentDto
 {
 public int Id { get; set; }
 public string? Type { get; set; }
 public string? Title { get; set; }
 public DateTime? EffectiveDate { get; set; }
 public bool? Active { get; set; }
 }
}