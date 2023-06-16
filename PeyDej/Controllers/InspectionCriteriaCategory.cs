using Ccms.Common.Utilities;

using Dapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Dtos;
using PeyDej.Models.Parameters;
using PeyDej.Servive;
using PeyDej.Tools;

using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using static Dapper.SqlMapper;

namespace PeyDej.Controllers;

[Authorize(Roles = "Admin")]
public class InspectionCriteriaCategory : Controller
{
    private PeyDejContext _context;
    private IConfiguration Configuration;

    public InspectionCriteriaCategory(PeyDejContext context, IConfiguration configuration)
    {
        _context = context;
        Configuration = configuration;
    }

    public IActionResult Index()
    {
        IEnumerable<InspectionCriteriaSubCategoryIS> data = new List<InspectionCriteriaSubCategoryIS>();

        ViewBag.startDate = PeyDejTools.GetCurPersianDate();
        ViewBag.endDate = PeyDejTools.GetCurPersianDate();
        return View(data);
    }


    [HttpPost]
    public async Task<IActionResult> Index(string start_date, string end_date)
    {
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        using IDbConnection db = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));

        db.Open();
        var q = @$"SELECT 
	                	InspectionCriteriaSubCategoryIS.Id
	                    ,c.Caption
	                    ,c.InspectionCriteriaCategoryId
                        ,InspectionDate
                FROM 
                	Inspection.InspectionCriteriaSubCategoryIS
                	INNER JOIN Base.InspectionCriteriaSubCategory c ON c.Id = InspectionCriteriaSubCategoryIS.InspectionCriteriaSubCategoryId
                WHERE
                	InspectionCriteriaSubCategoryIS.InspectionDate >= '{(start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200)}'
                    AND InspectionCriteriaSubCategoryIS.InspectionDate <= '{(end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200)}'
                    AND InspectionCriteriaSubCategoryIS.InspectionFinishedDate IS NULL
                	AND [Status] = 0
                	AND c.InspectionCriteriaCategoryId IN (1 ,2 ,3 ,4)";
        var gridReader = await db.QueryAsync<InspectionCriteriaSubCategoryIS>(q);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        db.Close();
        return View(gridReader);
    }

    public async Task<IActionResult> PrintPage()
    {

        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        using IDbConnection db = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));

        db.Open();
        var q = @$"SELECT 
	                	InspectionCriteriaSubCategoryIS.Id
	                    ,c.Caption
	                    ,c.InspectionCriteriaCategoryId
                FROM 
                	Inspection.InspectionCriteriaSubCategoryIS
                	INNER JOIN Base.InspectionCriteriaSubCategory c ON c.Id = InspectionCriteriaSubCategoryIS.InspectionCriteriaSubCategoryId
                WHERE
                	InspectionCriteriaSubCategoryIS.InspectionDate >= '{(start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200)}'
                    AND InspectionCriteriaSubCategoryIS.InspectionDate <= '{(end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200)}'
                    AND InspectionCriteriaSubCategoryIS.InspectionFinishedDate IS NULL
                	AND [Status] = 0
                	AND c.InspectionCriteriaCategoryId IN (1 ,2 ,3 ,4)";
        var criteriaSubCategoryIses = await db.QueryAsync<InspectionCriteriaSubCategoryIS>(q);
        var list = criteriaSubCategoryIses.ToList();
        for (int g = 0; g < list.Count; g++)
        {
            list[g].SubCategories = (List<SubCategory>)await db.QueryAsync<SubCategory>(@$"SELECT * FROM Base.SubCategory WHERE CategoryId = {list[g].InspectionCriteriaCategoryId}");
        }

        return View(list);
    }

    public async Task<IActionResult> GetDataReport()
    {
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        using IDbConnection db = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));

        db.Open();
        var q = @$"SELECT 
	                	InspectionCriteriaSubCategoryIS.Id
	                    ,c.Caption
	                    ,cc.CriteriaCategoryId
	                    ,c.InspectionCriteriaCategoryId
                FROM 
                	Inspection.InspectionCriteriaSubCategoryIS
                	INNER JOIN Base.InspectionCriteriaSubCategory c ON c.Id = InspectionCriteriaSubCategoryIS.InspectionCriteriaSubCategoryId
	                INNER JOIN Base.InspectionCriteriaCategory cc ON cc.Id = c.InspectionCriteriaCategoryId
                WHERE
                	InspectionCriteriaSubCategoryIS.InspectionDate >= '{(start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200)}'
                    AND InspectionCriteriaSubCategoryIS.InspectionDate <= '{(end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200)}'
                    AND InspectionCriteriaSubCategoryIS.InspectionFinishedDate IS NULL
                	AND [Status] = 0
                	AND c.InspectionCriteriaCategoryId IN (1 ,2 ,3 ,4)";
        var criteriaSubCategoryIses = await db.QueryAsync<InspectionCriteriaSubCategoryIS>(q);
        var list = criteriaSubCategoryIses.ToList();
        for (int g = 0; g < list.Count; g++)
        {
            list[g].SubCategories = (List<SubCategory>)await db.QueryAsync<SubCategory>(@$"SELECT * FROM Base.SubCategory WHERE CategoryId = {list[g].CriteriaCategoryId}");
        }

        List<long[]> listsave = new List<long[]>();
        foreach (var item in list)
        {
            for (int i = 0; i < item.SubCategories.Count; i++)
            {
                listsave.Add(new long[] { item.Id, item.SubCategories[i].Id });
            }
        }

        HttpContext.Session.SetString("listRaioName", JsonSerializer.Serialize(listsave));
        return View(new GetDataReportViewDto()
        {
            Data = list,
            listRaioName = listsave
        });
    }

    public async Task<IActionResult> SaveDataReport(int person, ReportMotorStatusParameter[] data)
    {
        try
        {
            var listRaioName = HttpContext.Session.GetString("listRaioName");
            List<long[]> listsave = JsonSerializer.Deserialize<List<long[]>>(listRaioName);
            List<InspectionCriteriaSubCategoryDto> list = new List<InspectionCriteriaSubCategoryDto>();

            foreach (var item in data)
            {
                list.Add(new InspectionCriteriaSubCategoryDto { InspectionId = listsave[item.Id][0], CriteriaId = listsave[item.Id][1], PersonId = person, Status = item.status ? 1 : 0, Description = "" });
            }
            var result = Convertor.ConvertToDataTable(list.AsEnumerable());
            using IDbConnection db = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));

            db.Open();
            await db.ExecuteAsync("Inspection.InspectionCriteriaSubCategorySetInspection",
                new { InspectionCriteriaSubCategory = result.AsTableValuedParameter("Inspection.InspectionCriteriaSubCategoryTT") }, commandType: CommandType.StoredProcedure);
            db.Close();
            return Json(new { r = true });
        }
        catch (Exception ex)
        {
            return Json(new { r = false, m = ex.Message });
        }
    }
}