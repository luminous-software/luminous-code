namespace Luminous.Code.VisualStudio.Commands
{
    public class ProblemResult : CommandResult
    {
        //***
        //===M

        public ProblemResult(string message)
        {
            Status = CommandStatuses.Problem;
            Message = message;
        }

        //===M

        //***
    }
}