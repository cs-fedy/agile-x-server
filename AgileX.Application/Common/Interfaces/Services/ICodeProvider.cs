using AgileX.Domain.ObjectValues;

namespace AgileX.Application.Common.Interfaces.Services;

public interface ICodeProvider
{
    Code Generate(int digits, DateTime expiresIn);
}
