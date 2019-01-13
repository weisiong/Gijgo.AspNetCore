using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gijgo.Asp.NetCore.Data;
using Gijgo.AspNetCore.Binders;
using Gijgo.AspNetCore.Models.DTO;

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
        
        public JsonResult GetPlayers([ModelBinder(typeof(GetPlayersDtoBinder))] GetPlayersDto vm)
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
            
            if (!string.IsNullOrWhiteSpace(vm.name))
            {
                query = query.Where(q => q.Name.Contains(vm.name));
            }

            if (!string.IsNullOrWhiteSpace(vm.nationality))
            {
                query = query.Where(q => q.CountryName != null && q.CountryName.Contains(vm.nationality));
            }

            if (!string.IsNullOrWhiteSpace(vm.placeOfBirth))
            {
                query = query.Where(q => q.PlaceOfBirth != null && q.PlaceOfBirth.Contains(vm.placeOfBirth));
            }

            if (!string.IsNullOrEmpty(vm.sortBy) && !string.IsNullOrEmpty(vm.direction))
            {
                if (vm.direction.Trim().ToLower() == "asc")
                {
                    switch (vm.sortBy.Trim().ToLower())
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
                    switch (vm.sortBy.Trim().ToLower())
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
            if (vm.page.HasValue && vm.limit.HasValue)
            {
                int start = (vm.page.Value - 1) * vm.limit.Value;
                records = query.Skip(start).Take(vm.limit.Value).ToList();
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
            Asp.NET.Examples.Models.Entities.Player entity;

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
                _context.Players.Add(new Asp.NET.Examples.Models.Entities.Player
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

        public IActionResult Example2()
        {          
            return View();
        }
 
        public IActionResult Example3()
        {
            return View();
        }
        
        public JsonResult GetTeams([ModelBinder(typeof(GetTeamsDtoBinder))] GetTeamsDto vm)
        {
            List<Asp.NetCore.Models.DTO.PlayerTeam> records;
            
            var query = _context.PlayerTeams.Where(pt => pt.PlayerID == vm.playerId).Select(pt => new Asp.NetCore.Models.DTO.PlayerTeam
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
            if (vm.page.HasValue && vm.limit.HasValue)
            {
                int start = (vm.page.Value - 1) * vm.limit.Value;
                records = query.OrderBy(pt => pt.FromYear).Skip(start).Take(vm.limit.Value).ToList();
            }
            else
            {
                records = query.ToList();
            }
            return new JsonResult(new { records, total });
        }
 
        public IActionResult Example4()
        {
            return View();
        }

        public IActionResult Example5()
        {
            return View();
        }

        public JsonResult GetPlayersGrouping([ModelBinder(typeof(GetPlayersGroupingDtoBinder))] GetPlayersGroupingDto vm) 
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

                if (vm.groupBy == "countryName")
                {
                    if (vm.groupByDirection.Trim().ToLower() == "asc")
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
                if (vm.page.HasValue && vm.limit.HasValue)
                {
                    int start = (vm.page.Value - 1) * vm.limit.Value;
                    records = query.Skip(start).Take(vm.limit.Value).ToList();
                }
                else
                {
                    records = query.ToList();
                }

            return new JsonResult(new { records, total });           
        }
        
        public JsonResult GetPlayersGroupByCountry()
        {
            var list = new List<LocationPlayers>();     
            var countries = _context.Locations.ToList();
            countries.ForEach(c => {
                list.Add(new LocationPlayers
                {
                    id = c.ID,
                    @checked = false,
                    text = c.Name,
                    hasChildren = HasPlayer(c.ID),
                    type = "country"
                });
            });

            return new JsonResult(list);
        }

        public JsonResult GetPlayersBy(int CountryId)
        {
            var query = _context.Players.Where(c => c.CountryID.Equals(CountryId))
                .Select(p => new
                {
                    id = $"{CountryId}-{p.ID}",
                    playerId = p.ID,
                    text = p.Name,
                    p.PlaceOfBirth,
                    p.DateOfBirth,
                    p.CountryID,
                    CountryName = p.Country.Name,
                    p.OrderNumber,
                    type = "player"
                });
            return new JsonResult(query.ToList());
        }

        private bool HasPlayer(int CountryId)
        {
            return _context.Players.Any(c => c.CountryID.Equals(CountryId));
        }
      
        public IActionResult PlayerDetail(int PlayerId)
        {
            var vm = _context.Players.Where(p => p.ID.Equals(PlayerId))
                .Select(p => new Asp.NetCore.Models.DTO.Player
                {
                    ID = p.ID,
                    Name = p.Name,
                    PlaceOfBirth = p.PlaceOfBirth,
                    DateOfBirth = p.DateOfBirth,
                    CountryID = p.CountryID,
                    CountryName = p.Country != null ? p.Country.Name : "",
                    IsActive = p.IsActive,
                    OrderNumber = p.OrderNumber
                }).FirstOrDefault();

            return PartialView(vm);
        }

        public IActionResult CountryDetail(int CountryId)
        {
            var vm = _context.Locations.Where(p => p.ID.Equals(CountryId))
             .Select(p => new Asp.NET.Examples.Models.Entities.Location
             {
                 ID = p.ID,
                 Name = p.Name,
                 Checked = p.Checked,
                 FlagUrl = p.FlagUrl,
                 Population = p.Population,
                 OrderNumber = p.OrderNumber
             }).FirstOrDefault();

            return PartialView(vm);
        }
    }
}
