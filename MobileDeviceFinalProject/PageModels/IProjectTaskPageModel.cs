using CommunityToolkit.Mvvm.Input;
using MobileDeviceFinalProject.Models;

namespace MobileDeviceFinalProject.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}