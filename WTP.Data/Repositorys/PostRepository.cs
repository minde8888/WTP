using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Data.Interfaces;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
   public  class PostRepository: IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddItem(Post post)
        {
            await _context.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(Guid Id)
        {
            var post = await _context.Posts.FindAsync(Id);       

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetItemIdAsync(Guid Id)
        {
            return await _context.Posts.Where(x => x.PostId == Id).ToListAsync(); 
        }

        public async Task<List<Post>> GetItemAsync(string ImageSrc)
        {
            var items = await _context.Posts.ToListAsync();
            foreach (var item in items)
            {
                //item.ImageSrc = String.Format("{0}/Images/{1}", ImageSrc, item.ImageName);
            }
            return items;
        }

        public async Task UpdateItem(Guid Id, Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var postUpdate = await GetItemIdAsync(Id);
        }
        public async Task<IEnumerable<Post>> Search(string name)
        {
            IQueryable<Post> query = _context.Posts;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Title.Contains(name));
            }
            return await query.ToListAsync();
        }
        public void DeleteImage(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}
