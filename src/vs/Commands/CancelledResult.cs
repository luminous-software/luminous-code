namespace Luminous.Code.VisualStudio.Commands
{
    public class CancelledResult : CommandResult
    {
        public CancelledResult()
        {
            Status = CommandStatuses.Cancelled;
        }
    }
}