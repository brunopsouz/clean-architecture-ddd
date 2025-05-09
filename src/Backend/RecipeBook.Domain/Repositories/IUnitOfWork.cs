using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Domain.Repositories
{
    /// <summary>
    /// Interface da classe UnitOfWork de boas praticas para deixar o SaveChanges separado.
    /// </summary>
    public interface IUnitOfWork
    {
        // Interface nao precisa de metodo async. 
        public Task Commit();
    }
}
