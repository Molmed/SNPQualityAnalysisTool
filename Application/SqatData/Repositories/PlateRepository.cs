using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SqatData.Entities;

namespace SqatData.Repositories
{
    public class PlateRepository
    {
        private readonly SqatContext _context;

        public PlateRepository(string dbName)
        {
            _context = new SqatContext(dbName);
        }

        public List<Plate> GetPlatesBySession(string sessionName)
        {
            var sql = $"exec pSQAT_GetResultPlatesForSession '{sessionName}'";
            var plates = _context.Plates.FromSql(sql);
            return plates.ToList();
        }
    }
}
