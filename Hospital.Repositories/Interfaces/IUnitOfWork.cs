﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
       
        IRepository<T> Repository<T>() where T:class;
        void Save();
    }
}
