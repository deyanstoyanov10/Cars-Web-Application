using CarsWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace TestQueries
{
    public class Program
    {
        public static void Main()
        {
            var stopWatch = Stopwatch.StartNew();

            //using (var db = new CarsDbContext())
            //{
            //    var models = db.Brands.Where(x => x.Id == 5)
            //        .Select(m => new
            //        {
            //            Models = m.Models.Where(x => x.BrandId == 5).ToList()
            //        }).ToList();
            //}

            //Console.WriteLine(stopWatch.Elapsed);

            //stopWatch = Stopwatch.StartNew();
            //using (var db = new CarsDbContext())
            //{
            //    var models = db.Models.Where(x => x.BrandId == 5).Select(x => new { Model = x.ModelType, ModelId = x.Id }).ToList();
            //}

            Console.WriteLine(stopWatch.Elapsed);
        }
    }
}
