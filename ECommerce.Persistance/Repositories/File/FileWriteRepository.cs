using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Repositories;
using ECommerce.Persistance.Contexts;
using File = ECommerce.Domain.Entities.File;

namespace ECommerce.Persistance.Repositories
{
    public class FileWriteRepository : WriteRepository<File>, IFileWriteRepository
    {
        public FileWriteRepository(ETradeDbContext context) : base(context)
        {
        }
    }
}
