﻿using WorkHunter.Models.Dto.Users;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Views.Users;

namespace Abstractions.Users;

public interface IUserService
{
    Task<UserBaseView> GetCurrent();

    Task<TokensView> Login(LoginDto dto);

    Task<UserView> GetById(string userId);

    Task<User?> GetByName(string userName);

    Task<HashSet<User>> GetInRoles(params string[] roles);

    Task<IReadOnlyList<UserBaseView>> GetAll();

    Task<UserView> Create(UserCreateDto dto);

    Task<UserView> Edit(UserEditDto dto);

    Task<User> GetByToken(string? accessToken);
}
