namespace AutogestionSena.MAUI.Api.Dtos
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? FirstLastName { get; set; }
        public string? SecondLastName { get; set; }
        public int PhoneNumber { get; set; }
        public int TypeIdentification { get; set; }
        public int NumberIdentification { get; set; }
        public bool Active { get; set; }
        public string? Image { get; set; }
    }

    public class Role
    {
        public string? Id { get; set; }
        public string? TypeRole { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public Person? Person { get; set; }
        public Role? Role { get; set; }
        public bool IsActive { get; set; }
        public bool Registered { get; set; }
    }

    public class RegisterResponse
    {
        public string? Detail { get; set; }
        public Person? Person { get; set; }
        public User? User { get; set; }
        public bool Success { get; set; }
    }

    public class ValidateLoginResponse
    {
        public string? Success { get; set; }  // Para el primer endpoint
        public string? Access { get; set; }
        public string? Refresh { get; set; }
        public UserLoginData? User { get; set; }
        public string? Email { get; set; }
        public int? Role { get; set; }
        public string? Person { get; set; }
    }

    public class UserLoginData
    {
        public string? Email { get; set; }
        public int Id { get; set; }
        public int Role { get; set; }
        public int Person { get; set; }
        public bool Registered { get; set; }
    }

    public class UserStatus
    {
        public bool? IsActive { get; set; }
        public string? Estado { get; set; }
    }

    public class SecondFactorRequest
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
    }

    public class PasswordResetRequestResponse
    {
        public string? code { get; set; }  // Código de recuperación que retorna el backend
        public string? fecha_expiracion { get; set; }  // Fecha de expiración del código
        public string? success { get; set; }  // Mensaje de éxito
    }

    public class PasswordResetResponse
    {
      public string? success { get; set; } // Mensaje de éxito: "Contraseña actualizada correctamente."
    }
}