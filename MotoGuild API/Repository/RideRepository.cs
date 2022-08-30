﻿using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using MotoGuild_API.Repository.Interface;

namespace MotoGuild_API.Repository
{
    public class RideRepository : IRideRepository
    {
        private MotoGuildDbContext _context;

        public RideRepository(MotoGuildDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Ride> GetAll()
        {
            return _context.Rides
                .Include(g => g.Owner)
                .Include(g => g.Participants)
                .Include(g => g.Posts)
                .ThenInclude(p => p.Author)
                .ToList();
        }
        
        public Ride Get(int id)
        {
            return _context.Rides.Find(id);
        }

        public void Insert(Ride ride)
        {
            var ownerFull = _context.Users.FirstOrDefault(u => u.Id == ride.Owner.Id);
            ride.Owner = ownerFull;
            var routeFull = _context.Routes.FirstOrDefault(r => r.Id == ride.Route.Id);
            ride.Route = routeFull; 
            _context.Rides.Add(ride);
            ride.Participants.Add(ride.Owner);
        }
        
        public void Delete(int rideId)
        {
            Ride ride = _context.Rides
                .Include(g => g.Posts)
                .FirstOrDefault(g => g.Id == rideId);
            _context.Rides.Remove(ride);
        }
        
        public void Update(Ride ride)
        {
            _context.Entry(ride).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}