namespace Luminous.Code.VisualStudio.Commands
{
    public class SuccessResult : CommandResult
    {
        public SuccessResult(string message = "")
        {
            Status = CommandStatuses.Success;
            Message = message;
        }
    }
}