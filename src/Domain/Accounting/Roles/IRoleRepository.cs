using AuroraScienceHub.Framework.Entities.Storage;

namespace TelegramBot.Domain.Accounting.Roles;

public interface IRoleRepository : IRepository<Role, RoleId>
{
    Task<Role?> GetByName(string name, CancellationToken cancellationToken = default);
}
