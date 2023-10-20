using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using TeamUserManagementSystem.Data;
using TeamUserManagementSystem.Models;

namespace TeamUserManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(Context context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            try
            {
                var context = _context.Users.Include(u => u.UserTeams).ThenInclude(ut => ut.Team);
                return View(await context.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
                return StatusCode(500, "An error occurred while fetching users.");
            }
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null || _context.Users == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                   .Include(u => u.UserTeams)
                   .ThenInclude(ut => ut.Team)
                   .FirstOrDefaultAsync(m => m.UserId == id);

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user details.");
                return StatusCode(500, "An error occurred while fetching user details.");
            }
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var teamList = _context.Teams.ToList(); 
            ViewBag.TeamList = teamList; 

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,UserEmail,TeamId")] User user, int[] selectedTeams)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (selectedTeams != null)
                    {
                        user.UserTeams = selectedTeams.Select(teamId => new UserTeam { TeamId = teamId }).ToList();

                    }
                    else
                    {
                        _logger.LogInformation("No teams selected for the user.");
                    }

                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User created successfully.");
                    return RedirectToAction(nameof(Index));
                }

                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateEntry = ModelState[modelStateKey];
                    foreach (var error in modelStateEntry.Errors)
                    {
                        _logger.LogError($"Validation error in field {modelStateKey}: {error.ErrorMessage}");
                    }
                }

                _logger.LogWarning("Validation errors occurred while creating a user.");
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }



        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || _context.Users == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .Include(u => u.UserTeams) 
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    return NotFound();
                }

                var selectedTeams = user?.UserTeams?.Select(ut => ut.TeamId)?.ToArray() ?? new int[0];

                ViewBag.SelectedTeams = selectedTeams;

                var teamList = _context.Teams.ToList();
                ViewBag.TeamList = teamList;

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing a user.");
                return StatusCode(500, "An error occurred while editing a user.");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,UserEmail,UserTeams")] User user, int[] selectedTeams)
        {
            try
            {
                if (id != user.UserId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var existingUser = await _context.Users
                            .Include(u => u.UserTeams)
                            .FirstOrDefaultAsync(u => u.UserId == user.UserId);

                        if (selectedTeams != null)
                        {
                            var existingUserTeamIds = existingUser.UserTeams.Select(ut => ut.TeamId).ToList();
                            var teamsToAdd = selectedTeams.Except(existingUserTeamIds).ToList();
                            var teamsToRemove = existingUserTeamIds.Except(selectedTeams).ToList();

                            foreach (var teamId in teamsToRemove)
                            {
                                var teamToRemove = existingUser.UserTeams.FirstOrDefault(ut => ut.TeamId == teamId);
                                if (teamToRemove != null)
                                    existingUser.UserTeams.Remove(teamToRemove);
                            }

                            foreach (var teamId in teamsToAdd)
                            {
                                existingUser.UserTeams.Add(new UserTeam { UserId = user.UserId, TeamId = teamId });
                            }
                        }
                        else
                        {
                            existingUser.UserTeams.Clear();
                        }

                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.UserId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }

                var teamList = _context.Teams.ToList();
                ViewBag.TeamList = teamList;

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the user.");
                return StatusCode(500, "An error occurred while editing the user.");
            }
        }



        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || _context.Users == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .Include(u => u.UserTeams)
                    .ThenInclude(ut => ut.Team)
                    .FirstOrDefaultAsync(m => m.UserId == id);

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user details for deletion.");
                return StatusCode(500, "An error occurred while fetching user details for deletion.");
            }
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.Users == null)
                {
                    _logger.LogError("Entity set 'Context.Users' is null.");
                    return StatusCode(500, "An error occurred.");
                }
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound("User not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user.");
                return BadRequest("An error occurred while deleting the user.");
            }
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
