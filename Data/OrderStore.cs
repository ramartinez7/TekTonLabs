using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class OrderStore : EntityStore<Order>, IOrderStore
    {
        public OrderStore(DatabaseContext context) : base(context)
        {

        }

        public override Task<Order> GetByIdAsync(object id)
        {
            var query = this.EntitySet()
                .Where(o => o.Id == (int)id)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product);

            return query.FirstOrDefaultAsync();
        }
    }
}
