namespace AgileX.Domain.ObjectValues;

public record Email(List<string> To, string Subject, string PlainTextContent);
