using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DotnetEFRazorCompSSR.App.Models;

namespace DotnetEFRazorCompSSR.App.Services
{
    public class WebsiteDataService
    {
        private readonly WebsitesContext _context;

        public WebsiteDataService(WebsitesContext context)
        {
            _context = context;
        }

        public async Task<List<Websites>> GetWebsitesAsync(string searchDate = null, int? topNumber = null)
        {
            DateTime dateValue;
            List<Websites> websites = new List<Websites>();
            try
            {
                return await Task.Run<List<Websites>>(() =>
                {
                    string getFinalDate = (searchDate is null
                                            ? _context.WebsiteDetails.DefaultIfEmpty().Max(item => item.VisitDate).ToShortDateString() // get max searchDate from DB
                                            : searchDate
                                          );

                    if (DateTime.TryParse(getFinalDate.ToString(), out dateValue))
                    {
                        var lstWebsites = _context.WebsiteDetails
                                                .Where(s => s.VisitDate.Date == dateValue.Date)
                                                .GroupBy(g => g.WebsiteId)
                                                .Select(a => new { WebsiteId = a.Key, TotalVisits = a.Sum(x => x.TotalVisits) })
                                                .OrderByDescending(o => o.TotalVisits)
                                                .Take(topNumber ?? 5)
                                                .Join(_context.Websites
                                                    , left => left.WebsiteId
                                                    , right => right.WebsiteId
                                                    , (left, right) => new Websites
                                                    {
                                                        WebsiteId = left.WebsiteId,
                                                        Url = right.Url,
                                                        TotalVisits = left.TotalVisits,
                                                        VisitDate = dateValue.Date
                                                    })
                                                //.Select(CreateNewStatement<Websites>(columns ?? "WebsiteId,Url,TotalVisits,VisitDate")) // nullify values for not required columns
                                                .AsQueryable();

                        return lstWebsites.ToList<Websites>();
                    }
                    else
                    {
                        return websites.ToList<Websites>();
                    }
                });
            }
            catch (Exception ex)
            {
                return websites.ToList<Websites>();
            }
        }

        public Task<MinMaxDate> GetMinMaxDateAsync()
        {
            return Task.Run<MinMaxDate>(() =>
            {
                var visitDates = _context.WebsiteDetails.Select(s => s.VisitDate).Distinct().OrderBy(o => o);
                return new MinMaxDate { MinDate = visitDates.Take(1).FirstOrDefault().Date, MaxDate = visitDates.OrderByDescending(o => o).Take(1).FirstOrDefault().Date };
            });
        }

        private Func<T, T> CreateNewStatement<T>(string fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(T), "o");

            // new statement "new T()"
            var xNew = Expression.New(typeof(T));

            // create initializers
            var bindings = fields.Split(',').Select(o => o.Trim())
                .Select(o => {

                    // property "Field1"
                    var mi = typeof(T).GetProperty(o);

                    // original value "o.Field1"
                    var xOriginal = Expression.Property(xParameter, mi);

                    // set value "Field1 = o.Field1"
                    return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new T { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new T { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);

            // compile to Func<T, T>
            return lambda.Compile();
        }

    }

    public class Websites
    {
        public int WebsiteId { get; set; }
        public string Url { get; set; }
        public int? TotalVisits { get; set; }
        public DateTime? VisitDate { get; set; }
    }

    public class MinMaxDate
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
    }

}
