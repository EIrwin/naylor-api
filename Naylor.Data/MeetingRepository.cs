using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Naylor.Data
{
    public class MeetingRepository : IRepository<Meeting>
    {
        private MongoCollection<Meeting> _collection;

        public MeetingRepository(string connectionString)
        {
            InitializeConnection(connectionString);
            RegisterConventions();
        }

        private void InitializeConnection(string connectionString)
        {
            var database = MongoDatabase.Create(connectionString);
            _collection = database.GetCollection<Meeting>("meetings");
            _collection.CreateIndex(IndexKeys<Meeting>.Hashed(index => index.Id));
        }

        private void RegisterConventions()
        {

            if (!BsonClassMap.IsClassMapRegistered(typeof(Meeting)))
            {
                BsonClassMap.RegisterClassMap<Meeting>();
            }

        }
        public IQueryable AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public Meeting Save(Meeting entity)
        {
            _collection.Save(entity);
            return entity;
        }

        public void Delete(Meeting entity)
        {
            _collection.Remove(Query<Meeting>.EQ(p => p.Id, entity.Id));
        }

        public void Delete(Expression<Func<Meeting, bool>> query)
        {
            var items = Query<Meeting>.Where(query);
            var sort = SortBy<Meeting>.Ascending(p => p.Id);
            _collection.FindAndRemove(items, sort);
        }

        IQueryable<Meeting> IRepository<Meeting>.AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public object Save(object entity)
        {
            return Save((Meeting)entity);
        }

        public void Delete(object entity)
        {
            Delete((Meeting)entity);
        }
    }
}
