﻿using System;

namespace Api.Models
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId{ get; set; }
    }
}
