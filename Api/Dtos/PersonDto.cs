using System.Text.Json.Serialization;

namespace AutogestionSena.MAUI.Api.Dtos
{
    /// <summary>
    /// DTO para el registro de aprendices
    /// Estructura del API:
    /// {
    ///   "first_name": "string",
    ///   "second_name": "string",
    ///   "first_last_name": "string",
    ///   "second_last_name": "string",
    ///   "phone_number": 9223372036854776000,
    ///   "type_identification": 0,
    ///   "number_identification": 2147483647,
    ///   "active": true,
    ///   "email": "user@example.com"
    /// }
    /// </summary>
    public class RegisterPayloadDto
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("second_name")]
        public string? SecondName { get; set; }

        [JsonPropertyName("first_last_name")]
        public string? FirstLastName { get; set; }

        [JsonPropertyName("second_last_name")]
        public string? SecondLastName { get; set; }

        [JsonPropertyName("type_identification")]
        public int TypeIdentification { get; set; }

        [JsonPropertyName("number_identification")]
        public int NumberIdentification { get; set; }

        [JsonPropertyName("phone_number")]
        public long PhoneNumber { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;
    }

    public class PersonDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("second_name")]
        public string? SecondName { get; set; }

        [JsonPropertyName("first_last_name")]
        public string? FirstLastName { get; set; }

        [JsonPropertyName("second_last_name")]
        public string? SecondLastName { get; set; }

        [JsonPropertyName("phone_number")]
        public long PhoneNumber { get; set; }

        [JsonPropertyName("type_identification")]
        public int TypeIdentification { get; set; }

        [JsonPropertyName("number_identification")]
        public int NumberIdentification { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }
    }
}