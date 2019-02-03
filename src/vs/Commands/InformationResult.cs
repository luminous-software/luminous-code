namespace Luminous.Code.VisualStudio.Commands
{
    public class InformationResult : CommandResult
    {
        public InformationResult(string message)
        {
            Status = CommandStatuses.Information;
            Message = message;
        }
    }
}