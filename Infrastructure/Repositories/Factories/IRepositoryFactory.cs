using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.EntityContext;

namespace DotLogix.Architecture.Infrastructure.Repositories.Factories
{
    public interface IRepositoryFactory {
        IRepository Create(IEntityContext entityContext);
    }
}
