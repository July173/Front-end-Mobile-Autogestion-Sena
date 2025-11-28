namespace AutogestionSena.MAUI.Api.Dtos.General
{
 public class LegalSectionDto
 {
 public int Id { get; set; }
 public int Document { get; set; }
 public int? Parent { get; set; }
 public int Order { get; set; }
 public string? Code { get; set; }
 public string? Title { get; set; }
 public string? Content { get; set; }
 public bool? Active { get; set; }
 }
}