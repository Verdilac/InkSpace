using InkSpaceWeb.Models;

namespace InkSpace.DataAccess.Repository.IRepository;

public interface ICompanyRepository :IRepository<Company>
{
    void Update(Company company);
    
}