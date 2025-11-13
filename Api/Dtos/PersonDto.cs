namespace AutogestionSena.MAUI.Api.Dtos
{
    public class RegisterPayloadDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public int TypeIdentification { get; set; }
        public int NumberIdentification { get; set; }
        public int PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
    }

    public class PersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public int PhoneNumber { get; set; }
        public int TypeIdentification { get; set; }
        public int NumberIdentification { get; set; }
        public bool Active { get; set; }
        public string Image { get; set; }
    }
}