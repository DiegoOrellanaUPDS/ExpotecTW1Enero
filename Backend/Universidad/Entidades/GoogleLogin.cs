using System;

namespace Entidades
{
    public class GoogleLogin
    {
        public int Id { get; set; }
        public string GoogleId { get; set; }   // sub del usuario
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string AccessToken { get; set; }
        public DateTime LoginDate { get; set; }
    }
}