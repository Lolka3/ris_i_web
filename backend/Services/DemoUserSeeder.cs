using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public sealed class DemoUserSeeder(
    AppDbContext dbContext,
    IPasswordHasher<User> passwordHasher,
    IConfiguration configuration)
{
    private static readonly (string Login, string FullName, string Role, string PasswordKey)[] Accounts =
    [
        ("student.demo", "Иванов Иван Иванович", "Student", "StudentPassword"),
        ("teacher.demo", "Петров Петр Петрович", "Teacher", "TeacherPassword"),
        ("teacher.other", "Сидоров Алексей Алексеевич", "Teacher", "OtherTeacherPassword"),
        ("head.demo", "Смирнова Анна Сергеевна", "HeadOfDepartment", "HeadPassword")
    ];

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        foreach (var (login, fullName, role, passwordKey) in Accounts)
        {
            var password = configuration[$"Auth:DemoUsers:{passwordKey}"];
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    $"Configure Auth:DemoUsers:{passwordKey} with a local user-secret before starting in Development.");
            }

            var user = await dbContext.Users
                .SingleOrDefaultAsync(item => item.Login == login, cancellationToken);
            if (user is null)
            {
                user = new User
                {
                    Login = login,
                    FullName = fullName,
                    Role = role,
                    PasswordHash = string.Empty
                };
                dbContext.Users.Add(user);
            }

            user.FullName = fullName;
            user.Role = role;
            user.PasswordHash = passwordHasher.HashPassword(user, password);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
