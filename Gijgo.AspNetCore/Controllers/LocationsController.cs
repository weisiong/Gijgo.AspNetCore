using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gijgo.Asp.NET.Examples.Models.Entities;
using Gijgo.Asp.NetCore.Data;

namespace Gijgo.AspNetCore.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Example1()
        {
            return View();
        }

        public IActionResult Example2()
        {
            return View();
        }

        public IActionResult Example3()
        {
            return View();
        }



        public JsonResult GetCountries(string query)
        {
            List<Asp.NetCore.Models.DTO.Location> records;

            records = _context.Locations.Where(l => l.Parent != null && l.Parent.ParentID == null)
                .Select(l => new Asp.NetCore.Models.DTO.Location
                {
                    id = l.ID,
                    text = l.Name,
                    @checked = l.Checked,
                    population = l.Population,
                    flagUrl = l.FlagUrl
                }).ToList();

            return new JsonResult(records);
        }

        public JsonResult Get(string query)
        {
            List<Location> locations;
            List<Asp.NetCore.Models.DTO.Location> records;

                locations = _context.Locations.ToList();

                if (!string.IsNullOrWhiteSpace(query))
                {
                    locations = locations.Where(q => q.Name.Contains(query)).ToList();
                }

                records = locations.Where(l => l.ParentID == null).OrderBy(l => l.OrderNumber)
                    .Select(l => new Asp.NetCore.Models.DTO.Location
                    {
                        id = l.ID,
                        text = l.Name,
                        @checked = l.Checked,
                        population = l.Population,
                        flagUrl = l.FlagUrl,
                        children = GetChildren(locations, l.ID)
                    }).ToList();


            return new JsonResult(records);
        }

        public JsonResult LazyGet(int? parentId)
        {
            List<Location> locations;
            List<Asp.NetCore.Models.DTO.Location> records;
            
                locations = _context.Locations.ToList();

                records = locations.Where(l => l.ParentID == parentId).OrderBy(l => l.OrderNumber)
                    .Select(l => new Asp.NetCore.Models.DTO.Location
                    {
                        id = l.ID,
                        text = l.Name,
                        @checked = l.Checked,
                        population = l.Population,
                        flagUrl = l.FlagUrl,
                        hasChildren = locations.Any(l2 => l2.ParentID == l.ID)
                    }).ToList();
            

            return new JsonResult(records);
        }

        private List<Asp.NetCore.Models.DTO.Location> GetChildren(List<Location> locations, int parentId)
        {
            return locations.Where(l => l.ParentID == parentId).OrderBy(l => l.OrderNumber)
                .Select(l => new Asp.NetCore.Models.DTO.Location
                {
                    id = l.ID,
                    text = l.Name,
                    population = l.Population,
                    flagUrl = l.FlagUrl,
                    @checked = l.Checked,
                    children = GetChildren(locations, l.ID)
                }).ToList();
        }

        [HttpPost]
        public JsonResult SaveCheckedNodes(List<int> checkedIds)
        {
            if (checkedIds == null)
            {
                checkedIds = new List<int>();
            }
         
            var locations = _context.Locations.ToList();
            foreach (var location in locations)
            {
                location.Checked = checkedIds.Contains(location.ID);
            }
            _context.SaveChanges();           

            return this.Json(true);
        }

        [HttpPost]
        public JsonResult ChangeNodePosition(int id, int parentId, int orderNumber)
        { 
            var location = _context.Locations.First(l => l.ID == id);

            var newSiblingsBelow = _context.Locations.Where(l => l.ParentID == parentId && l.OrderNumber >= orderNumber);
            foreach (var sibling in newSiblingsBelow)
            {
                sibling.OrderNumber = sibling.OrderNumber + 1;
            }

            var oldSiblingsBelow = _context.Locations.Where(l => l.ParentID == location.ParentID && l.OrderNumber > location.OrderNumber);
            foreach (var sibling in oldSiblingsBelow)
            {
                sibling.OrderNumber = sibling.OrderNumber - 1;
            }

            location.ParentID = parentId;
            location.OrderNumber = orderNumber;

            _context.SaveChanges();            

            return this.Json(true);
        }

    }
}
