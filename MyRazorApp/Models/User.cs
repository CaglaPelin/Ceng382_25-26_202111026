/*
GPT Prompt:
Razor Pages projemde kullanıcı giriş bilgilerini bir JSON dosyasında saklamak istiyorum. 
Bu dosya içeriğini karşılamak üzere bir `User` sınıfı tanımlamam gerekiyor. 
Sınıf aşağıdaki özelliklere sahip olmalı: Username, Password, Role, IsActive, CreatedAt. 
Bu sınıfı nullable uyarılarını dikkate alarak nasıl yazmalıyım?
*/


using System;

namespace MyRazorApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
