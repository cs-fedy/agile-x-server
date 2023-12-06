namespace AgileX.Domain.Entities;

public record Email(string From, string To, string Subject, string PlainTextConetnt);
