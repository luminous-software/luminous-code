
namespace Luminous.Code.VisualStudio.Commands
{
    using Packages;

    public abstract class CommandResult
    {
        //***

        public string Message { get; set; }

        public CommandStatuses Status { get; set; }

        public bool Succeeded
             => (Status == CommandStatuses.Success);

        //===M
        //===M

        public CommandResult ShowSuccess(string title = "Success")
        {
            if (Status == CommandStatuses.Success)
            {
                PackageBase.DisplayMessage(title, Message);
            }

            return this;
        }

        public CommandResult ShowProblem(string title = "Problem")
        {
            if (Status == CommandStatuses.Problem)
            {
                PackageBase.DisplayProblem(title, Message);
            }

            return this;
        }

        public CommandResult ShowCancelled(string title = "Cancelled")
        {
            if (Status == CommandStatuses.Cancelled)
            {
                PackageBase.DisplayCancelled(title, Message);
            }

            return this;
        }

        public CommandResult ShowInformation(string title = "Please Note")
        {
            if (Status == CommandStatuses.Information)
            {
                PackageBase.DisplayInformation(title, Message);
            }

            return this;
        }

        //***
    }
}