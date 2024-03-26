using NewsForYou.Logger;
using NewsForYou.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsForYou.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NewsForYou.DAL
{
    public class DataAccess : IDataAccess
    {
        private readonly NewsForYouContext context;

        private readonly ILogger logger;

        public DataAccess(NewsForYouContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<bool> SignUp(SignUp signup)
        {
            bool flag = false;
            try
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        User newStudent = new User
                        {
                            Name = signup.Name,
                            Email = signup.Email,
                            Password = signup.Password
                        };
                        await context.Users.AddAsync(newStudent);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        logger.AddException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return flag;
        }


        public async Task<(bool, string)> Login(LoginModel login)
        {
            bool flag = false;
            string token = null;
            try
            {
                User user = await context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

                if (user != null)
                {
                    if (user.Password == login.Password)
                    {
                        flag = true;
                        token = GenerateJwtToken(user);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return (flag, token);
        }


        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyWithAtLeast16Bytes"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
                        {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                };

            var token = new JwtSecurityToken(
                issuer: "",
                audience: "",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<bool> AddCategory(CategoryModel category)
        {
            bool flag = false;
            try
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Category newCategory = new Category
                        {
                            CategoryTitle = category.Title,
                        };
                        await context.Categories.AddAsync(newCategory);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        logger.AddException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return flag;
        }

        public async Task<bool> AddAgency(AgencyModel agency)
        {
            bool flag = false;
            try
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Agency newAgency = new Agency
                        {
                            AgencyLogoPath = agency.Logopath,
                            AgencyName = agency.Name,
                        };
                        await context.Agencies.AddAsync(newAgency);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        logger.AddException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return flag;
        }

        public async Task<bool> AddAgencyFeed(AgencyFeedModel agencyfeed)
        {
            bool flag = false;
            try
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        AgencyFeed newAgencyFeed = await context.AgencyFeeds.Where(a => a.AgencyId == agencyfeed.AgencyId && a.CategoryId == agencyfeed.CategoryId).FirstOrDefaultAsync();
                        if (newAgencyFeed == null)
                        {
                            newAgencyFeed = new AgencyFeed
                            {
                                AgencyId = agencyfeed.AgencyId,
                                AgencyFeedUrl = agencyfeed.AgencyFeedUrl,
                                CategoryId = agencyfeed.CategoryId,
                            };
                            await context.AgencyFeeds.AddAsync(newAgencyFeed);
                        }
                        else
                        {
                            newAgencyFeed.AgencyFeedUrl=agencyfeed.AgencyFeedUrl;
                        }
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        logger.AddException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return flag;
        }

        public async Task<List<CategoryModel>> GetCategory()
        {
            List<CategoryModel> alldata = null;

            try
            {
                List<Category> result = await context.Categories.ToListAsync();

                alldata = new List<CategoryModel>();

                foreach (Category category in result)
                {
                    alldata.Add(new CategoryModel
                    {
                        Title = category.CategoryTitle,
                        Id = category.CategoryId,
                    });
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }

            return alldata;
        }

        public async Task<List<AgencyModel>> GetAgency()
        {
            List<AgencyModel> alldata = new List<AgencyModel>();

            try
            {
                List<Agency> result = await context.Agencies.ToListAsync();

                foreach (Agency agency in result)
                {
                    alldata.Add(new AgencyModel
                    {
                        Logopath = agency.AgencyLogoPath,
                        Name = agency.AgencyName,
                        Id = agency.AgencyId,
                    });
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }

            return alldata;
        }

        public async Task<string> GetAgencyUrl(int categoryid, int agencyid)
        {
            string url = null;

            try
            {
                AgencyFeed agencyfeed = await context.AgencyFeeds.FirstOrDefaultAsync(af => af.CategoryId == categoryid && af.AgencyId == agencyid);
                url = agencyfeed.AgencyFeedUrl;
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }

            return url;
        }

        public async Task<bool> DeleteAllNews()
        {
            bool flag = false;
            try
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE news");
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        logger.AddException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return flag;
        }

        public async Task<List<CategoryModel>> GetCategoriesFromAgencyId(int id)
        {
            List<CategoryModel> allcategories = new List<CategoryModel>();

            try
            {
                allcategories = await context.AgencyFeeds.Where(a => a.AgencyId == id).Select(a => new CategoryModel
                {
                    Id = a.CategoryId,
                    Title = a.Category.CategoryTitle
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }

            return allcategories;
        }

        public async Task<List<NewsModel>> GetAllNews(int id)
        {
            List<NewsModel> allnews = new List<NewsModel>();

            try
            {
                allnews = await context.News.Where(c => c.AgencyId == id).Select(c => new NewsModel
                {
                    AgencyId = c.AgencyId,
                    CategoryId = c.CategoryId,
                    NewsId = c.NewsId,
                    NewsTitle = c.NewsTitle,
                    NewsDescription = c.NewsDescription,
                    NewsPublishDateTime = c.NewsPublishDateTime,
                    NewsLink = c.NewsLink,
                    ClickCount = c.ClickCount,
                }).OrderByDescending(c => c.NewsPublishDateTime).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }

            return allnews;
        }

        public async Task<List<ReportModel>> GeneratePdf(DateTime start, DateTime end)
        {
            List<ReportModel> data = null;
            try
            {
                data = await context.News.Select(n => new ReportModel
                {
                    AgencyName=context.Agencies.FirstOrDefault(a=>a.AgencyId==n.AgencyId).AgencyName,
                    ClickCount= (int)n.ClickCount,
                    NewsTitle=n.NewsTitle
                }).OrderByDescending(a=>a.ClickCount).ToListAsync();

            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return data;
        }

        public async Task<List<NewsModel>> GetNewsByCategories(List<int> categories, int id)
        {
            if (categories != null && id > 0)
            {
                DataAccess d = new DataAccess(context, logger);
                foreach (int catid in categories)
                {
                    Task<string> agencyFeedUrl = d.GetAgencyUrl(catid, id);
                    string url = await agencyFeedUrl;
                    if (!string.IsNullOrEmpty(url))
                    {
                        string xmlData = FetchXmlData(url);
                        if (!string.IsNullOrEmpty(xmlData))
                        {
                            List<NewsModel> newsData = ParseXmlData(xmlData);
                            if (newsData != null && newsData.Any())
                            {
                                await d.AddNews(newsData, catid, id);
                            }
                        }
                    }
                }
            }
            List<NewsModel> news = null;
            try
            {
                news = await context.News
                    .Where(n => categories.Contains(n.CategoryId) && n.AgencyId == id)
                    .Select(c => new NewsModel
                    {
                        AgencyId = c.AgencyId,
                        CategoryId = c.CategoryId,
                        NewsId = c.NewsId,
                        NewsTitle = c.NewsTitle,
                        NewsDescription = c.NewsDescription,
                        NewsPublishDateTime = c.NewsPublishDateTime,
                        NewsLink = c.NewsLink,
                        ClickCount = c.ClickCount,
                    }).OrderByDescending(c => c.NewsPublishDateTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return news;
        }

        public async Task<bool> IncrementNewsClickCount(int id)
        {
            bool flag = false;
            try
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        News news = await context.News.FirstOrDefaultAsync(n => n.NewsId == id);
                        if (news != null)
                        {
                            if (news.ClickCount == null)
                            {
                                news.ClickCount = 1;
                            }
                            else
                            {
                                news.ClickCount++;
                            }
                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();
                            flag = true;
                        }
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        logger.AddException(e);
                    }
                }
            }
            catch (Exception e)
            {
                logger.AddException(e);
            }
            return flag;
        }

        public async Task<bool> AddNews(List<NewsModel> news, int categoryid, int agencyid)
        {
            bool flag = false;
            try
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (NewsModel item in news)
                        {
                            bool extnews = await context.News.AnyAsync(n => n.NewsLink == item.NewsLink);
                            if (!extnews)
                            {
                                News n = new News
                                {
                                    AgencyId = agencyid,
                                    CategoryId = categoryid,
                                    ClickCount = item.ClickCount,
                                    NewsDescription = item.NewsDescription,
                                    NewsLink = item.NewsLink,
                                    NewsTitle = item.NewsTitle,
                                    NewsPublishDateTime = item.NewsPublishDateTime
                                };
                                await context.News.AddAsync(n);
                                await context.SaveChangesAsync();
                            }
                        }
                        await transaction.CommitAsync();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        logger.AddException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return flag;
        }

        private static string FetchXmlData(string agencyFeedUrl)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(agencyFeedUrl);
            }
        }

        private static List<NewsModel> ParseXmlData(string data)
        {
            List<NewsModel> newsDataList = new List<NewsModel>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);

            XmlNodeList Nodes = xmlDocument.SelectNodes("//item");

            foreach (XmlNode itemNode in Nodes)
            {
                string newstitle = itemNode.SelectSingleNode("title")?.InnerText;
                string newsdescription = itemNode.SelectSingleNode("description")?.InnerText;
                string newslink = itemNode.SelectSingleNode("link")?.InnerText;
                string newsdatetime = itemNode.SelectSingleNode("pubDate")?.InnerText;

                DateTime newspublishDateTime;
                if (DateTime.TryParse(newsdatetime, out newspublishDateTime))
                {
                    newsDataList.Add(new NewsModel
                    {
                        NewsTitle = newstitle,
                        NewsDescription = newsdescription,
                        NewsLink = newslink,
                        NewsPublishDateTime = newspublishDateTime
                    });
                }
            }

            return newsDataList;
        }

        public async Task<bool> FindEmail(string email)
        {
            bool flag = false;
            try
            {
                int count = context.Users.Where(u => u.Email == email).Count();
                if (count == 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                logger.AddException(ex);
            }
            return flag;
        }
    }
}
