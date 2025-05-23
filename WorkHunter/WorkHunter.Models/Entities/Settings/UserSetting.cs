﻿using WorkHunter.Models.Entities.Users;

namespace WorkHunter.Models.Entities.Settings;

public sealed class UserSetting : BaseSetting
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public User? User { get; set; }
}
