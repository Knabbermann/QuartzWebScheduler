using HomePost8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomePost8.DataAccess.Repository.IRepository
{
    public interface IMailAccountRepository : IRepository<MailAccount>
    {
        void Update(MailAccount mailAccount);
    }
}
