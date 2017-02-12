namespace Luminous.Code.VisualStudio.Commands
{
    public class CancelledResult : CommandResult
    {
        //***
        //===M

        public CancelledResult()
        {
            Status = CommandStatuses.Cancelled;
        }

        //===M

        //***
    }
}