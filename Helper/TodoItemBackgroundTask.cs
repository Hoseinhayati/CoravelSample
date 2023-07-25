using Coravel.Invocable;
using CoravelProject.Data;
using CoravelProject.Models;

namespace CoravelProject.Helper
{
    public class TodoItemBackgroundTask : IInvocable
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoItemBackgroundTask(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public async Task Invoke()
        {
            var newItem = new TodoItem
            {
                TaskName = "Sample Task",
                IsCompleted = false,
                //CreatedAt = DateTime.Now
            };

            _dbContext.TodoItems.Add(newItem);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
