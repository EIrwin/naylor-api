using System;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Naylor.Data
{
    public class UserRepository : IRepository<User>
    {
        private MongoCollection<User> _collection;

        public UserRepository(string connectionString)
        {
            InitializeConnection(connectionString);
            RegisterConventions();
        }

        private void InitializeConnection(string connectionString)
        {
            var database = MongoDatabase.Create(connectionString);
            _collection = database.GetCollection<User>("users");
            _collection.CreateIndex(IndexKeys<User>.Hashed(index => index.Id));
        }

        private void RegisterConventions()
        {

            if (!BsonClassMap<User>.IsClassMapRegistered(typeof(User)))
            {
                BsonClassMap.RegisterClassMap<User>();
            }

        }
        public IQueryable AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public User Save(User entity)
        {
            _collection.Save(entity);
            return entity;
        }

        public void Delete(User entity)
        {
            _collection.Remove(Query<User>.EQ(p => p.Id, entity.Id));
        }

        public void Delete(Expression<Func<User, bool>> query)
        {
            var items = Query<User>.Where(query);
            var sort = SortBy<User>.Ascending(p => p.Id);
            _collection.FindAndRemove(items, sort);
        }

        IQueryable<User> IRepository<User>.AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public object Save(object entity)
        {
            return Save((User)entity);
        }

        public void Delete(object entity)
        {
            Delete((User)entity);
        }
    }
}