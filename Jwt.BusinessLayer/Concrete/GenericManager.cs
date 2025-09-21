using Jwt.BusinessLayer.Abstract;
using Jwt.DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.BusinessLayer.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        private readonly IGenericDal<T> _dal;

        public GenericManager(IGenericDal<T> dal)
        {
            _dal = dal;
        }

        public void Insert(T entity)
        {
            _dal.Insert(entity);
        }

        public void Update(T entity)
        {
            _dal.Update(entity);
        }

        public void Delete(T entity)
        {
            _dal.Delete(entity);
        }

        public T GetById(int id)
        {
            return _dal.GetById(id);
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return _dal.GetAll(filter);
        }
    }
}
