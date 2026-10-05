// GolBet.Web/Controllers/MatchesController.cs  (versión completa)
using GolBet.Entities.Enums;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GolBet.Web.Controllers;

public class MatchesController : Controller
{
    private readonly IMatchService _matchService;
    private readonly ITeamService _teamService;

    public MatchesController(IMatchService matchService, ITeamService teamService)
    {
        _matchService = matchService;
        _teamService = teamService;
    }

    // GET /Matches            -> all matches
    // GET /Matches?status=Scheduled -> filtered board
    public async Task<IActionResult> Index(MatchStatus? status)
    {
        ViewBag.CurrentStatus = status;
        var board = await _matchService.GetBoardAsync(status);
        return View(board);
    }

    // GET /Matches/Detail/3
    public async Task<IActionResult> Detail(int id)
    {
        var match = await _matchService.GetDetailAsync(id);
        if (match is null) return NotFound();   // HTTP 404

        return View(match);
    }
    // GET /Matches/Create
    public async Task<IActionResult> Create()
    {
        await LoadTeamsAsync();
        return View(new MatchFormDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MatchFormDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadTeamsAsync();
            return View(dto);
        }

        try
        {
            await _matchService.CreateAsync(dto);
            TempData["Success"] = "Partido creado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)      // business rule violated
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await LoadTeamsAsync();
            return View(dto);
        }
    }

    // GET /Matches/Edit/5  y  POST /Matches/Edit  siguen el mismo molde
    // (con GetForEditAsync y UpdateAsync);
    // POST /Matches/Deactivate/5 es idéntico al de Teams.

    private async Task LoadTeamsAsync()
    {
        var teams = await _teamService.GetAllAsync();
        ViewBag.Teams = new SelectList(teams, "Id", "Name");
    }

}


