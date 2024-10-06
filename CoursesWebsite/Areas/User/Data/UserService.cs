using CoursesWebsite.Data;

namespace CoursesWebsite.Areas.User.Data
{
    public class UserService<T> : IServiceData<T> where T : class
    {
        private readonly CoursesWebsiteDbContext _context;

        public UserService(CoursesWebsiteDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetItems()
        {
            return _context.Set<T>().ToList();
        }

        public Task<bool> Create(T item)
        {
            if(item == null)
                return Task.FromResult(false);
            
            _context.Add(item);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public bool Delete(string id)
        {
            var model = _context.Find<T>(id);
            if (model == null)
                return false;
            _context.Remove(model);
            _context.SaveChanges();
            return true;
        }

        public Task<bool> Edit(T item)
        {
            if( item == null)
                return Task.FromResult(false);
            _context.Update(item);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public T GetItem(string id)
        {
            var item = _context.Find<T>(id);
            return item!;
        }

    }
}
