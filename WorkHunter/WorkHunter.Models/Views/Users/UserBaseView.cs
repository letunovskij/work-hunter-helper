namespace WorkHunter.Models.Views.Users;

public class UserBaseView
{
    public required string Id { get; set; }

    /// <summary>
    /// Логин
    /// </summary>
    public required string UserName { get; set; }

    public required string Email { get; set; }

    /// <summary>
    /// ФИО пользователя
    /// </summary>
    public required string Name { get; set; }

    public bool IsDeleted { get; set; }
}
