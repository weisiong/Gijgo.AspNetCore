using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gijgo.Asp.NET.Examples.Models.Entities;
using Gijgo.Asp.NetCore.Data;

namespace Gijgo.AspNetCore.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(Id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Example1));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }
        
        public JsonResult GetPlayers(int? page, int? limit, string sortBy, string direction, string name, string nationality, string placeOfBirth)
        {
            List<Asp.NetCore.Models.DTO.Player> records;
            int total;

            var query = _context.Players.Select(p => new Asp.NetCore.Models.DTO.Player
            {
                ID = p.ID,
                Name = p.Name,
                PlaceOfBirth = p.PlaceOfBirth,
                DateOfBirth = p.DateOfBirth,
                CountryID = p.CountryID,
                CountryName = p.Country != null ? p.Country.Name : "",
                IsActive = p.IsActive,
                OrderNumber = p.OrderNumber
            });

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(q => q.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(nationality))
            {
                query = query.Where(q => q.CountryName != null && q.CountryName.Contains(nationality));
            }

            if (!string.IsNullOrWhiteSpace(placeOfBirth))
            {
                query = query.Where(q => q.PlaceOfBirth != null && q.PlaceOfBirth.Contains(placeOfBirth));
            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    switch (sortBy.Trim().ToLower())
                    {
                        case "name":
                            query = query.OrderBy(q => q.Name);
                            break;
                        case "countryname":
                            query = query.OrderBy(q => q.CountryName);
                            break;
                        case "placeOfBirth":
                            query = query.OrderBy(q => q.PlaceOfBirth);
                            break;
                        case "dateofbirth":
                            query = query.OrderBy(q => q.DateOfBirth);
                            break;
                    }
                }
                else
                {
                    switch (sortBy.Trim().ToLower())
                    {
                        case "name":
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case "countryname":
                            query = query.OrderByDescending(q => q.CountryName);
                            break;
                        case "placeOfBirth":
                            query = query.OrderByDescending(q => q.PlaceOfBirth);
                            break;
                        case "dateofbirth":
                            query = query.OrderByDescending(q => q.DateOfBirth);
                            break;
                    }
                }
            }
            else
            {
                query = query.OrderBy(q => q.OrderNumber);
            }

            total = query.Count();
            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = query.Skip(start).Take(limit.Value).ToList();
            }
            else
            {
                records = query.ToList();
            }

            return new JsonResult(new { records, total });
        }
        
        [HttpPost]
        public JsonResult Save(Asp.NetCore.Models.DTO.Player record)
        {
            Player entity;

            if (record.ID > 0)
            {
                entity = _context.Players.First(p => p.ID == record.ID);
                entity.Name = record.Name;
                entity.PlaceOfBirth = record.PlaceOfBirth;
                entity.DateOfBirth = record.DateOfBirth;
                entity.CountryID = record.CountryID;
                entity.IsActive = record.IsActive;
            }
            else
            {
                _context.Players.Add(new Player
                {
                    Name = record.Name,
                    PlaceOfBirth = record.PlaceOfBirth,
                    DateOfBirth = record.DateOfBirth,
                    CountryID = record.CountryID,
                    IsActive = record.IsActive
                });
            }
            _context.SaveChanges();

            return new JsonResult(new { result = true });
        }

        public IActionResult Example1()
        {
            return View();
        }

        #region "bootstrap-grid-inline-edit"
        // GET: Players
        public IActionResult Example2()
        {          
            return View();
        }
        #endregion

        #region "nested-jquery-grids"
        public IActionResult Example3()
        {
            return View();
        }
        
        public JsonResult GetTeams(int playerId, int? page, int? limit)
        {
            List<Asp.NetCore.Models.DTO.PlayerTeam> records;
            
            var query = _context.PlayerTeams.Where(pt => pt.PlayerID == playerId).Select(pt => new Asp.NetCore.Models.DTO.PlayerTeam
            {
                ID = pt.ID,
                PlayerID = pt.PlayerID,
                FromYear = pt.FromYear,
                ToYear = pt.ToYear,
                Team = pt.Team,
                Apps = pt.Apps,
                Goals = pt.Goals
            });

            int total = query.Count();
            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = query.OrderBy(pt => pt.FromYear).Skip(start).Take(limit.Value).ToList();
            }
            else
            {
                records = query.ToList();
            }
            return new JsonResult(new { records, total });
        }
        #endregion

        public IActionResult Example4()
        {
            return View();
        }

        public IActionResult Example5()
        {
            return View();
        }

        public JsonResult GetPlayersGrouping(string groupBy, string groupByDirection, int? page, int? limit)
        {
            List<Asp.NetCore.Models.DTO.Player> records;
            int total;
    
                var query = _context.Players.Select(p => new Asp.NetCore.Models.DTO.Player
                {
                    ID = p.ID,
                    Name = p.Name,
                    PlaceOfBirth = p.PlaceOfBirth,
                    DateOfBirth = p.DateOfBirth,
                    CountryID = p.CountryID,
                    CountryName = p.Country.Name,
                    OrderNumber = p.OrderNumber
                });

                if (groupBy == "countryName")
                {
                    if (groupByDirection.Trim().ToLower() == "asc")
                    {
                        query = query.OrderBy(q => q.CountryName).ThenBy(q => q.OrderNumber);
                    }
                    else
                    {
                        query = query.OrderByDescending(q => q.CountryName).ThenBy(q => q.OrderNumber);
                    }
                }
                else
                {
                    query = query.OrderBy(q => q.OrderNumber);
                }

                total = query.Count();
                if (page.HasValue && limit.HasValue)
                {
                    int start = (page.Value - 1) * limit.Value;
                    records = query.Skip(start).Take(limit.Value).ToList();
                }
                else
                {
                    records = query.ToList();
                }

            return new JsonResult(new { records, total });           
        }


    }
}
