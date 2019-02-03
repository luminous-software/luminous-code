namespace Luminous.Code.VisualStudio.Commands
{
    public class ProblemResult : CommandResult
    {
        public ProblemResult(string message)
        {
            Status = CommandStatuses.Problem;
            Message = message;
        }
    }
}