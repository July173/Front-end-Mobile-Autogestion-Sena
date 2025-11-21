using System;
using System.Linq;

namespace AutogestionSena.MAUI.Validators
{
    /// <summary>
    /// Validador para la página de verificación de código (CodeVerificationPage)
    /// Valida códigos de recuperación y verificación de 2FA
    /// </summary>
    public static class CodeVerificationValidator
    {
        /// <summary>
        /// Valida que el código no esté vacío
        /// </summary>
        public static bool IsCodeValid(string? code)
        {
            return !string.IsNullOrWhiteSpace(code);
        }

        /// <summary>
        /// Valida que el código tenga exactamente 6 dígitos numéricos
        /// </summary>
        public static bool IsCode6DigitsValid(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            return code.Length == 6 && code.All(char.IsDigit);
        }

        /// <summary>
        /// Obtiene el mensaje de error para código vacío
        /// </summary>
        public static string GetCodeEmptyErrorMessage()
        {
            return "Por favor ingresa el código de verificación.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para código inválido
        /// </summary>
        public static string GetCodeInvalidErrorMessage()
        {
            return "El código debe tener 6 dígitos.";
        }
    }

    /// <summary>
    /// Validador para la página de recuperación de contraseña (PasswordRecoveryPage)
    /// </summary>
    public static class PasswordRecoveryValidator
    {
        /// <summary>
        /// Valida que el email no esté vacío
        /// </summary>
        public static bool IsEmailValid(string? email)
        {
            return !string.IsNullOrWhiteSpace(email);
        }

        /// <summary>
        /// Valida el formato básico de email
        /// </summary>
        public static bool IsEmailFormatValid(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return email.Contains("@") && email.Contains(".");
        }

        /// <summary>
        /// Valida que sea un email institucional del SENA
        /// </summary>
        public static bool IsInstitutionalEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return email.EndsWith("@soy.sena.edu.co", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Obtiene el mensaje de error para email vacío
        /// </summary>
        public static string GetEmailEmptyErrorMessage()
        {
            return "Por favor ingresa tu correo electrónico.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para email inválido
        /// </summary>
        public static string GetEmailInvalidErrorMessage()
        {
            return "Por favor ingresa un correo electrónico válido.";
        }
    }

    /// <summary>
    /// Validador para la página de restablecimiento de contraseña (PasswordResetPage)
    /// </summary>
    public static class PasswordResetValidator
    {
        /// <summary>
        /// Longitud mínima de contraseña
        /// </summary>
        public const int MinPasswordLength = 8;

        /// <summary>
        /// Valida que la contraseña no esté vacía
        /// </summary>
        public static bool IsPasswordValid(string? password)
        {
            return !string.IsNullOrWhiteSpace(password);
        }

        /// <summary>
        /// Valida que la contraseña tenga la longitud mínima
        /// </summary>
        public static bool IsPasswordLengthValid(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return password.Length >= MinPasswordLength;
        }

        /// <summary>
        /// Valida que ambas contraseñas coincidan
        /// </summary>
        public static bool DoPasswordsMatch(string? password, string? confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                return false;

            return password == confirmPassword;
        }

        /// <summary>
        /// Valida la fortaleza básica de la contraseña (al menos una letra y un número)
        /// </summary>
        public static bool IsPasswordStrong(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            bool hasLetter = password.Any(char.IsLetter);
            bool hasDigit = password.Any(char.IsDigit);

            return hasLetter && hasDigit;
        }

        /// <summary>
        /// Obtiene el mensaje de error para contraseña vacía
        /// </summary>
        public static string GetPasswordEmptyErrorMessage()
        {
            return "Por favor ingresa la nueva contraseña.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para contraseña corta
        /// </summary>
        public static string GetPasswordLengthErrorMessage()
        {
            return $"La contraseña debe tener al menos {MinPasswordLength} caracteres.";
        }

        /// <summary>
        /// Obtiene el mensaje de error cuando las contraseñas no coinciden
        /// </summary>
        public static string GetPasswordMismatchErrorMessage()
        {
            return "Las contraseñas no coinciden.";
        }
    }

    /// <summary>
    /// Validador para la página de registro (RegisterPage)
    /// </summary>
    public static class RegisterValidator
    {
        /// <summary>
        /// Valida que el email no esté vacío
        /// </summary>
        public static bool IsEmailValid(string? email)
        {
            return !string.IsNullOrWhiteSpace(email);
        }

        /// <summary>
        /// Valida que el nombre no esté vacío
        /// </summary>
        public static bool IsNameValid(string? name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        /// <summary>
        /// Valida que el apellido no esté vacío
        /// </summary>
        public static bool IsLastNameValid(string? lastName)
        {
            return !string.IsNullOrWhiteSpace(lastName);
        }

        /// <summary>
        /// Valida que el número de documento no esté vacío
        /// </summary>
        public static bool IsDocumentNumberValid(string? documentNumber)
        {
            return !string.IsNullOrWhiteSpace(documentNumber);
        }

        /// <summary>
        /// Valida que el número de documento sea numérico
        /// </summary>
        public static bool IsDocumentNumberNumeric(string? documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                return false;

            return documentNumber.All(char.IsDigit);
        }

        /// <summary>
        /// Valida que el teléfono no esté vacío
        /// </summary>
        public static bool IsPhoneValid(string? phone)
        {
            return !string.IsNullOrWhiteSpace(phone);
        }

        /// <summary>
        /// Valida que el teléfono sea numérico
        /// </summary>
        public static bool IsPhoneNumeric(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return phone.All(char.IsDigit);
        }

        /// <summary>
        /// Valida que el teléfono tenga una longitud válida (7-10 dígitos)
        /// </summary>
        public static bool IsPhoneLengthValid(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return phone.Length >= 7 && phone.Length <= 10;
        }

        /// <summary>
        /// Valida que todos los campos requeridos estén completos
        /// </summary>
        public static bool AreAllFieldsValid(string? email, string? firstName, string? lastName, 
            string? documentNumber, string? phone, bool hasDocumentType)
        {
            return IsEmailValid(email) &&
                   IsNameValid(firstName) &&
                   IsLastNameValid(lastName) &&
                   IsDocumentNumberValid(documentNumber) &&
                   IsPhoneValid(phone) &&
                   hasDocumentType;
        }

        /// <summary>
        /// Obtiene el mensaje de error para email obligatorio
        /// </summary>
        public static string GetEmailRequiredErrorMessage()
        {
            return "El correo institucional es obligatorio.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para nombres obligatorios
        /// </summary>
        public static string GetNamesRequiredErrorMessage()
        {
            return "Los nombres y apellidos son obligatorios.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para tipo de documento obligatorio
        /// </summary>
        public static string GetDocumentTypeRequiredErrorMessage()
        {
            return "Debes seleccionar un tipo de documento.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para número de documento obligatorio
        /// </summary>
        public static string GetDocumentNumberRequiredErrorMessage()
        {
            return "El número de documento es obligatorio.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para teléfono obligatorio
        /// </summary>
        public static string GetPhoneRequiredErrorMessage()
        {
            return "El teléfono es obligatorio.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para documento con formato inválido
        /// </summary>
        public static string GetDocumentFormatErrorMessage()
        {
            return "El número de documento debe contener solo dígitos.";
        }

        /// <summary>
        /// Obtiene el mensaje de error para teléfono con formato inválido
        /// </summary>
        public static string GetPhoneFormatErrorMessage()
        {
            return "El teléfono debe contener solo dígitos y tener entre 7 y 10 caracteres.";
        }
    }

    /// <summary>
    /// Validador para el modal de autenticación de dos factores (TwoFactorModal)
    /// </summary>
    public static class TwoFactorValidator
    {
        /// <summary>
        /// Valida que el código completo tenga exactamente 6 dígitos
        /// </summary>
        public static bool IsCodeComplete(string code1, string code2, string code3, 
            string code4, string code5, string code6)
        {
            return !string.IsNullOrWhiteSpace(code1) &&
                   !string.IsNullOrWhiteSpace(code2) &&
                   !string.IsNullOrWhiteSpace(code3) &&
                   !string.IsNullOrWhiteSpace(code4) &&
                   !string.IsNullOrWhiteSpace(code5) &&
                   !string.IsNullOrWhiteSpace(code6);
        }

        /// <summary>
        /// Combina los 6 dígitos en un código completo
        /// </summary>
        public static string CombineCode(string code1, string code2, string code3, 
            string code4, string code5, string code6)
        {
            return $"{code1}{code2}{code3}{code4}{code5}{code6}";
        }

        /// <summary>
        /// Valida que cada dígito sea numérico
        /// </summary>
        public static bool IsCodeNumeric(string code1, string code2, string code3, 
            string code4, string code5, string code6)
        {
            return IsDigit(code1) && IsDigit(code2) && IsDigit(code3) &&
                   IsDigit(code4) && IsDigit(code5) && IsDigit(code6);
        }

        /// <summary>
        /// Valida que un string sea un solo dígito
        /// </summary>
        public static bool IsDigit(string? digit)
        {
            return !string.IsNullOrWhiteSpace(digit) && 
                   digit.Length == 1 && 
                   char.IsDigit(digit[0]);
        }

        /// <summary>
        /// Obtiene el mensaje de error para código incompleto
        /// </summary>
        public static string GetCodeIncompleteErrorMessage()
        {
            return "Por favor ingresa los 6 dígitos del código";
        }
    }
}
