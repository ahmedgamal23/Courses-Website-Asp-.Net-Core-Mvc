using CoursesWebsite.Models;

namespace CoursesWebsite.Data
{
    public interface IServiceData<T>
    {
        public IEnumerable<T> GetItems();
        public T GetItem(string id);
        public Task<bool> Create(T item);
        public Task<bool> Edit(T item);
        public bool Delete(string id);
    }
}
