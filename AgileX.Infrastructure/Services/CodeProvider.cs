using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.ObjectValues;

namespace AgileX.Infrastructure.Services;

public class CodeProvider : ICodeProvider
{
    public Code Generate(int digits, DateTime expiresIn)
    {
        Random random = new();
        var max = (int)Math.Pow(10, digits) - 1;
        var min = (int)Math.Pow(10, digits - 1);

        return new Code(random.Next(min, max), expiresIn);
    }
}
