﻿using ChatRoom.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Data
{
    public interface IUserPersister : IRepository<User>
    {
        IEnumerable<User> GetAll();
    }
}
