namespace Business.Exceptions;

public class RegisterException : Exception {
    public RegisterException() {
    }

    public RegisterException(string? message) : base(message) {
    }

    public RegisterException(string? message, Exception? innerException) : base(message, innerException) {
    }
}