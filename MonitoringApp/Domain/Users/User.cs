﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Users;

public abstract class User : Entity<int> {
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public UserRole UserRole { get; set; }
    
    public User() : base(default) { }

    protected User(string username, string name, string password, UserRole userRole) 
        : this(default, username, name, password, userRole) { }

    protected User(int id, string username, string name, string password, UserRole userRole) : base(id) {
        Username = username;
        Name = name;
        Password = password;
        UserRole = userRole;
    }

    public override string ToString() {
        return $"User: {Name} ({Username})";
    }
}