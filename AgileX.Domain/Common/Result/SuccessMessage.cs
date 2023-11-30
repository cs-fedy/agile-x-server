namespace AgileX.Domain.Common.Result;


public class SuccessMessage
{
    public string Messsage { get; }

    public SuccessMessage(string messsage) {  Messsage = messsage; }
}
