using System.ComponentModel;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace PortfolioTracker.Helpers
{
    // from https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        public NotifyTaskCompletion([NotNull] Task<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                Task _ = WatchTaskAsync(task);
            }
        }

        [NotNull]
        private Task<TResult> Task { get; }

        public TResult Result => Task.Status == TaskStatus.RanToCompletion ? Task.Result : default(TResult);

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task WatchTaskAsync([NotNull] Task task)
        {
            try
            {
                await task;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }

            OnPropertyChanged(nameof(Result));
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}