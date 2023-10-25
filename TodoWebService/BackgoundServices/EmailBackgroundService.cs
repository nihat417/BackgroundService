using System.Net.Mail;
using System.Net;
using TodoWebService.Models.Entities;
using TodoWebService.Services;

namespace TodoWebService.BackgoundServices
{
    public class EmailBackgroundService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private TodoService? _todoService;
        private IServiceProvider? _serviceProvider;

        public EmailBackgroundService(IServiceProvider? serviceProvider)
        {
            this._serviceProvider = serviceProvider;

        }

        private async void DoWork(object? state)
        {

            using var scope = _serviceProvider!.CreateScope();
            _todoService= scope.ServiceProvider.GetService(typeof(ITodoService)) as TodoService;

            if (_todoService is null)
                return;
            List<TodoItem> todo = await _todoService.GetAllTodoItems();
            if (todo is not null)
            {
                foreach (var i in todo)
                {
                    if (!i.Notify)
                    {
                        if (7 <= (DateTime.Now - i.CreatedTime).Days)
                        {
                            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                            {
                                Port = 587,
                                Credentials = new NetworkCredential("akreminihat@gmail.com", "lvxt fkgr coiu epms"),
                                EnableSsl = true
                            };

                            smtpClient.Send("akreminihat@gmail.com", "royalb9514@gmail.com", "subject", "Walooommm");
                            _todoService?.UpdateTodoItemNotify(i.Id, !i.Notify);
                        }
                    }
                }
            }
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken) 
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
