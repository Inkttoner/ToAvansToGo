namespace ToAvansToGo.Application;

    public class NotFoundException(string message) : InvalidOperationException(message);
    public class ConflictException(string message) : InvalidOperationException(message);
    public class BusinessRuleException(string message) : InvalidOperationException(message);
