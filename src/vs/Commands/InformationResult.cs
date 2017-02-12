namespace Luminous.Code.VisualStudio.Commands
{
    public class InformationResult : CommandResult
    {
        //***
        //===M

        public InformationResult(string message)
        {
            Status = CommandStatuses.Information;
            Message = message;
        }

        //===M

        //***
    }
}