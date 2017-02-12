namespace Luminous.Code.VisualStudio.Commands
{
    public class SuccessResult : CommandResult
    {
        //***
        //===M

        public SuccessResult(string message = "")
        {
            Status = CommandStatuses.Success;
            Message = message;
        }

        //===M

        //***
    }
}